var LoginRegisterView = function () {

    //
    // Setup module components
    //

    var _componentes = function () {

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
                        if(pendiente != "") {
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
                            url: "Persona/PersonaInsertarJson",
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