var PanelFooter = function () {
   
    var _componentes = function () {
    }

    var _metodos = function () {
        $(document).on('change','file',function(){
            let reader=new FileReader();
            reader.onload=function(){
                let preview=document.getElementById('vista_previa');
                let image=document.getElementById('imagen_derecha');
                image.src=reader.result;
                // preview.innerHTML="";
                // preview.append(image);
            }
            reader.readAsDataURL(e.targe.files[0]);
        })
    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _componentes();
            _metodos();

        },
        init_ListarComentarios: function () {
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelFooter.init();
});