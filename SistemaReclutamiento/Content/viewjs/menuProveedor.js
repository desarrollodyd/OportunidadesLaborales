var ProveedorVista = function () {
    var _inicio = function () {
    };
    var _claseActiva = function () {
        
        var CURRENT_URL = window.location.href.split('#')[0].split('?')[0],
            $BODY = $('body'),
            $MENU_TOGGLE = $('#menu_toggle'),
            $SIDEBAR_MENU = $('#sidebar-menu'),
            $SIDEBAR_FOOTER = $('.sidebar-footer'),
            $LEFT_COL = $('.left_col'),
            $RIGHT_COL = $('.right_col'),
            $NAV_MENU = $('.nav_menu'),
            $FOOTER = $('footer');

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
        //$MENU_TOGGLE.on('click', function () {
        //    console.log('clicked - menu toggle');

        //    if ($BODY.hasClass('nav-md')) {
        //        $SIDEBAR_MENU.find('li.active ul').hide();
        //        $SIDEBAR_MENU.find('li.active').addClass('active-sm').removeClass('active');
        //    } else {
        //        $SIDEBAR_MENU.find('li.active-sm ul').show();
        //        $SIDEBAR_MENU.find('li.active-sm').addClass('active').removeClass('active-sm');
        //    }

        //    $BODY.toggleClass('nav-md nav-sm');
        //});
    };

    var _CrearMenu = function () {
        responseSimple({
            url: "Proveedor/ListarDataMenuJson",
            refresh: false,
            callBackSuccess: function (response) {
                CloseMessages();
                var data = response.data;
                var menu = "";
                var submenu = "";
                $("#moduloPrincipal").html("");
                if (response.respuesta) {
                    $.each(data, function (index, menu) {
                        if (menu.SubMenu.length > 0) {
                            console.log(menu.SubMenu);
                            $("#moduloPrincipal").append('<li  class="menu' + menu.men_id + '" modulo ocult><a href="#">' + menu.men_descripcion + '<span class="fa fa-chevron-down"></span></a></li>');
                            $('.menu' + menu.men_id + "").append('<ul class="nav child_menu"></ul>');
                            $.each(menu.SubMenu, function (i, submenu) {
                                $('.menu' + menu.men_id + ">ul").append('<li class="sub_menu ocult"><a href="' +basePath+ submenu.snu_url + '">' + submenu.snu_descripcion + '</a></li>');
                            });
                        }
                        else {
                            $("#moduloPrincipal").append('<li  class="menu' + menu.men_id + ' modulo ocult"><a href="#">' + menu.men_descripcion + '</a></li>');
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