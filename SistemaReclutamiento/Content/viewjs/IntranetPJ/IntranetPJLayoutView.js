var LayoutVista = function () {
    var _Layout = function () {
        //console.log(menu_id);

        var meses = ["ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO",
            "JULIO", "AGOSTO", "SETIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE"
        ];
        dias = ["Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo"];
        var diahoy = new Date();
        var menuInicio = menu_id;
        //console.log(meses[diahoy.getMonth()]);
        responseSimple({
            url: "IntranetPJ/ListarDataJson",
            data: JSON.stringify({ menu_id: menuInicio }),
            refresh: false,
            callBackSuccess: function (response) {
                var dataMenus = response.dataMenus;
                var dataCumpleanios = response.dataCumpleanios;
                var dataActividades = response.dataActividades;
                var listaNoticias = response.listaNoticias;
                var secciones = response.dataSecciones;

                //Creacion de Menus
                if (dataMenus.length > 0) {
                    $("#menuIntranet").html("");
                    $("#barnav").html("");
                    var appendMenuLateral = "";
                    var appendMenuPrincipal = "";
                    $.each(dataMenus, function (index, menu) {
                        appendMenuLateral += '<li><a href="' + menu.menu_url + '">' + menu.menu_titulo + '</a></li>';
                        var clase = "";
                        if (menuInicio == 0) {
                            clase = (index == 0) ? "active" : "";
                        }
                        else {
                            clase = (menu.menu_id == menuInicio) ? "active" : "";
                        }

                        appendMenuPrincipal += '<li class="' + clase + '"><a href="' + basePath + 'intranetPJ/index?menu=' + menu.menu_id+'" data-id="' + menu.menu_id + '" class="sf-with-ul">' + menu.menu_titulo + '</a><ul></ul></li>';
                        //noticia.push(menu.menu_titulo)
                    });
                    $("#menuIntranet").html(appendMenuLateral);
                    $("#barnav").html(appendMenuPrincipal);
                }

                //Creacion de Aside para Cumpleaños
                if (dataCumpleanios.length > 0) {
                    $("#cumpleaniosIntranet").html("");
                    var appendCumpleanios = '';
                    appendCumpleanios += '<h3 class="blocktitle">CUMPLEAÑOS DE ' + meses[diahoy.getMonth()] + ' <span><a href="#">MAS</a></span></h3><div class="getcat"><ul class="catlist" >';
                    $.each(dataCumpleanios, function (index, cumpleanios) {
                        var diaCumpleanios = new Date(moment(cumpleanios.per_fechanacimiento).format('YYYY-MM-DD'));
                        appendCumpleanios += '<li><a href = "#" ><img src="' + basePath + '/Content/intranet/images/png/calendar.png" /><div class="spannumber">' + (diaCumpleanios.getDate() + 1) + '</div><p class="meta-date">' + meses[diahoy.getMonth()] + ' ' + (diaCumpleanios.getDate() + 1) + ', ' + diahoy.getFullYear() + '</p><h2 class="wtitle">' + cumpleanios.per_nombre.toUpperCase() + ' ' + cumpleanios.per_apellido_pat.toUpperCase() + ' ' + cumpleanios.per_apellido_mat.toUpperCase() + '</h2></a >    </li >';
                    });
                    appendCumpleanios += '</ul></div >';
                    $("#cumpleaniosIntranet").html(appendCumpleanios);
                }

                //Creacion de Aside Para Actividades
                if (dataActividades.length > 0) {
                    $("#actividadesMes").html("");
                    var appendActividades = '';
                    appendActividades += '<h3 class="blocktitle">ACTIVIDADES DE ' + meses[diahoy.getMonth()] + '<span><a href="#">MAS</a></span></h3>                            <div class="getcat" > <ul class="catlist">';
                    $.each(dataActividades, function (index, actividad) {
                        var diaActividad = new Date(moment(actividad.act_fecha).format('YYYY-MM-DD'));
                        appendActividades += '<li><a href="#"><img src="' + basePath + '/Content/intranet/images/png/actividad.png" />                                <p class="meta-date">' + meses[diahoy.getMonth()] + ' ' + (diaActividad.getDate() + 1) + ', ' + diahoy.getFullYear() + '</p><h2 class="wtitle">' + actividad.act_descripcion + '</h2> </a></li>';
                    });
                    appendActividades += '</ul></div >';
                    $("#actividadesMes").append(appendActividades);
                }

                //Listado en Seccion de Noticias
                if (listaNoticias.length > 0) {
                    $("#ticker01").html("");
                    var appendNoticias = '';
                    $.each(listaNoticias, function (index, noticia) {
                        var fechaNoticiaBD = new Date(moment(noticia.Item1).format('YYYY-MM-DD'));
                        var fechaNoticia = (fechaNoticiaBD.getDate() + 1) + " de " + meses[fechaNoticiaBD.getMonth()];
                        var comentarioExtra = noticia.Item3;
                        appendNoticias += '<li><span>' + fechaNoticia + '</span><a href="#">' + comentarioExtra + noticia.Item2 + '</a></li>';
                    })
                    $("#ticker01").html(appendNoticias);
                    $("ul#ticker01").liScroll().css({ 'opacity': 1 });
                  
                }

                //listado secciones
                if (secciones.length > 0) {
                    console.log(secciones)
                    $("#content").html("");
                    var appendSeccion = "";
                    slider = false;
                    $.each(secciones, function (index, seccion) {
                        var elementos = seccion.elementos;
                        var appendElementos = "";
                        if (elementos.length > 0) {
                            $.each(elementos, function (index, elemento) {

                                if (elemento.fk_tipo_elemento == 1) {
                                    appendElementos += '<header>' +
                                        '<div class="loverate"><a href="#"><i class="fa fa-exclamation-circle"></i></a></div>' +
                                        '<h1>' + elemento.elem_titulo + '</h1>' +
                                        '</header>';
                                }

                                if (elemento.fk_tipo_elemento == 2) {
                                    appendElementos += '<header>' +
                                        '<div class="postmeta">' +
                                        '<div class="meta-date">' + elemento.elem_titulo+'</div>' +
                                        '</div>' +
                                        '</header>';
                                }

                                if (elemento.fk_tipo_elemento == 3) {
                                    appendElementos += 
                                        '<p> ' + elemento.elem_titulo+'</p>';

                                }

                                if (elemento.fk_tipo_elemento == 4) {
                                    appendElementos += '<article>' +
                                        '<div class="post-content">' +
                                        '<h4 class="lista_titulo">' + elemento.elem_titulo + '</h4>'+
                                        '</div>' +
                                        '<div class="clear"></div>' +
                                        '</article>';
                                }

                                if (elemento.fk_tipo_elemento == 5) {

                                }

                                if (elemento.fk_tipo_elemento == 6) {

                                }

                                if (elemento.fk_tipo_elemento == 7) {

                                }

                                if (elemento.fk_tipo_elemento == 8) {

                                }

                                if (elemento.fk_tipo_elemento == 9) {

                                }

                                if (elemento.fk_tipo_elemento == 10) {
                                    slider = true;
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendslider = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            appendslider += '<li data-orbit-slide="headline-' + (index + 1) + '" style="width:100%">' +
                                                '<div class="row">' +
                                                '<div class="twelve columns">' +
                                                '<div class="itemcatslide">' +
                                                '<div class="catf">' +
                                                '<div class="catf-format">' +
                                                '<div class="fdate">15<br /><span>Dec</span></div>' +
                                                '</div>' +
                                                '<div class="imgslide">' +
                                                '<img src="' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt="" title="" class="slidefeatured">' +
                                                '</div>' +
                                                '<div class="catf-caption">' +
                                                '<h2>Star Trek Into Darkness is a 2013 American</h2>' +
                                                '<span class="meta"><a href="#">MOVIE</a> / 20 Comments / 5 Rate </span>' +
                                                '</div>' +
                                                '</div>' +
                                                '</div>' +
                                                '</div>' +
                                                '</div>' +
                                                '</li >';

                                        });

                                        appendElementos += '<div class="row">'+
                                            '<div class="twelve columns">'+
                                                '<div id="slidercontent" class="mixedContent onlyimgblank">';
                                    }
                                }

                                if (elemento.fk_tipo_elemento == 11) {
                                    slider = true;
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendslider = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            appendslider += '<li data-orbit-slide="headline-'+(index+1)+'" style="width:100%">'+
                                                                '<div class="row">'+
                                                                    '<div class="twelve columns">'+
                                                                        '<div class="itemcatslide">'+
                                                                            '<div class="catf">'+
                                                                                '<div class="catf-format">'+
                                                                                    '<div class="fdate">15<br /><span>Dec</span></div>'+
                                                                                '</div>'+
                                                                                '<div class="imgslide">'+
                                                '<img src="' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt="" title="" class="slidefeatured">'+
                                                                                '</div>'+
                                                                                    '<div class="catf-caption">'+
                                                                                        '<h2>Star Trek Into Darkness is a 2013 American</h2>'+
                                                                                        '<span class="meta"><a href="#">MOVIE</a> / 20 Comments / 5 Rate </span>'+
                                                                                    '</div>'+
                                                                                '</div>'+
                                                                            '</div>'+
                                                                        '</div>'+
                                                                    '</div>'+
                                                            '</li >';

                                        });

                                        appendElementos += '<article>' +
                                            '<div class="post-content">' +
                                            '<div class="">' +
                                                                '<div class="twelve columns">'+
                                                                    '<div class="catslide">'+
                                                                        '<ul class="slider" data-orbit>' +
                                                                            appendslider+
                                                                        '</ul>' +
                                                                    '</div>' +
                                                                '</div>' +
                                            '</div>' +
                                            '</div>' +
                                            '<div class="clear"></div>' +
                                            '</article>';
                                    }
                                }

                                if (elemento.fk_tipo_elemento == 12) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendDetalleElementoheader = "";
                                    var appendDetalleElementobody = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (index == 0) {
                                                appendDetalleElementoheader += '<div class="featured">' +
                                                    '<div class="thumb">' +
                                                    ' <img src="' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt="">' +
                                                    '<div class="overlay">' +
                                                    '<div class="title-carousel">' +
                                                    '<div class="ticarousel"> ' + detalleelemento.detel_descripcion + '</div>' +
                                                    '</div>' +
                                                    '</div>' +
                                                    ' </div>' +
                                                    '<div class="excerpt">' +
                                                    '<p class="meta-date">December 23,2012</p>' +
                                                    '<div class="desc">' +
                                                    '<p class="pcats">uc option voluptaria ex, nec habeo viris ei. Ne qui tota legendos, nam at debitis tractatos.</p>' +
                                                    '<a href="#"><i class="fa fa-external-link"></i> LEER MAS </a>' +
                                                    '</div>' +
                                                    '</div>' +
                                                    '</div>';
                                            }
                                            else {

                                                appendDetalleElementobody += '<li>' +
                                                    '<div class="octhumb">' +
                                                    '<a href="#"><img src="' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt=""></a>' +
                                                    '</div>' +
                                                    '<div class="desc">' +
                                                    '<a href="#">' + detalleelemento.detel_descripcion + '</a>' +
                                                    '<h4><a href="#"> 5 YouTube Exercises to Strengthen Your Core</a></h4>' +
                                                    '</div>' +
                                                    '</li>';
                                            }
                                        });
                                    }


                                    appendElementos += '<article>' +
                                        '<div class="post-content">' +
                                        '<section id="cat2news">'+
                                            appendDetalleElementoheader+
                                             '<div class="othercat">'+
                                                    '<ul class="oc-horizon">'+
                                                        appendDetalleElementobody+
                                                    '</ul>'+
                                            '</div>'+
                                        '</section>'+
                                        '</div>' +
                                        '<div class="clear"></div>' +
                                        '</article >';
                                }
                            })
                        }

                        appendSeccion += '<section id="singlepost">' + appendElementos + '</section><div class="separador"></div>';

                    })
                    $("#content").html(appendSeccion);
                    if (slider) {
                        $('.slider').orbit({
                            animation: 'fade',
                            timer_speed: 10000,
                            pause_on_hover: true,
                            resume_on_mouseout: false,
                            swipe: true,
                            animation_speed: 500
                        });
                    }

                }
            }
        });
    };
    var _componentes = function () {


    };
    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _Layout();
            _componentes();
        }
    }
}();
// Initialize module
// ------------------------------
document.addEventListener('DOMContentLoaded', function () {
    LayoutVista.init();
});