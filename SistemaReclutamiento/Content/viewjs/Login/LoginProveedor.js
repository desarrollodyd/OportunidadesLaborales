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
                    url: "Login/ProveedorValidarLoginJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        console.log(response);
                        if (response.respuesta) {
                            redirect({ site: "Proveedor/Index" });
                        }
                        else {
                            $("#usu_password").val("");
                        }
                    },
                    time: 5000
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