var OfertaLaboralMisPostulacionesVista = function () {

    var _inicio = function () {
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);
    };
    var _ActivarTextBox = function () {
        $('#frm-Postular input').on('change', function () {
            var dato = $(this).data("id");
            if ($("input.texto" + dato + ":radio").is(':checked')) {
                $("input[name=opt_respuestalabel" + dato + "]").attr('disabled', false);
                //$('input[name=text]').attr('disabled', false);
            }
            else {
                $("input[name=opt_respuestalabel" + dato + "]").attr('disabled', true);
                //$('.radio>input[type=text]').attr('disabled', true);
            }
        });

    };
    var _ListarOfertas = function () {
        responseSimple({
            url: "OfertaLaboral/OfertaLaboralListarMisFavoritosJson",
            refresh: false,
            callBackSuccess: function (response) {
                CloseMessages();
                console.log(response);
                var data = response.data;
                var respuesta = response.respuesta;
                $("#ofertasContenido").html("");
                if (respuesta) {

                    $.each(data, function (index, value) {
                        var span = '';
                        //if (value.es_favorito) {
                        //    span += 'fa fa-star';
                        //}
                        //else {
                        //    span += 'fa fa-star-o';
                        //}
                        if (!value.ya_postulo) {
                            span += '<button type="button" id=postular' + value.ola_id + ' data-id=' + value.ola_id + ' class="btn btn-success btn-sm btn_postular" style="font-size: 15px !important;" data-toggle="modal" data-target=".bs-example-modal-postular"> Postula</button>';
                        }
                        $("#ofertasContenido").append('<div class=col-md-4 col-sm-4 col-xs-12"><div class="profile_details">' +
                            '<div class="well profile_view">' +
                            '<div class="col-md-12 col-sm-12 col-xs-12" style="text-align: center;">' +
                            '<h3 class="brief" style="margin: 0px !important;"><i>' + value.ola_nombre + '</i></h3>' +
                            '<h6 style="margin: 0px !important;">dirección</h6>' +
                            '<div class=" col-xs-12" style="padding-bottom: 10px;">' +
                            '<div class="ln_solid" style="margin-top: 10px !important;"></div>'
                            + '<button type="button" class="btn btn-primary btn_detalle" style="font-size:15px !important" data-toggle="modal" data-target=".bs-example-modal-detalle" data-id=' + value.ola_id + '>Detalle</button>' +
                            span +
                            '</div>' +
                            '</div>' +
                            '<div class="col-xs-12 bottom text-center">' +
                            '<div class="cold-md-12 col-xs-12 col-sm-12 emphasis">' +
                            '<p class="ratings" style="text-align: center;">' +
                            '<a>Publicado el ' + moment(value.ola_fecha_pub).format('DD/MM/YYYY') + '</a>' +
                            '<a class="favoritos" data-id="' + value.ola_id + '" href="#" style="float: right;"><span class="fa fa-star" style="color:#fff000"></span></a>' +
                            '</p>' +
                            '</div>' +
                            '</div>' +
                            '</div>' +
                            '</div></div>');
                    });

                    if (data.length == 0) {
                        CloseMessages();
                        messageResponse({
                            text: "No se Encontraron Postulaciones",
                            type: "warning"
                        });
                    }
                }
            }
        });
        /*Detalle Postulacion*/
        $(document).on("click", ".btn_detalle", function (e) {
            var data = { ola_id: $(this).data("id") };

            responseSimple({
                url: "OfertaLaboral/OfertaLaboralIdObtenerJson",
                data: JSON.stringify(data),
                refresh: false,
                callBackSuccess: function (response) {
                    CloseMessages();
                    if (response.respuesta) {
                        var oferta = response.data;
                        $("#detalleModalBody>.panel-default>.panel-body").html("");
                        $("#detalleModalLabel").text(oferta.ola_nombre);
                        $("#detalleModalBody>.panel-default>.requisitos").append("<p>" + oferta.ola_requisitos + "</p>");
                        $("#detalleModalBody>.panel-default>.funciones").append("<p>" + oferta.ola_funciones + "</p>");
                        $("#detalleModalBody>.panel-default>.competencias").append("<p>" + oferta.ola_competencias + "</p>");
                        $("#detalleModalBody>.panel-default>.condiciones_lab").append("<p>" + oferta.ola_condiciones_lab + "</p>");
                    }
                }
            });
        });
        /*Fin de Detalle*/
        /*Modal para Postular*/
        $(document).on("click", ".btn_postular", function (e) {
            $(".btn_postularme").attr('data-id', $(this).data("id"));
            var data = { ola_id: $(this).data("id") };
            responseSimple({
                url: "OfertaLaboral/DetPreguntaOLAListarJson",
                data: JSON.stringify(data),
                refresh: false,
                callBackSuccess: function (response) {
                    CloseMessages();
                    var oferta = response.oferta;
                    //$("#postularModalBody>.panel-default>.panel-body").html("");
                    //$("#postularModalBody>.panel-default>.requisitos").append("<p>" + oferta.ola_requisitos + "</p>");
                    //$("#postularModalBody>.panel-default>.funciones").append("<p>" + oferta.ola_funciones + "</p>");
                    //$("#postularModalBody>.panel-default>.competencias").append("<p>" + oferta.ola_competencias + "</p>");
                    //$("#postularModalBody>.panel-default>.condiciones_lab").append("<p>" + oferta.ola_condiciones_lab + "</p>");
                    var listaPreguntas = response.data;
                    var anexar = $("#postularModalBody>.x_panel>.x_content>form");
                    $(anexar).html("");
                    $.each(listaPreguntas, function (index, pregunta) {
                        var listaRespuestas = pregunta.DetalleRespuesta;
                        var tituloPregunta = "";
                        tituloPregunta += '<div class="form-group"><label data-id=' + pregunta.dop_id + '>' + pregunta.dop_pregunta + '</label><div class="pregunta' + pregunta.dop_id + '"></div></div>';
                        $(anexar).append(tituloPregunta);
                        $.each(listaRespuestas, function (i, respuesta) {
                            var tituloRespuesta = "";
                            if (respuesta.dro_respuesta == "") {
                                tituloRespuesta += '<div class="radio"><label><input name="opt_respuesta' + pregunta.dop_id + '" data-id="' + pregunta.dop_id + '" class="texto' + pregunta.dop_id + '" type="radio" value="" /> Otra Respuesta:</label> <input class="form-control" type="text" placeholder="Respuesta" disabled="true" name="opt_respuestalabel' + pregunta.dop_id + '" /></div>';
                            }
                            else {
                                tituloRespuesta += '<div class="radio"><label><input data-id="' + pregunta.dop_id + '" value="' + respuesta.dro_respuesta + '"name="opt_respuesta' + pregunta.dop_id + '" type="radio"/>' + respuesta.dro_respuesta + '</label></div>';
                            }
                            $(".pregunta" + respuesta.fk_det_pregunta_of).append(tituloRespuesta);
                        });
                    });
                    _ActivarTextBox();
                }
            });
        });
        /*Fin de Modal para Postular*/
        /*Boton Postularme de Modal Postular*/
        $(document).on("click", ".btn_postularme", function (e) {
            var id_oferta_laboral = $(this).data("id");
            var form = $("#frm-Postular").serialize();

            var elementoPregunta = $("#frm-Postular>.form-group>label");
            var elementoRespuesta = $("#frm-Postular>.form-group>div");
            preguntas = [];
            respuestas = [];
            $(elementoPregunta).each(function () {
                preguntas.push($(this).text() + "~" + $(this).data("id"));
                //respuestas.push($("input[name='opt_respuesta" + $(this).data("id") + "']:checked").val())
            });
            var data = { preguntas: preguntas, form: form, fk_oferta_laboral: id_oferta_laboral };
            responseSimple({
                url: "Postulante/PostulantePostularJson",
                data: JSON.stringify(data),
                callBackSuccess: function (response) {
                    refresh(true);
                }
            });

            //console.log(preguntas);
            //console.log(respuestas);
        });
        /*Fin de Evento de Boton*/
       
    };

    var _componentes = function () {
        /*Favoritos*/
        $(document).on('click', ".favoritos", function () {
            var ola_id = $(this).data('id');
            var dataForm = {
                ola_id: ola_id,
            };
            var clase_fa = $(this).find("span");
            if (clase_fa.hasClass("fa-star-o")) {
                responseSimple({
                    url: 'PostulanteFavoritos/PostulanteFavoritosInsertaJson',
                    refresh: false,
                    data: JSON.stringify(dataForm),
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            clase_fa.removeClass('fa-star-o');
                            clase_fa.addClass('fa-star');
                            refresh(true);
                        }
                    },
                })
            }
            else {
                responseSimple({
                    url: 'PostulanteFavoritos/PostulanteFavoritosEliminarJson',
                    refresh: false,
                    data: JSON.stringify(dataForm),
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            clase_fa.removeClass('fa-star');
                            clase_fa.addClass('fa-star-o');
                            refresh(true);
                        }
                    },
                })
            }

        });
        /*End Favoritos*/
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmOfertaLaboral',
            contenedor: '#frmOfertaLaboral-form',
            rules: {

            },
            messages: {

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
            _ListarOfertas();
            _metodos();
        },
        __ListarOfertas: function () {
            _ListarOfertas();
        },
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    OfertaLaboralMisPostulacionesVista.init();
});