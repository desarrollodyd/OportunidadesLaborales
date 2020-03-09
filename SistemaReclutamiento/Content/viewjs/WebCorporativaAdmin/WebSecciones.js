var PanelContenido=function(){
    var _ListarMenus = function () {
        $("#tabmenu").html("");
        $("#tabcontenido").html("");
        responseSimple({
            url: "WebMenu/WebMenuListarJson",
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
                                '<i class="red ace-icon fa fa-tachometer bigger-110"></i> '+
                                value.menu_titulo+
                            '</a >' +
                        '</li >';
                        //crear tabla para elementos
                        if(index==0){
                            var rows=value.elemento;
                            var tr = "";
                            $.each(rows,function(indice,elemento){
                                
                                var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_elemento" data-id="' + elemento.elem_id + '" data-menu_id="'+elemento.fk_menu+'"><i class="ace-icon fa fa-pencil"></i> </button>' +
                                ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_elemento" data-id="' + elemento.elem_id + '" data-menu_id="'+elemento.fk_menu+'" ><i class="ace-icon fa fa-trash"></i> </button>';
                                var clase_estado = 'success';
                                var estado = "Activo";
                                if (elemento.elem_estado == "I") {
                                    clase_estado = 'danger';
                                    estado = "Inactivo";
                                };
                                var tipo = elemento.fk_tipo_elemento;
                                var clasedetalle = "blue";
                                var clasedetalleboton = "btn_detalle_elemento";
                                if (tipo == 1 || tipo == 2 || tipo == 3) {
                                    clasedetalle = "grey";
                                    clasedetalleboton = "";
                                }
                                var detalle = '<div class="action-buttons">' +
                                '<a data-id="' + elemento.elem_id + '" href="javascript:void(0);" class="' + clasedetalle +
                                ' bigger-140 ' + clasedetalleboton+'" title = "Detalle">' +
                                '<i class="ace-icon fa fa-angle-double-up"></i>' +
                                '<span class="sr-only">Detalle</span>' +
                                '</a>' +
                                '</div>';
                                tr += '<tr class="men_'+elemento.fk_menu+'" id="elemento_' + elemento.elem_id+'" data-id="' + elemento.elem_id + 
                                '" data-orden="' + elemento.elem_orden + 
                                '" data-tipo="' + elemento.fk_tipo_elemento + '"><td class="center">' + 
                                detalle + '</td><td><span class="elem_orden label label-default label-white middle">' + (indice + 1) + 
                                '</span> ' + elemento.tipo_nombre + '</td><td>' + elemento.elem_contenido + '</td><td><span class="label label-' + 
                                clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                                
                            });
                            tr = '<table class="table table-bordered table-condensed table-hover"><thead><tr><th style="width: 5%;"></th><th style="width: 18%;">Tipo</th><th>Titulo</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody class="tbody_elemento_'+value.menu_id+'">' + tr + '</tbody></table>';
                            if (rows.length == 0) {
                                tr = '<tr><td colspan="5"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tr>';
                            }
                            contenidoDiv+=tr;
                        }
                        else{
                            contenidoDiv='';
                        }

                        //Fin de tabla
                    div += '<div id="_tab_contenido_' + value.menu_id + '" class="tab-pane ' + activo + '">' +
                        '<div class="row" style="margin-bottom:8px">' +
                        '<div class="col-md-3 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_elemento" data-id="' + value.menu_id + '" data-rel="tooltip" title="Nuevo Elemento"><i class="ace-icon fa fa-file"></i> Nuevo elemento </button></div>' +
                        '<div class="col-md-3 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-warning btn-sm btn-block btn-round btn_ordenar_elemento" data-id="' + value.menu_id + '" data-rel="tooltip" title="Ordenar Elementos"><i class="ace-icon glyphicon glyphicon-list-alt"></i> Ordenar Elementos </button></div>' +
                        '</div><div id="contenido_elemento'+value.menu_id+'">'+
                                contenidoDiv +
                        '</div></div>';
                });
                $("#tabmenu").html(li);
                $("#tabcontenido").html(div);
                
            }
        });
    };
    var _ListarElementos=function(menu_id){
        $("#_tab_contenido_" + menu_id).html("");
        var dataForm = {
            menu_id: menu_id
        };
        responseSimple({
            url:"WebElemento/WebElementoListarxMenuIDJson",
            data:JSON.stringify(dataForm),
            refresh:false,
            callBackSuccess:function(response){
                var rows=response.data;
                var tr = "";
                var contenidoDiv = "";
                $.each(rows,function(index,elemento){
                    
                    var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_elemento" data-id="' + elemento.elem_id + '" data-menu_id="'+elemento.fk_menu+'"><i class="ace-icon fa fa-pencil"></i> </button>' +
                    ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_elemento" data-id="' + elemento.elem_id + '" data-menu_id="'+elemento.fk_menu+'"><i class="ace-icon fa fa-trash"></i> </button>';
                    var clase_estado = 'success';
                    var estado = "Activo";
                    if (elemento.elem_estado == "I") {
                        clase_estado = 'danger';
                        estado = "Inactivo";
                    };
                    var tipo = elemento.fk_tipo_elemento;
                    var clasedetalle = "blue";
                    var clasedetalleboton = "btn_detalle_elemento";
                    if (tipo == 1 || tipo == 2 || tipo == 3) {
                        clasedetalle = "grey";
                        clasedetalleboton = "";
                    }
                    var detalle = '<div class="action-buttons">' +
                    '<a data-id="' + elemento.elem_id + '" href="javascript:void(0);" class="' + clasedetalle +
                    ' bigger-140 ' + clasedetalleboton+'" title = "Detalle">' +
                    '<i class="ace-icon fa fa-angle-double-up"></i>' +
                    '<span class="sr-only">Detalle</span>' +
                    '</a>' +
                    '</div>';
                    tr += '<tr class="men_'+elemento.fk_menu+'" id="elemento_' + elemento.elem_id+'" data-id="' + elemento.elem_id + 
                    '" data-orden="' + elemento.elem_orden + 
                    '" data-tipo="' + elemento.fk_tipo_elemento + '"><td class="center">' + 
                    detalle + '</td><td><span class="elem_orden label label-default label-white middle">' + (index + 1) + 
                    '</span> ' + elemento.tipo_nombre + '</td><td>' + elemento.elem_contenido + '</td><td><span class="label label-' + 
                    clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                    
                });
                tr = '<table class="table table-bordered table-condensed table-hover"><thead><tr><th style="width: 5%;"></th><th style="width: 18%;">Tipo</th><th>Titulo</th><th style="width: 10%;">Estado</th><th style="width: 15%;">Acciones</th></tr></thead><tbody class="tbody_elemento_'+menu_id+'">' + tr + '</tbody></table>';
                if (rows.length == 0) {
                    tr = '<tr><td colspan="5"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tr>';
                }
                contenidoDiv+=tr;

                contenidoDiv = '<div class="row" style="margin-bottom:8px">' +
                    '<div class="col-md-3 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_elemento" data-id="' + menu_id + '" data-rel="tooltip" title="Nuevo Elemento"><i class="ace-icon fa fa-file"></i> Nuevo Elemento </button></div>' +
                    '<div class="col-md-3 col-sm-6 col-xs-6 pull-right"><button class="btn btn-white btn-warning btn-sm btn-block btn-round btn_ordenar_elemento" data-id="' + menu_id + '" data-rel="tooltip" title="Ordenar Elementos"><i class="ace-icon glyphicon glyphicon-list-alt"></i> Ordenar Elementos </button></div>' +
                    '</div>' +
                    '<div id="accordion_' + menu_id + '" class="accordion-style1 panel-group">' + contenidoDiv + '</div>';
                $("#_tab_contenido_" + menu_id).html(contenidoDiv);
            }
        })
    }
    var _sort_elemento = function (menu_id) {
        $('.tbody_elemento_' + menu_id).sortable({
            cursor: 'move',
            placeholder: 'box placeholder',
            stop: function (event, ui) {
                let lista_orden = [];
                var lista = $('.tbody_elemento_' + menu_id + ' tr.men_' + menu_id);
                $.each(lista, function (index, value) {
                    lista_orden.push({
                        elem_id: $(this).data("id"),
                        elem_orden: (index + 1)
                    });
                    $(this).find("span.elem_orden").text((index + 1));
                });
                responseSimple({
                    url: "WebElemento/WebElementoEditarOrdenJson",
                    data: JSON.stringify({ arrayElementos: lista_orden }),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            //PanelContenido.init_ListarElementos(menu_id);
                        }
                    }
                });
            }
        });
    };
    var _ListarDetalleElementos = function (elemento_id) {
        var dataForm = {
            elem_id: elemento_id
        };
        var tipo_elemento = $("#elemento_" + elemento_id).data("tipo");
        responseSimple({
            url: "WebDetalleElemento/WebDetalleElementoListarxElementoIDJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                var rows=response.data;
                var tr='';
                $.each(rows, function (index, value) {
                    var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + elemento_id +'"><i class="ace-icon fa fa-pencil"></i> </button>' +
                        ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + elemento_id+'"><i class="ace-icon fa fa-trash"></i> </button>';

                    var clase_estado = 'success';
                    var estado = "Activo";
                    if (value.detel_estado == "I") {
                        clase_estado = 'danger';
                        estado = "Inactivo";
                    };
                    var posicion = '';

                    var tipo = $("#elemento_" + elemento_id).data("tipo");
                    var clasedetalle = "grey";
                    var clasedetalleboton = "";
                    var columan_imagen = "";
                    var columna_imagen_detalle = "";
                    if (tipo == 1||tipo==2||tipo==3||tipo==8) {
                        columan_imagen = "hidden";
                        columna_imagen_detalle = "hidden";
                    }
                    if (tipo == 4 || tipo == 5 || tipo == 6 ) {
                        columna_imagen_detalle = "hidden";
                        // if (tipo == 11|| tipo==7) {
                        //     value.detel_descripcion = "IMAGEN";
                        // }
                    }

                    

                    var detalle = '<div class="action-buttons">' +
                        '<a data-id="' + value.detel_id + '" data-seccion="' + value.detel_id + '" href="javascript:void(0);" class="' + clasedetalle + ' bigger-140 ' + clasedetalleboton+'" title = "Detalle">' +
                        '<i class="ace-icon fa fa-angle-double-up"></i>' +
                        '<span class="sr-only">Detalle</span>' +
                        '</a>' +
                        '</div>';
                    var nombre = value.detel_titulo;
                    if (value.detel_imagen == "") {
                        nombre = "";
                    }
                    if(value.detel_imagen!=""){
                        nombre=value.detel_imagen;
                    }
                    tr += '<tr class="elem_' + elemento_id + '"  data-id="' + value.detel_id + '" data-orden="' + value.detel_orden 
                    + '"><td class="center">' + detalle + '</td><td><span class="detelem_orden label label-white middle label-default">' 
                    + (index + 1) + '</span> ' + value.detel_titulo + '</td><td class="' + columan_imagen + '">' 
                    + nombre + '</td><td class="' + columna_imagen_detalle + '" >' + posicion + '</td><td><span class="label label-' 
                    + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
                });
                var boton_nuevo = ' <div class="row" style="margin-bottom:10px;">' +
                    '<div class="col-md-3 col-md-offset-6 col-sm-2 col-xs-12"><button class="btn btn-white btn-warning btn-sm btn-block btn-round btn_ordenar_detalle_elemento" data-id="' + elemento_id + '" data-rel="tooltip" title="Ordenar Detalle Elemento"><i class="ace-icon glyphicon glyphicon-list-alt"></i> Ordenar </button></div>' +
                    '<div class="col-md-3 col-sm-5 col-xs-12"><button class="btn btn-white btn-success btn-sm btn-block btn-round btn_nuevo_detalle_elemento" data-id="' + elemento_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento"><i class="ace-icon fa fa-file"></i> Nuevo Detalle Elemento </button></div>' +
                    
                    '</div>';

                
                var columan_imagen = "";
                var columna_imagen_detalle = "";
                if (tipo_elemento == 1||tipo_elemento==2||tipo_elemento==3||tipo_elemento==8) {
                    columan_imagen = "hidden";
                    columna_imagen_detalle = "hidden";
                }

                if (tipo_elemento == 4 || tipo_elemento == 5 || tipo_elemento == 6 ) {
                    columna_imagen_detalle = "hidden";
                }

                if (rows.length > 0) {
                    tr = '<table class="table table-bordered table-condensed table-xs table-hover"><thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' + columan_imagen 
                    + '">Imagen</th><th style="width: 12%;" class="' + columna_imagen_detalle 
                    + '">Ubicacion</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody class="tbody_detalle_elemento_' 
                    + elemento_id + '">' + tr + '</tbody></table>';
                    $('#tr_elemento_contenido_' + elemento_id).html('<td colspan="5" style="padding-left: 2%;"><div class="table-detail"><div class="rows">' 
                    + boton_nuevo + '' + tr + '</div></div></td>');
                }
                else {
                    tr = '<table class="table table-bordered table-condensed table-xs table-hover"><thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' 
                    + columan_imagen + '">Imagen</th><th style="width: 12%;" class="' + columna_imagen_detalle 
                    + '">Ubicacion</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody><tr><td colspan="6"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tr></tbody></table>';
                    $('#tr_elemento_contenido_' + elemento_id).html('<td colspan="5"><div class="table-detail">' + boton_nuevo + ''+tr+'</div></td>');
                }
            }
        });
    };
    var _sort_detalle_elemento = function (elemento_id) {
        $('.tbody_detalle_elemento_' + elemento_id).sortable({
            cursor: 'move',
            placeholder: 'box placeholder',
            stop: function (event, ui) {
                let lista_orden = [];
                var lista = $('.tbody_detalle_elemento_' + elemento_id + ' tr.elem_' + elemento_id);
                $.each(lista, function (index, value) {
                    lista_orden.push({
                        detel_id: $(this).data("id"),
                        detel_orden: (index + 1)
                    });
                    $(this).find("span.detelem_orden").text((index + 1));
                });
                console.log(lista_orden);
                responseSimple({
                    url: "WebDetalleElemento/WebDetalleElementoEditarOrdenJson",
                    data: JSON.stringify({ arrayDetElemento: lista_orden }),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response.respuesta) {
                            //PanelContenido.init_ListarDetalleElementos(elemento_id);
                        }
                    }
                });
            }
        });
    };
    var _componentes=function(){
        /**Seccion Elementos */
        $(document).on('click', '#tabmenu li', function () {
            var menu_id = $(this).data("id");
            PanelContenido.init_ListarElementos(menu_id);
        });
        $(document).on('click', '.btn_nuevo_elemento', function (e) {
            $("#tituloModalElemento").text("Nuevo");
            var menu_id = $(this).data("id");
            $("#fk_menu").val(menu_id);
            $("#elem_id").val(0);
            $("#elem_contenido").val("");
            $("#div_fk_tipo_elemento").removeClass("hidden");
            $("#div_texto_fk_tipo_elemento").addClass("hidden");
            $("#fk_tipo_elemento").val("").trigger('change');
            _objetoForm_form_elemento.resetForm();
           
            $("#modalFormularioElemento").modal("show");
        });
        $(document).on('click', '.btn_editar_elemento', function () {
            $("#tituloModalElemento").text("Editar");
            var elem_id = $(this).data('id');
            var dataForm = {
                elem_id: elem_id,
            }
            $("#div_fk_tipo_elemento").addClass("hidden");
            $("#div_texto_fk_tipo_elemento").removeClass("hidden");
            responseSimple({
                url: 'WebElemento/WebElementoIdObtenerJson',
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        var elemento = response.data;
                        var input = '<input type="text" name="elem_contenido" id="elem_contenido" class="form-control" placeholder="Texto">';
                        var textarea = '<textarea name="elem_contenido" id="elem_contenido" class="form-control"></textarea>';
                        var tipo = elemento.fk_tipo_elemento;
                        $("#fk_tipo_elemento").val(elemento.fk_tipo_elemento).trigger('change');
                        $("#fk_menu").val(elemento.fk_menu);
                        $("#elem_id").val(elemento.elem_id);
                        $("#elem_orden").val(elemento.elem_orden);
                        $("#elem_estado").val(elemento.elem_estado);
                        if (tipo == 1 || tipo == 2 || tipo == 3 || tipo == 4 || tipo == 6) {
                            if (tipo == 3) {
                                $("#parrafo_elemento").text("Parrafo");
                                $("#contenido_input").html(textarea);
                                $("#elem_contenido").val(elemento.elem_contenido);
                                $('#elem_contenido').richText({
                                    imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                                    fileUpload: false, urls: false
                                });
                                $("a.richText-help").hide();
                            } else {
                                $("#parrafo_elemento").text("Texto");
                                $("#contenido_input").html(input);
                                $("#elem_contenido").val(elemento.elem_contenido);
                            };
                            $("div.contenido_elemento").show();
                        }
                        else {
                            $("#contenido_input").html(input);
                            $("#parrafo_elemento").text("Texto");
                            $("div.contenido_elemento").hide();
                            $("#elem_contenido").val(elemento.elem_contenido);
                        }
                        $("#texto_fk_tipo_elemento").html($("#fk_tipo_elemento option:selected").text());
                        $("#modalFormularioElemento").modal("show");
                    }
                }
            })
        })
        $(document).on("click", ".btn_eliminar_elemento", function (e) {
            var elemento_id = $(this).data("id");
            var menu_id = $(this).data("menu_id");
            if (elemento_id != "" || elemento_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro que desea ELIMINAR este Elemento?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "WebElemento/WebElementoEliminarJson",
                            data: JSON.stringify({ elem_id: elemento_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if (response.respuesta) {
                                    PanelContenido.init_ListarElementos(menu_id);
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
        $(document).on('change', '#fk_tipo_elemento', function (e) {
        var estado = $("#tituloModalElemento").text();
        if (estado == "Nuevo") {
            var input = '<input type="text" name="elem_contenido" id="elem_contenido" class="form-control" placeholder="Texto">';
            var textarea = '<textarea name="elem_contenido" id="elem_contenido" class="form-control"></textarea>';
            var tipo = $(this).val();
            if (tipo == 1 || tipo == 2 || tipo == 3) {
                if (tipo == 3) {
                    $("#parrafo_elemento").text("Parrafo");
                    $("#contenido_input").html(textarea);
                    $('#elem_contenido').richText({
                        imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                        fileUpload: false, urls: false
                    });
                    $("a.richText-help").hide();
                } else {
                    $("#parrafo_elemento").text("Texto");
                    $("#contenido_input").html(input);
                };
                $("div.contenido_elemento").show();
                $("#elem_contenido").val("");

            }
            else {
                $("#contenido_input").html(input);
                $("#parrafo_elemento").text("Texto");
                $("div.contenido_elemento").hide();
                $("#elem_contenido").val($("#fk_tipo_elemento option:selected").text());
            }
        }
        });
        $(document).on('click', '.btn_guardar_elemento', function () {
            $("#form_elemento").submit();
            if (_objetoForm_form_elemento.valid()) {
                var dataForm = $('#form_elemento').serializeFormJSON();
                var url = "";
                if ($("#elem_id").val() == 0) {
                    url = 'WebElemento/WebElementoInsertarJson';
                }
                else {
                    url = 'WebElemento/WebElementoEditarJson';
                }
                responseSimple({
                    url: url,
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            var menu_id = $("#fk_menu").val();
                            PanelContenido.init_ListarElementos(menu_id);
                            $("#modalFormularioElemento").modal("hide");
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
        $(document).on('click', '.btn_ordenar_elemento', function (e) {
            var menu_id = $(this).data("id");
            var spans = $('tbody.tbody_elemento_' + menu_id + ' tr span.elem_orden');
            $.each(spans, function (index, value) {
                $(this).removeClass("label-default");
                $(this).addClass("label-warning");
            });
            PanelContenido.init_Sort_Elemento(menu_id);
        });
        /**End Seccion Elementos */
        /**Seccion Detalle de Elementos */
        $(document).on('click', '.btn_detalle_elemento', function (e) {
            e.preventDefault();
            var elemento_id = $(this).data("id");
            var act_tr = $(this).closest("tr");
            var tipo = act_tr.data("tipo");
            if (tipo == 1 || tipo==2 || tipo==3) {
                return false;
            }
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
            if ($('#tr_elemento_contenido_' + elemento_id).hasClass("open")) {
                $('#tr_elemento_contenido_' + elemento_id).removeClass("open");
            }
            else {
                $('#tr_elemento_contenido_' + elemento_id).remove();
                $('<tr id="tr_elemento_contenido_' + elemento_id + '" class="detail-row open"><td colspan="5"><div class="alert alert-warning" style="margin-bottom:0px;">Cargando Data ...</div></td></tr>').insertAfter(act_tr);
                PanelContenido.init_ListarDetalleElementos(elemento_id);
            }
        });
        $(document).on('click','.btn_nuevo_detalle_elemento',function(){
            var elemento_id = $(this).data("id");
            var tipo_elemento = $("#elemento_" + elemento_id).data("tipo");

            console.log(tipo_elemento);
            $("#div_parrafo_detalleelemento").html("");
            var input = '<input type="text" name="detel_parrafo" id="detel_parrafo" class="form-control" placeholder="Texto">';
            var textarea = '<textarea name="detel_parrafo" id="detel_parrafo" class="form-control"></textarea>';
            $("#div_parrafo_detalleelemento").html(input);
            $("#tituloModalDetalleElemento").text("Nuevo");
            $("#fk_elemento").val(elemento_id);
            $("#fk_tipo").val(tipo_elemento);
            $("#detel_id").val(0);
            $("#detel_nombre_imagen").text("");
            $("#detel_nombre_imagen_detalle").val("");
            $("#spandetel_detalle").html('<i class="fa fa-upload"></i>  Subir Imagen');
            $("#spandetel").html('<i class="fa fa-upload"></i>  Subir Imagen');
            $("#detel_titulo").val("");
            $("#detel_subtitulo").val("");
            $("#detel_parrafo").val("");
            $("#detel_estado").val("A");
            $("#divdetel").hide();
            $("#divdetel_detalle").hide();
            if(tipo_elemento==4){
                //hide
                $(".detel-subtitulo").hide();
                $(".detel-parrafo").hide();
                $(".detel-imagen-detalle").hide();
                //show
                $(".detel-titulo").show();
            }
            else if(tipo_elemento==5){
                //hide
                $(".detel-parrafo").hide();
                $(".detel-imagen-detalle").hide();
                //show
                $(".detel-titulo").show();
                $(".detel-subtitulo").show();
            }
            else if(tipo_elemento==6){
                //hide
                $(".detel-imagen-detalle").hide();
                //show
                $(".detel-titulo").show();
                $(".detel-subtitulo").show();
                
                $("#div_parrafo_detalleelemento").html(textarea);
                $('#detel_parrafo').richText({
                    imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                    fileUpload: false, urls: false
                });
                $("a.richText-help").hide();
                $(".detel-parrafo").show();
            }
            else if(tipo_elemento==7){
                $(".detel-titulo").hide();
                $(".detel-subtitulo").hide();
                $(".detel-parrafo").hide();
                $(".detel-imagen-detalle").show();
            }
            $("#modalFormularioDetalleElemento").modal("show");
        })

        $(document).on('click', '.btn_guardar_detalle_elemento', function () {
            //$("#form_detalle_elemento").submit();
            var elemento_id = $("#fk_elemento").val();
            var tipo_elemento = $("#elemento_" + elemento_id).data("tipo");

            if (tipo_elemento == 4) {
                if ($("#detel_titulo").val() == "") {
                    messageResponse({
                        text: 'Titulo es obligatorio',
                        type: "error"
                    });
                    return false;
                }
                if ($("#detel_imagen").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Seleccione Imagen',
                        type: "error"
                    });
                    return false;
                }
            }

            if (tipo_elemento == 5) {
                if ($("#detel_titulo").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Titulo es obligatorio',
                        type: "error"
                    });
                    return false;
                }

                if ($("#detel_subtitulo").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'subtitulo es obligatorio',
                        type: "error"
                });
                    return false;
                }
                if ($("#detel_imagen").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Seleccione Imagen',
                        type: "error"
                });
                    return false;
                }
            }

            if (tipo_elemento == 6) {
                if ($("#detel_titulo").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Titulo es obligatorio',
                        type: "error"
                    });
                    return false;
                }

                if ($("#detel_subtitulo").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Subtitulo es Obligatorio',
                        type: "error"
                    });

                    return false;
                }
                if ($("#detel_parrafo").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Parrafo es Obligatorio',
                        type: "error"
                    });

                    return false;
                }
                if ($("#detel_imagen").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Selecione Imagen',
                        type: "error"
                    });

                    return false;
                }
            }

            if (tipo_elemento == 7) {
                if ($("#detel_imagen").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Selecione Imagen',
                        type: "error"
                    });
                    return false;
                }
                if ($("#detel_imagen_detalle").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                    messageResponse({
                        text: 'Selecione Imagen para Detalle',
                        type: "error"
                    });
                    return false;
                }
            }
            var dataForm = new FormData(document.getElementById("form_detalle_elemento"));
            var url = '';
            if ($("#detel_id").val() == 0) {
                url = 'WebDetalleElemento/WebDetalleElementoInsertarJson';
            }
            else {
                url = 'WebDetalleElemento/WebDetalleElementoEditarJson';
            }
            responseFileSimple({
                url: url,
                data: dataForm,
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        PanelContenido.init_ListarDetalleElementos(elemento_id);
                        $("#modalFormularioDetalleElemento").modal("hide");
                    }
                }
            })
        });
        $(document).on("change", "#detel_imagen", function () {
            var _image = $('#detel_imagen')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                //console.log(extension);
                if (extension!= "jpg" && extension != "png" && extension != "jpeg") {
                    messageResponse({
                        text: 'Sólo Se Permite formato jpg, png ó jpeg',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 11);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "png" || actualicon == "jpg" || actualicon == 'jpeg') {
                        icon = '<i class="fa fa-file-word-o"></i>';
                    }
                    else {
                        icon = '<i class="fa fa-file-pdf-o"></i>';
                    };
                    $("#spandetel").html("");
                    $("#spandetel").append(icon + " " + nombre + "... ." + actualicon);
                    $("#spandetel").css({ 'font-size': '10px' });
                }
            }
            else {
                $("#spandetel").html("");
                $("#spandetel").append('<i class="fa fa-upload"></i>  Subir Imagen');
            }
        });
        $(document).on("change", "#detel_imagen_detalle", function () {
            var _image = $('#detel_imagen_detalle')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                //console.log(extension);
                if (extension!= "jpg" && extension != "png" && extension != "jpeg") {
                    messageResponse({
                        text: 'Sólo Se Permite formato jpg, png ó jpeg',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 11);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "png" || actualicon == "jpg" || actualicon == 'jpeg') {
                        icon = '<i class="fa fa-file-word-o"></i>';
                    }
                    else {
                        icon = '<i class="fa fa-file-pdf-o"></i>';
                    };
                    $("#spandetel_detalle").html("");
                    $("#spandetel_detalle").append(icon + " " + nombre + "... ." + actualicon);
                    $("#spandetel_detalle").css({ 'font-size': '10px' });
                }
            }
            else {
                $("#spandetel_detalle").html("");
                $("#spandetel_detalle").append('<i class="fa fa-upload"></i>  Subir Imagen');
            }
        });
        $(document).on("click", ".btn_eliminar_detalle_elemento", function (e) {
            var det_elemento_id = $(this).data("id");
            var elemento_id = $(this).data("elemento_id");
            if (det_elemento_id != "" || det_elemento_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro que desea ELIMINAR este Detalle de Elemento?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "WebDetalleElemento/WebDetalleElementoEliminarJson",
                            data: JSON.stringify({ detel_id: det_elemento_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if (response.respuesta) {
                                    PanelContenido.init_ListarDetalleElementos(elemento_id);
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
        $(document).on('click', '.btn_editar_detalle_elemento', function (e) {
            var det_elemento_id = $(this).data("id");
            $("#tituloModalDetalleElemento").text("Editar");
            var dataForm = {
                detel_id: det_elemento_id,
            }
            responseSimple({
                url: 'WebDetalleElemento/WebDetalleElementoIdObtenerJson',
                refresh: false,
                data: JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    var data = response.data;
                    if (response.respuesta) {
                        $("#detel_imagen").val("");
                        $("#detel_imagen_detalle").val("");

                        $("#divdetel").hide();
                        $("#divdetel_detalle").hide();
                        $(".detel-titulo").hide();
                        $(".detel-subtitulo").hide();
                        $(".detel-parrafo").hide();
                        $("#divdetel_detalle").hide();
                        $(".detel-imagen-detalle").hide();



                        $('#detel_id').val(data.detel_id);
                        $('#fk_elemento').val(data.fk_elemento);
                        $("#fk_tipo").val(data.fk_tipo);
                        $("#spandetel").html("");
                        $("#spandetel").append('<i class="fa fa-upload"></i>  Subir Imagen');
                        $("#spandetel_detalle").html("");
                        $("#spandetel_detalle").append('<i class="fa fa-upload"></i>  Subir Detalle');
                        if (data.fk_tipo == 4||data.fk_tipo==5||data.fk_tipo==6||data.fk_tipo==7)
                         {
                            $(".detel-imagen").show();
                            $("#detel_nombre_imagen_modal").text("Nombre: " + data.detel_imagen);
                            $("#detel_nombre_imagen").val(data.detel_imagen);
                            $("#icono_actual_detel").attr("src", basePath+"WebFiles/" + data.detel_imagen);
                            $("#divdetel").show();
                            
                            if(data.fk_tipo==4){
                                $(".detel-titulo").show();
                            }
                             else if(data.fk_tipo==5){
                                $(".detel-titulo").show();
                                $(".detel-subtitulo").show();
                                
                            }
                            else if(data.fk_tipo==6){
                                $(".detel-titulo").show();
                                $(".detel-subtitulo").show();
                                $(".detel-parrafo").show();
                                
                            }
                            else if(data.fk_tipo==7){
                                $("#detel_nombre_imagen_detalle_modal").text("Nombre: " + data.detel_imagen_detalle);
                                $("#detel_nombre_imagen_detalle").val(data.detel_imagen_detalle);
                                $("#icono_actual_detel_detalle").attr("src", basePath+"WebFiles/" + data.detel_imagen_detalle);
                                $("#divdetel_detalle").show();
                                $(".detel-imagen-detalle").show();
                            }
                        }
                        $("#div_parrafo_detalleelemento").html("");
                        var input = '<input type="text" name="detel_parrafo" id="detel_parrafo" class="form-control" placeholder="Texto">';
                        var textarea = '<textarea name="detel_parrafo" id="detel_parrafo" class="form-control"></textarea>';
                        $("#div_parrafo_detalleelemento").html(input);

                        if (data.fk_tipo == 6) {
                            $("#div_parrafo_detalleelemento").html(textarea);
                            $("#detel_parrafo").val(data.detel_parrafo);
                            $('#detel_parrafo').richText({
                                imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                                fileUpload: false, urls: false
                            });
                            $("a.richText-help").hide();
                        }

                        
                        $("#detel_estado").val(data.detel_estado);
                        $("#detel_titulo").val(data.detel_titulo);
                        $("#detel_subtitulo").val(data.detel_subtitulo);
                        $("#detel_parrafo").val(data.detel_parrafo);
                        $("#detel_orden").val(data.detel_orden);
                        $('#modalFormularioDetalleElemento').modal('show');
                    }
                },
            })

        });
        $(document).on('click', '.btn_ordenar_detalle_elemento', function (e) {
            var elemento_id = $(this).data("id");
            var spans = $('tbody.tbody_detalle_elemento_' + elemento_id + ' tr span.detelem_orden');
            console.log(spans)
            $.each(spans, function (index, value) {
                $(this).removeClass("label-default");
                $(this).addClass("label-warning");
            });
            PanelContenido.init_Sort_Detalle_Elemento(elemento_id);
        });
        
        /**End Seccion Detalle de Elementos */

    }
    var _metodos=function(){
        selectResponse({
            url: "WebTipoElemento/WebTipoElementoListarJson",
            select: "fk_tipo_elemento",
            campoID: "tipo_id",
            CampoValor: "tipo_nombre",
            select2: true,
            closeMessages: true,
        });
        validar_Form({
            nameVariable: 'form_elemento',
            contenedor: '#form_elemento',
            rules: {
                fk_tipo_elemento:
                {
                    required: true,

                },
                elem_estado:
                {
                    required: true,

                }
            },
            messages: {
                fk_tipo_elemento:
                {
                    required: 'Campo Obligatorio',
                },
                elem_estado:
                {
                    required: 'Campo Obligatorio',
                }
            }
        });
    }
    return{
        init:function(){
            _ListarMenus();
            _componentes();
            _metodos();
        },
        init_ListarMenus:function(){
            _ListarMenus();
        },
        init_ListarElementos:function(menu_id){
            _ListarElementos(menu_id);
        },
        init_Sort_Elemento: function (menu_id) {
            _sort_elemento(menu_id);
        },
        init_ListarDetalleElementos: function (elemento_id) {
            _ListarDetalleElementos(elemento_id);
        },
        init_Sort_Detalle_Elemento: function (elemento_id) {
            _sort_detalle_elemento(elemento_id);
        },
    }
}();
document.addEventListener('DOMContentLoaded',function(){
    PanelContenido.init();
})