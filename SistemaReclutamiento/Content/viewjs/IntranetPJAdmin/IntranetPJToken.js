var PanelToken = function () {
    var _ListarUsuarios = function () {

        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        responseSimple({
            url: "IntranetToken/IntranetTokenListarUsuariosJson",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable_usuariosListado",
                    table: "#usuariosListado",
                    tableColumnsData: response.data,
                    tableHeaderCheck: true,
                    tableHeaderCheckIndex: 0,
                    headerCheck: "chk_usuarios",
                    tableColumns: [
                        {
                            data: "usu_id",
                            title: "",
                            "bSortable": false,
                            className: 'align-center',
                            "render": function (value) {
                                var check = '<input type="checkbox" class="form-check-input-styled-info usuariosListado" data-id="' + value + '" name="chk[]">';
                                return check;
                            },
                            width: "50px",
                        },
                        {
                            data: "usu_id",
                            title: "Id Usuario",
                        },
                        {
                            data: "usu_id",
                            title: "Nombre",
                            "render":function(value,type,row){
                                var span=''+row.per_apellido_pat+' '+row.per_apellido_mat+', '+row.per_nombre;
                                return span.toUpperCase();
                            }
                        },
                        {
                            data: "usu_nombre",
                            title: "Usuario",
                        },
                        {
                            data: "usu_token",
                            title: "Token",
                        },
                    ]
                })
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
            _ListarUsuarios();
            _componentes();
            _metodos();

        },
        init_ListarUsuarios: function () {
            _ListarUsuarios();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelToken.init();
});