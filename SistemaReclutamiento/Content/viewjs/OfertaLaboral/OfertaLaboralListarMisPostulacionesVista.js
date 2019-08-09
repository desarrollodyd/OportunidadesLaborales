var OfertaLaboralMisPostulacionesVista = function () {

    var _inicio = function () {
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);
    };

    var _ListarOfertas = function () {
        var dataForm = $('#frmOfertaLaboral-form').serializeFormJSON();
        responseSimple({
            url: "OfertaLaboral/OfertaLaboralListarMisPostulacionesJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                var data = response.data;
                var respuesta = response.respuesta;
                $("#ofertasContenido").html("");
                if (respuesta) {
                    console.log(data);
                    $.each(data, function (index, value) {
                        $("#ofertasContenido").append('<div class="col-md-4 col-sm-4 col-xs-12 profile_details">' +
                            '<div class="well profile_view">' +
                            '<div class="col-md-12 col-sm-12 col-xs-12" style="text-align: center;">' +
                            '<h3 class="brief" style="margin: 0px !important;"><i>' + value.ola_nombre + '</i></h3>' +
                            '<h6 style="margin: 0px !important;">direccion</h6>' +
                            '<div class=" col-xs-12" style="padding-bottom: 10px;">' +
                            '<div class="ln_solid" style="margin-top: 10px !important;"></div>' +
                            '<button type="button" class="btn btn-primary btn-sm" style="font-size: 15px !important;">  Detalle </button>' +                          
                            '</div>' +
                            '</div>' +
                            '<div class="col-xs-12 bottom text-center">' +
                            '<div class="cold-md-12 col-xs-12 col-sm-12 emphasis">' +
                            '<p class="ratings" style="text-align: center;">' +
                            '<a>Publicado hace 4 dias</a>' +
                            '<a href="#" style="float: right;"><span class="fa fa-star-o"></span></a>' +
                            '</p>' +
                            '</div>' +
                            '</div>' +
                            '</div>' +
                            '</div >');
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
    };

    var _componentes = function () {

    };

    var _metodos = function () {
        
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