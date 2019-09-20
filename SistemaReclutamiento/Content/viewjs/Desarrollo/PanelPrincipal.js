var PanelPrincipal = function () {
    var _inicio = function () {
        selectResponse({
            url: "Proveedor/RolListarJson",
            select: "cboRol_",
            campoID: "rol_id",
            CampoValor: "rol_nombre",
            select2: true,
            allOption: false,
            placeholder: "Seleccione Rol"
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
                        if (estado == true) {
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


document.addEventListener('DOMContentLoaded', function () {
    PanelPrincipal.init();
});