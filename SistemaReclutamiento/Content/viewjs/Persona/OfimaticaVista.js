var OfimaticaVista = function () {
    var _llenarPorcentaje = function () {
        responseSimple({
            url: "Postulante/PostulanteObtenerPorcentajeAvanceJson",
            refresh: false,
            callBackSuccess: function (response) {
                CloseMessages();
                $('#porcentajeProgreso').css({ 'width': response.data + '%' });
                $('.progress_wide>span>i').html("")
                $('.progress_wide>span>i').append(response.data + "%")
            }
        });
    }
    var _inicio = function () {
        $('#cbotipoOfimatica').select2();
        $('#cboofimaticaNivel').select2();
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
            useCurrent: false,
            ignoreReadonly: true,
            allowInputToggle: true
        });
        $("#perfil_principal").attr("src", "data:image/gif;base64," + rutaImage);
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);

        selectResponse({
            url: "EstOfimatica/EstOfimaticaListarJson",
            select: "cboofimaticaHerramienta",
            campoID: "eof_id",
            CampoValor: "eof_nombre",
            select2: true,
            allOption: false
        });
    };
    var _ListarOfimatica = function () {

        responseSimple({
            url: "Ofimatica/OfimaticaListarJson",
            data: JSON.stringify({ fkPosID: $("[name='fk_postulante']").val() }),
            refresh: false,
            callBackSuccess: function (response) {
                var respuesta = response.respuesta;
                var datos = response.data;
                if (respuesta) {
                    $("#tbody_Ofimatica").html("");
                    $.each(datos, function (index, value) {
                        $("#tbody_Ofimatica").append('<tr><td>' + value.ofi_tipo + '</td><td>' + value.ofi_centro_estudio + '</td><td>' + value.eof_nombre + '</td><td>' + value.ofi_nivel + '</td><td>' + moment(value.ofi_periodo_ini).format("DD/MM/YYYY") + '</td><td>' + moment(value.ofi_periodo_fin).format("DD/MM/YYYY") + '</td><td><button type="button" data-id="' + value.ofi_id + '" class="btn btn-danger btn-xs btn_delete"><i class="fa fa-times"></i></button></td></tr>');
                    });
                    
                    if (datos.length == 0) {                    
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
       
        $("#myDatepicker1").on("dp.change", function (e) {
            $('#myDatepicker2').data("DateTimePicker").minDate(e.date);
        });
        $("#myDatepicker2").on("dp.change", function (e) {
            $('#myDatepicker1').data("DateTimePicker").maxDate(e.date);
        });
     
        $(document).on("click", ".btn_guardar", function (e) {
            $("#frmOfimatica-form").submit();
            if (_objetoForm_frmOfimatica.valid()) {
                var dataForm = $('#frmOfimatica-form').serializeFormJSON();
                responseSimple({
                    url: "Ofimatica/OfimaticaInsertarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            _llenarPorcentaje();
                            limpiar_form({ contenedor: "#frmOfimatica-form" });
                            _objetoForm_frmOfimatica.resetForm();
                            OfimaticaVista.init__ListarOfimatica();
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
            _objetoForm_frmOfimatica.resetForm();
        });

        $(document).on("click", ".btn_delete", function (e) {
            var id = $(this).data("id");
            if (id != "" || id > 0) {
                messageConfirmation({
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "Ofimatica/OfimaticaEliminarJson",
                            data: JSON.stringify({ id: id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                _llenarPorcentaje();
                                OfimaticaVista.init__ListarOfimatica();
                            }
                        });
                    }
                });
            }
            else {
                messageResponse({
                    text: "Error, no se encontro ID",
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
            nameVariable: 'frmOfimatica',
            contenedor: '#frmOfimatica-form',
            rules: {
                ofi_tipo:
                {
                    required: true,

                },
                ofi_centro_estudio:
                {
                    required: true,

                },
                fk_ofimatica:
                {
                    required: true,

                },
                ofi_nivel:
                {
                    required: true,

                },
                ofi_periodo_ini:
                {
                    required: true,

                },
                ofi_periodo_fin:
                {
                    required: true,

                },

            },
            messages: {
                ofi_tipo:
                {
                    required: 'Tipo Obligatorio',
                },
                ofi_centro_estudio:
                {
                    required: 'Centro Estudios Obligatorio',
                },
                fk_ofimatica:
                {
                    required: 'Herramienta Obligatoria',
                },
                ofi_nivel:
                {
                    required: 'Nombre Obligatorio',
                },
                ofi_periodo_ini:
                {
                    required: 'Fecha Inicio Obligatorio',
                },
                ofi_periodo_fin:
                {
                    required: 'Fecha Fin Obligatorio',
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
            _ListarOfimatica();
            _componentes();
            _metodos();
            _llenarPorcentaje();
        },
        init__ListarOfimatica: function () {
            _ListarOfimatica();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    OfimaticaVista.init();
});