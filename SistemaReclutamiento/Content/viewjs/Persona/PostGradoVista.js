var PostGradoVista = function () {
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
    var _ListarPostGrado = function () {

        responseSimple({
            url: "Postgrado/PostgradoListarJson",
            data: JSON.stringify({ fkPosID: $("[name='fk_postulante']").val() }),
            refresh: false,
            callBackSuccess: function (response) {
                var respuesta = response.respuesta;
                var datos = response.data;
                if (respuesta) {
                    $("#tbody_PostGrado").html("");
                    $.each(datos, function (index, value) {
                        $("#tbody_PostGrado").append('<tr><td>' + value.pos_tipo + '</td><td>' + value.pos_centro_estudio + '</td><td>' + value.pos_carrera + '</td><td>'+value.pos_nombre+'</td><td>' + moment(value.pos_periodo_ini).format("DD/MM/YYYY") + '</td><td>' + moment(value.pos_periodo_fin).format("DD/MM/YYYY") + '</td><td>' + value.pos_condicion + '</td><td><button type="button" data-id="' + value.pos_id + '" class="btn btn-danger btn-xs btn_delete"><i class="fa fa-times"></i></button></td></tr>');
                    });
                    
                    if (datos.length == 0) {
                        CloseMessages();
                        messageResponse({
                            text: "No se Encontraron Registros",
                            type: "warning"
                        });
                    }
                    CloseMessages();
                }
            }
        });

    };

    var _componentes = function () {

        $(document).on("click", ".btn_guardar", function (e) {
            $("#frmPostGrado-form").submit();
            if (_objetoForm_frmPostGrado.valid()) {
                var dataForm = $('#frmPostGrado-form').serializeFormJSON();
                responseSimple({
                    url: "Postgrado/PostgradoInsertarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            limpiar_form({ contenedor: "#frmPostGrado-form" });
                            _objetoForm_frmPostGrado.resetForm();
                            PostGradoVista.init__ListarPostGrado();
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
            _objetoForm_frmPostGrado.resetForm();
        });

        $(document).on("click", ".btn_delete", function (e) {
            var id = $(this).data("id");
            if (id != "" || id > 0) {
                messageConfirmation({
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "Postgrado/PostgradoEliminarJson",
                            data: JSON.stringify({ id: id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                PostGradoVista.init__ListarPostGrado();
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
            nameVariable: 'frmPostGrado',
            contenedor: '#frmPostGrado-form',
            rules: {
                pos_tipo:
                {
                    required: true,

                },
                pos_centro_estudio:
                {
                    required: true,

                },
                pos_carrera:
                {
                    required: true,

                },
                pos_nombre:
                {
                    required: true,

                },
                pos_periodo_ini:
                {
                    required: true,

                },
                pos_periodo_fin:
                {
                    required: true,

                },
                pos_condicion:
                {
                    required: true,

                }

            },
            messages: {
                pos_tipo:
                {
                    required: 'Tipo Obligatorio',
                },
                pos_centro_estudio:
                {
                    required: 'Centro Estudios Obligatorio',
                },
                pos_carrera:
                {
                    required: 'Carrera Obligatorio',
                },
                pos_nombre:
                {
                    required: 'Nombre Obligatorio',
                },
                pos_periodo_ini:
                {
                    required: 'Fecha Inicio Obligatorio',
                },
                pos_periodo_fin:
                {
                    required: 'Fecha Fin Obligatorio',
                },
                pos_condicion:
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
            _ListarPostGrado();
            _componentes();
            _metodos();

        },
        init__ListarPostGrado: function () {
            _ListarPostGrado();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PostGradoVista.init();
});