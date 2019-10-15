var MesaPartes = function () {
    var total_menus = 0;
    //var lista_unchecked_menus = [];
    var _inicio = function () {
        $('#myDatepicker1').datetimepicker({
            format: 'YYYY-MM-DD',
            ignoreReadonly: true,
            allowInputToggle: true
        });
        $('#myDatepicker2').datetimepicker({
            format: 'YYYY-MM-DD',
            ignoreReadonly: true,
            allowInputToggle: true,
            useCurrent: false
        });
        selectResponse({
            url: "Proveedor/MesaPartesListarCompaniasJson",
            select: "cboCompania",
            campoID: "cia_codigo",
            CampoValor: "cia_nombre",
            select2: true,
            allOption: false,
            placeholder: "Seleccione Compañia"
        });
    };
    var _componentes = function () {
        $("#myDatepicker1").on("dp.change", function (e) {
            $('#myDatepicker2').data("DateTimePicker").minDate(e.date);
        });
        $("#myDatepicker2").on("dp.change", function (e) {
            $('#myDatepicker1').data("DateTimePicker").maxDate(e.date);
        });
        $(document).on("click", ".btn_filtrar", function (e) {
            $("#frmReportePagos-form").submit();
            if (_objetoForm_frmReportePagos.valid()) {
                var nombre_tabla = $("#cboCompania").val();
                var dataForm = $('#frmReportePagos-form').serializeFormJSON();

                if (!$().DataTable) {
                    console.warn('Advertencia - datatables.min.js no esta declarado.');
                    return;
                }
                simpleAjaxDataTable({
                    uniform: true,
                    ajaxUrl: "Proveedor/ListarPagosporCompaniaJson",
                    ajaxDataSend: dataForm,
                    tableNameVariable: "permiso",
                    tableHeaderCheck: false,
                    table: "#tablepermiso",
                    tableColumns: [
                        {
                            data: "CP_CVANEXO",
                            title: "CP_CVANEXO",
                        },
                        {
                            data: "CP_CCODIGO",
                            title: "CP_CCODIGO"
                        },
                        {
                            data: "CP_CNUMDOC",
                            title: "CP_CNUMDOC"
                        },
                        
                        {
                            data: "CP_NIMPOUS",
                            title: "CP_NIMPOUS"
                        },
                        {
                            data: "CP_NIMPOMN",
                            title: "CP_NIMPOMN"
                        },

                        {
                            data: "CP_NSALDMN",
                            title: "CP_NSALDMN"
                        },
                        {
                            data: "CP_NSALDUS",
                            title: "CP_NSALDUS"
                        },
                        {
                            data: "subtotal",
                            title: "subtotal"
                        },
                        {
                            data: "subtotal",
                            title: "subtotal",
                            "render": function (value, type, oData, meta) {
                                var pagado = value;
                                var mensaje_estado = "";
                                if (pagado == oData.CP_NIMPOMN) {
                                    estado = "success";
                                    mensaje_estado = "PAGADO";
                                } else if (pagado == 0) {
                                    estado = "danger";
                                    mensaje_estado = "PENDIENTE";
                                }
                                else {
                                    estado = "warning";
                                    mensaje_estado = "PARCIAL";
                                }
                                var span = '<span class="badge badge-' + estado + '">' + mensaje_estado + '</span>';
                                return span;
                            }
                        },
                        {
                            data: "CP_DFECDOC",
                            title: "CP_DFECDOC",
                            "render": function (value) {
                                var span = '<span>' + moment(value).format("DD/MM/YYYY") + '</span>';
                                return span;
                            }
                        },

                        {
                            data: 'CP_CNUMDOC',
                            title: "Acciones",
                            className: 'text-center',
                            "bSortable": false,
                            "render": function (value, type, oData, meta) {
                                var boton =
                                    '<a href="#" class="btn btn-success btn_detalle" data-toggle="modal" data-target=".bs-example-modal-detalle" data-numdoc="' + value + '" data-tabla="' + nombre_tabla + '"> Ver Detalle</a>'
                                    ;
                                return boton;
                            }
                        }

                    ]
                });
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }

         
   
        });

    };
    var _detalles = function () {
        $(document).on("click", ".btn_detalle", function (e) {
            var num_doc = $(this).data("numdoc");
            var nombre_tabla = $(this).data("tabla");
            var dataForm = { num_doc: num_doc, nombre_tabla: nombre_tabla };
            if (!$().DataTable) {
                console.warn('Advertencia - datatables.min.js no esta declarado.');
                return;
            }
            simpleAjaxDataTable({
                uniform: true,
                ajaxUrl: "Proveedor/ListarPagosporNumeroDocumentoJson",
                ajaxDataSend:dataForm,
                tableNameVariable: "detalle",
                tableHeaderCheck: false,
                table: "#table-detalle",
                tableColumns: [
                    {
                        data: "PG_CVANEXO",
                        title: "PG_CVANEXO",
                    },
                    {
                        data: "PG_CCODIGO",
                        title: "PG_CCODIGO"
                    },
                    {
                        data: "PG_CTIPDOC",
                        title: "PG_CTIPDOC"
                    },
                    {
                        data: "PG_CNUMDOC",
                        title: "PG_CNUMDOC"
                    },

                    {
                        data: "PG_NIMPOMN",
                        title: "PG_NIMPOMN"
                    },
                    {
                        data: "PG_NIMPOUS",
                        title: "PG_NIMPOUS"
                    },
                    {
                        data: "PG_CGLOSA",
                        title: "PG_CGLOSA"
                    }
                ]
            });
            //responseSimple({
            //    url: "Proveedor/ListarPagosporNumeroDocumentoJson",
            //    data: JSON.stringify(dataForm),
            //    refresh: false,
            //    callBackSuccess: function (response) {
            //        console.info(response);
            //    }
            //});
            
        });
    };
    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmReportePagos',
            contenedor: '#frmReportePagos-form',
            rules: {
                fecha_final:
                {
                    required: true,

                },
                fecha_inicio:
                {
                    required: true,

                },
                cboCompania:
                {
                    required: true,

                }

            },
            messages: {
                fecha_final:
                {
                    required: 'Campo Obligatorio',
                },
                fecha_inicio:
                {
                    required: 'Campo Obligatorio',
                },
                cboCompania:
                {
                    required: 'Campo Obligatorio',
                },
            }
        });

    };
    return {
        init: function () {
            _metodos();
            _detalles();
            _inicio();
            _componentes();
        }
    }
}();
document.addEventListener('DOMContentLoaded', function () {
    MesaPartes.init();
});