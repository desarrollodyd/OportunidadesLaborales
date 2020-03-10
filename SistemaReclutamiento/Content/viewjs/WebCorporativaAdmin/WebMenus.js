var WebMenus = function () {
    var _ListarMenus = function () {
        var dataForm={
            menu_id:1,
            tipo:1
        }
        responseSimple({
            url: "WebCorporativaAdmin/WebElementoListarxMenuIDxtipoJson",
            refresh: false,
            data:JSON.stringify(dataForm),
            callBackSuccess: function (response) {
                console.log(response);
            }
        });
    };
  
    var _componentes = function () {
       $(document).on('click','.btnNuevo',function(){
            var id=$(this).data("id");
            $("#modalFormularioDetalleElemento").show();    
       })
       $(document).on('click','#tabmenu li',function(){
        var menu_id = $(this).data("id");
        console.log(menu_id);
   })
    }
    var _metodos = function () {
       
        validar_Form({
            nameVariable: 'form_elemento',
            contenedor: '#form_elemento',
            rules: {
                fk_tipo_elemento:
                {
                    required: true,

                },
                elem_estado:
                {
                    required: true,

                }
            },
            messages: {
                fk_tipo_elemento:
                {
                    required: 'Campo Obligatorio',
                },
                elem_estado:
                {
                    required: 'Campo Obligatorio',
                }
            }
        });
    }
    return {
        init: function () {
            _componentes();
            _metodos();
            _ListarMenus();
        },
       
    }
}();
document.addEventListener('DOMContentLoaded', function () {
    WebMenus.init();
})