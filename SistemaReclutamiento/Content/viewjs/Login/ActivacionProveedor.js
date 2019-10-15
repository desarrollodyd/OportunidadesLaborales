var ActivacionView = function () {

    //
    // Setup module components
    //

    var _inicio = function () {
        console.warn(mensaje)
        if (!respuesta && mensaje != "") {
            messageResponse({
                text: mensaje,
                type: "warning"
            });
            $(".btn_guardar").attr("disabled", "disabled");
            redirect({ site: "Login/ProveedorIndex" });
        }
        else {
            $("#usu_id").val(data[0].usuarioID);
        }
    };

    var _componentes = function () {

        $(document).on("click", ".btn_guardar", function (e) {
            $("#activacion-form").submit();
            if (_objetoForm_frmActivacion.valid()) {
                var dataForm = $('#activacion-form').serializeFormJSON();
                responseSimple({
                    url: "Login/ProveedorCambiarPasswordUsuario",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    redirect: true,
                    redirectUrl: "",
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
            nameVariable: 'frmActivacion',
            contenedor: '#activacion-form',
            rules: {
                usu_password: {
                    required: true,
                },

                usu_password_repetido: {
                    required: true,
                    equalTo: '#usu_password'
                },
            },
            messages: {
                usu_password: {
                    required: 'Debe Ingresar una Contraseña'
                },

                usu_password_repetido: {
                    required: 'Debe Ingresar una Contraseña',
                    equalTo: 'Las contraseñas deben ser iguales'
                },
            }
        });

    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();
            _componentes();
            _metodos();

        },
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    ActivacionView.init();
});