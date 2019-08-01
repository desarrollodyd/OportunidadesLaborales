var EducacionSuperiorVista = function () {
    var _inicio = function () {
        $("[name='per_id']").val(persona.per_id);
        $("[name='pos_id']").val(postulante.pos_id);
        $("[name='fk_postulante']").val(postulante.pos_id);
        $("#persona_nombre").text(persona.per_nombre + " " + persona.per_apellido_pat + " " + persona.per_apellido_mat);
        $('#myDatepicker1').datetimepicker({
            format: 'DD/MM/YYYY',
            ignoreReadonly: true,
            allowInputToggle: true
        });
        $('#myDatepicker2').datetimepicker({
            format: 'DD/MM/YYYY',
            ignoreReadonly: true,
            allowInputToggle: true
        });
        $("#perfil_principal").attr("src", "data:image/gif;base64," + rutaImage);
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);

    };
    var _ListarEducacionSuperior = function () {

        responseSimple({
            url: "EducacionSuperior/EducacionSuperiorListarJson",
            data: JSON.stringify({ fkPosID: $("[name='fk_postulante']").val() }),
            refresh: false,
            callBackSuccess: function (response) {
                var respuesta = response.respuesta;
                var datos = response.data;
                if (respuesta) {
                    $("#tbody_EducacionSuperior").html("");
                    $.each(datos, function (index, value) {
                        $("#tbody_EducacionSuperior").append('<tr><td>' + value.esu_tipo + '</td><td>' + value.esu_centro_estudio + '</td><td>' + value.esu_carrera + '</td><td>' +moment(value.esu_periodo_ini).format("DD/MM/YYYY") + '</td><td>' + moment(value.esu_periodo_fin).format("DD/MM/YYYY") + '</td><td>' + value.esu_condicion+'</td><td><button type="button" data-id="' + value.esu_id +'" class="btn btn-danger btn-xs btn_delete"><i class="fa fa-times"></i></button></td></tr>');
                    });

                }
            }
        });

    };

    var _componentes = function () {

        $(document).on("click", ".btn_guardar", function (e) {
            $("#frmEducacionSuperior-form").submit();
            if (_objetoForm_frmEducacionSuperior.valid()) {
                var dataForm = $('#frmEducacionSuperior-form').serializeFormJSON();
                responseSimple({
                    url: "EducacionSuperior/EducacionSuperiorInsertarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            limpiar_form({ contenedor: "#frmEducacionSuperior-form" });
                            _objetoForm_frmEducacionSuperior.resetForm();
                            EducacionSuperiorVista.init__ListarEducacionSuperior();
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
            _objetoForm_frmEducacionSuperior.resetForm();
        });

        $(document).on("click", ".btn_delete", function (e) {
            var id = $(this).data("id");
            if (id != "" || id > 0) {
                messageConfirmation({
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "EducacionSuperior/EducacionSuperiorEliminarJson",
                            data: JSON.stringify({ id: id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                EducacionSuperiorVista.init__ListarEducacionSuperior();
                            }
                        });
                    }
                });
            }
            else {
                messageResponse({
                    text: "Error no se encontro ID",
                    type: "error"
                })
            }
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
            nameVariable: 'frmEducacionSuperior',
            contenedor: '#frmEducacionSuperior-form',
            rules: {
                esu_tipo:
                {
                    required: true,

                },
                esu_centro_estudio:
                {
                    required: true,

                },
                esu_carrera:
                {
                    required: true,

                },
                esu_periodo_ini:
                {
                    required: true,

                },
                esu_periodo_fin:
                {
                    required: true,

                },
                esu_condicion:
                {
                    required: true,

                }

            },
            messages: {
                esu_tipo:
                {
                    required: 'Tipo Obligatorio',
                },
                esu_centro_estudio:
                {
                    required: 'Centro Estudios Obligatorio',
                },
                esu_carrera:
                {
                    required: 'Carrera Obligatorio',
                },
                esu_periodo_ini:
                {
                    required: 'Fecha Inicio Obligatorio',
                },
                esu_periodo_fin:
                {
                    required: 'Fecha Fin Obligatorio',
                },
                esu_condicion:
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
            _ListarEducacionSuperior();
            _componentes();
            _metodos();

        },
        init__ListarEducacionSuperior: function () {
            _ListarEducacionSuperior();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    EducacionSuperiorVista.init();
});