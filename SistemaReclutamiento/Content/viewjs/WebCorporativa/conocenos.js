var WebConocenos = function () {
    var _ListarElementos = function () {
        var dataForm={
            menu_id:menu_id,
        }
        responseSimple({
            url:'WebCorporativaAdmin/WebElementoListarxMenuIDJson',
            refresh:false,
            data:JSON.stringify(dataForm),
            callBackSuccess:function(response){
                if(response.respuesta){
                    var rows=response.data;
                    var span='';
                    // console.log(rows[0].elemento);
                    rows.forEach(function(row){
                        if(row.elemento!=null){
                            var elemento=row.elemento;
                            if(row.detalle!=null&&row.detalle.length>0){
                                var tipo=elemento.fk_tipo_elemento;
                                var detalle=row.detalle;
                                //slider superior
                                if(tipo==6){
                                    $("#ElementoSuperior").html("");
                                    span='';
                                    detalle.forEach(detalle => {
                                        span+='<li style="height:550px">'+
                                        '<img src="'+basePath+"WebFiles/" + detalle.detel_imagen+'" alt="" />'+
                                        '<h2 class="gallery-caption"><div class="gatit">'+detalle.detel_titulo+'</div><br>'+detalle.detel_subtitulo+'</h2>'+
                                    '</li>';
                                    });
                                    $("#ElementoSuperior").html(span);
                                }
                                //slider inferior
                                else if(tipo==7){
                                    $("#ElementoInferior").html("");
                                    span='';
                                    detalle.forEach(detalle=>{
                                        span+='<li style="width: 234px;">'+
                                        '<article class="entry-item clearfix">'+
                                           '<div class="entry-thumb" style="margin-bottom: 0px;">'+
                                                '<a href="#"><img src="'+basePath+'WebFiles/'+detalle.detel_imagen+'" alt="" style="" /></a>'+
                                            '</div>'+
                                            '<div class="entry-content">'+
                                                '<div class="imgsubt">'+detalle.detel_titulo+'</div>'+
                                                '<div class="titu_noti"><strong>'+detalle.detel_subtitulo+'</strong></div>'+
                                                '<div class="psubtitu_noti">'+detalle.detel_parrafo+'</div>'+
                                            '</div>'+
                                            '</li>';
                                    });
                                    $("#ElementoInferior").html(span)
                                }
                            }
                        }
                    });
                }
            }
        })
    };
    return {
        init: function () {
            _ListarElementos();
        },
    }
}();
document.addEventListener('DOMContentLoaded', function () {
    WebConocenos.init();
})