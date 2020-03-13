var WebSalas = function () {
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
                    var span_detalle='';
                    var span_departamento='';
                    rows.forEach(function(row){
                        if(row.elemento!=null){
                            var elemento=row.elemento;
                            if(row.detalle!=null&&row.detalle.length>0){
                                var tipo=elemento.fk_tipo_elemento;
                                var detalle=row.detalle;
                                var contador=1;
                                var selected=1;
                                var hidden='';
                                var current='';
                                //slider superior
                                if(tipo==1){
                                    $("#ElementoSalas").html("");
                                    span='';
                                    span_detalle='';
                                    detalle.forEach(detalle => {
                                        if(selected==1){
                                            current='current';
                                            hidden='';
                                        }
                                        else{
                                            current='';
                                            hidden='style="display:none;"';
                                        }
                                        span_departamento+=' <div class="span4 '+current+' span'+detalle.detel_id+'" style="text-align: center;">'+
                                        '<a href="javascript:void(0);"><img src="'+basePath+"WebFiles/" + detalle.detel_imagen+'" alt="" /></a>'+
                                        '<div class="subtitulop">'+detalle.detel_titulo+'</div>'+
                                        '<span></span>'+
                                        '</div>';
                                        span_detalle+='  <div class="row-fluid row'+detalle.detel_id+'" '+hidden+'>'+
                                        '<div id="header-middle_" class="clearfix" >'+
                                            '<img src="'+basePath+"WebFiles/" + detalle.detel_imagen_detalle+'" style="width: 100%;height: 100%;float: left;z-index: 1">'+
                                        '</div>'+
                                        '</div>';
                                        if(contador==3){
                                            span+='<div class="row-fluid" style="">'+span_departamento+'</div>'+span_detalle;
                                            span_departamento='';
                                            span_detalle='';
                                            contador=1;
                                        }
                                        else{
                                            contador++;
                                        }
                                        selected++;
                                        
                                    });
                                    $("#ElementoSalas").html(span);
                                }
                            }
                        }
                    });
                }
            }
        })
    };
    var _componentes=function(){
        $(document).ready(function(){
            $("#prueba").hover(function(){
                $("#prueba").css("background-color", "#000000");
                }, function(){
                    $("#prueba").css("background-color", "#C3C3C3");
                });
        });
    }
    return {
        init: function () {
            _ListarElementos();
            _componentes;
        },
    }
}();
document.addEventListener('DOMContentLoaded', function () {
    WebSalas.init();
})