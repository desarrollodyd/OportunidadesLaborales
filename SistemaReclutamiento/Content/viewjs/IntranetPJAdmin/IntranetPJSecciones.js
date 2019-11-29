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
                    url: "IntranetSeccion/IntranetSeccionListarxMenuIDJson",
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
                                            var span = '<div class="hidden-sm hidden-xs action-buttons" > <a class="red btn-eliminar" href="#" data-id="' + sec_id + '" > <i class="ace-icon fa fa-trash-o bigger-130"></i></a ></div > <div class="hidden-md hidden-lg"><div class="inline pos-rel"><button class="btn btn-minier btn-yellow dropdown-toggle" data-toggle="dropdown" data-position="auto"><i class="ace-icon fa fa-caret-down icon-only bigger-120"></i> </button><ul class="dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close"><li><a href="#" class="tooltip-error btn-eliminar" data-id="' + sec_id + '" data-rel="tooltip" title="Delete"><span class="red"><i class="ace-icon fa fa-trash-o bigger-120"></i></span></a> </li></ul></div></div>';
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
            console.log("click");
            $("#form_secciones").submit();
            if (_objetoForm_form_secciones.valid()) {
                var dataForm = $('#form_secciones').serializeFormJSON();
                responseSimple({
                    url: "IntranetSeccion/IntranetSeccionInsertarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        console.log(response);
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
        table += '<th>ID</th>';
        table += '<th>titulo</th>';
        table += '<th>Estado</th>';
        table += '<th>Orden</th>';
        table += '<th>Tipo</th>';
        table += '<th>Acciones</th></tr>';
        $.each(row, function (index, value) {
            console.log(value);
            table += '<tr><td>' + value.elem_id + '</td>';
            table += '<td>' + value.elem_titulo + '</td>';
            table += '<td>' + (value.elem_estado=='A'?'Activo':'Inactivo') + '</td>';
            table += '<td>' + value.elem_orden + '</td>';
            table += '<td>' + value.tipo_nombre + '</td>';
            table += '<td><a href="#" class="tooltip-info" data-id="' + value.elem_id + '" data-rel="tooltip" title="Eliminar"><span class="blue" ><i class="ace-icon fa fa-trash-o bigger-130"></i></span ></a></td></tr>';
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