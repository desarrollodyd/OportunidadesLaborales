var ProveedorVista = function () {
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
        $("#selectall").on("click", function () {
            console.log("asdd");
            $(".to_do>li>p>input").prop("checked", this.checked);
        }); 
    };
    var _claseActiva = function () {
        $SIDEBAR_MENU = $("#moduloPrincipal");
        $BODY = $('body');

        $SIDEBAR_MENU.find('a').on('click', function (ev) {
            console.log('clicked - sidebar_menu');
            var $li = $(this).parent();

            if ($li.is('.active')) {
                $li.removeClass('active active-sm');
                $('ul:first', $li).slideUp();
            } else {
                // prevent closing menu if we are on child menu
                if (!$li.parent().is('.child_menu')) {
                    $SIDEBAR_MENU.find('li').removeClass('active active-sm');
                    $SIDEBAR_MENU.find('li ul').slideUp();
                } else {
                    if ($BODY.is(".nav-sm")) {
                        $li.parent().find("li").removeClass("active active-sm");
                        $li.parent().find("li ul").slideUp();
                    }
                }
                $li.addClass('active');

                $('ul:first', $li).slideDown();
            }
        });
    };

    var _CrearMenu = function () {
        responseSimple({
            url: "Proveedor/ListarDataMenuJson",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
                var data = response.data;
                var menu = "";
                var submenu = "";
                $("#moduloPrincipal").html("");
                if (response.respuesta) {
                    $.each(data, function (index, value) {
                        //$("#moduloPrincipal").append('<li class="modulo"' + value. + '>')
                        console.log(value);
                    });
                }
            }
        });
    };

    var _ListarModulos = function () {
        responseSimple({
            url: "Proveedor/SubMenuListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                CloseMessages(); 
                var listaMenu = response.dataMenu;
                var listaSubMenu = response.data;
                if (response.respuesta) {
                    $("#moduloPrincipal").html("");
                    submenus = [];
                    $.each(listaSubMenu, function (index, value) {
                        submenus.push(value.fk_menu);
                    })
                    $.each(listaMenu, function (index, menu) {
                        var existeSubmenu = jQuery.inArray(menu.id, submenus);
                        if (existeSubmenu) {
                            $('<li>', {
                                'class': 'modulo ocult ' + menu.men_descripcion + menu.men_id,
                                'data-menu1': 'Menu' + index,
                                'data-modulo': 'Menu' + index,
                                'data-titulo': 'Menu' + index
                            }).append(
                                $('<a>', {
                                    'href': '#',
                                    'text': menu.men_descripcion
                                }).append($('<span>', {
                                    'class': 'fa fa-chevron-down'
                                })
                                )).appendTo("#moduloPrincipal");
                            $('<ul>', {
                                'class': 'nav child_menu ' + menu.men_id + menu.men_descripcion
                            }).appendTo("#moduloPrincipal>li." + menu.men_descripcion + menu.men_id);
                            $.each(listaSubMenu, function (i, submenu) {
                                if (submenu.fk_menu == menu.men_id) {
                                    $('<li>', {
                                        'class': 'modulo ocult',
                                        'data-menu1': 'SubMenu' + i,
                                        'data-modulo': 'SubMenu' + i,
                                        'data-titulo': 'subMenu' + i
                                    }).append(
                                        $('<a>', {
                                            'href': '#',
                                            'text': submenu.snu_descripcion
                                        })).appendTo($("#moduloPrincipal>li>ul." + menu.men_id + menu.men_descripcion))
                                }
                            })
                        }
                        else {
                            $('<li>', {
                                'class': 'modulo ocult',
                                'data-menu1': 'Menu' + index,
                                'data-modulo': 'Menu' + index,
                                'data-titulo': 'Menu' + index
                            }).append(
                                $('<a>', {
                                    'href': '#',
                                    'text': menu.men_descripcion
                                })).appendTo("#moduloPrincipal");
                        }

                    });
                    _claseActiva();
                }
                
                //var respuesta = response.data;
                //console.log(response);
                //if (response.mensaje) {

                //}
                //if (respuesta) {

                    

                //    $("#libody").html("");
                //    menus = [];
                //    $.each(respuesta, function (index, value) {
                //        menus.push(value.snu_descripcion);
                //    });
                //    console.log(menus);
                //    $("#moduloPrincipal> li.modulo").each(function (i) {
                //        var element = $(this);
                //        var modulo = element.data('modulo');
                //        var nombreModulo = element.data('titulo');
                //        var cabecerasMenu = $("[data-modulo='" + nombreModulo + "']");
                //        console.log(nombreModulo);
                //        $.each(cabecerasMenu, function (j) {
                //            var nombreCabecera = $(this).data('titulo');
                //            var datamenuCabecera = $(this).data('menu1');
                //            var hijos = $("li." + datamenuCabecera);
                //            $.each(hijos, function (i) {
                //                var nombrehijo_ = $(this).data('titulo');
                //                var datamenuhijo_ = $(this).data('menu1');
                //                console.log(datamenuhijo_);
                //            })
                         
                //        })
                //    })
                //}
                
            }
        });
    };
    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();
            _ListarModulos();
            //_CrearMenu();
        }
    }
}();
// Initialize module
// ------------------------------
document.addEventListener('DOMContentLoaded', function () {
    ProveedorVista.init();
});