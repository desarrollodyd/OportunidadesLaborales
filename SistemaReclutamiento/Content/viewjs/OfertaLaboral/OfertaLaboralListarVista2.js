var OfertaLaboralVista = function () {

    var _inicio = function () {
     
        $('#cbocodCargo').select2();
        $('#cboDepartamento').select2();
        $('#cboProvincia').select2();
        $('#cboDistrito').select2();
        $('#cborangoFecha').select2();
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);
        selectResponse({
            url: "Ubigeo/UbigeoListarPaisPeruJson",
            select: "cboPais",
            campoID: "ubi_pais_id",
            CampoValor: "ubi_nombre",
            select2: true,
            allOption: false,
            placeholder: "PAIS"
        });
    
    };
    var _ActivarTextBox = function () {
        $('#frm-Postular input').on('change', function () {
            var dato = $(this).data("id");
            if ($("input.texto"+dato+":radio").is(':checked')) {
                $("input[name=opt_respuestalabel" + dato + "]").attr('disabled', false);
            }
            else {
                $("input[name=opt_respuestalabel" + dato + "]").attr('disabled', true);
            }
        });
      
    };
    var _ListarOfertas = function () {
        var dataForm = $('#frmOfertaLaboral-form').serializeFormJSON();
        responseSimple({
            url: "OfertaLaboral/OfertaLaboralIndexListarJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                CloseMessages();
                var data = response.data;
                var respuesta = response.respuesta;
                $("#ofertasContenido>.row").html("");
                if (respuesta) {
                    $.each(data, function (index, value) {
                        var span = '';
                        if (value.es_favorito) {
                            span += 'fa fa-star';
                        }
                        else {
                            span += 'fa fa-star-o';
                        }
                        $("#ofertasContenido>.row").append('<div class=col-md-4 col-sm-4 col-xs-12"><div class="profile_details">'+
                                                            '<div class="well profile_view">'+
                                                               '<div class="col-md-12 col-sm-12 col-xs-12" style="text-align: center;">'+
                                                                    '<h3 class="brief" style="margin: 0px !important;"><i>'+value.ola_nombre+'</i></h3>'+
                                                                    '<h6 style="margin: 0px !important;">dirección</h6>'+
                                                                    '<div class=" col-xs-12" style="padding-bottom: 10px;">'+
                            '<div class="ln_solid" style="margin-top: 10px !important;"></div>'
                            + '<button type="button" class="btn btn-primary btn_detalle" style="font-size:15px !important" data-toggle="modal" data-target=".bs-example-modal-detalle" data-id=' + value.ola_id + '>Detalle</button>' +
                            '<button type="button" id=postular' + value.ola_id + ' data-id=' + value.ola_id + ' class="btn btn-success btn-sm btn_postular" style="font-size: 15px !important;" data-toggle="modal" data-target=".bs-example-modal-postular"> Postula</button>' +
                                                                    '</div>'+
                                                                '</div>'+
                                                                '<div class="col-xs-12 bottom text-center">'+
                                                                     '<div class="cold-md-12 col-xs-12 col-sm-12 emphasis">'+
                                                                          '<p class="ratings" style="text-align: center;">'+
                            '<a>Publicado el ' + moment(value.ola_fecha_pub).format('DD/MM/YYYY') + '</a>' +
                            '<a class="favoritos" data-id="' + value.ola_id + '" href="#" style="float: right;"><span class="' + span + '" style="color:#fff000"></span></a>' +
                                                                          '</p>'+
                                                                     '</div>'+
                                                                '</div>'+
                                                            '</div>'+
                                                      '</div></div>');
                    });
                    if (data.length == 0) {
                        CloseMessages();
                        messageResponse({
                            text: "No se Encontraron Ofertas",
                            type: "warning"
                        });
                    }
                }
            }
        });
        /*Boton Postularme de Modal Postular*/
        $(document).on("click", ".btn_postularme", function (e) {
            var id_oferta_laboral = $(this).data("id");
            var elementoPregunta = $("#frm-Postular>.form-group>label");
            var pregunta='';
            var respuesta='';
            arrayPreguntas=[];
            if(elementoPregunta.length>0){
                $(elementoPregunta).each(function () {
                    
                    var dro_respuesta='';
                    var dro_orden='';
                    var dro_estado='';
                    var dro_calificacion='';
                    var dro_respuesta='';
                    var dro_tipo='';
                    var pregunta_id='';
                    pregunta=$(this).text();
                    pregunta_id=$(this).data("id");

                    if ($("input.texto"+pregunta_id+":radio").is(':checked')) {
                        respuesta=$("input[name=opt_respuestalabel"+pregunta_id+"]");
                    }
                    else {
                        if($("input.texto2"+pregunta_id+":radio").is(':checked')){
                            respuesta=$("input[name=opt_respuesta"+pregunta_id+"]:checked");
                        }
                        else{
                            messageResponse({
                                text: "Debe responder todas las preguntas",
                                type: "warning"
                            });
                            return false;
                        }
                    }
                    dro_respuesta=respuesta.val();
                    dro_orden=respuesta.data('orden');
                    dro_estado=respuesta.data('estado');
                    dro_calificacion=respuesta.data('calificacion');
                    dro_tipo=respuesta.data('tipo');

                    var objPregunta={
                        dop_id:pregunta_id,
                        dop_pregunta:pregunta,
                        respuesta:{
                            dro_orden:dro_orden,
                            dro_respuesta:dro_respuesta,
                            dro_estado:dro_estado,
                            dro_calificacion:dro_calificacion,
                            dro_tipo:dro_tipo
                        }
                    }
                    arrayPreguntas.push(objPregunta);
                });
            }
            var data = { arrayPreguntas: arrayPreguntas, fk_oferta_laboral: id_oferta_laboral };
            responseSimple({
                    url: "Postulante/PostulantePostularJson2",
                    data: JSON.stringify(data),
                    refresh:false,
                    callBackSuccess: function (response) {
                        if(response.response){
                            refresh(true);
                        }
                    }
            });
        });
        /*Fin de Evento de Boton*/
        /*Modal Preguntas Prefitro*/
        $(document).on("click", ".btn_postular", function (e) {
            $(".btn_postularme").attr('data-id', $(this).data("id"));
            var data = { ola_id: $(this).data("id") };
            responseSimple({
                url: "OfertaLaboral/DetPreguntaOLAListarJson",
                data: JSON.stringify(data),
                refresh: false,
                callBackSuccess: function (response) {
                    CloseMessages();
                    console.log(response);
                    var oferta = response.oferta;
                    var listaPreguntas = response.data;
                    var anexar = $("#frm-Postular");
                    $(anexar).html("");
                    $.each(listaPreguntas, function (index, pregunta) {
                        var listaRespuestas = pregunta.DetalleRespuesta;
                        var tituloPregunta = "";
                        tituloPregunta += '<div class="form-group"><label data-id=' + pregunta.dop_id + '>' + pregunta.dop_pregunta + '</label><div class="pregunta' + pregunta.dop_id + '"></div></div>';
                        $(anexar).append(tituloPregunta);
                        $.each(listaRespuestas, function (i, respuesta) {
                            var tituloRespuesta = "";
                            if (respuesta.dro_respuesta == "") {
                                tituloRespuesta += '<div class="radio"><label><input name="opt_respuesta' + pregunta.dop_id + '" data-id="' + pregunta.dop_id + '" class="texto' + pregunta.dop_id + '" type="radio" value="" /> Otra Respuesta:</label> <input class="form-control" type="text" placeholder="Respuesta" disabled="true" name="opt_respuestalabel' + pregunta.dop_id + '" data-calificacion='+respuesta.dro_calificacion+' data-tipo='+respuesta.dro_tipo+' data-orden='+respuesta.dro_orden+' data-estado='+respuesta.dro_estado+' /></div>';
                            }
                            else {
                                tituloRespuesta += '<div class="radio"><label><input class="texto2'+pregunta.dop_id+'" data-id="' + pregunta.dop_id + '" value="' + respuesta.dro_respuesta + '" name="opt_respuesta' + pregunta.dop_id + '" type="radio" data-calificacion='+respuesta.dro_calificacion+' data-tipo='+respuesta.dro_tipo+' data-orden='+respuesta.dro_orden+' data-estado='+respuesta.dro_estado+' />' + respuesta.dro_respuesta + '</label></div>';
                            }
                            $(".pregunta" + respuesta.fk_det_pregunta_of).append(tituloRespuesta);
                        });
                    });
                    _ActivarTextBox();
                }
            });
        });
        /*Fin de Modal*/
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
       

    };
    var _componentes = function () {
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
                        }
                    },
                })
            }

        });
        $(document).on("click", ".btn_filtrar", function (e) {
            var dataForm = $('#frmOfertaLaboral-form').serializeFormJSON();
            responseSimple({
                url: "OfertaLaboral/OfertaLaboralListarJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    CloseMessages();
                    var data = response.data;
                    var respuesta = response.respuesta;
                    $("#ofertasContenido>.row").html("");
                    if (respuesta) {
                        $.each(data, function (index, value) {
                            var span = '';
                            if (value.es_favorito) {
                                span += 'fa fa-star';
                            }
                            else {
                                span += 'fa fa-star-o';
                            }
                            $("#ofertasContenido>.row").append('<div class=col-md-4 col-sm-4 col-xs-12"><div class="profile_details">' +
                                '<div class="well profile_view">' +
                                '<div class="col-md-12 col-sm-12 col-xs-12" style="text-align: center;">' +
                                '<h3 class="brief" style="margin: 0px !important;"><i>' + value.ola_nombre + '</i></h3>' +
                                '<h6 style="margin: 0px !important;">dirección</h6>' +
                                '<div class=" col-xs-12" style="padding-bottom: 10px;">' +
                                '<div class="ln_solid" style="margin-top: 10px !important;"></div>'
                                + '<button type="button" class="btn btn-primary btn_detalle" style="font-size:15px !important" data-toggle="modal" data-target=".bs-example-modal-detalle" data-id=' + value.ola_id + '>Detalle</button>' +
                                '<button type="button" id=postular' + value.ola_id + ' data-id=' + value.ola_id + ' class="btn btn-success btn-sm btn_postular" style="font-size: 15px !important;" data-toggle="modal" data-target=".bs-example-modal-postular"> Postula</button>' +
                                '</div>' +
                                '</div>' +
                                '<div class="col-xs-12 bottom text-center">' +
                                '<div class="cold-md-12 col-xs-12 col-sm-12 emphasis">' +
                                '<p class="ratings" style="text-align: center;">' +
                                '<a>Publicado el ' + moment(value.ola_fecha_pub).format('DD/MM/YYYY') + '</a>' +
                                '<a  class="favoritos" data-id="' + value.ola_id + '"href="#" style="float: right;"><span class="' + span + '" style="color:#fff000"></span></a>' +
                                '</p>' +
                                '</div>' +
                                '</div>' +
                                '</div>' +
                                '</div></div>');
                        });
                        if (data.length == 0) {
                            CloseMessages();
                            messageResponse({
                                text: "No se Encontraron Ofertas",
                                type: "warning"
                            });
                        }
                    }
                }
            });
        });

        $("#cboPais").change(function () {
            var ubi_id_pais = $("#cboPais option:selected").val();
            selectResponse({
                url: "Ubigeo/UbigeoListarDepartamentosporPaisJson",
                select: "cboDepartamento",
                data: { ubi_pais_id: ubi_id_pais },
                campoID: "ubi_departamento_id",
                CampoValor: "ubi_nombre",
                select2: true,
                allOption: false,
                placeholder:"DEPARTAMENTO"
            });

            $("#cboProvincia").html('<option value="">PROVINCIA</option>');
            if ($('#cboProvincia').hasClass('select2-hidden-accessible')) {
                $('#cboProvincia').select2('destroy');
            }
            $("#cboDistrito").html('<option value="">DISTRITO</option>');
            if ($('#cboDistrito').hasClass('select2-hidden-accessible')) {
                $('#cboDistrito').select2('destroy');
            }
            if (ubi_id_pais != "") {
                $("#cboPais").rules('remove', 'required');
                $("#cboDepartamento").rules('remove', 'required');
                $("#cboProvincia").rules('remove', 'required');
                $("#cboDistrito").rules('remove', 'required');
            }
          
        });

        $("#cboDepartamento").change(function () {
            var ubi_pais_id = $("#cboPais option:selected").val();
            var ubi_departamento_id = $("#cboDepartamento option:selected").val();
            selectResponse({
                url: "Ubigeo/UbigeoListarProvinciasporDepartamentoJson",
                select: "cboProvincia",
                data: { ubi_pais_id: ubi_pais_id, ubi_departamento_id: ubi_departamento_id },
                campoID: "ubi_provincia_id",
                CampoValor: "ubi_nombre",
                select2: true,
                allOption: false,
                placeholder: "PROVINCIA"
            });
            $("#cboDistrito").html('<option value="">DISTRITO</option>');
            if ($('#cboDistrito').hasClass('select2-hidden-accessible')) {
                $('#cboDistrito').select2('destroy');
            }
            if (ubi_departamento_id != "") {
                $("#cboPais").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboDepartamento").rules('remove', 'required');
                $("#cboProvincia").rules('remove', 'required');
                $("#cboDistrito").rules('remove', 'required');
            }
        });

        $("#cboProvincia").change(function () {
            var ubi_pais_id = $("#cboPais option:selected").val();
            var ubi_departamento_id = $("#cboDepartamento option:selected").val();
            var ubi_provincia_id = $("#cboProvincia option:selected").val();

            selectResponse({
                url: "Ubigeo/UbigeoListarDistritosporProvinciaJson",
                select: "cboDistrito",
                data: { ubi_pais_id: ubi_pais_id, ubi_departamento_id: ubi_departamento_id, ubi_provincia_id: ubi_provincia_id },
                campoID: "ubi_distrito_id",
                CampoValor: "ubi_nombre",
                select2: true,
                allOption: false,
                placeholder: "DISTRITO"
            });
            if (ubi_provincia_id != "") {
                $("#cboPais").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboDepartamento").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboProvincia").rules('remove', 'required');
                $("#cboDistrito").rules('remove', 'required');
            }
          
        });

        $("#cboDistrito").change(function () {
            var ubi_distrito_id = $("#cboDistrito option:selected").val();
            if (ubi_distrito_id != "") {
                $("#cboPais").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboDepartamento").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboProvincia").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboDistrito").rules('remove', 'required');
            }
        });
        $(document).on("click", ".btn_cancelar", function (e) {
            $("#ola_nombre").val("");
            $("#cborangoFecha").val("");
            selectResponse({
                url: "Ubigeo/UbigeoListarPaisPeruJson",
                select: "cboPais",
                campoID: "ubi_pais_id",
                CampoValor: "ubi_nombre",
                select2: true,
                allOption: false,
                placeholder: "PAIS"
            });
            $("#cboDepartamento").html('<option value="">DEPARTAMENTO</option>');
            if ($('#cboDepartamento').hasClass('select2-hidden-accessible')) {
                $('#cboDepartamento').select2('destroy');
            }
            $("#cboProvincia").html('<option value="">PROVINCIA</option>');
            if ($('#cboProvincia').hasClass('select2-hidden-accessible')) {
                $('#cboProvincia').select2('destroy');
            }
            $("#cboDistrito").html('<option value="">DISTRITO</option>');
            if ($('#cboDistrito').hasClass('select2-hidden-accessible')) {
                $('#cboDistrito').select2('destroy');
            }
        });
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
    OfertaLaboralVista.init();
});