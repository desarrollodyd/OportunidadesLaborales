var LoginRegisterView = function () {
    //
    // Setup module components
    //
    var _componentes = function () {
        $(document).on("click", ".btn_registrar", function (e) {
            console.log("adasd");
            $("#registro-form").submit();
            if (_objetoForm_frmRegistro.valid()) {
                var dataForm = $('#registro-form').serializeFormJSON();
                messageConfirmation({
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "Proveedor/ProveedorInsertarJson",
                            data: JSON.stringify(dataForm),
                            refresh: true,
                            time: 2000,
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

        $(document).on("click", ".btn_ingresar", function (e) {
            $("#login-form").submit();
            if (_objetoForm_frmLogin.valid()) {
                var dataForm = $('#login-form').serializeFormJSON();
                responseSimple({
                    url: "Login/ProveedorValidarLoginJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        console.log(response);
                        var pendiente = response.estado;
                        if (pendiente != "") {
                            redirect({ site: "Login/ProveedorActivacion?id=" + pendiente });
                        }
                        else {
                            if (response.respuesta) {
                                redirect({ site: "Proveedor/Index" });
                            }
                            else {
                                $("#usu_password").val("");
                            }
                        }
                        
                    },
                    time: 500
                });
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
                    url: "Login/ProveedorRecuperarContrasenia",
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
                    //email: true
                },
                usu_password: {
                    required: true
                },
            },
            messages: {
                usu_login: {
                    required: 'Campo Obligatorio',
                    //email: 'Formato Correo Incorrecto'
                },
                usu_password: {
                    required: 'Campo Obligatorio'
                },
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
        validar_Form({
            nameVariable: 'frmRegistro',
            contenedor: '#registro-form',
            rules: {
                per_numdoc:
                {
                    required: true,
                    minlength: 11,
                    maxlength: 11,
                    digits: true
                },
                per_correoelectronico:
                {
                    required: true,
                    email: true
                }
            },
            messages: {
               
                per_numdoc:
                {
                    required: 'Nro de RUC Obligatorio',
                    minlength: 'Minimo 11 Caracteres',
                    maxlength: 'Maximo 11 caracteres',
                    digits: 'Solo Numeros'
                },
                per_correoelectronico:
                {
                    required: 'Email Obligatorio',
                    email: 'Formato Correo Incorrecto'
                }
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