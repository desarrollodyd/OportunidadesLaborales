
var PanelContenido = function () {

    var _ListarMenus = function () {
        $("#tabmenu").html("");
        $("#tabcontenido").html("");
        responseSimple({
            url: "IntranetMenu/IntranetMenuSeccionListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                var data = response.data;
                var li = "";
                var div = "";
                var activo = "";
                var contenidoDiv = "";
                $.each(data, function (index, value) {
                    activo = (index == 0) ? "in active" : "";
                    
                    li += '<li class="' + activo + '" data-id="' + value.menu_id +'">' +
                        '<a data-toggle="tab" href="#_tab_contenido_' + value.menu_id + '"> ' +
                                '<i class="pink ace-icon fa fa-tachometer bigger-110"></i> '+
                                value.menu_titulo+
                            '</a >' +
                        '</li >';
                    
                    if (index == 0) {
                        var data_seccion = value.secciones;
                        var contentd = "";
                        $.each(data_seccion, function (index_, value_) {
                            contentd += '<div class="panel panel-default">'+
                                            '<div class="panel-heading">'+
                                                '<h4 class="panel-title">'+
                                                        '<a class="accordion-toggle cabecera-seccion" data-id="' + value_.sec_id +'" data-toggle="collapse" data-parent="accordion_' + value.menu_id + '" href="#collapse_' + value_.sec_id + '">'+
                                                        '<i class="ace-icon fa fa-angle-down bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>'+
                                'Seccion #' + value_.sec_orden +' <div class="widget-toolbar" style="margin-top: -7px;line-height: 24px;"><span class="label label-success label-white middle">Activo</span></div>'+
									                '</a>'+
                                                '</h4>'+
								            '</div >'+

                                            '<div class="panel-collapse collapse " id="collapse_' + value_.sec_id + '">' +
                                               '<div class="panel-body" id="menu_panel' + value_.sec_id + '">' +
                                                    '<div class="row">'+
                                                        '<div class="col-md-12"><div class="row">' +
                                                                '<div class="col-md-2">' +
                                                                    '<div class="input-group">' +
                                                                        '<span class="input-group-addon">' +
                                                                        'Estado' +
                                                                        '</span>' +
                                                                        '<select data-id="' + value_.sec_id +'" class="form-control input-sm select-estado-seccion"><option values="A">Activo</option><option values="I">Inactivo</option></select>' +
                                                                    '</div>' +
                                                                '</div>' +
                                                                '<div class="col-md-10"><div class="row">' +
                                                                    '<div class="col-md-2 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-danger btn-sm btn-block btn-round btn-eliminar-seccion" data-id="' + value_.sec_id +'" data-rel="tooltip" title="Eliminar Seccion">Eliminar</button></div>'+
                                                                    '<div class="col-md-3 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn-nuevo-elemento-seccion" data-id="' + value_.sec_id +'" data-rel="tooltip" title="Nuevo Elemento">Nuevo Elemento</button></div>' +
                                                                '</div></div>' +
                                                        '</div></div>' +
                                                        '<div class="col-md-12"><div class="hr hr8 hr-double"></div><div class="" id="seccion_lista_elemento_' + value_.sec_id+'"></div></div>' +
                                                    '</div>' +
                                                '</div>' +
                                            '</div>'+
							            '</div>';
                        });
                        contenidoDiv += '<div id="accordion_' + value.menu_id + '" class="accordion-style1 panel-group">' + contentd + '</div>';
                    }
                    else {
                        contenidoDiv = "";
                    }
                    div += '<div id="_tab_contenido_' + value.menu_id + '" class="tab-pane ' + activo + '">' + contenidoDiv+
                            '</div>';
                });
                $("#tabmenu").html(li);
                $("#tabcontenido").html(div);
                
            }
        });
    };


    var _componentes = function () {
        $(document).on('click', '.cabecera-seccion', function () {
            var sec_id = $(this).data("id");
            if (!$(this).hasClass("collapsed")) {
                if (sec_id > 0 || sec_id != "") {
                    var dataForm = {
                        sec_id: sec_id
                    };
                    $("#seccion_lista_elemento_" + sec_id).html("");
                    responseSimple({
                        url: "IntranetElemento/IntranetElementoListarxSeccionIDJson",
                        data: JSON.stringify(dataForm),
                        refresh: false,
                        callBackSuccess: function (response) {
                            var rows = response.data;
                            var tr = "";
                            $.each(rows, function (index, value) {
                                tr += '<tr data-id="' + value.elem_id + '"><td>' + value.elem_orden + '</td><td>' + value.tipo_nombre + '</td><td>' + value.elem_titulo + '</td><td>' + value.elem_id + '</td></tr>';
                            });
                            tr = '<table class="table table-bordered table-condensed table-hover"><thead><tr><th style="width: 6%;">Orden</th><th style="width: 18%;">Tipo</th><th>Titulo</th><th>Acciones</th></tr></thead><tbody>'+tr+'</tbody></table>';
                            $("#seccion_lista_elemento_" + sec_id).html(tr);
                        }
                    })
                }
            }
            
        });
       

    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'form_menus',
            contenedor: '#form_menus',
            rules: {
                menu_titulo:
                {
                    required: true,

                },
                menu_url:
                {
                    required: true,

                }
            },
            messages: {
                menu_titulo:
                {
                    required: 'Campo Obligatorio',
                },
                menu_url:
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
            _ListarMenus();
            _componentes();
//_metodos();

        },
        init_ListarMenus: function () {
            _ListarMenus();
        },
        init_ListarSecciones: function (menu_id) {
            _ListarSecciones(menu_id);
        },
    }
}();
// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelContenido.init();
});