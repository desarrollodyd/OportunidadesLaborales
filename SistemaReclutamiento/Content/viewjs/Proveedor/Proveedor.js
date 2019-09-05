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
    };
    var _ListarModulos = function () {
        responseSimple({
            url: "Proveedor/SubMenuListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                var listaMenu = response.dataMenu;
                var listaSubMenu = response.data;
                //$("#moduloPrincipal").html("");
                var cadena = "";
          
                submenus = [];
                $.each(listaSubMenu, function (index, value) {
                    submenus.push(value.fk_menu);
                })
             
                //console.log(menus);
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
                                'class':'fa fa-chevron-down'
                            })
                            )).appendTo("#moduloPrincipal");
                        $('<ul>', {
                            'class': 'nav child_menu ' + menu.men_id + menu.men_descripcion
                        }).appendTo("#moduloPrincipal>li."+menu.men_descripcion + menu.men_id,);
                        $.each(listaSubMenu, function (i,submenu) {
                            if (listaSubMenu.fk_menu == menu.id) {
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
                    
                })
              
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
    var _componentes = function () {
    };

    var _metodos = function () {
    };
    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();
            _componentes();
            _ListarModulos();
            _metodos();
        },
        __ListarModulos: function () {
            _ListarModulos();
        },
    }
}();
// Initialize module
// ------------------------------
document.addEventListener('DOMContentLoaded', function () {
    ProveedorVista.init();
});