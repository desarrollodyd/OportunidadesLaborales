
var PanelDepartamentos = function () {
    var _ListarMenus = function () {
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }

        responseSimple({
            url: "WebDepartamento/WebDepartamentoListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
       
            }
        });
    };
    var _componentes = function () {


    };

    var _metodos = function () {
  

    };

 

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarMenus();
            _componentes();
            _metodos();

        },
        init_ListarMenus: function () {
            _ListarMenus();
        },
  
    }
}();
// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelDepartamentos.init();
});