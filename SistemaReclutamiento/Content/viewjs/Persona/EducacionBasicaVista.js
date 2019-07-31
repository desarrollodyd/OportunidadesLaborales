var EducacionBasicaVista = function () {
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
            $("#frmEducacionBasica-form").submit();
            if (_objetoForm_frmEducacionBasica.valid()) {
                var dataForm = $('#frmEducacionBasica-form').serializeFormJSON();
                responseSimple({
                    url: "EducacionBasica/EducacionBasicaInsertarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                });
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        });

        $(document).on("click", ".btn_cancelar", function (e) {
            _objetoForm_frmEducacionBasica.resetForm();
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
                        readImage(_image, "#perfil_principal");
                        readImage(_image, "#img_layout_post");
                    }
                }
            });
        });
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmEducacionBasica',
            contenedor: '#frmEducacionBasica-form',
            rules: {
                eba_tipo:
                {
                    required: true,

                },
                eba_nombre:
                {
                    required: true,

                },
                eba_condicion:
                {
                    required: true,

                }

            },
            messages: {
                eba_tipo:
                {
                    required: 'Tipo Obligatorio',
                },
                eba_nombre:
                {
                    required: 'Instituto Obligatorio',
                },
                eba_condicion:
                {
                    required: 'Condicion Obligatorio',
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
    EducacionBasicaVista.init();
});