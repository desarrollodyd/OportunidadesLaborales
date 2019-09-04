var ProveedorVista = function () {

    var _inicio = function () {
        selectResponse({
            url: "Proveedor/RolListarJson",
            select: "cboRol_",
            campoID: "rol_id",
            CampoValor: "rol_nombre",
            select2: true,
            allOption: false,
            placeholder: "Seleccione Rol"
        });
    };
    var _ListarModulos = function () {
        responseSimple({
            url: "Proveedor/ModuloListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                var respuesta = response.data;
                if (respuesta) {
                    $("#libody").html("");
                    menus = [];
                    $.each(respuesta, function (index, value) {
                        menus.push(value.mod_descripcion);
                    });
                    console.log(menus);
                }
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
            _inicio();
            _componentes();
            _ListarModulos();
            _metodos();
        },
        __ListarModulos: function () {
            _ListarModulos();
        },
    }
}();
// Initialize module
// ------------------------------
document.addEventListener('DOMContentLoaded', function () {
    ProveedorVista.init();
});