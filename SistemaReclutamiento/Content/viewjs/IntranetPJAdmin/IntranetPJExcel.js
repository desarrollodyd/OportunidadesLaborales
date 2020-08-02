var PanelExcel = function () {
   
    var _inicio = function () {
        $('#excel').ace_file_input({
            no_file: 'sin archivo ...',
            btn_choose: 'escoger',
            btn_change: 'cancelar',
            droppable: false,
            onchange: null,
            thumbnail: false
        });
   
    };

    var _metodos = function () {
        $(document).on('click','.btn_descargar_modelo',function(e){
            var excelBase64='';
            responseSimple({
                url: "IntranetPJAdmin/MostrarExcelModeloJson",
                refresh: false,
                callBackSuccess: function (response) {
                    if(response.respuesta){
                        excelBase64=response.data;
                        var link = document.createElement('a');
                        document.body.appendChild(link); //required in FF, optional for Chrome
                        link.href = "data:application/vnd.ms-excel;base64, "+excelBase64;;
                        link.download = "ExcelPrueba.xlsx";
                        link.click();
                        link.remove();
                        console.log(response);
                    }
                    else{
                        messageResponse({
                            text: response.mensaje,
                            type: "error"
                        })
                    }
                }
            });
      
        });
        $(document).on('click','.btn_subir_excel',function(e){
            var file = $('#excel')[0].files[0];
            if(file==null){
                messageResponse({
                    text: "Debe Seleccionar Un Archivo Adjunto",
                    type: "error"
                })
                return false;
            }
            else{
                var image_arr = file.name.split(".");
                var extension = image_arr[1].toLowerCase();
                url='IntranetPJAdmin/SubirExcelJson';
                var dataForm=new FormData();
                dataForm.append('file',file);
                // console.log(extension);
           
                if (extension != 'xls' && extension !='xlsx') {
                    messageResponse({
                        text: 'Sólo Se Permite formato Excel (.xls||xlsx)',
                        type: "warning"
                    });
                  
                }
                else {
                    responseFileSimple({
                        url:url,
                        data:dataForm,
                        refresh:false,
                        callBackSuccess:function(response){
                            var link = document.createElement('a');
                            document.body.appendChild(link); //required in FF, optional for Chrome
                            link.href = "data:application/vnd.ms-excel;base64, "+response.base64;;
                            link.download = "ExcelResultado.xlsx";
                            link.click();
                            link.remove();
                        }
                    })
                }
            }
          
        });
    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();
            _metodos();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelExcel.init();
});