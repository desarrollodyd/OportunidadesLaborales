
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
                                var boton = '<a class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id=' + value.elem_id + ' href="javascript:void(0);"><i class="ace-icon fa fa-pencil"></i> Editar</a>' +
                                    ' <a class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id=' + value.elem_id + ' href="javascript:void(0);"><i class="ace-icon fa fa-trash"></i> Eliminar</a>';

                                var clase_estado = 'success';
                                var estado = "Activo";
                                if (value.elem_estado == "I") {
                                    clase_estado = 'danger';
                                    estado = "Inactivo";
                                };

                                var detalle = '<div class="action-buttons">'+
                                    '<a data-id="' + value.elem_id + '" href="javascript:void(0);" class="blue bigger-140 btn_detalle_elemento show-details-btn" title = "Detalle">'+
                                        '<i class="ace-icon fa fa-angle-double-up"></i>'+
                                        '<span class="sr-only">Detalle</span>'+
									'</a>'+
                                '</div>';
                                tr += '<tr data-id="' + value.elem_id + '" data-orden="' + value.elem_orden + '"><td class="center">' + detalle+'</td><td>' + value.tipo_nombre + '</td><td>' + value.elem_titulo + '</td><td><span class="label label-' + clase_estado + ' label-white middle">' + estado +'</span></td><td>' + boton + '</td></tr>';
                            });

                            

                            tr = '<table class="table table-bordered table-condensed table-hover"><thead><tr><th style="width: 5%;"></th><th style="width: 18%;">Tipo</th><th>Titulo</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody>'+tr+'</tbody></table>';
                            $("#seccion_lista_elemento_" + sec_id).html(tr);
                        }
                    })
                }
            }
        });

        $(document).on('click', '.btn_detalle_elemento', function (e) {
            e.preventDefault();
            var elemento_id = $(this).data("id");
            var act_tr = $(this).closest("tr");
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
            if ($('#tr_elemento_contenido_' + elemento_id).hasClass("open")) {
                $('#tr_elemento_contenido_' + elemento_id).removeClass("open");
            }
            else {
                $('#tr_elemento_contenido_' + elemento_id).remove();
                $('<tr id="tr_elemento_contenido_' + elemento_id + '" class="detail-row open"><td colspan="5"><div class="alert alert-warning" style="margin-bottom:0px;">Cargando Data ...</div></td></tr>').insertAfter(act_tr);

                var dataForm = {
                    elem_id: elemento_id
                };
                responseSimple({
                    url: "IntranetDetalleElemento/IntranetDetalleElementoListarxElementoIDJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var rows = response.data;
                        var tr = "";
                        $.each(rows, function (index, value) {
                            var boton = '<a class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id=' + value.detel_id + ' href="javascript:void(0);"><i class="ace-icon fa fa-pencil"></i> Editar</a>' +
                                ' <a class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id=' + value.detel_id + ' href="javascript:void(0);"><i class="ace-icon fa fa-trash"></i> Eliminar</a>';

                            var clase_estado = 'success';
                            var estado = "Activo";
                            if (value.detel_estado == "I") {
                                clase_estado = 'danger';
                                estado = "Inactivo";
                            };
                            var posicion = '';
                            if (value.detel_posicion == 'L') {
                                posicion = 'Izquierda';
                            }
                            else if (value.detel_posicion == 'C') {
                                posicion = 'Centro';
                            }
                            else if (value.detel_posicion == 'R') {
                                posicion = 'Derecha';
                            }
                            else {
                                posicion = '';
                            };

                            var detalle = '<div class="action-buttons">' +
                                '<a data-id="' + value.detel_id + '" data-seccion="' + value.fk_seccion_elemento+'" href="javascript:void(0);" class="blue bigger-140 btn_detalle_elemento_modal show-details-btn" title = "Detalle">' +
                                '<i class="ace-icon fa fa-angle-double-up"></i>' +
                                '<span class="sr-only">Detalle</span>' +
                                '</a>' +
                                '</div>';

                            tr += '<tr  data-id="' + value.detel_id + '" data-orden="' + value.detel_orden + '"><td class="center">' + detalle+'</td><td>' + value.detel_descripcion + '</td><td>' + value.detel_nombre + '.' + value.detel_extension + '</td><td>' + posicion + '</td><td><span class="label label-' + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                        });

                        var boton_nuevo = ' <div class="row" style="margin-bottom:10px;">' +
                            '<div class="col-md-2 col-sm-4 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_detalle_elemento" data-id="' + elemento_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento"> <i class="fa fa-file"></i> Nuevo</button></div>' +
                            '</div>';


                        tr = boton_nuevo +'<table class="table table-bordered table-condensed table-xs table-hover"><thead><tr><th style="width: 5%;"></th><th style="width: 18%;">Descripcion</th><th>Nombre</th><th style="width: 12%;">Ubicacion</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody>' + tr + '</tbody></table>';
                        if (rows.length > 0) {
                            $('#tr_elemento_contenido_' + elemento_id).html('<td colspan="5" style="padding-left: 2%;"><div class="table-detail"><div class="rows">' + tr + '</div></div></td>');
                        }
                        else {
                            $('#tr_elemento_contenido_' + elemento_id).html('<td colspan="5" style="padding-left: 2%;"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td>');
                        }
                        
                    }
                });
            }
        });

        $(document).on('click', '.btn_detalle_elemento_modal', function (e) {
            e.preventDefault();
            var detalle_elemento_id = $(this).data("id");
            var fk_seccion_elemento = $(this).data("seccion");
            var act_tr = $(this).closest("tr");
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
            if ($('#tr_elemento_contenido_modal_' + detalle_elemento_id).hasClass("open")) {
                $('#tr_elemento_contenido_modal_' + detalle_elemento_id).removeClass("open");
            }
            else {
                $('#tr_elemento_contenido_modal_' + detalle_elemento_id).remove();
                $('<tr id="tr_elemento_contenido_modal_' + detalle_elemento_id + '" class="detail-row open"><td colspan="5"><div class="alert alert-warning" style="margin-bottom:0px;">Cargando Data ...</div></td></tr>').insertAfter(act_tr);

                var dataForm = {
                    fk_seccion_elemento: fk_seccion_elemento
                };

                responseSimple({
                    url: "IntranetElementoModal/IntranetElementoModalListarxSeccionElementoJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var rows = response.data;
                        var tr = "";
                        $.each(rows, function (index, value) {
                            var boton = '<a class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id=' + value.emod_id + ' href="javascript:void(0);"><i class="ace-icon fa fa-pencil"></i> Editar</a>' +
                                ' <a class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id=' + value.emod_id + ' href="javascript:void(0);"><i class="ace-icon fa fa-trash"></i> Eliminar</a>';

                            var clase_estado = 'success';
                            var estado = "Activo";
                            if (value.emod_estado == "I") {
                                clase_estado = 'danger';
                                estado = "Inactivo";
                            };

                            var detalle = '<div class="action-buttons">' +
                                '<a data-id="' + value.emod_id + '" href="javascript:void(0);" class="blue bigger-140 btn_elemento_modal show-details-btn" title = "Detalle">' +
                                '<i class="ace-icon fa fa-angle-double-up"></i>' +
                                '<span class="sr-only">Detalle</span>' +
                                '</a>' +
                                '</div>';
                            tr += '<tr data-id="' + value.emod_id + '" data-orden="' + value.emod_orden + '"><td class="center">' + detalle + '</td><td>' + value.tipo_nombre + '</td><td>' + value.emod_titulo + '</td><td><span class="label label-' + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                        });

                        var boton_nuevo = ' <div class="row" style="margin-bottom:10px;">' +
                            '<div class="col-md-2 col-sm-4 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_elemento_modal" data-seccion="' + fk_seccion_elemento+'" data-id="' + detalle_elemento_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento"> <i class="fa fa-file"></i> Nuevo</button></div>' +
                            '</div>';

                        tr = boton_nuevo + '<table class="table table-bordered table-condensed table-hover"><thead><tr><th style="width: 5%;"></th><th style="width: 18%;">Tipo</th><th>Titulo</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody>' + tr + '</tbody></table>';
                        if (rows.length > 0) {
                            $('#tr_elemento_contenido_modal_' + detalle_elemento_id).html('<td colspan="6" style="padding-left: 2%;"><div class="table-detail"><div class="rows">' + tr + '</div></div></td>');
                        }
                        else {
                            $('#tr_elemento_contenido_modal_' + detalle_elemento_id).html('<td colspan="6" style="padding-left: 2%;"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td>');
                        }
                    }
                });
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