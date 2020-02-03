var PanelDescargas = function () {
    var _ListarArchivos = function () {
      
        responseSimple({
            url: "IntranetPJ/IntranetObtenerListadoArchivos",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
                var span='';
                var datos=response.data;
                if(response.respuesta){
                    $("#tbody_Archivos").html("");
                    $.each(datos, function (index, value) {
                        $("#tbody_Archivos").append('<tr><td>' + (index+1) + '</td><td>'+value.nombre+'</td><td>' + value.extension + '</td><td><button type="button" data-nombre="' + value.nombre_completo+ '" class="btn btn-danger btn-xs btn_descargar">Descargar</button></td></tr>');
                }); 
                }
                
            }
        });
    };
    var _componentes = function () {
        $(document).on('click','.btn_descargar',function(){
            var archivo=$(this).data("nombre");
            if(archivo!=''){
                window.location.href = basePath + "IntranetPJ/IntranetDescargarArchivo?fileName=" + archivo;
            }
            
        })
    };



    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarArchivos();
            _componentes();

        },
        init_ListarActividades: function () {
            _ListarArchivos();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelDescargas.init();
});