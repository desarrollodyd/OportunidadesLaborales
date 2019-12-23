var EducacionBasicaVista = function () {
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
        $('select option').each(function () {
            $(this).text($(this).text().toUpperCase());
        });
    }
    var _inicio = function () {

        $('#cbotipoEducacionBasica').select2();
        $("[name='per_id']").val(persona.per_id);
        $("[name='pos_id']").val(postulante.pos_id);
        $("[name='fk_postulante']").val(postulante.pos_id);
        $("#persona_nombre").text(persona.per_nombre + " " + persona.per_apellido_pat + " " + persona.per_apellido_mat);
        $("#perfil_principal").attr("src", "data:image/gif;base64," + rutaImage);
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);

    };
    var _ListarEducacionBasica = function () {
       
        responseSimple({
            url: "EducacionBasica/EducacionBasicaListarJson",
            data: JSON.stringify({ fkPosID: $("[name='fk_postulante']").val()}),
            refresh: false,
            callBackSuccess: function (response) {
                var respuesta = response.respuesta;
                var datos = response.data;
                if (respuesta) {
                    $("#tbody_EducacionBasica").html("");
                    $.each(datos, function (index, value) {
                        $("#tbody_EducacionBasica").append('<tr><td>' + value.eba_tipo + '</td><td>' + value.eba_nombre + '</td><td>' + value.eba_condicion + '</td><td><button type="button" data-id="' + value.eba_id + '" class="btn btn-danger btn-xs btn_delete"><i class="fa fa-times"></i></button></td></tr>');
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

        $(document).on("click", ".btn_guardar", function (e) {
            $("#frmEducacionBasica-form").submit();
            if (_objetoForm_frmEducacionBasica.valid()) {
                var dataForm = $('#frmEducacionBasica-form').serializeFormJSON();
                responseSimple({
                    url: "EducacionBasica/EducacionBasicaInsertarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            _llenarPorcentaje();
                            limpiar_form({ contenedor: "#frmEducacionBasica-form" });
                            _objetoForm_frmEducacionBasica.resetForm();
                            EducacionBasicaVista.init_ListarEducacionBasica();
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
            _objetoForm_frmEducacionBasica.resetForm();
        });

        $(document).on("click", ".btn_delete", function (e) {
            var id = $(this).data("id");
            if (id != "" || id > 0) {
                messageConfirmation({
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "EducacionBasica/EducacionBasicaEliminarJson",
                            data: JSON.stringify({ id: id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                _llenarPorcentaje();
                                EducacionBasicaVista.init_ListarEducacionBasica();
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
                    required: 'Condición Obligatorio',
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
            _ListarEducacionBasica();
            _componentes();
            _metodos();
            _llenarPorcentaje();
        },
        init_ListarEducacionBasica: function() {
            _ListarEducacionBasica();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    EducacionBasicaVista.init();
});