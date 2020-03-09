var WebMenus = function () {
    var _ListarMenus = function () {
        
        responseSimple({
            url: "WebCorporativaAdmin/IntranetMenuSeccionListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                var data = response.data;
                var li = "";
                var div = "";
                var activo = "";
                var contenidoDiv = "";
                var menu_id_ = 0;


            }
        });
    };
  
    var _componentes = function () {
       

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
        },
       
    }
}();
document.addEventListener('DOMContentLoaded', function () {
    WebMenus.init();
})