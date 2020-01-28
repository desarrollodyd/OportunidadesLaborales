
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
                var menu_id_ = 0;
                $.each(data, function (index, value) {
                    activo = (index == 0) ? "in active" : "";
                    
                    li += '<li class="' + activo + '" data-id="' + value.menu_id +'">' +
                        '<a data-toggle="tab" href="#_tab_contenido_' + value.menu_id + '"> ' +
                                '<i class="red ace-icon fa fa-tachometer bigger-110"></i> '+
                                value.menu_titulo+
                            '</a >' +
                        '</li >';
                    
                    if (index == 0) {
                        var data_seccion = value.secciones;
                        var contentd = "";
                        var menu_id_ = value.menu_id;
                        $.each(data_seccion, function (index_, value_) {
                            var estadoText = (value_.sec_estado == "A") ? "Activo" : "Inactivo";
                            var estadoClase = (value_.sec_estado == "A") ? "success" : "danger";
                            var estadoActivo = (value_.sec_estado == "A") ? "selected" : "";
                            var estadoInActivo = (value_.sec_estado == "I") ? "selected" : "";
                            contentd += '<div class="panel panel-default">'+
                                            '<div class="panel-heading">'+
                                                '<h4 class="panel-title">'+
                                                        '<a class="accordion-toggle cabecera-seccion collapsed" data-id="' + value_.sec_id +'" data-toggle="collapse" data-parent="accordion_' + value.menu_id + '" href="#collapse_' + value_.sec_id + '">'+
                                                        '<i class="ace-icon fa fa-angle-right bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>'+
                                (index_ +1)+ '.- Seccion ID_' + value_.sec_id + ' <div class="widget-toolbar" style="margin-top: -7px;line-height: 24px;"><span id="span_estado_' + value_.sec_id + '" class="label label-' + estadoClase+' label-white middle">' + estadoText+'</span></div>'+
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
                                '<select data-id="' + value_.sec_id + '" data-menu_id="' + value.menu_id + '" class="form-control input-sm select-estado-seccion"><option value="A" ' + estadoActivo + '>Activo</option><option value="I" ' + estadoInActivo +'>Inactivo</option></select>' +
                                                                    '</div>' +
                                                                '</div>' +
                                                                '<div class="col-md-10"><div class="row">' +
                                '<div class="col-md-2 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-danger btn-sm btn-block btn-round btn-eliminar-seccion" data-id="' + value_.sec_id + '" data-menu_id="' + value.menu_id + '" data-rel="tooltip" title="Eliminar Seccion"><i class="ace-icon fa fa-trash"></i> Eliminar</button></div>'+
                                '<div class="col-md-3 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn-nuevo-elemento-seccion" data-id="' + value_.sec_id +'" data-rel="tooltip" title="Nuevo Elemento"><i class="ace-icon fa fa-file"></i> Nuevo Elemento </button></div>' +
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
                    div += '<div id="_tab_contenido_' + value.menu_id + '" class="tab-pane ' + activo + '">' +
                        '<div class="row" style="margin-bottom:8px">' +
                        '<div class="col-md-2 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nueva_seccion" data-id="' + value.menu_id + '" data-rel="tooltip" title="Nueva Seccion"><i class="ace-icon fa fa-file"></i> Nueva Seccion </button></div>' +
                        '</div>' +
                                contenidoDiv +
                        '</div>';
                    
                });
                $("#tabmenu").html(li);
                $("#tabcontenido").html(div);
                $('#accordion_' + menu_id_+' div.panel-default').sortable();
            }
        });
    };

    var _ListarSecciones = function (menu_id) {
        $("#_tab_contenido_" + menu_id).html("");
        var dataForm = {
            menu_id: menu_id
        };
        responseSimple({
            url: "IntranetSeccion/IntranetSeccionListarTodoxMenuIDJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                var data = response.data;
                var contentd = "";
                $.each(data, function (index, value_) {
                    var estadoText = (value_.sec_estado == "A") ? "Activo" : "Inactivo";
                    var estadoClase = (value_.sec_estado == "A") ? "success" : "danger";
                    var estadoActivo = (value_.sec_estado == "A") ? "selected" : "";
                    var estadoInActivo = (value_.sec_estado == "I") ? "selected" : "";
                    contentd += '<div class="panel panel-default lista_orden_' + menu_id +'">' +
                        '<div class="panel-heading">' +
                        '<h4 class="panel-title">' +
                        '<a class="accordion-toggle cabecera-seccion collapsed" data-id="' + value_.sec_id + '" data-toggle="collapse" data-parent="accordion_' + menu_id + '" href="#collapse_' + value_.sec_id + '">' +
                        '<i class="ace-icon fa fa-angle-right bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>' +
                        (index + 1) + '.- Seccion ID_' + value_.sec_id + ' <div class="widget-toolbar" style="margin-top: -7px;line-height: 24px;"><span id="span_estado_' + value_.sec_id + '" class="label label-' + estadoClase + ' label-white middle">' + estadoText + '</span></div>' +
                        '</a>' +
                        '</h4>' +
                        '</div >' +

                        '<div class="panel-collapse collapse " id="collapse_' + value_.sec_id + '">' +
                        '<div class="panel-body" id="menu_panel' + value_.sec_id + '">' +
                        '<div class="row">' +
                        '<div class="col-md-12"><div class="row">' +
                        '<div class="col-md-2">' +
                        '<div class="input-group">' +
                        '<span class="input-group-addon">' +
                        'Estado' +
                        '</span>' +
                        '<select data-id="' + value_.sec_id + '" data-menu_id="' + menu_id + '" class="form-control input-sm select-estado-seccion"><option value="A" ' + estadoActivo + '>Activo</option><option value="I" ' + estadoInActivo+'>Inactivo</option></select>' +
                        '</div>' +
                        '</div>' +
                        '<div class="col-md-10"><div class="row">' +
                        '<div class="col-md-2 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-danger btn-sm btn-block btn-round btn-eliminar-seccion" data-id="' + value_.sec_id + '" data-menu_id="' + menu_id + '" data-rel="tooltip" title="Eliminar Seccion"><i class="ace-icon fa fa-trash"></i> Eliminar</button></div>' +
                        '<div class="col-md-3 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn-nuevo-elemento-seccion" data-id="' + value_.sec_id + '" data-rel="tooltip" title="Nuevo Elemento"><i class="ace-icon fa fa-file"></i> Nuevo Elemento </button></div>' +
                        '</div></div>' +
                        '</div></div>' +
                        '<div class="col-md-12"><div class="hr hr8 hr-double"></div><div class="" id="seccion_lista_elemento_' + value_.sec_id + '"></div></div>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</div>';
                });
                contentd = '<div id="accordion_' + menu_id + '" class="accordion-style1 panel-group">' +
                    '<div class="row" style="margin-bottom:8px">' +
                    '<div class="col-md-2 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nueva_seccion" data-id="' + menu_id + '" data-rel="tooltip" title="Nueva Seccion"><i class="ace-icon fa fa-file"></i> Nueva Seccion </button></div>' +
                    '</div>' +
                    contentd + '</div>';

                $("#_tab_contenido_" + menu_id).html(contentd);

            }
        });
    };

    var _ListarElementos = function (fk_seccion_elemento) {
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
                    var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id=' + value.emod_id + '><i class="ace-icon fa fa-pencil"></i> </button>' +
                        ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id=' + value.emod_id + '><i class="ace-icon fa fa-trash"></i> </button>';

                    var clase_estado = 'success';
                    var estado = "Activo";
                    if (value.emod_estado == "I") {
                        clase_estado = 'danger';
                        estado = "Inactivo";
                    };

                    var detalle = '<div class="action-buttons">' +
                        '<a data-id="' + value.emod_id + '" href="javascript:void(0);" class="blue bigger-140 btn_detalle_elemento_modal show-details-btn" title = "Detalle">' +
                        '<i class="ace-icon fa fa-angle-double-up"></i>' +
                        '<span class="sr-only">Detalle</span>' +
                        '</a>' +
                        '</div>';
                    tr += '<tr data-id="' + value.emod_id + '" data-orden="' + value.emod_orden + '"><td class="center">' + detalle + '</td><td>' + value.tipo_nombre + '</td><td>' + value.emod_titulo + '</td><td><span class="label label-' + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                });

                var boton_nuevo = ' <div class="row" style="margin-bottom:10px;">' +
                    '<div class="col-md-3 col-sm-4 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_elemento_modal" data-seccion="' + fk_seccion_elemento + '" data-id="' + detalle_elemento_id + '" data-rel="tooltip" title="Nuevo Elemento"> <i class="fa fa-file"></i> Nuevo Elemento Modal</button></div>' +
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
    };

    var _componentes = function () {

        $(document).on('click', '#tabmenu li', function () {
            var menu_id = $(this).data("id");
            PanelContenido.init_ListarSecciones(menu_id);
        });

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
                                var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id=' + value.elem_id + '><i class="ace-icon fa fa-pencil"></i> </button>' +
                                    ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id=' + value.elem_id + '><i class="ace-icon fa fa-trash"></i> </button>';

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
                            if (rows.length == 0) {
                                tr = '<tr><td colspan="5"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tr>';
                            }
                            

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
                            var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id=' + value.detel_id + '><i class="ace-icon fa fa-pencil"></i> </button>' +
                                ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id=' + value.detel_id + '><i class="ace-icon fa fa-trash"></i> </button>';

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
                                '<a data-id="' + value.detel_id + '" data-seccion="' + value.fk_seccion_elemento+'" href="javascript:void(0);" class="blue bigger-140 btn_elemento_modal show-details-btn" title = "Detalle">' +
                                '<i class="ace-icon fa fa-angle-double-up"></i>' +
                                '<span class="sr-only">Detalle</span>' +
                                '</a>' +
                                '</div>';

                            tr += '<tr  data-id="' + value.detel_id + '" data-orden="' + value.detel_orden + '"><td class="center">' + detalle+'</td><td>' + value.detel_descripcion + '</td><td>' + value.detel_nombre + '.' + value.detel_extension + '</td><td>' + posicion + '</td><td><span class="label label-' + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                        });

                        var boton_nuevo = ' <div class="row" style="margin-bottom:10px;">' +
                            '<div class="col-md-3 col-sm-4 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_detalle_elemento" data-id="' + elemento_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento"> <i class="fa fa-file"></i> Nuevo Detalle Elemento</button></div>' +
                            '</div>';


                        tr = boton_nuevo +'<table class="table table-bordered table-condensed table-xs table-hover"><thead><tr><th style="width: 5%;"></th><th>Texto</th><th>Imagen</th><th style="width: 12%;">Ubicacion</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody>' + tr + '</tbody></table>';
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

        $(document).on('click', '.btn_elemento_modal', function (e) {
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

                PanelContenido.init_ListarElementos(fk_seccion_elemento);
            }
        });

        $(document).on('click', '.btn_detalle_elemento_modal', function (e) {
            e.preventDefault();
            var elemento_modal_id = $(this).data("id");
            var act_tr = $(this).closest("tr");
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
            if ($('#tr_elemento_contenido_detalle_modal' + elemento_modal_id).hasClass("open")) {
                $('#tr_elemento_contenido_detalle_modal' + elemento_modal_id).removeClass("open");
            }
            else {
                $('#tr_elemento_contenido_detalle_modal' + elemento_modal_id).remove();
                $('<tr id="tr_elemento_contenido_detalle_modal' + elemento_modal_id + '" class="detail-row open"><td colspan="5"><div class="alert alert-warning" style="margin-bottom:0px;">Cargando Data ...</div></td></tr>').insertAfter(act_tr);

                var dataForm = {
                    fk_elemento_modal: elemento_modal_id
                };
                responseSimple({
                    url: "IntranetDetalleElementoModal/IntranetDetalleElementoModalListarxElementoModalJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var rows = response.data;
                        var tr = "";
                        $.each(rows, function (index, value) {
                            var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento_modal" data-id=' + value.detelm_id + '><i class="ace-icon fa fa-pencil"></i> </button>' +
                                ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento_modal" data-id=' + value.detelm_id + '><i class="ace-icon fa fa-trash"></i> </button>';

                            var clase_estado = 'success';
                            var estado = "Activo";
                            if (value.detelm_estado == "I") {
                                clase_estado = 'danger';
                                estado = "Inactivo";
                            };
                            var posicion = '';
                            if (value.detelm_posicion == 'L') {
                                posicion = 'Izquierda';
                            }
                            else if (value.detelm_posicion == 'C') {
                                posicion = 'Centro';
                            }
                            else if (value.detelm_posicion == 'R') {
                                posicion = 'Derecha';
                            }
                            else {
                                posicion = '';
                            };


                            tr += '<tr  data-id="' + value.detelm_id + '" data-orden="' + value.detelm_orden + '"><td>' + value.detelm_descripcion + '</td><td>' + value.detelm_nombre + '.' + value.detelm_extension + '</td><td>' + posicion + '</td><td><span class="label label-' + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                        });

                        var boton_nuevo = ' <div class="row" style="margin-bottom:10px;">' +
                            '<div class="col-md-5 col-sm-4 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_detalle_elemento_modal" data-id="' + elemento_modal_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento Modal"> <i class="fa fa-file"></i> Nuevo Detalle Elemento Modal</button></div>' +
                            '</div>';


                        tr = boton_nuevo+'<table class="table table-bordered table-condensed table-xs table-hover"><thead><tr><th>Texto</th><th>Imagen</th><th style="width: 12%;">Ubicacion</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody>' + tr + '</tbody></table>';
                        if (rows.length > 0) {
                            $('#tr_elemento_contenido_detalle_modal' + elemento_modal_id).html('<td colspan="5" style="padding-left: 2%;"><div class="table-detail"><div class="rows">' + tr + '</div></div></td>');
                        }
                        else {
                            $('#tr_elemento_contenido_detalle_modal' + elemento_modal_id).html('<td colspan="5" style="padding-left: 2%;"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td>');
                        }

                    }
                });
            }
        });

        $(document).on('click', '.btn_nueva_seccion', function (e) {
            var menu_id = $(this).data("id");
            $("#fk_menu").val(menu_id);
            $("#sec_id").val(0);
            $("#modalFormularioSeccion").modal("show");
        });

        $(document).on('click', '.btn_guardar_seccion', function () {
            // $("#form_seccion").submit();
            var dataForm = $('#form_seccion').serializeFormJSON();
            var url = '';
            if ($("#sec_id").val() == 0) {
                url = 'IntranetSeccion/IntranetSeccionInsertarJson';
            }
            responseSimple({
                url: url,
                refresh: false,
                data: JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        var menu_id = $("#fk_menu").val();
                        PanelContenido.init_ListarSecciones(menu_id);
                    }
                    $("#modalFormularioSeccion").modal("hide");
                },
            })
        });

        $(document).on('input', '.input-number', function () {
            this.value = this.value.replace(/[^0-9]/g, '');
        });
        
        $(document).on('change', '.select-estado-seccion', function () {
            var seccion_id = $(this).data("id");
            var sec_estado = $(this).val();
            var menu_id = $(this).data("menu_id");
            var dataForm = {
                sec_id: seccion_id,
                sec_estado: sec_estado
            }
            responseSimple({
                url: "IntranetSeccion/IntranetSeccionEditarJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if (!response.respuesta) {
                        PanelContenido.init_ListarSecciones(menu_id);
                    }
                    else {
                        var texto = (sec_estado == "A") ? "Activo" : "Inactivo";
                        var clasetexto = (sec_estado == "A") ? "label label-success label-white middle" : "label label-danger label-white middle";
                        $("#span_estado_" + seccion_id).text(texto);
                        $("#span_estado_" + seccion_id).removeClass();
                        $("#span_estado_" + seccion_id).addClass(clasetexto);
                    }
                }
            });
        });

        $(document).on("click", ".btn-eliminar-seccion", function (e) {
            var seccion_id = $(this).data("id");
            var menu_id = $(this).data("menu_id");
            if (seccion_id != "" || seccion_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro que desea ELIMINAR esta Seccion?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetSeccion/IntranetSeccionEliminarJson",
                            data: JSON.stringify({ sec_id: seccion_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if (response.respuesta) {
                                    PanelContenido.init_ListarSecciones(menu_id);
                                }
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
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'form_seccion',
            contenedor: '#form_seccion',
            rules: {
                sec_estado:
                {
                    required: true,

                }
            },
            messages: {
                sec_estado:
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
        init_ListarElementos: function (seccion_id) {
            _ListarElementos(seccion_id);
        }
    }
}();
// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelContenido.init();
});