var PanelSecciones = function () {
    var _inicio = function () {
        selectResponse({
            url: "IntranetTipoElemento/IntranetTipoElementoListarJson",
            select: "cboTipoElemento",
            campoID: "tipo_id",
            CampoValor: "tipo_nombre",
            select2: true,
            allOption: false
        });
        selectResponse({
            url: "IntranetTipoElemento/IntranetTipoElementoListarJson",
            select: "cboTipoElementoModal",
            campoID: "tipo_id",
            CampoValor: "tipo_nombre",
            select2: true,
            allOption: false
        });
        $("#btn_nuevo").prop('disabled', true);
        $("#btn_eliminar_varios").prop('disabled', true);
        if (menu.length > 0) {
            var appendMenus = '';
            var selectMenus = $("#cboMenus");
            selectMenus.html("");
            $.each(menu, function (index, value) {
                appendMenus += '<option value="' + value.menu_id + '">"' + value.menu_titulo + '"</option>';
            });
            selectMenus.html(appendMenus);
            selectMenus.select2();
        }
    };
    var _ListarSecciones = function () {
        var menu_id = $("#id_menu").val();
        var dataForm = {
            menu_id: menu_id,
        }
        responseSimple({
            url: "IntranetSeccion/IntranetSeccionListarTodoxMenuIDJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                $("#btn_nuevo").prop('disabled', false);
                $("#btn_eliminar_varios").prop('disabled', false);
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "seccionesListado",
                    table: "#seccionesListado",
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
                                var span =
                                    
                                    '<a href = "#" class="btn btn-white btn-primary btn-sm btn-round btn-editar-seccion" data-id="' + sec_id + '" data-rel="tooltip" title ="Editar">Editar</a> <a href="#"class="btn btn-white btn-success btn-sm btn-round btn-nuevo-elemento" data-id="' + sec_id + '" data-rel="tooltip"title="Nuevo Elemento">Nuevo</a> <a href="#"class="btn btn-white btn-danger btn-sm btn-round btn-eliminar-seccion" data-id="' + sec_id + '" data-rel="tooltip"title="Eliminar Seccion">Eliminar</a>';
                                return span;
                            }
                        }

                    ]
                })
            }
        })
        
    };
    var _componentes = function () {

        $(document).on("change", "#cboMenus", function () {
            var menu_id = $(this).val();
            if (menu_id != "") {
                $("#id_menu").val(menu_id);
                PanelSecciones.init_ListarSecciones();
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
                $("#sec_id").val(0);
                var menu_seleccionado = $('#cboMenus').val();
                $("#fk_menu").val(menu_seleccionado);
                $("#tituloModalSeccion").text("Nueva ");
                $("#sec_estado").prop('disabled', false);
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
        //$(document).on('click', ".btn-guardar", function (e) {
        //    $("#form_secciones").submit();
        //    if (_objetoForm_form_secciones.valid()) {
        //        var dataForm = $('#form_secciones').serializeFormJSON();
        //        responseSimple({
        //            url: "IntranetSeccion/IntranetSeccionInsertarJson",
        //            data: JSON.stringify(dataForm),
        //            refresh: false,
        //            callBackSuccess: function (response) {
        //                var respuesta = response.respuesta;
        //                if (respuesta) {
        //                    //limpiar_form({ contenedor: "#form_menus" });
        //                    //_objetoForm_form_menus.resetForm();
        //                    PanelSecciones.init_ListarSecciones();
        //                    //refresh(true);
        //                    $("#modalFormularioSeccion").modal("hide");
        //                }
        //            }
        //        });
        //    } else {
        //        messageResponse({
        //            text: "Complete los campos Obligatorios",
        //            type: "error"
        //        })
        //    }
        //});
        $(document).on('click', '.btn-guardar-seccion', function () {
            $("#form_seccion").submit();
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
                        $("#cboMenus").val($("#id_menu").val());
                        $("#cboMenus").change();
                        //PanelSecciones.init_ListarSecciones();
                    }
                    
                    $("#modalFormularioSeccion").modal("hide");
                },
            })
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
                            //$("#cboMenus").val($("#id_menu").val());
                            //$("#cboMenus").change();
                            //refresh(true);
                        }
                    });
                }
            });


        });

        $(document).on("click", ".btn-eliminar-seccion", function (e) {
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
                                if (response.respuesta) {
                                    PanelSecciones.init_ListarSecciones();
                                }
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
        });
        //Mostrar Detalle de Elemento si lo tuviera
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
            if (row.hasClass('detalle-oculto')) {
                row.removeClass('detalle-oculto');
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
                                var spanAgregarElementoModal = '';
                                var spanVerDetalleElemento = '';
                                if (value.fk_seccion_elemento > 0) {
                                    spanAgregarElementoModal = '<a href="#" class="btn btn-white btn-success btn-sm btn-round btn-nuevo-elemento-modal" data-seccionelemento="' + value.fk_seccion_elemento + '" data-rel="tooltip" title="Nuevo Detalle Elemento">Nuevo</a>';

                                    spanVerDetalleElemento += '<tr class="det-elemento' + value.detel_id + '"><td data-elemento="' + value.detel_id + '" data-id="' + value.fk_seccion_elemento + '" class="elemento-modal detalle-oculto"><a href="#" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a></td>';
                                }
                                else {
                                    spanAgregarElementoModal += '';
                                    spanVerDetalleElemento += '<tr><td></td>';
                                }
                                htmlTags += spanVerDetalleElemento;
                                htmlTags += '<td>' + value.detel_id + '</td>';
                                htmlTags += '<td>' + value.detel_descripcion + '</td>';
                                htmlTags += '<td>' + value.detel_nombre + '</td>';
                                htmlTags += '<td>' + value.detel_orden + '</td>';
                                htmlTags += '<td>' + value.detel_estado + '</td>';
                                htmlTags += '<td><a href="#" class="btn btn-white btn-primary btn-sm btn-round btn_editar_detalle_elemento" data-id="' + value.detel_id + '" data-rel="tooltip" title="Editar Detalle Elemento">Editar</a>' + spanAgregarElementoModal + '<a href="#" class="btn btn-white btn-danger btn-sm btn-round btn-eliminar-detalle-elemento" data-id="' + value.detel_id + '">Eliminar</a></td></tr>';
                            })
                            htmlTags += '</table></td></tr></fieldset>';
                            row.after(htmlTags);
                        }
                    },
                })
            } else {
                row.addClass('detalle-oculto');
                if (!rownext.hasClass('detalle-oculto')) {
                    rownext.remove();
                }
            }

        });

        //Listar Elementos que Abren Modales
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
                                /*Ver si tiene Detalle para Agregar*/
                                var fk_tipo_elemento = value.fk_tipo_elemento;
                                var spanTipoElemento = '';
                                var spanDetalleElemento = '';
                                if (fk_tipo_elemento == 1 || fk_tipo_elemento == 2 || fk_tipo_elemento == 3 || fk_tipo_elemento == 4) {
                                    spanTipoElemento = '';
                                    spanDetalleElemento = '<tr><td></td>';
                                }
                                else {
                                    spanTipoElemento = '<a href="#" class="btn btn-white btn-success btn-sm btn-round btn-nuevo-detalle-elemento-modal" data-tipo="' + value.fk_tipo_elemento + '" data-id="' + value.emod_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento">Nuevo</a>';
                                    spanDetalleElemento += '<tr class="elem-modal' + value.emod_id + '"><td data-id="' + value.emod_id + '" class="detalle-elemento-modal detalle-oculto"><a href="#" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a></td>';
                                }

                                //htmlTags += '<tr class="elem-modal' + value.emod_id + '"><td data-id="' + value.emod_id + '" class="detalle-elemento-modal detalle-oculto"><a href="#" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a></td>';
                                htmlTags += spanDetalleElemento;
                                htmlTags += '<td>' + value.emod_id + '</td>';
                                htmlTags += '<td>' + value.emod_titulo + '</td>';
                                htmlTags += '<td>' + value.tipo_nombre + '</td>';
                                htmlTags += '<td>' + value.emod_orden + '</td>';
                                htmlTags += '<td>' + value.emod_estado + '</td>';
                                htmlTags += '<td><a href="#" class="btn btn-white btn-primary btn-sm btn-round btn_editar_detalle_elemento_modal" data-id="' + value.emod_id + '" data-rel="tooltip" title="Editar">Editar</a>' + spanTipoElemento + '<a href="#" class="btn btn-white btn-danger btn-sm btn-round btn-eliminar-elemento-modal" data-id="' + value.emod_id + '">Eliminar</a></td></tr>';
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

        //Mostrar Detalles de Elementos que Abren Modales
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
                                htmlTags += '<td><a class="btn btn-white btn-primary btn-sm btn-round btn-editar-detalle-elemento-modal" data-id="' + value.detelm_id + '">Editar</a> <a class="btn btn-white btn-danger btn-sm btn-round btn-eliminar-detalle-elemento-modal" data-id="' + value.detelm_id + '">Eliminar</a></td></tr>';
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

        //Botones para abrir modal de nuevo elemento y evento para guardar nuevo elemento
        $(document).on('click', '.btn-nuevo-elemento', function () {
            var sec_id = $(this).data('id');
            $("#fk_seccion").val(sec_id);
            $("#elem_id").val(0);
            $("#modalFormularioElemento").modal("show");
        });
        $(document).on('click', '.btn-guardar-elemento', function () {
            $("#form_elemento").submit();
            var dataForm = $('#form_elemento').serializeFormJSON();
            var url = '';
            if ($("#elem_id").val() == 0) {
                url = 'IntranetElemento/IntranetElementoInsertarJson';
            }
            responseSimple({
                url: url,
                refresh: false,
                data: JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    PanelSecciones.init_ListarSecciones();
                    $("#modalFormularioElemento").modal("hide");
                }
            });
        });
        $(document).on('click', '.btn_eliminar_elemento', function () {
            var elem_id = $(this).data("id");
            var dataForm = {
                elem_id: elem_id,
            }
            responseSimple({
                url: 'IntranetElemento/IntranetElementoElementoEliminarJson',
                refresh: false,
                data: JSON.stringify(dataForm),
                callbackSuccess: function (response) {
                    if (response.respuesta) {
                        $(".details-control").trigger('click');
                        $(".details-control").trigger('click');
                    }
                },
            })
        });
        $(document).on('change', '#cboOpcion', function () {
            if ($(this).val() == 1) {
                $(".detel-imagen").hide();
            } else {
                $(".detel-imagen").show();
            }
        })


        //Botones para abrir modal de nuevo detalle Elemento (Caso listas de texto, citas, imagenes) y evento para agregar un nuevo Detalle de Elemento
        $(document).on('click', '.btn-nuevo-detalle-elemento', function () {
            var elem_id = $(this).data("id");
            var tipo_elemento = $(this).data("tipo");
            $("#fk_elemento").val(elem_id);
            $("#detel_id").val(0);
            $(".detel-imagen").show();
            $(".detel-nombre").show();
            $(".detel-posicion").show();
            if (tipo_elemento == 5 || tipo_elemento == 6) {
                $("#fk_seccion_elemento").val(2);
                $(".detel-imagen").hide();
                $(".detel-nombre").hide();
                $(".detel-posicion").hide();
            }
            //Abren Modales fk_seccion_elemento en int_detalle_elemento debe tener un id
            else if (tipo_elemento==8||tipo_elemento == 13 || tipo_elemento == 14 || tipo_elemento == 15 || tipo_elemento == 16) {
                //va a abrir modal
                $("#fk_seccion_elemento").val(1);
                $(".detel-imagen").hide();
            }
            else {
                $("#fk_seccion_elemento").val(0);
                $(".detel-posicion").hide();
            }
            $("#modalFormularioDetalleElemento").modal("show");
        })
        $(document).on('click', '.btn-guardar-detalle-elemento', function () {
            //$("#form_detalle_elemento").submit();
            var dataForm = new FormData(document.getElementById("form_detalle_elemento"));
            var url = '';
            if ($("#detel_id").val() == 0) {
                url = 'IntranetDetalleElemento/IntranetDetalleElementoInsertarJson';
            }
            responseFileSimple({
                url: url,
                data: dataForm,
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        $("#modalFormularioDetalleElemento").modal("hide");
                    }
                    $(".detalle-elemento").trigger('click');
                    $(".detalle-elemento").trigger('click');
                    //PanelSecciones.init_ListarSecciones();
                }
            })
        });
        $(document).on('click', '.btn-eliminar-detalle-elemento', function () {
            var detel_id = $(this).data("id");
            var dataForm = {
                detel_id: detel_id,
            }
            responseSimple({
                url: 'IntranetDetalleElemento/IntranetDetalleElementoEliminarJson',
                refresh: false,
                data: JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        $(".detalle-elemento").trigger('click');
                        $(".detalle-elemento").trigger('click');
                    }
                },
            })
        })

        //Botones para abrir modal de Nuevo Elemento Modal y evento para agregar un Nuevo elemento al Modal
        $(document).on('click', '.btn-nuevo-elemento-modal', function () {
            var fk_seccion_elemento = $(this).data('seccionelemento');
            $("#fk_seccion_elemento_modal").val(fk_seccion_elemento);
            $("#emod_id").val(0);
            //Deshabilitar los tipos de elemento que abren modales
            $('#cboTipoElementoModal option[value="8"]').prop('disabled', true);
            $('#cboTipoElementoModal option[value="13"]').prop('disabled', true);
            $('#cboTipoElementoModal option[value="14"]').prop('disabled', true);
            $('#cboTipoElementoModal option[value="15"]').prop('disabled', true);
            $('#cboTipoElementoModal option[value="16"]').prop('disabled', true);
            $("#modalFormularioElementoModal").modal("show");
        });
        $(document).on('click', '.btn-guardar-elemento-modal', function () {
            //$("#form_elemento_modal").submit();
            var dataForm = $("#form_elemento_modal").serializeFormJSON();
            var url = '';
            if ($("#emod_id").val() == 0) {
                url = 'IntranetElementoModal/IntranetElementoModalInsertarJson';
            }
            responseSimple({
                url: url,
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        $("#modalFormularioElementoModal").modal("hide");
                    }
                    $(".elemento-modal").trigger('click');
                    $(".elemento-modal").trigger('click');
                    //PanelSecciones.init_ListarSecciones();
                },
            });
        });
        $(document).on('click', '.btn-eliminar-elemento-modal', function () {
            var emod_id = $(this).data("id");
            var dataForm = {
                emod_id: emod_id,
            }
            responseSimple({
                url: 'IntranetElementoModal/IntranetElementoModalEliminarJson',
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        $(".elemento-modal").trigger('click');
                        $(".elemento-modal").trigger('click');
                    }
                }
            })
        });

        //Botones para agregar un nuevo Detalle de Elemento al Modal(caso listas de texto, citas o Imagenes) y evento para guardar ese detalle
        $(document).on('click', '.btn-nuevo-detalle-elemento-modal', function () {
            var emod_id = $(this).data("id");
            var tipo_elemento = $(this).data("tipo");

            $("#fk_elemento_modal").val(emod_id);
            $("#detelm_id").val(0);
            $(".detelm-imagen").show();
            $(".detelm-nombre").show();
            $(".detelm-posicion").show();
            debugger;
            if (tipo_elemento == 5 || tipo_elemento == 6) {
                $("#fk_seccion_elemento_modal_").val(2);
                $(".detelm-imagen").hide();
                $(".detelm-nombre").hide();
                $(".detelm-posicion").hide();
            }
            else if (tipo_elemento == 8 || tipo_elemento == 13 || tipo_elemento == 14 || tipo_elemento == 15 || tipo_elemento == 16) {
                //va a abrir modal
                $("#fk_seccion_elemento_modal_").val(1);
                $(".detel-imagen").hide();
            }
            else {
                $("#fk_seccion_elemento_modal_").val(0);
                $(".detelm-posicion").hide();
            }
            $("#modalFormularioDetalleElementoModal").modal("show");
            
        })
        $(document).on('click', '.btn-guardar-detalle-elemento-modal', function () {
            //$("#form_detalle_elemento_modal").submit();
            var dataForm = new FormData(document.getElementById("form_detalle_elemento_modal"));
            var url = '';
            if ($("#detel_id").val() == 0) {
                url = 'IntranetDetalleElementoModal/IntranetDetalleElementoModalInsertarJson';
            }
            responseFileSimple({
                url: url,
                data: dataForm,
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        $("#modalFormularioDetalleElementoModal").modal("hide");
                    }
                    $(".detalle-elemento-modal").trigger('click');
                    $(".detalle-elemento-modal").trigger('click');
                   
                    //PanelSecciones.init_ListarSecciones();
                }
            })
        })
        $(document).on('click', '.btn-editar-detalle-elemento-modal', function () {
            var detelm_id = $(this).data("id");
            var dataForm = {
                detelm_id:detelm_id,
            }
            responseSimple({
                url: 'IntranetDetalleElementoModal/IntranetDetalleElementoModalIdObtenerJson',
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    console.log(response);
                }
            })
            //$("#modalFormularioDetalleElementoModal").modal("show");
        })
        $(document).on('click', '.btn-eliminar-detalle-elemento-modal', function () {
            var detelm_id = $(this).data("id");
            var dataForm = {
                detelm_id: detelm_id,
            }
            responseSimple({
                url: 'IntranetDetalleElementoModal/IntranetDetalleElementoModalEliminarJson',
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    $(".detalle-elemento-modal").trigger('click');
                    $(".detalle-elemento-modal").trigger('click');
                }
            })
        })
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
            _inicio();
        },
        init_ListarSecciones: function () {
            _ListarSecciones();
        }
    }
}();
function MostrarDetalle(row) {
    var table = '';
    if (row.length > 0) {
        table += '<table class="table table-bordered table-sm"><tr><th colspan="7"><legend>Elementos</legend></th></tr><tr class="thead-dark">';
        table += '<th>Detalle</th>';
        table += '<th>ID</th>';
        table += '<th>titulo</th>';
        table += '<th>Estado</th>';
        table += '<th>Orden</th>';
        table += '<th>Tipo</th>';
        table += '<th>Acciones</th></tr>';
        $.each(row, function (index, value) {
            var fk_tipo_elemento = value.fk_tipo_elemento;
            var spanTipoElemento = '';
            var spanDetalleElemento = '';
            var spanEditarElemento = '';
            var spanEliminarElemento = '';
            if (fk_tipo_elemento == 1 || fk_tipo_elemento == 2 || fk_tipo_elemento == 3 || fk_tipo_elemento == 4) {
                spanTipoElemento = '';
                spanDetalleElemento = '<tr><td></td>';
                spanEditarElemento = '<a href="#" class="btn btn-white btn-primary btn-sm btn-round btn_editar_elemento" data-id="' + value.elem_id + '" data-rel="tooltip" title="Editar">Editar</a>';
                spanEliminarElemento = '<a  href="#" class="btn btn-white btn-danger btn-sm btn-round btn_eliminar_elemento" data-id="' + value.elem_id + '" data-rel="tooltip" title="eliminar">Eliminar</a>';
            }
            else if (fk_tipo_elemento == 8 || fk_tipo_elemento == 13 || fk_tipo_elemento == 14 || fk_tipo_elemento == 15 || fk_tipo_elemento == 16) {
                spanTipoElemento = '<a href="#" class="btn btn-white btn-success btn-sm btn-round btn-nuevo-detalle-elemento" data-tipo="' + value.fk_tipo_elemento + '" data-id="' + value.elem_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento">Nuevo</a>';
                spanDetalleElemento += '<tr class="elemento' + value.elem_id + ' detalle-oculto"><td data-id="' + value.elem_id + '" class="detalle-elemento ">' + '<a href="#" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a>' + '</td>';
                spanEliminarElemento = '<a  href="#" class="btn btn-white btn-danger btn-sm btn-round btn_eliminar_elemento" data-id="' + value.elem_id + '" data-rel="tooltip" title="eliminar">Eliminar</a>';
            }
            else {
                spanTipoElemento = '<a href="#" class="btn btn-white btn-success btn-sm btn-round btn-nuevo-detalle-elemento" data-tipo="' + value.fk_tipo_elemento + '" data-id="' + value.elem_id + '" data-rel="tooltip" title="Nuevo Detalle Elemento">Nuevo</a>';
                spanDetalleElemento += '<tr class="elemento' + value.elem_id + ' detalle-oculto"><td data-id="' + value.elem_id + '" class="detalle-elemento ">' + '<a href="#" class="tooltip-info "  data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a>' + '</td>';
                spanEditarElemento = '<a href="#" class="btn btn-white btn-primary btn-sm btn-round btn_editar_elemento" data-id="' + value.elem_id + '" data-rel="tooltip" title="Editar">Editar</a>';
                spanEliminarElemento = '<a  href="#" class="btn btn-white btn-danger btn-sm btn-round btn_eliminar_elemento" data-id="' + value.elem_id + '" data-rel="tooltip" title="eliminar">Eliminar</a>';
            }

            table += spanDetalleElemento;
            table += '<td>' + value.elem_id + '</td>';
            table += '<td>' + value.elem_titulo + '</td>';
            table += '<td>' + (value.elem_estado == 'A' ? 'Activo' : 'Inactivo') + '</td>';
            table += '<td>' + value.elem_orden + '</td>';
            table += '<td>' + value.tipo_nombre + '</td>';
            table += '<td>' + spanEditarElemento + spanTipoElemento + spanEliminarElemento + '</td></tr>';
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