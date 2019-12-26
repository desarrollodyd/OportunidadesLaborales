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
                        var span = '';
                        if (value.es_favorito) {
                            span += 'fa fa-star';
                        }
                        else {
                            span += 'fa fa-star-o';
                        }
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
                            '<a>Publicado el' + moment(value.ola_fecha_pub).format('DD/MM/YYYY') + '</a>' +
                            '<a href="#" style="float: right;"><span class="' + span + '" style="color:#fff000"></span></a>' +
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