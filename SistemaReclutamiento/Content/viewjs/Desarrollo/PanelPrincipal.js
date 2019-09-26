var PanelPrincipal = function () {
    var total_menus = 0;
    //var lista_unchecked_menus = [];
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
    var ListarMenus = function () {
        var data = { usu_id: usu_id }
        lista_checked = [];
        responseSimple({
            url: "Super/PermisosListarJson",
            data: JSON.stringify(data),
            refresh: false,
            callBackSuccess: function (response) {
                CloseMessages();
                total_menus = response.data;
                var data_lista_menu = response.data_lista_menu;
                lista_checked = [];
                $.each(data_lista_menu, function (key, value) {
                    lista_checked.push(value.fk_submenu);
                });
                CheckTodosMenus();
                simpleDataTable({
                    table: "#tablepermiso",
                    tableLengthChange: false,
                    tableColumnsData: response.data,
                    tableColumns: [
                        { data: "snu_id", title: "Id" },
                        { data: "snu_descripcion", title: "Menu" },
                        { data: "snu_url", title: "URI" },
                        { data: "snu_orden", title: "Orden" },
                        {
                            data: "snu_id", title: "Permiso",
                            "bsortable": false,
                            "render": function (o) {
                                var checked = "";
                                var validar = lista_checked.includes(o);
                                checked = validar == true ? 'checked' : '';
                                return '<div class=""><input type="checkbox" data-id="' + o + '" ' + checked + ' checkbox="icheckbox_square-blue" class="flat"/></div>';
                            }, class: "text-center"
                        }
                    ]
                });
                checkUno();
            }
        });
    };
    var CheckTodosMenus = function () {
        var contador = 0;
        $.each(total_menus, function (key, value) {
            if (lista_checked.includes(value.snu_id)) {
                contador++;
            }
        });
        if (total_menus.length == contador) {
            $('.div_check_total input:checkbox').prop('checked', true);
        } else {
            $('.div_check_total input:checkbox').prop('checked', false);
        }
    };
    var _componentes = function () {
        $('.div_check_total input:checkbox').on('change', function () {
            if ($(this).is(':checked')) {
                messageConfirmation({
                    content: "Se dara permiso a todos los menus a este usuario, ¿Desea Continuar?",
                    callBackSAceptarComplete: function () {
                        //Guardar Acceso a todos los menus
                        var lista_menus = [];
                        lista_unchecked_menus = [];
                        var usu_id = $("#cboUsuario").val();
                        $.each(total_menus, function (key, value) {
                            lista_menus.push(value.snu_id);
                        });
                        lista_unchecked_menus = lista_menus.filter(x => !lista_checked.includes(x));
                        var dataForm = {
                            submenus: lista_unchecked_menus,
                            fk_usuario: usu_id
                        };
                        responseSimple({
                            url: "Super/SubMenuPermisoTodoInsertar",
                            data: JSON.stringify(dataForm),
                            refresh: false,
                            callBackSuccess: function (response) {
                                var respuesta = response.respuesta;
                                if (respuesta) {
                                    setTimeout(function () {
                                        ListarMenus(usu_id);
                                    }, 500);
                                   // ListarMenus(usu_id);
                                }
                                else {
                                    messageResponse({
                                        text: response.mensaje,
                                        type: "error"
                                    });
                                }
                            }
                        });
                    },
                    callBackSCCerraromplete: function () {
                        CheckTodosMenus();
                    }
                })
               
                
            }
            else {
                messageConfirmation({
                    content:"Se eliminaran todos los permisos a este usuario, ¿Desea Continuar?",
                    callBackSAceptarComplete: function () {
                        //Quitar Acceso a Menus
                        var lista_menus = [];
                        lista_unchecked_menus = [];
                        var usu_id = $("#cboUsuario").val();
                        $.each(total_menus, function (key, value) {
                            lista_menus.push(value.snu_id);
                        });
                        lista_unchecked_menus = lista_menus.filter(x => lista_checked.includes(x));
                        var dataForm = {
                            fk_usuario: usu_id,
                            submenus: lista_unchecked_menus
                        };
                        responseSimple({
                            url: "Super/SubMenuPermisoTodoEliminar",
                            data: JSON.stringify(dataForm),
                            refresh: false,
                            callBackSuccess: function (response) {
                                var respuesta = response.respuesta;
                                if (respuesta) {
                                    setTimeout(function () {
                                        ListarMenus(usu_id);
                                    }, 500);
                                }
                                else {
                                    messageResponse({
                                        text: response.mensaje,
                                        type: "error"
                                    });
                                }
                            }
                        });
                    },
                    callBackSCCerraromplete: function () {
                        CheckTodosMenus();
                    }
                    
                })
                
               
            }

        });
        $('#cboUsuario').change(function () {
            usu_id = $(this).val();
            if (usu_id == "") {
                messageResponse({
                    text: "Debe seleccionar un Usuario",
                    type: "error"
                });
            }
            else {
                $(".div_check_total input:checkbox").attr("disabled", false);
                ListarMenus(usu_id);
            }
        });
    };
    var checkUno = function () {
        $('#tablepermiso input:checkbox').on('change', function () {
            if ($(this).is(':checked')) {
                var submenu = $(this).data("id");
                var dataForm = {
                    fk_submenu: submenu,
                    fk_usuario: usu_id
                };
                responseSimple({
                    url: "Super/SubMenuPermisoInsertar",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response) {
                            lista_checked = [];
                            var lista_menu_usuario = response.lista_menu_usuario;
                            $.each(lista_menu_usuario, function (key, value) {
                                lista_checked.push(value.fk_submenu);
                            });
                            CheckTodosMenus();
                        }
                        else {
                            messageResponse({
                                text: response.mensaje,
                                type: "error"
                            });
                        }
                    }
                });
            }
            else {
                var submenu = $(this).data("id");
                var dataForm = {
                    fk_submenu: submenu,
                    fk_usuario: usu_id
                };
                responseSimple({
                    url: "Super/SubMenuPermisoQuitar",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        if (response) {
                            lista_checked = [];
                            var lista_menu_usuario = response.lista_menu_usuario;
                            $.each(lista_menu_usuario, function (key, value) {
                                lista_checked.push(value.fk_submenu);
                            });
                            CheckTodosMenus();
                        }
                        else {
                            messageResponse({
                                text: response.mensaje,
                                type: "error"
                            });
                        }
                    }
                });
            }
           
        });
    }
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
//function checkTable(){
//    $('#table input:checkbox').on('ifChecked', function (event) {
//        console.log(numero);
//    });
//}