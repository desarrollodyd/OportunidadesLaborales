var PanelSecciones = function () {

    var _ListarSecciones = function () {
        $("#btn_nuevo").prop('disabled', true);
        $("#btn_eliminar_varios").prop('disabled', true);
        if (menu.length > 0) {
            var appendMenus = '';
            var selectMenus = $("#cboMenus");
            selectMenus.html("");
            appendMenus += '<option value="">Seleccione Menu</option>';
            $.each(menu, function (index, value) {
                appendMenus += '<option value="' + value.menu_id + '">"' + value.menu_titulo + '"</option>';
            });
            selectMenus.html(appendMenus);
            selectMenus.select2();
        }
    };
    var _componentes = function () {
        $(document).on("change", "#cboMenus", function () {
            var menu_id = $(this).val();
            if (menu_id != "") {
                var dataForm = { menu_id: menu_id };
                responseSimple({
                    url: "IntranetSeccion/IntranetSeccionListarTodoxMenuIDJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response.data.length > 0) {
                            $("#btn_nuevo").prop('disabled', false);
                            $("#btn_eliminar_varios").prop('disabled', false);
                            $("#id_menu").val(menu_id);
                            simpleDataTable({
                                uniform: false,
                                tableNameVariable: "seccionesListado",
                                table: ".datatable-seccionlistado",
                                tableColumnsData: response.data,
                                tableHeaderCheck: false,
                                tableColumns: [
                                    {
                                        "className": 'details-control',
                                        "orderable": false,
                                        "data": "sec_id",
                                        "title": "Secciones",
                                        "defaultContent": '',
                                        "render": function (value) {
                                            var sec_id = value;
                                            var span = '';
                                            span += '<a href="#" class="tooltip-info" data-id="' + sec_id + '" data-rel="tooltip"                  title="Ver Detalle"><span class="blue" >             <i class="ace-icon fa fa-search-plus bigger-120"></       i></span ></a>';
                                            return span;
                                        },
                                        width: "50px"
                                    },
                                    {
                                        data: "sec_id",
                                        title: "Id"
                                    },
                                    {
                                        data: "menu_titulo",
                                        title: "Menu"
                                    },
                                    {
                                        data: "sec_orden",
                                        title: "Orden"
                                    },
                                    {
                                        data: "sec_estado",
                                        title: "Estado",
                                        "render": function (value, type, row) {
                                            var seleccionado = value == 'A' ? "selected" : "";
                                            var select = '<select class="browser-default custom-select select-estado-seccion" data-id=' + row.sec_id + '>';

                                            if (value == 'A') {
                                                select += '<option value="A" selected>Activo</option><option value="I">Inactivo</option>'
                                            }
                                            else {
                                                select += '<option value="A">Activo</option><option value="I" selected>Inactivo</option>'
                                            }
                                            select += '</select>';
                                            return select;
                                        }
                                    },
                                    {
                                        data: "sec_id",
                                        title: "Acciones",
                                        "render": function (value) {
                                            var span = '';
                                            var sec_id = value;
                                            var span = '   <div class="hidden-sm hidden-xs action-buttons"><a class="green btn-editar" href = "#" data-id="' + sec_id + '"> <i class="ace-icon fa fa-pencil bigger-130"></i></a></div><div class="hidden-md hidden-lg"><div class="inline pos-rel"><button class="btn btn-minier btn-yellow dropdown-toggle" data-toggle="dropdown" data-position="auto"><i class="ace-icon fa fa-caret-down icon-only bigger-120"></i></button><ul class="dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close"><li><a href="#" class="tooltip-success btn-editar" data-id="' + sec_id + '" data-rel="tooltip" title="edit"><span class="green"><i class="ace-icon fa fa-pencil-square-o bigger-120"></i></span></a></li></ul></div></div>';
                                            return span;
                                        }
                                    }

                                ]
                            })
                        }
                        else {
                            messageResponse({
                                text: "Error Al Cargar Datos",
                                type: "error"
                            });
                            
                        }
                       
                    }
                })
            }
            else {
                messageResponse({
                    text: "Debe Seleccionar un Menu",
                    type: "error"
                })
            }
        });

        $('#seccionesListado tbody').on('click', 'td.details-control', function () {
            var table = $("#seccionesListado").DataTable();
            var tr = $(this).closest('tr');
            var row = table.row(tr);
            var sec_id = row.data().sec_id;
            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                // Open this row
                if (sec_id > 0) {
                    var dataForm = {
                        sec_id: sec_id
                    };
                    responseSimple({
                        url: "IntranetElemento/IntranetElementoListarxMenuIDJson",
                        data: JSON.stringify(dataForm),
                        refresh: false,
                        callBackSuccess: function (response) {
                            var rows = response.data;
                            row.child(MostrarDetalle(rows)).show();
                            tr.addClass('shown');
                        }
                    })
                }
            }
        });

        $(document).on("click", "#btn_nuevo", function (e) {
            if ($("#cboMenus").val() != "") {
                $("#menu_id").val(0);
                $("#tituloModalSeccion").text("Nueva ");
                $("#sec_orden").prop('disabled', false);
                $("#sec_estado").prop('disabled', false);

                $("#sec_orden").val(1);
                $("#sec_estado").val("A");

                $(".btn-guardar").show();

                $("#modalFormularioSeccion").modal("show");
            }
            else {
                messageResponse({
                    text: "Debe Seleccionar un Menu para poder crear Secciones",
                    type: "error"
                })
            }
           
        });

        $(document).on('click', ".btn-guardar", function (e) {
            $("#form_secciones").submit();
            if (_objetoForm_form_secciones.valid()) {
                var dataForm = $('#form_secciones').serializeFormJSON();
                responseSimple({
                    url: "IntranetSeccion/IntranetSeccionInsertarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            //limpiar_form({ contenedor: "#form_menus" });
                            //_objetoForm_form_menus.resetForm();
                            PanelSecciones.init_ListarSecciones();
                            //refresh(true);
                            $("#modalFormularioSeccion").modal("hide");
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

        $(document).on("change", ".select-estado-seccion", function () {
            var sec_id = $(this).data("id");
            var sec_estado = $(this).val();
            messageConfirmation({
                content: '¿Esta seguro de Cambiar de Estado a esta Sección?',
                callBackSAceptarComplete: function () {
                    var dataForm = {
                        sec_id: sec_id,
                        sec_estado: sec_estado
                    }
                    responseSimple({
                        url: "IntranetSeccion/IntranetSeccionEditarJson",
                        data: JSON.stringify(dataForm),
                        refresh: false,
                        callBackSuccess: function (response) {
                            PanelSecciones.init_ListarSecciones();
                            $("#cboMenus").val($("#id_menu").val());
                            $("#cboMenus").change();
                            //refresh(true);
                        }
                    });
                }
            });


        });

        $(document).on("click", ".btn-eliminar", function (e) {
            var sec_id = $(this).data("id");
            if (sec_id != "" || sec_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR esta Seccion?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetSeccion/IntranetSeccionEliminarJson",
                            data: JSON.stringify({ sec_id: sec_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                PanelComentarios.init_ListarComentarios();
                                //refresh(true);
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

        $(document).on("click", ".btn_editar_detalle", function (e) {
            var elem_id = $(this).data("id");
            console.log(elem_id);
        });
        $(document).on("click", ".detalle-elemento", function (e) {
            var elem_id = $(this).data("id");
            var elemento = $(this);
            var row = $('tr.elemento' + elem_id);
            var rowlength = $('tr.elemento' + elem_id + ' td').length;
            var rownext = row.next('tr');
            var htmlTags = '';
            var dataForm = {
                elem_id: elem_id
            };
            if ($(this).hasClass('detalle-oculto')) {
                elemento.removeClass('detalle-oculto');

                responseSimple({
                    url: "IntranetDetalleElemento/IntranetDetalleElementoListarxElementoIDJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var data = response.data;
                        if (data.length > 0) {
                            htmlTags += '<tr><td colspan="' + rowlength + '"><fieldset><legend>Detalle de Elemento</legend></fieldset><table class="table table-bordered table-sm"><tr class="thead-dark">';
                            htmlTags += '<th>Detalle</th>';
                            htmlTags += '<th>ID</th>';
                            htmlTags += '<th>Descripcion</th>';
                            htmlTags += '<th>Nombre</th>';
                            htmlTags += '<th>Orden</th>';
                            htmlTags += '<th>Estado</th>';
                            htmlTags += '<th>Acciones</th></tr>';
                            $.each(data, function (index, value) {

                                htmlTags += '<tr class="det-elemento' + value.detel_id + '"><td data-elemento="' + value.detel_id + '" data-id="' + value.fk_seccion_elemento + '" class="elemento-modal detalle-oculto"><a href="#" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a></td>';
                                htmlTags += '<td>' + value.detel_id + '</td>';
                                htmlTags += '<td>' + value.detel_descripcion + '</td>';
                                htmlTags += '<td>' + value.detel_nombre + '</td>';
                                htmlTags += '<td>' + value.detel_orden + '</td>';
                                htmlTags += '<td>' + value.detel_estado + '</td>';
                                htmlTags += '<td>'+((value.fk_seccion_elemento>0)?'Abre un Modal':'No abre Nada')+'</td></tr>';
                            })
                            htmlTags += '</table></td></tr></fieldset>';
                            row.after(htmlTags);
                        }
                    },
                })
            } else {
                elemento.addClass('detalle-oculto');
                rownext.remove();
            }
            
        });
        $(document).on("click", ".elemento-modal", function (e) {
            var detel_id = $(this).data("elemento");
            var fk_seccion_elemento = $(this).data("id");
            var elemento = $(this);
            var row = $('tr.det-elemento' + detel_id);
            var rowlength = $('tr.det-elemento' + detel_id + ' td').length;
            var rownext = row.next('tr');
            var htmlTags = '';
            var dataForm = {
                fk_seccion_elemento: fk_seccion_elemento
            };
            if ($(this).hasClass('detalle-oculto')) {
                elemento.removeClass('detalle-oculto');
                responseSimple({
                    url: 'IntranetElementoModal/IntranetElementoModalListarxSeccionElementoJson',
                    refresh: false,
                    data: JSON.stringify(dataForm),
                    callBackSuccess: function (response) {
                        console.log(response.data);
                        var data = response.data;
                        if (data.length > 0) {
                            htmlTags += '<tr><td colspan="' + rowlength + '"><fieldset><legend>Elemento Modal</legend></fieldset><table class="table table-bordered table-sm"><tr class="thead-dark">';
                            htmlTags += '<th>Detalle</th>';
                            htmlTags += '<th>ID</th>';
                            htmlTags += '<th>Titulo</th>';
                            htmlTags += '<th>Tipo Elemento</th>';
                            htmlTags += '<th>Orden</th>';
                            htmlTags += '<th>Estado</th>';
                            htmlTags += '<th>Acciones</th></tr>';
                            $.each(data, function (index, value) {

                                htmlTags += '<tr class="elem-modal' + value.emod_id + '"><td data-id="' + value.emod_id + '" class="detalle-elemento-modal detalle-oculto"><a href="#" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a></td>';
                                htmlTags += '<td>' + value.emod_id + '</td>';
                                htmlTags += '<td>' + value.emod_titulo + '</td>';
                                htmlTags += '<td>' + value.tipo_nombre + '</td>';
                                htmlTags += '<td>' + value.emod_orden + '</td>';
                                htmlTags += '<td>' + value.emod_estado + '</td>';
                                htmlTags += '<td></td></tr>';
                            })
                            htmlTags += '</table></td></tr></fieldset>';
                            row.after(htmlTags);
                        }
                    }
                });
            }
            else {
                elemento.addClass('detalle-oculto');
                rownext.remove();
            }
        });
        $(document).on('click', '.detalle-elemento-modal', function () {
            var emod_id = $(this).data("id");
            var elemento = $(this);
            var row = $('tr.elem-modal' + emod_id);
            var rowlength = $('tr.elem-modal' + emod_id + ' td').length;
            var rownext = row.next('tr');
            var htmlTags = '';
            var dataForm = {
                fk_elemento_modal: emod_id
            };
            if ($(this).hasClass('detalle-oculto')) {
                elemento.removeClass('detalle-oculto');
                responseSimple({
                    url: 'IntranetDetalleElementoModal/IntranetDetalleElementoModalListarxElementoModalJson',
                    refresh: false,
                    data: JSON.stringify(dataForm),
                    callBackSuccess: function (response) {
                        console.log(response.data);
                        var data = response.data;
                        if (data.length > 0) {
                            htmlTags += '<tr><td colspan="' + rowlength + '"><fieldset><legend>Detalle Elemento Modal</legend></fieldset><table class="table table-bordered table-sm"><tr class="thead-dark">';
                            htmlTags += '<th>ID</th>';
                            htmlTags += '<th>Titulo</th>';
                            htmlTags += '<th>Orden</th>';
                            htmlTags += '<th>Estado</th>';
                            htmlTags += '<th>Acciones</th></tr>';
                            $.each(data, function (index, value) {

                                htmlTags += '<td>' + value.detelm_id + '</td>';
                                htmlTags += '<td>' + value.detelm_descripcion + '</td>';
                                htmlTags += '<td>' + value.detelm_orden + '</td>';
                                htmlTags += '<td>' + value.detelm_estado + '</td>';
                                htmlTags += '<td></td></tr>';
                            })
                            htmlTags += '</table></td></tr></fieldset>';
                            row.after(htmlTags);
                        }
                    }
                });
            }
            else {
                elemento.addClass('detalle-oculto');
                rownext.remove();
            }
        });
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'form_secciones',
            contenedor: '#form_secciones',
            rules: {
                sec_orden:
                {
                    required: true,

                },
                sec_estado:
                {
                    required: true,

                }

            },
            messages: {
                sec_orden:
                {
                    required: 'Campo Obligatorio',
                },
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
            _ListarSecciones();
            _componentes();
            _metodos();

        },
        init_ListarSecciones: function () {
            _ListarSecciones();
        }
    }
}();
function MostrarDetalle(row) {
    var table = '';
    if (row.length > 0) {
        table += '<table class="table table-bordered table-sm"><tr class="thead-dark">';
        table += '<th>Detalle</th>';
        table += '<th>ID</th>';
        table += '<th>titulo</th>';
        table += '<th>Estado</th>';
        table += '<th>Orden</th>';
        table += '<th>Tipo</th>';
        table += '<th>Acciones</th></tr>';
        $.each(row, function (index, value) {
            table += '<tr class="elemento' + value.elem_id + '"><td data-id="' + value.elem_id + '" class="detalle-elemento detalle-oculto">' + '<a href="#" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a>' + '</td>';
            table += '<td>' + value.elem_id + '</td>';
            table += '<td>' + value.elem_titulo + '</td>';
            table += '<td>' + (value.elem_estado=='A'?'Activo':'Inactivo') + '</td>';
            table += '<td>' + value.elem_orden + '</td>';
            table += '<td>' + value.tipo_nombre + '</td>';
            table += '<td><a href="#" class="tooltip-warning btn_detalle_elemento" data-id="'+value.elem_id+'" data-rel="tooltip" title="Editar"><span class="green" ><i class="ace-icon fa fa-pencil-square-o bigger-120"></i></span ></a></td></tr>';
        });
        table += '</table>';
        
    }
    return table;
}
// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelSecciones.init();
});