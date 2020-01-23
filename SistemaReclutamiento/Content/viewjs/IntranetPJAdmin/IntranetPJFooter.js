var PanelFooter = function () {
   
    var _inicio=function(){
        responseSimple({
            url:"IntranetFooter/IntranetFooterObtenerImagenes",
            refresh:false,
            callBackSuccess:function(response){
                var data=response.data;
                if(data.length>0){
                    $.each(data, function (index, value) {
                       if(value.foot_posicion=="D"){
                            $("#ruta_anterior_derecha").val(value.foot_imagen);
                            $("#estado_derecha").val(value.foot_estado);
                       }
                       else{

                       }
                    });
                }
            }
        })        
    };
    var _componentes = function () {
        $(document).on('change','#file_izquierda',function(e){
            let reader=new FileReader();
            reader.onload=function(){
                let preview=document.getElementById('preview_izquierda'),
                image=document.createElement('img');
                image.src=reader.result;
                image.style.width="100%;";
                image.style.height="100;";
                preview.innerHTML='';
                preview.append(image);
            }
            reader.readAsDataURL(e.target.files[0]);
        })
        $(document).on('change','#file_derecha',function(e){
            let reader=new FileReader();
            reader.onload=function(){
                let preview=document.getElementById('preview_derecha'),
                image=document.createElement('img');
                image.src=reader.result;
                image.style.width="100% ;";
                image.style.height="100%;";
                preview.innerHTML='';
                preview.append(image);
            }
            reader.readAsDataURL(e.target.files[0]);
        })
        $(document).on('click','.btn-guardar-footer-izquierda',function(){
            console.log("click");
            $("#form_footer_izquierda").submit();
            if (_objetoForm_form_footer_izquierda.valid()) {
                var dataForm = new FormData(document.getElementById("form_footer_izquierda"));
                var url = "IntranetFooter/IntranetFooterInsertarJson";
                responseFileSimple({
                    url: url,
                    data: dataForm,
                    refresh: false,
                    callBackSuccess: function (response) {
                        if(response.respuesta==true){
                            
                        }
                    }
                });
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        })
    }

    var _metodos = function () {
        validar_Form({
            nameVariable: 'form_footer_izquierda',
            contenedor: '#form_footer_izquierda',
            rules: {
                foot_posicion:
                {
                    required: true,

                },
                foot_imagen:
                {
                    required: true,

                }

            },
            messages: {
                foot_posicion:
                {
                    required: 'Campo Obligatorio',
                },
                foot_imagen:
                {
                    required: 'Campo Obligatorio',
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