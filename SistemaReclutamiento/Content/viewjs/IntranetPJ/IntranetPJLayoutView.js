var LayoutVista = function () {
    var _Layout = function () {
        var meses = ["ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO",
            "JULIO", "AGOSTO", "SETIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE"
        ];
        dias = ["Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo"];
        var diahoy = new Date();
        var menuInicio = localStorage.getItem('posicionMenu');
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
                //Creacion de Menus
                if (dataMenus.length > 0) {
                    $("#menuIntranet").html("");
                    $("#barnav").html("");
                    var appendMenuLateral = "";
                    var appendMenuPrincipal = "";
                    $.each(dataMenus, function (index, menu) {
                        appendMenuLateral += '<li><a href="' + menu.menu_url + '">' + menu.menu_titulo + '</a></li>';
                        var clase = (index == 0) ? "active" : "";
                        appendMenuPrincipal += '<li class="' + clase + '"><a href="#" data-id="' + menu.menu_id+'" class="sf-with-ul">' + menu.menu_titulo + '</a><ul></ul></li>';
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
            }
        });
    };
    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _Layout();
        }
    }
}();
// Initialize module
// ------------------------------
document.addEventListener('DOMContentLoaded', function () {
    LayoutVista.init();
});