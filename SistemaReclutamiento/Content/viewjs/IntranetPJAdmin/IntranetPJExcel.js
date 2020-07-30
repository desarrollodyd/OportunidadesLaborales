var PanelExcel = function () {
    var excelBase64='';
    var _inicio = function () {
        $('#excel').ace_file_input({
            no_file: 'sin archivo ...',
            btn_choose: 'escoger',
            btn_change: 'cancelar',
            droppable: false,
            onchange: null,
            thumbnail: false
        });
        responseSimple({
            url: "IntranetPJAdmin/MostrarExcelModeloJson",
            refresh: false,
            callBackSuccess: function (response) {
               excelBase64=response.data;
            }
        });
    };

    var _metodos = function () {
        $(document).on('click','.btn_descargar_modelo',function(e){
        
            var link = document.createElement('a');
            document.body.appendChild(link); //required in FF, optional for Chrome
            link.href = "data:application/vnd.ms-excel;base64, "+excelBase64;;
            link.download = "ExcelPrueba.xls";
            link.click();
            link.remove();
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
                responseFileSimple({
                    url:url,
                    data:dataForm,
                    refresh:false,
                    callBackSuccess:function(response){
                        var link = document.createElement('a');
                        document.body.appendChild(link); //required in FF, optional for Chrome
                        link.href = "data:application/vnd.ms-excel;base64, "+response.base64;;
                        link.download = "ExcelResultado.xls";
                        link.click();
                        link.remove();
                    }
                })
                // if (extension != 'xls' ||extension !='xlsx') {
                //     messageResponse({
                //         text: 'Sólo Se Permite formato Excel (.xls||xlsx)',
                //         type: "warning"
                //     });
                  
                // }
                // else {
                
                // }
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