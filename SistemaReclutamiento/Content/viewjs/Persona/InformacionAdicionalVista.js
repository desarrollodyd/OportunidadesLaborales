var EducacionBasicaVista = function () {
    var _inicio = function () {
        $("[name='per_id']").val(persona.per_id);
        $("[name='pos_id']").val(postulante.pos_id);
        $("[name='fk_postulante']").val(postulante.pos_id);
        $("#persona_nombre").text(persona.per_nombre + " " + persona.per_apellido_pat + " " + persona.per_apellido_mat);
        $("#perfil_principal").attr("src", "data:image/gif;base64," + rutaImage);
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);
        $("#frm_cv").attr("href", "data:application/octet-stream;charset=utf-8;base64," + rutaCV);
        $("#pos_nombre_referido").val(postulante.pos_nombre_referido);
 

        if (postulante.pos_referido == "") {
            $('#cboReferido').bootstrapToggle('off');
            $("#pos_referido").val(false);
        }
        else {
            if (postulante.pos_referido == true) {
                $('#cboReferido').bootstrapToggle('on');
                $("#pos_referido").val(true);
            }
            else {
                $('#cboReferido').bootstrapToggle('off');
                $("#pos_referido").val(false);
            }
        }

    };

    var _componentes = function () {


        $("#cboReferido").change(function () {
            var check = $(this).prop('checked');
            if (check) {
                $("#pos_referido").val(true);
            }
            else {
                $("#pos_referido").val(false);
            }
        });

        $(document).on("click", ".btn_guardar", function (e) {
            $("#frmInformacionAdicional-form").submit();
            if (_objetoForm_frmInformacionAdicional.valid()) {
                var dataForm = new FormData(document.getElementById("frmInformacionAdicional-form"));
                responseFileSimple({
                    url: "Postulante/PostulanteInsertarInformacionAdicionalJson",
                    data: dataForm,
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
            _objetoForm_frmInformacionAdicional.resetForm();
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
            nameVariable: 'frmInformacionAdicional',
            contenedor: '#frmInformacionAdicional-form',
            rules: {
                pos_referido:
                {
                    required: true,

                },
                pos_cv:
                {
                    required: true,

                }

            },
            messages: {
                pos_referido:
                {
                    required: 'Referido Obligatorio',
                },
                pos_cv:
                {
                    required: 'Adjuntar Curriculum',
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