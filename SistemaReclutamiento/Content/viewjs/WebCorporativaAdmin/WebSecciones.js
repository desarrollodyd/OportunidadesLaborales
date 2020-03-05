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
                            PanelContenido.init_ListarElementos(menu_id);
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
              console.log(response);
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
    }
}();
document.addEventListener('DOMContentLoaded',function(){
    PanelContenido.init();
})