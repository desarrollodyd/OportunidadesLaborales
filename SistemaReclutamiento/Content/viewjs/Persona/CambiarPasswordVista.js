var CambiarPasswordVista = function () {
    var _llenarPorcentaje = function () {
        responseSimple({
            url: "Postulante/PostulanteObtenerPorcentajeAvanceJson",
            refresh: false,
            callBackSuccess: function (response) {
                CloseMessages();
                console.log(response);
                $('#porcentajeProgreso').css({ 'width': response.data + '%' });
                $('.progress_wide>span>i').html("")
                $('.progress_wide>span>i').append(response.data + "%")
            }
        });
    }
    var _inicio = function () {
        $("[name='per_id']").val(persona.per_id);
        $("[name='pos_id']").val(postulante.pos_id);
        $("[name='fk_postulante']").val(postulante.pos_id);
        $("#persona_nombre").text(persona.per_nombre + " " + persona.per_apellido_pat + " " + persona.per_apellido_mat);
        $("#perfil_principal").attr("src", "data:image/gif;base64," + rutaImage);
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);

    };

    var _componentes = function () {

        $(document).on("click", ".btn_guardar", function (e) {
            $("#frmCambiarPassword-form").submit();
            if (_objetoForm_frmCambiarPassword.valid()) {
                var dataForm = $('#frmCambiarPassword-form').serializeFormJSON();
                responseSimple({
                    url: "Persona/CambiarPasswordVistaJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var respuesta = response.respuesta;
                        console.log(response);
                        if (respuesta) {
                            limpiar_form({ contenedor: "#frmCambiarPassword-form" });
                            _objetoForm_frmCambiarPassword.resetForm();
                            redirect({ site: "" });
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

        $(document).on("click", ".btn_cancelar", function (e) {
            _objetoForm_frmCambiarPassword.resetForm();
        });

        $('#subir-img-perfil').change(function () {
            var dataForm = new FormData();
            var _image = $('#subir-img-perfil')[0].files[0];
            dataForm.append('file', _image);
            dataForm.append('postulanteID', $("#pos_id").val());
            responseFileSimple({
                url: "Postulante/PostulanteSubirFotoJson",
                data: dataForm,
                refresh: false,
                callBackSuccess: function (response) {
                    var respuesta = response.respuesta;
                    if (respuesta) {
                        _llenarPorcentaje();
                        readImage(_image, "#perfil_principal");
                        readImage(_image, "#img_layout_post");
                    }
                }
            });
        });
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmCambiarPassword',
            contenedor: '#frmCambiarPassword-form',
            rules: {
                usu_password:
                {
                    required: true,
                },
                usu_password_repetido:
                {
                    required: true,
                    equalTo:'#usu_password'
                }
            },
            messages: {
                usu_password:
                {
                    required: 'Campo Obligatorio',
                },
                usu_password_repetido:
                {
                    required: 'Campo Obligatorio',
                    equalTo: 'Las Contraseñas deben ser iguales'
                }
            },
            errorPlacement: function (error, element) {
                if (element.is(":radio") || element.is(":checkbox")) {
                    element.closest('.option-group').after(error);
                }
                else {
                    error.insertAfter(element);
                }
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
            _llenarPorcentaje();
        },
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    CambiarPasswordVista.init();
});