var MesaPartes = function () {
    var total_menus = 0;
    //var lista_unchecked_menus = [];
    var _inicio = function () {
      
        var hoy = new Date();
        var ayer = new Date(hoy.getTime() - 24 * 60 * 60 * 1000);
        var fecha_hoy = moment(hoy).format('YYYY-MM-DD');
        var fecha_ayer = moment(ayer).format('YYYY-MM-DD');
        $("#fecha_final").val(fecha_hoy);
        $("#fecha_inicio").val(fecha_ayer);
        $('#myDatepicker1').datetimepicker({
            format: 'YYYY-MM-DD',
            ignoreReadonly: true,
            allowInputToggle: true,
            
        });
        $('#myDatepicker2').datetimepicker({
            format: 'YYYY-MM-DD',
            ignoreReadonly: true,
            allowInputToggle: true,
            useCurrent: false,
           
        });
        selectResponse({
            url: "Proveedor/MesaPartesListarCompaniasJson",
            select: "cboCompania",
            campoID: "cia_codigo",
            CampoValor: "cia_nombre",
            select2: true,
            allOption: false,
            placeholder: "Seleccione Empresa"
        });
    };
    var _componentes = function () {
        $(document).on('click', ".btn_excel", function (e) {
            var fecha_inicio=$("#fecha_inicio_excel").val();
            var fecha_fin=$("#fecha_fin_excel").val();
            var nombre_tabla = $("#nombre_tabla_excel").val();
            ///Listing/%20GetByList?name=John&contact=calgary%2C%20vancouver
            window.location.href = basePath + "Proveedor/ReportePagosExportarExcel?fecha_inicio=" + fecha_inicio + "&fecha_fin=" + fecha_fin + "&nombre_tabla=" + nombre_tabla;
        });
        $("#myDatepicker1").on("dp.change", function (e) {
            $('#myDatepicker2').data("DateTimePicker").minDate(e.date);
        });
        $("#myDatepicker2").on("dp.change", function (e) {
            $('#myDatepicker1').data("DateTimePicker").maxDate(e.date);
        });
        $(document).on("click", ".btn_filtrar", function (e) {
            $("#frmReportePagos-form").submit()
            if (_objetoForm_frmReportePagos.valid()) {
                var nombre_tabla = $("#cboCompania").val();
                var dataForm = $('#frmReportePagos-form').serializeFormJSON();
                $("#fecha_inicio_excel").val($("#fecha_inicio").val());
                $("#fecha_fin_excel").val($("#fecha_final").val());
                $("#nombre_tabla_excel").val($("#cboCompania").val());
                $("#btn_excel_mostrar").show();
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
                    columnDefs: [
                        { className: 'text-center', targets: [3, 8] },
                        { className: 'text-right', targets: [5, 6,7] },
                    ],
                    tableColumns: [
                        {
                            data: "CP_CVANEXO",
                            title: "Tipo Anexo",
                        },
                        {
                            data: "CP_CCODIGO",
                            title: "RUC"
                        },
                        {
                            data: "CP_CNUMDOC",
                            title: "Nro. Documento"
                        },
                        
                        {
                            data: "CP_DFECDOC",
                            title: "Fecha Documento",
                            "render": function (value) {
                                var span = '<span>' + moment(value).format("DD/MM/YYYY") + '</span>';
                                return span;
                            }
                        },
                        {
                            data: "CP_CCODMON",
                            title: "Moneda",
                            "render": function (value) {
                                var moneda = "";
                                if (value == 'MN') {
                                    moneda = "Soles";
                                }
                                else {
                                    moneda = "Dolares";
                                }
                                return moneda;
                            }
                        },
                        {
                            data: "CP_CCODMON",
                            title: "Importe",
                            "render": function (value, type, oData, meta) {
                                if (value == 'MN') {
                                    return oData.CP_NIMPOMN;
                                }
                                else {
                                    return oData.CP_NIMPOUS;
                                }
                            }
                        },
                        {
                            data: "CP_CCODMON",
                            title: "Monto Pagado",
                            "render": function (value, type, oData, meta) {
                                if (value == "MN") {
                                    return oData.subtotalSoles;
                                }
                                else {
                                    return oData.subtotalDolares;
                                }
                            }
                        },
                        {
                            data: "CP_CCODMON",
                            title: "Saldo",
                            "render": function (value, type, oData, meta) {
                                var montoPagado = 0;
                                var importe = 0;
                                var saldo = 0;
                                if (value == "MN") {
                                    importe = oData.CP_NIMPOMN;
                                    montoPagado = oData.subtotalSoles;
                                   
                                }
                                else {
                                    importe = oData.CP_NIMPOUS;
                                    montoPagado = oData.subtotalDolares;
                                   
                                }
                                saldo = importe - montoPagado;
                                return saldo;
                            }
                        },
                        {
                            data: "CP_CCODMON",
                            title: "ESTADO",
                            "render": function (value, type, oData, meta) {
                                if (value == "MN") {
                                    var pagado = oData.subtotalSoles;
                                    var mensaje_estado = "";
                                    if (pagado == oData.CP_NIMPOMN) {
                                        estado = "success";
                                        mensaje_estado = "PAGADO";
                                    }
                                    else if (pagado == 0) {
                                        estado = "danger";
                                        mensaje_estado = "PENDIENTE";
                                    }
                                    else if (pagado != 0 && pagado < oData.CP_NIMPOMN) {
                                        estado = "warning";
                                        mensaje_estado = "PARCIAL";
                                    }
                                }
                                else {
                                    var pagado = oData.subtotalDolares;
                                    var mensaje_estado = "";
                                    if (pagado == oData.CP_NIMPOUS) {
                                        estado = "success";
                                        mensaje_estado = "PAGADO";
                                    }
                                    else if (pagado == 0) {
                                        estado = "danger";
                                        mensaje_estado = "PENDIENTE";
                                    }
                                    else if (pagado != 0 && pagado < oData.CP_NIMPOUS) {
                                        estado = "warning";
                                        mensaje_estado = "PARCIAL";
                                    }
                                }
                                var span = '<span class="label label-' + estado + '">' + mensaje_estado + '</span>';
                                return span;
                            }
                        },
                        {
                            data: 'CP_CNUMDOC',
                            title: "Acciones",
                            className: 'text-center',
                            "bSortable": false,
                            "render": function (value, type, oData, meta) {
                                var subtotal = 0;
                                if (oData.CP_CCODMON == "MN") {
                                    subtotal = oData.subtotalSoles;
                                }
                                else {
                                    subtotal = oData.subtotalDolares;
                                }
                                var boton =
                                    '<a href="#" class="btn btn-dark btn_detalle btn-sm" data-toggle="modal" data-target=".bs-example-modal-detalle" data-moneda="' + oData.CP_CCODMON+'" data-numdoc="' + value + '" data-tabla="' + nombre_tabla + '" data-subtotal="' + subtotal+'" data-fecha="'+oData.CP_CFECCOM+'" data-cuenta="'+(oData.CP_CCUENTA)+'"> Ver Detalle</a>'
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
            var subtotal = $(this).data("subtotal");
            var moneda = $(this).data("moneda");
            var fecha_compra=$(this).data("fecha");
            var cuenta=$(this).data("cuenta");
            var dataForm = { num_doc: num_doc, nombre_tabla: nombre_tabla,fecha_compra:fecha_compra,cuenta:cuenta };
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
                columnDefs: [
                    { className: 'text-center', targets: [6] },
                ],
                tableColumns: [
                    {
                        data: "PG_CVANEXO",
                        title: "T. Anexo",
                    },
                    {
                        data: "PG_CCODIGO",
                        title: "RUC"
                    },
                    {
                        data: "PG_CTIPDOC",
                        title: "Tipo de Doc"
                    },
                    {
                        data: "PG_CNUMDOC",
                        title: "Nro. Doc"
                    },
                    {
                        data: "PG_CCODMON",
                        title: "Moneda",
                        "render": function (value) {
                            var moneda = "";
                            if (value == "MN") {
                                moneda="Soles";
                            }
                            else {
                                moneda = "Dolares";
                            }
                            return moneda;
                        }
                    },
                    {
                        data: "PG_CCODMON",
                        title: "Importe",
                        "render": function (value, type, oData, meta) {
                            if (value == "MN") {
                                return oData.PG_NIMPOMN;
                            }
                            else {
                                return oData.PG_NIMPOUS;
                            }
                        }
                    },
                    {
                        data: "PG_DFECCOM",
                        title: "Fecha de Pago",
                        "render": function (value) {
                            var span = '<span>' + moment(value).format("DD/MM/YYYY") + '</span>';
                            return span;
                        }
                    },
                    {
                        data: "PG_CGLOSA",
                        title: "GLOSA"
                    },
                    {
                        data: "DNUMDOR",
                        title: "NRO. CONSTANCIA"
                    }
                ]
            });
            if (moneda == "MN") {
                $("#moneda").text("Soles : S/ ");
            }
            else {
                $("#moneda").text("Dolares : $ ");
            }
            $("#subtotal").text(subtotal);
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
    $(document).on("click", ".btn_cancelar", function (e) {
        _objetoForm_frmReportePagos.resetForm();
    });
    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmReportePagos',
            contenedor: '#frmReportePagos-form',
            rules: {
                fecha_final:
                {
                    required: true
                },
                fecha_inicio:
                {
                    required: true
                },
                cboCompania:
                {
                    required: true
                }
            },
            messages: {
                fecha_final:
                {
                    required: 'Campo Obligatorio'
                },
                fecha_inicio:
                {
                    required: 'Campo Obligatorio'
                },
                cboCompania:
                {
                    required: 'Campo Obligatorio'
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