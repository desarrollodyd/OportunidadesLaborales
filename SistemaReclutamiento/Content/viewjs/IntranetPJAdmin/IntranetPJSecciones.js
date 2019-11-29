var PanelSecciones = function () {

    var _ListarSecciones = function () {
        if (menu.length > 0) {
            var appendMenus = '';
            var selectMenus = $("#cboMenus");
            selectMenus.html("");
            appendMenus += '<option value="">Seleccione Menu</option>';
            $.each(menu, function (index, value) {
                appendMenus += '<option value="' + value.menu_id + '">"' + value.menu_titulo + '"</option>';
            });
            selectMenus.html(appendMenus);
            selectMenus.select2();
        }
    };
    var _componentes = function () {
        $(document).on("change", "#cboMenus", function () {
            var menu_id = $(this).val();
            if (menu_id != "") {
                var dataForm = { menu_id: menu_id };
                responseSimple({
                    url: "IntranetSeccion/IntranetSeccionListarxMenuIDJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        console.log(response);
                    }
                })
            }
            else {
                messageResponse({
                    text: "Debe Seleccionar un Menu",
                    type: "error"
                })
            }
        });
    };

    var _metodos = function () {
      
    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarSecciones();
            _componentes();
            _metodos();

        },
        init_ListarSecciones: function () {
            _ListarSecciones();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelSecciones.init();
});