var ProveedorVista = function () {
    var _inicio = function () {            
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
                var data = response.data;
                var menu = "";
                var submenu = "";
                $("#moduloPrincipal").html("");
                if (response.respuesta) {
                    $.each(data, function (index, menu) {
                        if (menu.SubMenu.length > 0) {
                            console.log(menu.SubMenu);
                            $("#moduloPrincipal").append('<li  class="menu' + menu.men_id + '"><a href="#">' + menu.men_descripcion + '<span class="fa fa-chevron-down"></span></a></li>');
                            $('.menu' + menu.men_id + ">a").append('<ul class="nav child_menu"></ul>');
                            $.each(menu.SubMenu, function (i, submenu) {
                                $('.menu' + menu.men_id + ">a>ul").append('<li class="sub_menu"><a href="' + submenu.snu_url + '">' + submenu.snu_descripcion + '</a></li>');
                            });
                        }
                        else {
                            $("#moduloPrincipal").append('<li  class="menu' + menu.men_id + ' ocult"><a href="#">' + menu.men_descripcion + '</a></li>');
                        }
                    });
                }
                _claseActiva();
            }
        });
    };
    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();
            _CrearMenu();
        }
    }
}();
// Initialize module
// ------------------------------
document.addEventListener('DOMContentLoaded', function () {
    ProveedorVista.init();
});