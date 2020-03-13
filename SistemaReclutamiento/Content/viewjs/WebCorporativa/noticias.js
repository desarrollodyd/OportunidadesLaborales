var WebNoticias = function () {
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
                    console.log(rows);
                    rows.forEach(function(row){
                        if(row.elemento!=null){
                            var elemento=row.elemento;
                            if(row.detalle!=null&&row.detalle.length>0){
                                var tipo=elemento.fk_tipo_elemento;
                                var detalle=row.detalle;
                                //slider superior
                                if(tipo==4){
                                    $("#ElementoSuperior").html("");
                                    span='';
                                    detalle.forEach(detalle => {
                                        span+='   <li style="width: 234px;">'+
                                        '<article class="entry-item clearfix">'+
                                            '<div class="entry-thumb" style="margin-bottom: 0px;">'+
                                                '<a href="#"><img src="'+basePath+'WebFiles/'+detalle.detel_imagen+'" alt="" style="" /></a>'+
                                            '</div>'+
                                            '<div class="entry-content">'+
                                                '<div class="imgsubt">'+detalle.detel_titulo+'</div>'+
                                            '</div>'+
                                     
                                    '</li>';
                                    });
                                    $("#ElementoSuperior").html(span);
                                }
                                //slider inferior
                                else if(tipo==5){
                                    $("#header-middlee").html("");
                                    span='';
                                    detalle.forEach(detalle=>{
                                        span+='<img src="'+basePath+'WebFiles/'+detalle.detel_imagen+'" style="width: 100%;height: 100%;float: left;z-index: 1">';
                                    });
                                    $("#header-middlee").html(span)
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
    WebNoticias.init();
})