var OfertaLaboralMisPostulacionesVista = function () {

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
        //selectResponse({
        //    url: "SQL/TMEMPRListarJson",
        //    select: "cbocodEmpresa",
        //    campoID: "CO_EMPR",
        //    CampoValor: "DE_NOMB",
        //    select2: true,
        //    allOption: false,
        //    placeholder: "Seleccione Empresa"
        //});
    };

    var _ListarOfertas = function () {
        var dataForm = $('#frmOfertaLaboral-form').serializeFormJSON();
        responseSimple({
            url: "OfertaLaboral/OfertaLaboralListarMisPostulacionesJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                CloseMessages();
                console.log(response);
                var data = response.data;
                var respuesta = response.respuesta;
                $("#ofertasContenido").html("");
                if (respuesta) {
                    
                    $.each(data, function (index, value) {
                        $("#ofertasContenido").append('<div class="col-md-4 col-sm-4 col-xs-12"><div class="profile_details">' +
                            '<div class="well profile_view">' +
                            '<div class="col-md-12 col-sm-12 col-xs-12" style="text-align: center;">' +
                            '<h3 class="brief" style="margin: 0px !important;"><i>' + value.ola_nombre + '</i></h3>' +
                            '<h6 style="margin: 0px !important;">dirección</h6>' +
                            '<div class=" col-xs-12" style="padding-bottom: 10px;">' +
                            '<div class="ln_solid" style="margin-top: 10px !important;"></div>' +
                            '<button type="button" class="btn btn-primary btn-sm btn_detalle" style="font-size: 15px !important;" data-toggle="modal" data-target=".bs-example-modal-detalle" data-id="' + value.ola_id + '">  Detalle </button>' +                          
                            '</div>' +
                            '</div>' +
                            '<div class="col-xs-12 bottom text-center">' +
                            '<div class="cold-md-12 col-xs-12 col-sm-12 emphasis">' +
                            '<p class="ratings" style="text-align: center;">' +
                            '<a>Publicado el'+moment(value.ola_fecha_pub).format('DD/MM/YYYY')+'</a>' +
                            '<a href="#" style="float: right;"><span class="fa fa-star-o"></span></a>' +
                            '</p>' +
                            '</div>' +
                            '</div>' +
                            '</div>' +
                            '</div ></div>');
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
    };

    var _componentes = function () {
        $(document).on("click", ".btn_filtrar", function (e) {
            $("#frmOfertaLaboral-form").submit();
            if (_objetoForm_frmOfertaLaboral.valid()) {
                OfertaLaboralMisPostulacionesVista.__ListarOfertas();
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
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
                placeholder: "DEPARTAMENTO"
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
        //$("#cbocodEmpresa").change(function () {
        //    var CO_EMPR = $("#cbocodEmpresa option:selected").val();
        //    selectResponse({
        //        url: "SQL/TTPUES_TRABListarJson",
        //        select: "cbocodCargo",
        //        data: { CO_EMPR: CO_EMPR },
        //        campoID: "CO_PUES_TRAB",
        //        CampoValor: "DE_PUES_TRAB",
        //        select2: true,
        //        allOption: false,
        //        placeholder: "Seleccione Puesto"
        //    });
        //});
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

            //selectResponse({
            //    url: "SQL/TMEMPRListarJson",
            //    select: "cbocodEmpresa",
            //    campoID: "CO_EMPR",
            //    CampoValor: "DE_NOMB",
            //    select2: true,
            //    allOption: false,
            //    placeholder: "Seleccione Empresa"
            //});

            //$("#cbocodCargo").html('<option value="">Seleccione Cargo</option>');
            //if ($('#cbocodCargo').hasClass('select2-hidden-accessible')) {
            //    $('#cbocodCargo').select2('destroy');
            //}
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
    OfertaLaboralMisPostulacionesVista.init();
});