var LoginRegisterView = function () {
    //
    // Setup module components
    //
    var _componentes = function () {     
        $('#per_numdoc').on('keydown keypress', function (e) {
            if (e.key.length === 1) {
                if ($(this).val().length < 8 && !isNaN(parseFloat(e.key))) {
                    $(this).val($(this).val() + e.key);
                    if ($(this).val().length == 8) {
                        /*Logica para busqueda*/
                        var dataForm = $('#registro-form').serializeFormJSON();
                        $('#busqueda').val("nuevo");
                        responseSimple({
                            url: "Persona/PersonaDniObtenerJson",
                            data: JSON.stringify(dataForm),
                            refresh: false,
                            callBackSuccess: function (response) {
                                CloseMessages();
                                var data = response.data;                              
                                var encontrado = response.encontrado;
                                if (encontrado == "postgres" || encontrado == "sql") {
                                    console.log(response);
                                    $("#busqueda").val(encontrado);
                                    $("#per_nombre").val(data.per_nombre);
                                    $("#per_apellido_pat").val(data.per_apellido_pat);
                                    $("#per_apellido_mat").val(data.per_apellido_mat);
                                }
                                else if (encontrado === "") {
                                    messageResponse({
                                        text: response.mensaje,
                                        type:"error"
                                    });
                                    //redirect({ site: "" });
                                }
                                else {
                                    console.log(response);
                                    $("#busqueda").val(encontrado);
                                    $("#per_nombre").val("");
                                    $("#per_apellido_pat").val("");
                                    $("#per_apellido_mat").val("");
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
                    url: "Login/PostulanteValidarLoginJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,                 
                    callBackSuccess: function (response) {
                        var pendiente = response.estado;
                        if (pendiente != "") {                           
                              redirect({ site: "Login/PostulanteActivacion?id=" + pendiente });
                        }
                        else {
                            if (response.respuesta) {
                                redirect({ site: "" });
                            }
                            else {
                                $("#usu_password").val("");
                            }
                        }
                    },
                    time:500
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
                            url: "Persona/PersonaInsertarJson",
                            data: JSON.stringify(dataForm),
                            refresh: true,
                            time:2000,
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
                    url: "Login/PostulanteRecuperarContrasenia",
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

        $(document).keypress(function (event) {

            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                console.warn("vista");
                
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