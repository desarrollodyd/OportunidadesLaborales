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
                        appendCumpleanios += '<li><a href = "javascript:void(0);" ><img src="' + basePath + 'Content/intranet/images/png/calendar.png" /><div class="spannumber">' + (diaCumpleanios.getDate() + 1) + '</div><p class="meta-date">' + meses[diahoy.getMonth()] + ' ' + (diaCumpleanios.getDate() + 1) + ', ' + diahoy.getFullYear() + '</p><h2 class="wtitle">' + cumpleanios.per_nombre.toUpperCase() + ' ' + cumpleanios.per_apellido_pat.toUpperCase() + ' ' + cumpleanios.per_apellido_mat.toUpperCase() + '</h2></a >    </li >';
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
                        var imagen = (actividad.act_imagen != "") ? '/png/' + actividad.act_imagen:"";
                        appendActividades += '<li><a href="javascript:void(0);"><img src="' + basePath + '/Content/intranet/images'+imagen+'" />                                <p class="meta-date">' + meses[diahoy.getMonth()] + ' ' + (diaActividad.getDate() + 1) + ', ' + diahoy.getFullYear() + '</p><h2 class="wtitle">' + actividad.act_descripcion + '</h2> </a></li>';
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
                    silertrans = false;
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
                                        '<h4 class="lista_titulo">' + elemento.elem_titulo + '</h4>' +
                                        '<div class="clear"></div>' +
                                        '</article>';
                                }

                                if (elemento.fk_tipo_elemento == 5) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendLista = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            appendLista += '<li>' + detalleelemento.detel_descripcion + '</li>';

                                        });

                                        appendElementos += '<ul class="listas">' +
                                            appendLista +
                                            '</ul>';
                                    }
                                }

                                if (elemento.fk_tipo_elemento == 6) {

                                }

                                if (elemento.fk_tipo_elemento == 7) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            appendElementos += '<article>' +
                                                '<div class="imgpost th">' +
                                                '<img src = "' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt = "">' +
                                                '</div>' +
                                                '</article>';

                                        });
                                    }
                                    
                                }

                                if (elemento.fk_tipo_elemento == 8) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendsIzquierda = "";
                                    var appendsDerecha = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (detalleelemento.detel_posicion == "L") {
                                                appendsIzquierda += "<li class='modal_o' data-sece='"+ JSON.stringify(detalleelemento.seccion_elemento).replace(/'/g, "\\'")+"'>" +
                                                    "<div class='ctitle wt" + (index + 2) +"'>" + detalleelemento.detel_descripcion + " </div></li>"; 
                                            }
                                            else {
                                                appendsDerecha += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;" src="' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt=""></a>';
                                            }

                                        });

                                        appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">'+
                                            '<div class="six columns" >'+
                                                '<div class="widget lreview">'+
                                                    '<ul class="listreview">'+
                                            appendsIzquierda+
                                                   ' </ul>'+
                                                '</div>'+
                                             '</div >'+
                                            '<div class="six columns">'+
                                            '<div class="sponsor">' +
                                            appendsDerecha+
                                                '</div>'+
                                            '</div>'+
                                        '</div></div>';
                                    }

                                }

                                if (elemento.fk_tipo_elemento == 9) {

                                }

                                if (elemento.fk_tipo_elemento == 10) {
                                    silertrans = true;
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendslider = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            appendslider += '<div class="contentBox">'+
                                                '<img src = "' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt = "Field" />'+
                                                    '<div class="overlay">'+
                                                        '<div class="title-carousel">'+
                                                            '<a href="javascript:void(0);">'+
                                                '<div class="ticarousel"> ' + detalleelemento.detel_descripcion + '</div>'+
                                                            '</a>'+
                                                        '</div>'+
                                                    '</div>'+
                                            '</div>';

                                        });

                                        appendElementos += '<article><div class="post-content" ><div class="row">'+
                                            '<div class="twelve columns">'+
                                            '<div id="slidercontent" class="mixedContent onlyimgblank slidercontent" style="padding-left: 10px;padding-right: 10px;">' +
                                            appendslider+
                                            '</div>' +
                                            '</div>' +
                                            '</div></div></article>';
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
                                                '</div>' +
                                                                                    //'<div class="catf-caption">'+
                                                                                    //    '<h2>Star Trek Into Darkness is a 2013 American</h2>'+
                                                                                    //    '<span class="meta"><a href="#">MOVIE</a> / 20 Comments / 5 Rate </span>'+
                                                                                    //'</div>'+
                                                                                '</div>'+
                                                                            '</div>'+
                                                                        '</div>'+
                                                                    '</div>'+
                                                            '</li>';

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
                                        var i = 0;
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (index == 0) {
                                                appendDetalleElementoheader += '<div class="featured" style="border: 4px solid #4e4c4c;padding-right: 20px;padding-top: 20px;">' +
                                                    '<div class="thumb foto_cuadro_izquiera">' +
                                                    ' <img src="' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt="">' +
                                                    '<div class="overlay">' +
                                                    '<div class="title-carousel">' +
                                                    '<div class="ticarousel"> ' + detalleelemento.detel_descripcion + '</div>' +
                                                    '</div>' +
                                                    '</div>' +
                                                    ' </div>' +
                                                    '<div class="excerpt">' +
                                                    '<div class="desc">' +
                                                    '<p class="pcats" style="padding-left: 0px !important;padding-right: 0px !important;">uc option voluptaria ex, nec habeo viris ei. Ne qui tota legendos, nam at debitis tractatos.</p>' +
                                                    '<a href="javascript:void(0);"><i class="fa fa-external-link"></i> LEER MAS </a>' +
                                                    '</div>' +
                                                    '</div>' +
                                                    '</div>';
                                            }
                                            else {
                                                i++;
                                                if (i < 4) {
                                                    appendDetalleElementobody += '<li>' +
                                                        '<div class="octhumb">' +
                                                        '<a href="javascript:void(0);" class="foto_cuadro_derecha"><img src="' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt=""></a>' +
                                                        '</div>' +
                                                        '<div class="desc">' +
                                                        '<a href="javascript:void(0);">' + detalleelemento.detel_descripcion + '</a>' +
                                                        '<h4><a href="#"> 5 YouTube Exercises to Strengthen Your Core</a></h4>' +
                                                        '</div>' +
                                                        '</li>';
                                                }
                                                
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

                                if (elemento.fk_tipo_elemento == 13) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendsIzquierda = "";
                                    var appendsDerecha = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (detalleelemento.detel_posicion == "R") {
                                                appendsDerecha += '<li><div class="ctitle wt' + (index + 2) + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                            }
                                            else {
                                                appendsIzquierda += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;" src="' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt=""></a>';
                                            }

                                        });

                                        appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                            '<div class="six columns">' +
                                            '<div class="sponsor">' +
                                            appendsIzquierda +
                                            '</div>' +
                                            '</div>' +
                                            '<div class="six columns" >' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsDerecha +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div >' +
                                            '</div>' +
                                            '</div>';
                                    }

                                }

                                if (elemento.fk_tipo_elemento == 14) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendsIzquierda = "";
                                    var appendsDerecha = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (detalleelemento.detel_posicion == "L") {
                                                appendsIzquierda += '<li><div class="ctitle wt' + (index + 2) + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                            }
                                            else {
                                                appendsDerecha += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;height: 200px;" src="' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt=""></a>';
                                            }

                                        });

                                        appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                            '<div class="eight columns" >' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsIzquierda +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div >' +
                                            '<div class="four columns">' +
                                            '<div class="sponsor">' +
                                            appendsDerecha +
                                            '</div>' +
                                            '</div>' +
                                            '</div></div>';
                                    }

                                }

                                if (elemento.fk_tipo_elemento == 15) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendsIzquierda = "";
                                    var appendsDerecha = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (detalleelemento.detel_posicion == "R") {
                                                appendsDerecha += '<li><div class="ctitle wt' + (index + 2) + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                            }
                                            else {
                                                appendsIzquierda += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;height: 200px;" src="' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt=""></a>';
                                            }

                                        });

                                        appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                            '<div class="four columns">' +
                                            '<div class="sponsor">' +
                                            appendsDerecha +
                                            '</div>' +
                                            '</div>' +
                                            '<div class="eight columns" >' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsIzquierda +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div>' +
                                            
                                            '</div></div>';
                                    }

                                }

                                if (elemento.fk_tipo_elemento == 16) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendsIzquierda = "";
                                    var appendsCentro = "";
                                    var appendsDerecha = "";
                                    if (detalleElementolista.length > 0) {
                                        iz = 1, ce = 1, r = 1;
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (detalleelemento.detel_posicion == "L") {
                                                appendsIzquierda += '<li><div class="ctitle wt' + iz + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                                iz++;
                                            }
                                            if (detalleelemento.detel_posicion == "C") {
                                                appendsCentro += '<li><div class="ctitle wt' + ce + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                                ce++;
                                            }
                                            if (detalleelemento.detel_posicion == "R") {
                                                appendsDerecha += '<li><div class="ctitle wt' + r + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                                r++;
                                            }
                                        });

                                        appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                            '<div class="four columns">' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsIzquierda +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div >' +
                                            '<div class="four columns">' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsCentro +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div>' +
                                            '<div class="four columns">' +
                                            '<div class="widget lreview">' +
                                            '<ul class="listreview">' +
                                            appendsDerecha +
                                            ' </ul>' +
                                            '</div>' +
                                            '</div>' +
                                            '</div></div>';
                                    }
                                }

                                if (elemento.fk_tipo_elemento == 17) {
                                    var detalleElementolista = elemento.detalleElemento;
                                    var appendimageneslistasimple = "";
                                    var float_ = "";
                                    if (detalleElementolista.length > 0) {
                                        $.each(detalleElementolista, function (index, detalleelemento) {
                                            if (index == (detalleElementolista.length - 1)) {

                                                float_ = 'style="float:left;"';
                                            }
                                            appendimageneslistasimple += ' <div class="three tilesli columns" ' + float_+'>' +
                                                '<div class="space_" >' +
                                                '<div class="itemblog">' +
                                                '<div class="clear"></div>' +
                                                '<div class="thumb">' +
                                                '<a href="javascript:void(0);"><img src="' + basePath + 'Content/intranet/images' + detalleelemento.detel_ubicacion + '' + detalleelemento.detel_nombre + '.' + detalleelemento.detel_extension + '" alt=""></a>' +
                                                '</div>' +
                                                '<div class="clear"></div>' +
                                                '<div class="excerpt">' +
                                                '<h2 class="ntitle"><a href="javascript:void(0);">' + detalleelemento.detel_descripcion + '</a></h2>' +
                                                '</div>' +
                                                '</div>' +
                                                '</div>' +
                                                '</div>';
                                        });

                                        appendElementos += '<article>' +
                                            '<div class="post-content">' +
                                            appendimageneslistasimple+
                                            '</div>' +
                                            '<div class="clear"></div>' +
                                            '</article>';
                                    }
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

                    if (silertrans) {

                        $(".slidercontent").smoothDivScroll({
                            touchScrolling: true
                        });

                        $(".contentBox .overlay").mouseover(function () {
                            $(this).prev().css({
                                opacity: 1,
                                WebkitTransition: 'opacity 1s ease-in-out',
                                MozTransition: 'opacity 1s ease-in-out',
                                MsTransition: 'opacity 1s ease-in-out',
                                OTransition: 'opacity 1s ease-in-out',
                                transition: 'opacity 1s ease-in-out'
                            });
                        });
                        $(".contentBox .overlay").mouseleave(function () {
                            $(this).prev().css({
                                opacity: 0.5,
                                WebkitTransition: 'opacity 1s ease-in-out',
                                MozTransition: 'opacity 1s ease-in-out',
                                MsTransition: 'opacity 1s ease-in-out',
                                OTransition: 'opacity 1s ease-in-out',
                                transition: 'opacity 1s ease-in-out'
                            });
                        });
                    }
                   
                }
            }
        });
    };
    var _componentes = function () {

        $(document).on('click','a.foto_cuadro_derecha', function () {

            var imgderecha = $(this);
            var imgder = imgderecha.find("img").attr('src');
            var imgizquierda = $("div.foto_cuadro_izquiera").find("img").attr('src');
            $("div.foto_cuadro_izquiera").find("img").attr('src', imgder);
            imgderecha.find("img").attr('src',imgizquierda);
        });

        $(document).on('click', 'li.modal_o', function () {

            var dataseccion = $(this).data("sece");
            var formattedJson = JSON.stringify(dataseccion);
            var my_object = JSON.parse(formattedJson);
            if (my_object.length > 0) {
                var seccion = my_object[0];
                var elementos = seccion.elemento_modal;
                $("#contenido_modal").html("");
                var appendSeccion = "";
                slider = false;
                silertrans = false;
                if (elementos.length > 0) {
                    console.log(elementos);
                    var appendElementos = "";

                    $.each(elementos, function (index, elemento) {

                        if (elemento.fk_tipo_elemento == 1) {
                            appendElementos += '<header>' +
                                '<div class="loverate"><a href="#"><i class="fa fa-exclamation-circle"></i></a></div>' +
                                '<h1>' + elemento.emod_titulo + '</h1>' +
                                '</header>';
                        }

                        if (elemento.fk_tipo_elemento == 2) {
                            appendElementos += '<header>' +
                                '<div class="postmeta">' +
                                '<div class="meta-date">' + elemento.emod_titulo + '</div>' +
                                '</div>' +
                                '</header>';
                        }

                        if (elemento.fk_tipo_elemento == 3) {
                            appendElementos +=
                                '<p> ' + elemento.emod_titulo + '</p>';

                        }

                        if (elemento.fk_tipo_elemento == 4) {
                            appendElementos += '<article>' +
                                '<h4 class="lista_titulo">' + elemento.emod_titulo + '</h4>' +
                                '<div class="clear"></div>' +
                                '</article>';
                        }

                        if (elemento.fk_tipo_elemento == 5) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendLista = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    appendLista += '<li>' + detalleelemento.detelm_descripcion + '</li>';

                                });

                                appendElementos += '<ul class="listas">' +
                                    appendLista +
                                    '</ul>';
                            }
                        }

                        if (elemento.fk_tipo_elemento == 6) {

                        }

                        if (elemento.fk_tipo_elemento == 7) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    appendElementos += '<article>' +
                                        '<div class="imgpost th">' +
                                        '<img src = "' + basePath + 'Content/intranet/images' + detalleelemento.detelm_ubicacion + '' + detalleelemento.detelm_nombre + '.' + detalleelemento.detelm_extension + '" alt = "">' +
                                        '</div>' +
                                        '</article>';

                                });
                            }

                        }

                        if (elemento.fk_tipo_elemento == 8) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendsIzquierda = "";
                            var appendsDerecha = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (detalleelemento.detelm_posicion == "L") {
                                        appendsIzquierda += "<li class='modal_o'>" +
                                            "<div class='ctitle wt" + (index + 2) + "'>" + detalleelemento.detelm_descripcion + " </div></li>";
                                    }
                                    else {
                                        appendsDerecha += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;" src="' + basePath + 'Content/intranet/images' + detalleelemento.detelm_ubicacion + '' + detalleelemento.detelm_nombre + '.' + detalleelemento.detelm_extension + '" alt=""></a>';
                                    }

                                });

                                appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                    '<div class="six columns" >' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsIzquierda +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div >' +
                                    '<div class="six columns">' +
                                    '<div class="sponsor">' +
                                    appendsDerecha +
                                    '</div>' +
                                    '</div>' +
                                    '</div></div>';
                            }

                        }

                        if (elemento.fk_tipo_elemento == 9) {

                        }

                        if (elemento.fk_tipo_elemento == 10) {
                            silertrans = true;
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendslider = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    appendslider += '<div class="contentBox">' +
                                        '<img src = "' + basePath + 'Content/intranet/images' + detalleelemento.detelm_ubicacion + '' + detalleelemento.detelm_nombre + '.' + detalleelemento.detelm_extension + '" alt = "Field" />' +
                                        '<div class="overlay">' +
                                        '<div class="title-carousel">' +
                                        '<a href="javascript:void(0);">' +
                                        '<div class="ticarousel"> ' + detalleelemento.detelm_descripcion + '</div>' +
                                        '</a>' +
                                        '</div>' +
                                        '</div>' +
                                        '</div>';

                                });

                                appendElementos += '<article><div class="post-content" ><div class="row">' +
                                    '<div class="twelve columns">' +
                                    '<div id="slidercontent" class="mixedContent onlyimgblank slidercontent" style="padding-left: 10px;padding-right: 10px;">' +
                                    appendslider +
                                    '</div>' +
                                    '</div>' +
                                    '</div></div></article>';
                            }
                        }

                        if (elemento.fk_tipo_elemento == 11) {
                            slider = true;
                            var detalleElementolista = elemento.detalle_elemento_modal;
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
                                        '<img src="' + basePath + 'Content/intranet/images' + detalleelemento.detelm_ubicacion + '' + detalleelemento.detelm_nombre + '.' + detalleelemento.detelm_extension + '" alt="" title="" class="slidefeatured">' +
                                        '</div>' +
                                        //'<div class="catf-caption">'+
                                        //    '<h2>Star Trek Into Darkness is a 2013 American</h2>'+
                                        //    '<span class="meta"><a href="#">MOVIE</a> / 20 Comments / 5 Rate </span>'+
                                        //'</div>'+
                                        '</div>' +
                                        '</div>' +
                                        '</div>' +
                                        '</div>' +
                                        '</li>';

                                });

                                appendElementos += '<article>' +
                                    '<div class="post-content">' +
                                    '<div class="">' +
                                    '<div class="twelve columns">' +
                                    '<div class="catslide">' +
                                    '<ul class="slider" data-orbit>' +
                                    appendslider +
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
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendDetalleElementoheader = "";
                            var appendDetalleElementobody = "";
                            if (detalleElementolista.length > 0) {
                                var i = 0;
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (index == 0) {
                                        appendDetalleElementoheader += '<div class="featured" style="border: 4px solid #4e4c4c;padding-right: 20px;padding-top: 20px;">' +
                                            '<div class="thumb foto_cuadro_izquiera">' +
                                            ' <img src="' + basePath + 'Content/intranet/images' + detalleelemento.detelm_ubicacion + '' + detalleelemento.detelm_nombre + '.' + detalleelemento.detelm_extension + '" alt="">' +
                                            '<div class="overlay">' +
                                            '<div class="title-carousel">' +
                                            '<div class="ticarousel"> ' + detalleelemento.detelm_descripcion + '</div>' +
                                            '</div>' +
                                            '</div>' +
                                            ' </div>' +
                                            '<div class="excerpt">' +
                                            '<div class="desc">' +
                                            '<p class="pcats" style="padding-left: 0px !important;padding-right: 0px !important;">uc option voluptaria ex, nec habeo viris ei. Ne qui tota legendos, nam at debitis tractatos.</p>' +
                                            '<a href="javascript:void(0);"><i class="fa fa-external-link"></i> LEER MAS </a>' +
                                            '</div>' +
                                            '</div>' +
                                            '</div>';
                                    }
                                    else {
                                        i++;
                                        if (i < 4) {
                                            appendDetalleElementobody += '<li>' +
                                                '<div class="octhumb">' +
                                                '<a href="javascript:void(0);" class="foto_cuadro_derecha"><img src="' + basePath + 'Content/intranet/images' + detalleelemento.detelm_ubicacion + '' + detalleelemento.detelm_nombre + '.' + detalleelemento.detelm_extension + '" alt=""></a>' +
                                                '</div>' +
                                                '<div class="desc">' +
                                                '<a href="javascript:void(0);">' + detalleelemento.detelm_descripcion + '</a>' +
                                                '<h4><a href="#"> 5 YouTube Exercises to Strengthen Your Core</a></h4>' +
                                                '</div>' +
                                                '</li>';
                                        }

                                    }
                                });
                            }


                            appendElementos += '<article>' +
                                '<div class="post-content">' +
                                '<section id="cat2news">' +
                                appendDetalleElementoheader +
                                '<div class="othercat">' +
                                '<ul class="oc-horizon">' +
                                appendDetalleElementobody +
                                '</ul>' +
                                '</div>' +
                                '</section>' +
                                '</div>' +
                                '<div class="clear"></div>' +
                                '</article >';
                        }

                        if (elemento.fk_tipo_elemento == 13) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendsIzquierda = "";
                            var appendsDerecha = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (detalleelemento.detelm_posicion == "R") {
                                        appendsDerecha += '<li><div class="ctitle wt' + (index + 2) + '"> ' + detalleelemento.detelm_descripcion + ' </div></li>';
                                    }
                                    else {
                                        appendsIzquierda += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;" src="' + basePath + 'Content/intranet/images' + detalleelemento.detelm_ubicacion + '' + detalleelemento.detelm_nombre + '.' + detalleelemento.detelm_extension + '" alt=""></a>';
                                    }

                                });

                                appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                    '<div class="six columns">' +
                                    '<div class="sponsor">' +
                                    appendsIzquierda +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="six columns" >' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsDerecha +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div >' +
                                    '</div>' +
                                    '</div>';
                            }

                        }

                        if (elemento.fk_tipo_elemento == 14) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendsIzquierda = "";
                            var appendsDerecha = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (detalleelemento.detelm_posicion == "L") {
                                        appendsIzquierda += '<li><div class="ctitle wt' + (index + 2) + '"> ' + detalleelemento.detelm_descripcion + ' </div></li>';
                                    }
                                    else {
                                        appendsDerecha += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;height: 200px;" src="' + basePath + 'Content/intranet/images' + detalleelemento.detelm_ubicacion + '' + detalleelemento.detelm_nombre + '.' + detalleelemento.detelm_extension + '" alt=""></a>';
                                    }

                                });

                                appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                    '<div class="eight columns" >' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsIzquierda +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div >' +
                                    '<div class="four columns">' +
                                    '<div class="sponsor">' +
                                    appendsDerecha +
                                    '</div>' +
                                    '</div>' +
                                    '</div></div>';
                            }

                        }

                        if (elemento.fk_tipo_elemento == 15) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendsIzquierda = "";
                            var appendsDerecha = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (detalleelemento.detelm_posicion == "R") {
                                        appendsDerecha += '<li><div class="ctitle wt' + (index + 2) + '"> ' + detalleelemento.detelm_descripcion + ' </div></li>';
                                    }
                                    else {
                                        appendsIzquierda += '<a href="javascript:void(0);"><img style="margin-bottom: 0px;height: 200px;" src="' + basePath + 'Content/intranet/images' + detalleelemento.detelm_ubicacion + '' + detalleelemento.detelm_nombre + '.' + detalleelemento.detelm_extension + '" alt=""></a>';
                                    }

                                });

                                appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                    '<div class="four columns">' +
                                    '<div class="sponsor">' +
                                    appendsDerecha +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="eight columns" >' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsIzquierda +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div>' +

                                    '</div></div>';
                            }

                        }

                        if (elemento.fk_tipo_elemento == 16) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendsIzquierda = "";
                            var appendsCentro = "";
                            var appendsDerecha = "";
                            if (detalleElementolista.length > 0) {
                                iz = 1, ce = 1, r = 1;
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (detalleelemento.detelm_posicion == "L") {
                                        appendsIzquierda += '<li><div class="ctitle wt' + iz + '"> ' + detalleelemento.detelm_descripcion + ' </div></li>';
                                        iz++;
                                    }
                                    if (detalleelemento.detelm_posicion == "C") {
                                        appendsCentro += '<li><div class="ctitle wt' + ce + '"> ' + detalleelemento.detelm_descripcion + ' </div></li>';
                                        ce++;
                                    }
                                    if (detalleelemento.detelm_posicion == "R") {
                                        appendsDerecha += '<li><div class="ctitle wt' + r + '"> ' + detalleelemento.detel_descripcion + ' </div></li>';
                                        r++;
                                    }
                                });

                                appendElementos += '<div class="post-content" style="padding-bottom:0px"><div class="row">' +
                                    '<div class="four columns">' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsIzquierda +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div >' +
                                    '<div class="four columns">' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsCentro +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div>' +
                                    '<div class="four columns">' +
                                    '<div class="widget lreview">' +
                                    '<ul class="listreview">' +
                                    appendsDerecha +
                                    ' </ul>' +
                                    '</div>' +
                                    '</div>' +
                                    '</div></div>';
                            }
                        }

                        if (elemento.fk_tipo_elemento == 17) {
                            var detalleElementolista = elemento.detalle_elemento_modal;
                            var appendimageneslistasimple = "";
                            var float_ = "";
                            if (detalleElementolista.length > 0) {
                                $.each(detalleElementolista, function (index, detalleelemento) {
                                    if (index == (detalleElementolista.length - 1)) {

                                        float_ = 'style="float:left;"';
                                    }
                                    appendimageneslistasimple += ' <div class="three tilesli columns" ' + float_ + '>' +
                                        '<div class="space_" >' +
                                        '<div class="itemblog">' +
                                        '<div class="clear"></div>' +
                                        '<div class="thumb">' +
                                        '<a href="javascript:void(0);"><img src="' + basePath + 'Content/intranet/images' + detalleelemento.detelm_ubicacion + '' + detalleelemento.detelm_nombre + '.' + detalleelemento.detelm_extension + '" alt=""></a>' +
                                        '</div>' +
                                        '<div class="clear"></div>' +
                                        '<div class="excerpt">' +
                                        '<h2 class="ntitle"><a href="javascript:void(0);">' + detalleelemento.detelm_descripcion + '</a></h2>' +
                                        '</div>' +
                                        '</div>' +
                                        '</div>' +
                                        '</div>';
                                });

                                appendElementos += '<article>' +
                                    '<div class="post-content">' +
                                    appendimageneslistasimple +
                                    '</div>' +
                                    '<div class="clear"></div>' +
                                    '</article>';
                            }
                        }
                    })
                    appendSeccion += '<section id="singlepost">' + appendElementos + '</section><div class="separador"></div>';
                    $("#contenido_modal").html(appendSeccion);
                   
                    $.pgwModal({
                        target: '#contenido_modal',
                        title: 'CORPORACION PJ',
                        maxWidth: 800
                    });
                }
               
                
            }
            console.log(my_object.length); 
        });
        

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