var PanelFichas = function () {
    var _ListarFichas = function (empresa,sede) {

        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        var dataForm = { empresa, sede };

        responseSimple({
            url: "IntranetFichas/IntranetFichasEmviarLink",
            refresh: false,
            data: dataForm,
            callBackSuccess: function (response) {
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable_fichasListado",
                    table: "#fichasenvioListado",
                    tableColumnsData: response.data,
                    tableHeaderCheck: true,
                    tableHeaderCheckIndex: 0,
                    headerCheck: "chk_fichas",
                    tableColumns: [
                        {
                            data: "id",
                            title: "",
                            "bSortable": false,
                            className: 'align-center',
                            "render": function (value) {
                                var check = '<input type="checkbox" class="form-check-input-styled-info fichasListado" data-id="' + value + '" name="chk[]">';
                                return check;
                            },
                            width: "50px",
                        },
                        {
                            data: "nombre",
                            title: "Nombre Empleado",
                        },
                        {
                            data: "empresa",
                            title: "Empresa",
                        },
                        {
                            data: "sede",
                            title: "Sede",
                        },
                        {
                            data: "correoCorporativo",
                            title: "C.Corporativo",
                        },
                        {
                            data: "correoPersonal",
                            title: "C.Personal",
                        },
                    ]
                })
            }
        });
    };

    var _componentes = function () {

    };

    var _metodos = function () {
        $('#txt-firma').ace_file_input({
            no_file: 'sin archivo ...',
            btn_choose: 'escoger',
            btn_change: 'cambiar',
            droppable: false,
            onchange: null,
            thumbnail: false //| true | large
            //whitelist:'gif|png|jpg|jpeg'
            //blacklist:'exe|php'
            //onchange:''
            //
        });

        var dateinicio = new Date(moment().format("MM-DD-YYYY"));
        $('#txt-fecha').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })

    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarFichas();
            _componentes();
            _metodos();

        },
        init_ListarFichas: function () {
            _ListarFichas();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelFichas.init();
});