
var PanelMenus = function () {
    var _ListarMenus = function () {
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        //if ($.fn.DataTable.isDataTable("#menusListado")) {
        //    $("#menusListado").DataTable().clear();
        //}
        responseSimple({
            url: "IntranetMenu/IntranetMenuListarTodoJson",
            refresh: false,
            callBackSuccess: function (response) {
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable_menulistado",
                    table: "#menusListado",
                    tableColumnsData: response.data,
                    tableHeaderCheck: true,
                    tableHeaderCheckIndex: 0,
                    rowReorder: true,
                    columnDefs: [
                        { orderable: true, targets: [0, 1, 2, 3, 4, 5] }
                    ],
                    tableColumns: [
                        {
                            data: "menu_id",
                            title: "",
                            className: "text-center",
                            "bSortable": false,
                            "render": function (value) {
                                var check = '<input type="checkbox" class="form-check-input-styled-info chk_id_rol datatable-roles" data-id="' + value + '" name="chk[]">';
                                return check;
                            },
                            width: "100px",
                        },
                        {
                            data: "menu_id",
                            title: "ID",
                        },
                        {
                            data: "menu_orden",
                            title: "Orden",
                            name: "menu_orden",

                        },
                        {
                            data: "menu_titulo",
                            title: "Titulo"
                        },
                        {
                            data: "menu_url",
                            title: "URI"
                        },
                        {
                            data: "menu_estado",
                            title: "Estado",
                            "render": function (value) {
                                var estado = value;
                                var mensaje_estado = "";
                                if (estado === 'A') {
                                    estado = "success";
                                    mensaje_estado = "Activo";
                                } else {
                                    estado = "danger";
                                    mensaje_estado = "InActivo";
                                }
                                var span = '<span class="label label-sm label-' + estado + ' arrowed arrowed-righ">' + mensaje_estado + '</span>';
                                //var span = '<span class="badge badge-' + estado + '">' + mensaje_estado + '</span>';
                                return span;
                            }
                        },
                        {
                            data: "menu_id",
                            title: "Acciones",
                            "render": function (value, type, oData) {
                                var span = '';
                                var menu_id = value;
                                var span = '<div class="hidden-sm hidden-xs action-buttons"><a class="blue btn-detalle" href="#" data-id="' + menu_id + '"><i class="ace-icon fa fa-search-plus bigger-130"></i></a><a class="green btn-editar" href="#" data-id="' + menu_id + '"><i class="ace-icon fa fa-pencil bigger-130"></i></a><a class="red btn-eliminar" href="#" data-orden="' + oData.menu_orden + '" data-id="' + menu_id + '"><i class="ace-icon fa fa-trash-o bigger-130"></i></a></div><div class="hidden-md hidden-lg" ><div class="inline pos-rel"><button class="btn btn-minier btn-yellow dropdown-toggle" data-toggle="dropdown" data-position="auto"><i class="ace-icon fa fa-caret-down icon-only bigger-120"></i>   </button><ul class="dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close"><li><a href="#" class="tooltip-info btn-detalle" data-id="' + menu_id + '" data-rel="tooltip" title="View"><span class="blue"><i class="ace-icon fa fa-search-plus bigger-120"></i></span></a></li><li><a href="#" class="tooltip-success btn-editar" data-id="' + menu_id + '" data-rel="tooltip" title="Edit"><span class="green"><i class="ace-icon fa fa-pencil-square-o bigger-120"></i></span></a></li><li><a href="#" class="tooltip-error btn-eliminar" data-orden="' + oData.menu_orden + '" data-id="' + menu_id + '" data-rel="tooltip" title="Delete"><span class="red"><i class="ace-icon fa fa-trash-o bigger-120"></i></span></a>            </li></ul></div></div>';
                                return span;
                            }
                        }

                    ]
                });
                PanelMenus.init_ordenar();
                //_rowReordering();
            }
        });
    };
    var _componentes = function () {
        
        $(document).on("click", "#btn_nuevo", function (e) {
            $("#menu_id").val(0);
            $("#tituloModalMenu").text("Nuevo");
            $("#menu_titulo").prop('disabled', false);
            $("#menu_url").prop('disabled', false);
            $("#menu_orden").prop('disabled', false);
            $("#menu_estado").prop('disabled', false);
            $("#menu_blank").prop('disabled', false);

            $("#menu_titulo").val("");
            $("#menu_url").val("");
            $("#menu_orden").val(1);
            $("#menu_estado").val("A");
            $("#menu_blank").val("false");

            $(".btn-guardar").show();

            $("#modalFormulario").modal("show");
        });

        $(document).on('click', ".btn-guardar", function (e) {
            $("#form_menus").submit();
            if (_objetoForm_form_menus.valid()) {
                var dataForm = $('#form_menus').serializeFormJSON();
                responseSimple({
                    url: "IntranetMenu/IntranetMenuGuardarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        console.log(response);
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            PanelMenus.init_ListarMenus();
                            $("#modalFormulario").modal("hide");
                            //refresh(true);
                        }
                    }
                });
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        })

        $(document).on("click", ".btn-detalle", function () {
            var menu_id = $(this).data("id");
            console.log(menu_id);
            var dataForm = { menu_id: menu_id };
            responseSimple({
                url: "IntranetMenu/IntranetMenuIdObtenerJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    console.log(response);
                    //llenando datos en inputs
                    if (response.respuesta) {
                        var menu = response.data;
                        $("#tituloModalMenu").text("Detalle");
                        $("#menu_titulo").val(menu.menu_titulo);
                        $("#menu_url").val(menu.menu_url);
                        $("#menu_orden").val(menu.menu_orden);
                        $("#menu_estado").val(menu.menu_estado);
                        menu.menu_blank == false ? $("#menu_blank").val("false") : $("#menu_blank").val("true");

                        $("#menu_id").val(menu.menu_id);

                        $("#menu_titulo").prop('disabled', true);
                        $("#menu_url").prop('disabled', true);
                        $("#menu_orden").prop('disabled', true);
                        $("#menu_estado").prop('disabled', true);
                        $("#menu_blank").prop('disabled', true);
                        $(".btn-guardar").hide();

                        $("#modalFormulario").modal("show");
                    }
                }
            })
        })

        $(document).on("click", ".btn-editar", function () {
            var menu_id = $(this).data("id");
            var dataForm = { menu_id: menu_id };
            responseSimple({
                url: "IntranetMenu/IntranetMenuIdObtenerJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    console.log(response);
                    //llenando datos en inputs
                    if (response.respuesta) {
                        var menu = response.data;
                        $("#tituloModalMenu").text("Editar");
                        $("#menu_titulo").val(menu.menu_titulo);
                        $("#menu_url").val(menu.menu_url);
                        $("#menu_orden").val(menu.menu_orden);
                        $("#menu_estado").val(menu.menu_estado);
                        menu.menu_blank == false ? $("#menu_blank").val("false") : $("#menu_blank").val("true");
                        $("#menu_id").val(menu.menu_id);

                        $("#menu_titulo").prop('disabled', false);
                        $("#menu_url").prop('disabled', false);
                        $("#menu_orden").prop('disabled', false);
                        $("#menu_estado").prop('disabled', false);
                        $("#menu_blank").prop('disabled', false);
                        $(".btn-guardar").show();

                        $("#modalFormulario").modal("show");
                    }
                }
            })
        })

        $(document).on("click", ".btn-eliminar", function (e) {
            var menu_id = $(this).data("id");
            var menu_orden = $(this).data("orden");
            var celda = $("tr>td:nth-child(2)");
            console.log(celda);
            if (menu_id != "" || menu_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR este Menú?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetMenu/IntranetMenuEliminarJson",
                            data: JSON.stringify({
                                menu_id: menu_id,
                                menu_orden:menu_orden
                            }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                refresh(true);
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

        //checkAll
        $(document).on("click", ".chk_all", function (e) {
            console.log("click all");
            $(this).closest('table').find('tbody :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "tbody :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row

            $(this).closest('table').find('.chk_all').prop('checked', ($(this).closest('table').find('tbody :checkbox:checked').length == $(this).closest('table').find('tbody :checkbox').length)); //Tira / coloca a seleção no .checkAll
        })
        //Boton Eliminar Varios Menus
        $(document).on("click", "#btn_eliminar_varios", function (e) {
            let arrayMenus = [];
            $('#menusListado tbody tr input[type=checkbox]:checked').each(function () {
                arrayMenus.push($(this).data("id"));
            });
            if (arrayMenus.length > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR todos los Menús Seleccionados?',
                    callBackSAceptarComplete: function () {
                        var dataForm = { listaMenuEliminar: arrayMenus };
                        responseSimple({
                            url: "IntranetMenu/IntranetMenuEliminarVariosJson",
                            data: JSON.stringify(dataForm),
                            refresh: false,
                            callBackSuccess: function (response) {
                                refresh(true);
                            }
                        })
                    }
                });
            }
            else {
                messageResponse({
                    text: "Debe Seleccionar al menos un Menu",
                    type: "error"
                })
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

    var _ordenar = function () {
        _objetoDatatable_datatable_menulistado.on('row-reorder', function (e, diff, edit) {
            var result = 'Reorder started on row: ' + edit.triggerRow.data()[1] + '<br>';

            for (var i = 0, ien = diff.length; i < ien; i++) {
                var rowData = _objetoDatatable_datatable_menulistado.row(diff[i].node).data();

                result += rowData[1] + ' updated to be in position ' +
                    diff[i].newData + ' (was ' + diff[i].oldData + ')<br>';
            }

            $('#result').html('Event result:<br>' + result);
        });

    };


    var _rowReordering = function () {
        if ($.fn.DataTable.isDataTable("#menusListado")) {
            $("#menusListado").DataTable().destroy();
        }
        var table = $("#menusListado").DataTable({
            rowReorder: {
                selector:'tr>td:nth-child(2),tr>td:nth-child(3), tr>td:nth-child(4)'
            },
            columnDefs: [
                { orderable: true, className: 'reorder', targets: [0,1],visible:false },
                { orderable: false, targets: '_all' }
            ],
        });
        table
            .order([[4, 'asc']])
            .draw(false);
        table.on('row-reorder', function (e, diff, edit) {
            block_general("body");
            let arrayOrdenesMenus = [];
            for (var i = 0, ien = diff.length; i < ien; i++) {
                var rowData = table.row(diff[i].node).data();
                var obj = {
                    menu_orden: diff[i].newData,
                    menu_id: rowData[1]
                };
                arrayOrdenesMenus.push(obj);
            }
            var dataform = {
                arrayMenus: arrayOrdenesMenus,
            }
            if (arrayOrdenesMenus.length > 0) {
                responseSimple({
                    url: "IntranetMenu/IntranetMenuEditarOrdenJson",
                    data: JSON.stringify(dataform),
                    refresh: false,
                    callBackSuccess: function (response) {
                        refresh(true);
                        //PanelMenus.init_ListarMenus();
                        //CloseMessages();
                        //unblock("body");
                    }
                })
            }
            else {
                unblock("body");
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
            _metodos();

        },
        init_ListarMenus: function () {
            _ListarMenus();
        },
        init_ordenar: function () {
            _ordenar();
        }
    }
}();
// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelMenus.init();
});