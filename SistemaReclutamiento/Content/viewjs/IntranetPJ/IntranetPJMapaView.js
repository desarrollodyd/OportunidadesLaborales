var MapaVista = function () {
    var _crearMapa = function (puntos) {
        $("#map").html("");
        var centro = (puntos.length > 0) ?
            { longitud: puntos[0].loc_longitud, latitud: puntos[0].loc_latitud }
            :
            { longitud: -77.0282400, latitud: -12.0431800 };
        //El centro sera el primer elemento del array, en caso este vacio sera un punto de LIMA
        var map = new ol.Map({
            target: 'map',
            layers: [
                new ol.layer.Tile({
                    source: new ol.source.OSM()
                })
            ],
            view: new ol.View({
                projection: "EPSG:4326",
                //center: [parseFloat(response.data[0].loc_longitud), parseFloat(response.data[0].loc_latitud)],
                center: [centro.longitud, centro.latitud],
                zoom: 14,
                minzoom: 1,
                maxzoom: 18
            })
        });
        var washingtonWebMercator;
        if (puntos.length > 0) {
            $.each(puntos, function (index, value) {
                washingtonWebMercator = [parseFloat(value.loc_longitud), parseFloat(value.loc_latitud)];
                $("#seccion-mapa").append('<div id="marker' + index + '" title="Marker"><img src="/Content/intranet/images/png/marker.png" /></div>');
                $("#seccion-mapa").append('<div class="overlay" id="tittle' + index + '"><span class="">' + value.loc_nombre + '</span></div>');
                var marker2 = new ol.Overlay({
                    position: washingtonWebMercator,
                    positioning: 'center-center',
                    element: document.getElementById('marker' + index),
                    stopEvent: false
                });
                map.addOverlay(marker2);
                var tittle = new ol.Overlay({
                    position: washingtonWebMercator,
                    element: document.getElementById('tittle' + index)
                });
                map.addOverlay(tittle);
            })
        }
    
    };
    var _inicio = function () {
        $("#tipo").val(tipo);
        responseSimple({
            url:"IntranetPJ/ListarLocalesporTipoJson",
            data:JSON.stringify({
                tipo: tipo
            }),
            refresh: false,
            callBackSuccess: function (response) {
                $("#resultados").html("");
                var puntos = response.data;
                _crearMapa(puntos);
                if (puntos.length > 0) {
                    var span = '';
                    $.each(puntos, function (index, value) {
                        span += '<li><h5>' + value.loc_nombre + '</h5><ul><li>' + value.loc_direccion + '</li><li>' + value.ubi_nombre + '</li></ul></li><br />';
                    })
                    $("#resultados").html(span);
                }
            }
        });
    };
    var _componentes = function () {
        $(document).on("click", ".btn-buscar", function () {
            //busquedaLocales
            $("#busquedaLocales").submit();
            if (_objetoForm_busquedaLocales.valid()) {
                var dataForm = $('#busquedaLocales').serializeFormJSON();
                responseSimple({
                    url: "IntranetPJ/ListarLocalesporTipoJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        $("#resultados").html("");
                        var puntos = response.data;
                        $("#map").html("");
                        _crearMapa(puntos);
                        if (puntos.length > 0) {
                            var span = '';
                            $.each(puntos, function (index, value) {
                                span += '<li><h5>' + value.loc_nombre + '</h5><ul><li>' + value.loc_direccion + '</li><li>' + value.ubi_nombre + '</li></ul></li><br />';
                            })
                            $("#resultados").html(span);
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
    };
    var _metodos = function () {
        validar_Form({
            nameVariable: 'busquedaLocales',
            contenedor: '#busquedaLocales',
            rules: {
                nombre:
                {
                    required: true,

                }
            },
            messages: {
                nombre:
                {
                    required: 'Campo Obligatorio',
                }
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
        }
    }
}();
// Initialize module
// ------------------------------
document.addEventListener('DOMContentLoaded', function () {
    MapaVista.init();
});