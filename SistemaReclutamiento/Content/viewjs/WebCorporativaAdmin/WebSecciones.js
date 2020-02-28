var PanelContenido=function(){
    var _ListarMenus = function () {
        $("#tabmenu").html("");
        $("#tabcontenido").html("");
        responseSimple({
            url: "WebMenu/WebMenuListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
                var data = response.data;
                var li = "";
                var div = "";
                var activo = "";
                var contenidoDiv = "";
                var menu_id_ = 0;
                $.each(data, function (index, value) {
                    activo = (index == 0) ? "in active" : "";
                    
                    li += '<li class="' + activo + '" data-id="' + value.menu_id +'">' +
                        '<a data-toggle="tab" href="#_tab_contenido_' + value.menu_id + '"> ' +
                                '<i class="red ace-icon fa fa-tachometer bigger-110"></i> '+
                                value.menu_titulo+
                            '</a >' +
                        '</li >';
                        //crear tabla para elementos

                        //Fin de tabla
                    div += '<div id="_tab_contenido_' + value.menu_id + '" class="tab-pane ' + activo + '">' +
                        '<div class="row" style="margin-bottom:8px">' +
                        '<div class="col-md-2 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nueva_seccion" data-id="' + value.menu_id + '" data-rel="tooltip" title="Nueva Seccion"><i class="ace-icon fa fa-file"></i> Nueva Seccion </button></div>' +
                        '<div class="col-md-3 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-warning btn-sm btn-block btn-round btn_ordenar_seccion" data-id="' + value.menu_id + '" data-rel="tooltip" title="Ordenar Seccion(es)"><i class="ace-icon glyphicon glyphicon-list-alt"></i> Ordenar Seccion(es) </button></div>' +
                        '</div>' +
                                contenidoDiv +
                        '</div>';
                });
                $("#tabmenu").html(li);
                $("#tabcontenido").html(div);
                
            }
        });
    };
    return{
        init:function(){
            _ListarMenus();
        },
        init_ListarMenus:function(){
            _ListarMenus();
        }
    }
}();
document.addEventListener('DOMContentLoaded',function(){
    PanelContenido.init();
})