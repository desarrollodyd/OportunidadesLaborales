var LoginRegisterView = function () {

    //
    // Setup module components
    //

    var _componentes = function () {     
        var encontrado = false;

       
        $('#per_numdoc').on('keydown keypress', function (e) {
            if (e.key.length === 1) {
                if ($(this).val().length < 8 && !isNaN(parseFloat(e.key))) {
                    $(this).val($(this).val() + e.key);
                    if ($(this).val().length == 8) {
                    /*Logica para busqueda*/
                        var dataForm = $('#registro-form').serializeFormJSON();
                        $('#busqueda').val("nuevo");
                        responseSimple({
                            url: "Persona/PersonaDniObtenerJson",//Postgres
                            data: JSON.stringify(dataForm),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if (response.respuesta == true) {
                                    messageConfirmation({
                                        callBackSAceptarComplete: function () {
                                            encontrado = true;
                                            console.log(response.data);
                                            $("#busqueda").val("postgres");
                                            $("#per_nombre").val(response.data.per_nombre);
                                            $("#per_apellido_pat").val(response.data.per_apellido_pat);
                                            $("#per_apellido_mat").val(response.data.per_apellido_mat);
                                            $("#per_correoelectronico").val(response.data.per_correoelectronico);
                                            console.warn(response.data);
                                        },
                                        content: "Usted ya se encuentra registrado en nuestra BD Postgres, sus datos seran usados para llenar el formulario."
                                    });
                                }
                                else {
                                    /*Busqueda en SQL*/
                                    responseSimple({
                                        url: "Persona/PersonaSQLDniObtenerJson",
                                        data: JSON.stringify(dataForm),
                                        refresh: false,
                                        callBackSuccess: function (response) {
                                            if (response.respuesta == true) {
                                                messageConfirmation({
                                                    callBackSAceptarComplete: function () {
                                                        encontrado = true;
                                                        console.log(response.data);
                                                        $("#busqueda").val("sql");
                                                        $("#per_nombre").val(response.data.NO_TRAB);
                                                        $("#per_apellido_pat").val(response.data.NO_APEL_PATE);
                                                        $("#per_apellido_mat").val(response.data.NO_APEL_MATE);
                                                        $("#per_correoelectronico").val(response.data.NO_DIRE_MAI1);
                                                    },
                                                    content: "Usted ya se encuentra registrado en nuestra BD SQL, sus datos seran usados para llenar el formulario."
                                                });
                                            }
                                            else {
                                                console.log("no encontrado");
                                                console.log(response.data);
                                                $("#busqueda").val("nuevo");
                                                $("#per_nombre").val("");
                                                $("#per_apellido_pat").val("");
                                                $("#per_apellido_mat").val("");
                                                $("#per_correoelectronico").val("");                                                
                                            }
                                        }
                                    });
                                    /*Fin Busqueda SQL*/                                    
                                }
                            }
                        });
                    /*Fin de Logica*/
                    }
                }
                return false;
            }
        });


        $(document).on("click", ".btn_ingresar", function (e) {
            $("#login-form").submit();
            if (_objetoForm_frmLogin.valid()) {
                var dataForm = $('#login-form').serializeFormJSON();
                responseSimple({
                    url: "Login/ValidarLoginJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,                 
                    callBackSuccess: function (response) {
                        var pendiente = response.estado;
                        if (pendiente != "") {
                            console.log(pendiente);
                              redirect({ site: "Login/Activacion?id=" + pendiente });
                        }
                        else {
                            redirect({ site: "" });
                        }
                        console.warn(response);   
                    }
                });
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        });

        $(document).on("click", ".btn_registrar", function (e) {
            $("#registro-form").submit();
            if (_objetoForm_frmRegistro.valid()) {
                var dataForm = $('#registro-form').serializeFormJSON();
                messageConfirmation({
                    callBackSAceptarComplete: function() {
                        responseSimple({
                            url: "Persona/PersonaInsertarJson2",
                            data: JSON.stringify(dataForm),
                            refresh: true,
                            //callBackSuccess: function (response) {
                            //    console.warn(response);
                            //}
                        });
                    }
                })
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        });

        $(document).on("click", ".btn_recuperar", function (e) {
            $("#recovery-form").submit();
            if (_objetoForm_frmRecovery.valid()) {
                var dataForm = $('#recovery-form').serializeFormJSON();
                responseSimple({
                    url: "Login/RecuperarContrasenia",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            $('.recovery .mssg').addClass('animate');
                            setTimeout(function () {
                                $('.recovery').swapClass('open', 'closed');
                                $('#toggle-terms').swapClass('open', 'closed');
                                $('.tabs-content .fa').swapClass('active', 'inactive');
                                $('.recovery .mssg').removeClass('animate');
                            }, 2500);
                            limpiar_form({
                                contenedor: '#recovery-form',
                            });
                            _objetoForm_frmRecovery.resetForm();
                        }            
                    }
                }); 

            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        });
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmLogin',
            contenedor: '#login-form',
            rules: {
                usu_login: {
                    required: true,
                    email: true
                },
                usu_password: {
                    required: true
                },
            },
            messages: {
                usu_login: {
                    required: 'Campo Obligatorio',
                    email:'Formato Correo Incorrecto'
                },
                usu_password: {
                    required: 'Campo Obligatorio'
                },
            }
        });

        validar_Form({
            nameVariable: 'frmRegistro',
            contenedor: '#registro-form',
            rules: {
                per_nombre:
                {
                    required: true,
                },
                per_apellido_pat:
                {
                    required: true,
                },
                per_apellido_mat:
                {
                    required: true,
                },
                per_tipodoc: {
                    required:true,
                },
                per_numdoc:
                {
                    required: true,
                    minlength: 8,
                    maxlength: 8,
                    digits: true
                },
                per_correoelectronico:
                {
                    required: true,
                    email:true
                }
            },
            messages: {
                per_nombre:
                {
                    required: 'Nombre Obligatorio',
                },
                per_apellido_pat:
                {
                    required: 'Apellido Paterno Obligatorio',
                },
                per_apellido_mat:
                {
                    required: 'Apellido Materno Obligatorio',
                },
                per_tipodoc: {
                    required: 'Tipo Documento Obligatorio',
                },
                per_numdoc:
                {
                    required: 'Nro Documento Obligatorio',
                    minlength: 'Minimo 8 Caracteres',
                    maxlength: 'Maximo 8 caracteres',
                    digits: 'Solo Numeros'
                },
                per_correoelectronico:
                {
                    required: 'Email Obligatorio',
                    email: 'Formato Correo Incorrecto'
                }
            }
        });

        validar_Form({
            nameVariable: 'frmRecovery',
            contenedor: '#recovery-form',
            rules: {
                correo_recuperacion: {
                    required: true,
                    email: true
                },
            },
            messages: {
                correo_recuperacion: {
                    required: 'Campo Obligatorio',
                    email: 'Formato Correo Incorrecto'
                },
            }
        });

    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _componentes();
            _metodos();

        },
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    LoginRegisterView.init();
});