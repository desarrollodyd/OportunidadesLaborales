var PanelPrincipal = function () {
    var _inicio = function () {
        selectResponse({
            url: "Proveedor/UsuarioProveedorListarJson",
            select: "cboUsuario",
            campoID: "usu_id",
            CampoValor: "usu_nombre",
            select2: true,
            allOption: false,
            placeholder: "Seleccione Usuario"
        });

        mostrarFechayHora();
    };
    var mostrarFechayHora = function () {
        if ($("#fechaHoy").length) {
            var today = moment().format('DD/MM/YYYY');
            document.getElementById("fechaHoy").innerHTML = today;
            window.onload = show5();
        };
    };
    var _componentes = function () {
        $('#cboUsuario').change(function () {
            usu_id = $(this).val();
            lista_checked = [];
            ListarMenus(usu_id);
        });

        $("#profile-tab2").click(function () {
            if (!$().DataTable) {
                console.warn('Advertencia - datatables.min.js no esta declarado.');
                return;
            }

            simpleAjaxDataTable({
                uniform: true,
                ajaxUrl: "Proveedor/UsuarioProveedorListarJson",
                tableNameVariable: "roles",
                tableHeaderCheck: true,
                table: ".datatable-roles",
                tableColumns: [{
                    data: "usu_id",
                    title: "",
                    "bSortable": false,
                    "render": function (value) {
                        var check = '<input type="checkbox" class="form-check-input-styled-info chk_id_rol datatable-roles" data-id="' + value + '" name="">';
                        return check;
                    }
                },
                {
                    data: "usu_id",
                    title: "ID",
                },
                {
                    data: "usu_nombre",
                    title: "Nombre"
                },
                {
                    data: "usu_tipo",
                    title: "Descripcion"
                },
                {
                    data: "usu_estado",
                    title: "Estado",
                    "render": function (value) {
                        var estado = value;
                        var mensaje_estado = "";
                        if (estado == 'A') {
                            estado = "success";
                            mensaje_estado = "Activo";
                        } else {
                            estado = "danger";
                            mensaje_estado = "InActivo";
                        }
                        var span = '<span class="badge badge-' + estado + '">' + mensaje_estado + '</span>';
                        return span;
                    }

                },
                {
                    data: 'usu_id',
                    title: "Acciones",
                    width: 100,
                    className: 'text-center',
                    "bSortable": false,
                    "render": function (value, type, oData, meta) {
                        var botones = '<div class="list-icons">' +
                            '<div class="dropdown">' +
                            '<a href="#" class="list-icons-item" data-toggle="dropdown">' +
                            '<i class="icon-menu9"></i>' +
                            '</a>' +
                            '<div class="dropdown-menu dropdown-menu-right">' +
                            '<a href="#" class="dropdown-item btn_editar" data-id="' + value + '"><i class="icon-hammer"></i> Editar</a>' +
                            '<a href="#" class="dropdown-item btn_estado" data-id="' + value + '" data-estado="' + oData.usu_estado + '"><i class="icon-circles"></i> Cambiar Estado</a>' +
                            '</div>' +
                            '</div>' +
                            '</div>';
                        return botones;
                    }
                }
                ]
            })
            //responseSimple({
            //    url: "Proveedor/UsuarioProveedorListarJson",
            //    refresh: false,
            //    callBackSuccess: function (response) {
            //        console.log(response);
            //    }
            //});

        });
    };
    return {
        init: function () {
            _inicio();
            _componentes();
            mostrarFechayHora();
        }
    }
}();
function activarCheckBox() {
    if ($("input.flat")[0]) {
        $(document).ready(function () {
            $('input.flat').iCheck({
                checkboxClass: 'icheckbox_flat-green',
                radioClass: 'iradio_flat-green'
            });
        });
    }
}
function show5() {
    if (!document.layers && !document.all && !document.getElementById)
        return;

    var Digital = new Date();
    var hours = Digital.getHours();
    var minutes = Digital.getMinutes();
    var seconds = Digital.getSeconds();

    var dn = "PM";
    if (hours < 12)
        dn = "AM";
    if (hours > 12)
        hours = hours - 12;
    if (hours == 0)
        hours = 12;

    if (minutes <= 9)
        minutes = "0" + minutes;
    if (seconds <= 9)
        seconds = "0" + seconds;
    //change font size here to your desire
    myclock = "<b>" + hours + ":" + minutes + ":"
        + seconds + " " + dn + "</b>";
    if (document.layers) {
        document.layers.liveclock.document.write(myclock);
        document.layers.liveclock.document.close();
    }
    else if (document.all)
        liveclock.innerHTML = myclock;
    else if (document.getElementById)
        document.getElementById("liveclock").innerHTML = myclock;
    setTimeout("show5()", 1000);
};
function ListarMenus(usu_id) {
    var dataForm = {
        usu_id: usu_id
    };
    var url = basePath + "Super/PermisosListarJson";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(dataForm),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
            CheckTodosMenus();
        },
        success: function (response) {
            total_menus = response.data;
            var data_lista_menu = response.data_lista_menu;
            lista_checked = [];
            $.each(data_lista_menu, function (key, value) {
                lista_checked.push(value.fk_submenu);
            });
            objetodatatable = $("#table").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "paging": true,
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                "initComplete": function (settings, json) {
                    //   afterTableInitialization(settings,json)
                    $('button#excel,a#pdf,a#imprimir').off("click").on('click', function () {
                        ocultar = ["Accion"];//array de columnas para ocultar , usar titulo de columna
                        columna_cambio = [{
                            nombre: "Estado",
                            render: function (o) {
                                valor = "";
                                if (o == 1) {
                                    valor = "Habilitado";
                                }
                                else { valor = "Deshabilitado"; }
                                return valor;
                            }
                        }]
                        cabecerasnuevas = [];
                        //cabecerasnuevas.push({ nombre: "cabecera", valor: "vdfcs" });
                        //tituloreporte = "Reporte Empleados";
                        funcionbotonesnuevo({
                            botonobjeto: this, tablaobj: objetodatatable, ocultar: ocultar/*, tituloreporte: tituloreporte*/, cabecerasnuevas: cabecerasnuevas, columna_cambio: columna_cambio
                        });
                    });
                },
                data: response.data,
                columns: [
                    { data: "snu_id", title: "Id" },
                    { data: "snu_descripcion", title: "Menu" },
                    {
                        data: "snu_id", title: "Permiso",
                        "bSortable": false,
                        "render": function (o) {
                            var checked = "";
                            var validar = lista_checked.includes(o);
                            checked = validar == true ? 'checked' : '';
                            return '<div class="icheck-inline"><input type="checkbox" data-id="' + o + '" ' + checked + ' checkbox="icheckbox_square-blue"/></div>';
                        }, class: "text-center"
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnEditar').tooltip({
                        title: "Editar"
                    });
                    $(".icheck-inline").iCheck({
                        checkboxClass: 'icheckbox_square-blue',
                        radioClass: 'iradio_square-red',
                        increaseArea: '25%'
                    });
                },
            });
        }
    });
};
function CheckTodosMenus() {
    $(".icheck_total").iCheck("destroy");

    if (total_menus.length == lista_checked.length) {
        document.getElementById("CheckTodosMenus").checked = true;
    } else {
        document.getElementById("CheckTodosMenus").checked = false;
    }
    $(".icheck_total").iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-red',
        increaseArea: '25%'
    });
}
document.addEventListener('DOMContentLoaded', function () {
    PanelPrincipal.init();
});