let panelSeguridad=function(){
    let colors = ['primary', 'warning', 'success', 'danger','pink','inverse','default'];
    let icons = ['cutlery', 'star', 'trophy', 'bug','leaf','beer','flask'];
    let _inicio=function(){
        permisosMenuDis2(0);
    }
    let _componentes=function(){

    }
    let _metodos=function(){

    }
    let permisosMenuDis=function(rol) {
        if (rol != 0) {
            rol = $("#cboRol_").val();
        }
        else {
            rol = rolid;
        }
    
        var data = { rolId: rol }
        var url = basePath + "SeguridadIntranet/ListadoMenusRolId";
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            beforeSend: function () {
                block_general("body");
            },
            complete: function () {
                unblock("body");
            },
            success: function (response) {
                console.log(response)
                var respuesta = response.dataResultado;
                if (response.mensaje) {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
                if (respuesta) {
                    $("#libody").html("");
                    menus = [];
                    $.each(respuesta, function (index, value) {
                        menus.push(value.WEB_PMeDataMenu);
                    });
                    $(".cabecera").each(function (i) {
                        var total = $(".cabecera").length - 1;
                        var element = $(this);
                        var menu = element.data('menu1');
                        var titulo = element.data('titulo');
                        var hijos = $('.' + menu);
                        var icon = $(this).find('span.main-menu-icon').find('span').attr('class');
                        var check = "";
    
                        var tr = '';
                        if (hijos.length > 0) {
                            $(hijos).each(function () {
                                var element1 = $(this);
                                var menu1 = element1.data('menu1');
                                var titulo1 = element1.data('titulo');
                                var hijos1 = $('.' + menu1);
                                if (hijos1.length > 0) {
                                    // console.log('Padre', '------' + element1.data('menu1'))
                                    var existeMenu_ = jQuery.inArray(menu1, menus);
                                    if (existeMenu_ >= 0) {
                                        check = "checked";
                                    } else {
                                        check = "";
                                    }
    
                                    tr = tr + '<tr>' +
                                        '<td style="font-weight: bolder;"><span style="color:red !important;background-color:transparent !important" class="glyphicon glyphicon-star"></span> ' + titulo1 + '</td>' +
                                        '<td>' +
                                        '<label style="float:right"><input type="checkbox"  ' + check + '  data-principal="2" data-tit="' + titulo1 + '" value="' + menu1 + '" name="square-checkbox"></label>' +
                                        '</td>' +
                                        '</tr>';
    
                                    $(hijos1).each(function () {
                                        var element2 = $(this);
                                        var menu2 = element2.data('menu1');
                                        var titulo2 = element2.data('titulo');
                                        //console.log('hijos2', '------' + element2.data('menu1'))
    
                                        var existeMenu2 = jQuery.inArray(menu2, menus);
                                        if (existeMenu2 >= 0) {
                                            check = "checked";
                                        } else {
                                            check = "";
                                        }
    
                                        tr = tr + '<tr class="' + menu1 + '">' +
                                            '<td> <span style="color:blue !important;background-color:transparent !important;padding-left: 20px;" class="glyphicon glyphicon-arrow-right"></span> ' + titulo2 + '</td>' +
                                            '<td>' +
                                            '<label style="float:right"><input type="checkbox" ' + check + ' data-tit="' + titulo2 + '" value="' + menu2 + '" name="square-checkbox"></label>' +
                                            '</td>' +
                                            '</tr>';
                                    });
                                }
                                else {
                                    //console.log('hijo1', '----' + element1.data('menu1'))
                                    var existeMenu1 = jQuery.inArray(menu1, menus);
                                    if (existeMenu1 >= 0) {
                                        check = "checked";
                                    } else {
                                        check = "";
                                    }
    
                                    tr = tr + '<tr>' +
                                        '<td style="font-weight: bolder;"><span style="color:red !important;background-color:transparent !important" class="glyphicon glyphicon-star"></span> ' + titulo1 + '</td>' +
                                        '<td>' +
                                        '<label style="float:right"><input type="checkbox"  ' + check + ' data-tit="' + titulo1 + '" value="' + menu1 + '" name="square-checkbox"></label>' +
                                        '</td>' +
                                        '</tr>';
                                }
                            });
                        }
    
    
                        var existeMenu = jQuery.inArray(menu, menus);
                        if (existeMenu >= 0) {
                            check = "checked";
                        } else {
                            check = "";
                        }
    
    
                        $('#libody').append('<li class="highlight-color-' + color[i] + '  highlight-color-' + color[i] + '-icon">' +
                           
                            '<span class="' + icon + '"></span>' +
                            '<div class="c_tmlabel">' +
                            '<div class="c_tmlabel_inner collaps">' +
                            '<h2>' + titulo + '<label style="float:right"><input type="checkbox" data-tit="' + titulo + '" data-principal="1" value="' + menu + '"  ' + check + ' name="square-checkbox"></label></h2>' +
                            '<table class="table table-condensed table-hover">' +
                            '<tbody>' +
                            tr +
                            '</tbody>' +
                            '</table>' +
                            '</div>' +
                            '</div>' +
                            '</li>');
    
                        datamenus.push(menu);
                        if (total == i) {
                            $("#libody").iCheck({
                                checkboxClass: 'icheckbox_square-blue',
                                radioClass: 'iradio_square-red',
                                increaseArea: '2%' // optional
                            });
    
                            var data = { dataMenu: datamenus, rolid: rol }
                            var url = basePath + "seguridad/ListadoFechasPrincipales";
                            ListadoPrincipales(url, data, false);
                        }
                    });
    
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
    
            }
        });
    
    }
    let permisosMenuDis2=function(rol) {
        $("#libody").html("")

        $(".cabecera").each(function (i) {
            var aleatorio = Math.round(Math.random()*6);
            var total = $(".cabecera").length - 1;
            var element = $(this);
            var menu = element.data('menu1');
            var titulo = element.data('titulo');
            var hijos = $('.' + menu);
            var icon = $(this).find('span.main-menu-icon').find('span').attr('class');
            var check = "";
            var tr = '';
            $("#libody2").append(`
                <div class="timeline-items">
                    <div class="timeline-item clearfix">
                        <div class="timeline-info">
                            <i class="timeline-indicator ace-icon fa fa-${icons[aleatorio]} btn btn-${colors[aleatorio]} no-hover"></i>
                        </div>

                        <div class="widget-box clearfix">
                            <div class="widget-body">
                                <div class="widget-main">
                                    ${titulo}
                                    <div class="pull-right">
                                        <!-- <i class="ace-icon fa fa-clock-o bigger-110"></i>-->
                                        <input type="checkbox" data-tit="${titulo}" data-principal="1" value="${menu}"  ${check} name="square-checkbox">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `)
            $("#libody2").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '2%' // optional
            })

        });
    
    }
    let ListadoPrincipales=function(url, data, loading) {

        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json",
            beforeSend: function () {
                if (loading === true) {
                    block_general("body");
                }
            },
            success: function (response) {
                var mensaje = response.mensaje;
                if (mensaje) {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
                var listado = response.data;
                //console.log(listado)
                if (listado) {
                    $.each(listado, function (index, value) {
                        $('#fecha' + value.WEB_ModuloNombre).text(moment(value.WEB_PMeFechaRegistro).format("DD/MM/YYYY"));
                        $('#hora' + value.WEB_ModuloNombre).text(moment(value.WEB_PMeFechaRegistro).format("hh:mm a"));
                    });
    
                }
    
            },
            complete: function () {
                if (loading === true) {
                    unblock("body");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
    
            }
        });
    }
    return {
        init: function () {
            _inicio()
            _componentes()
            _metodos()
        }
    }
}()
document.addEventListener('DOMContentLoaded',function(){
    panelSeguridad.init()
})