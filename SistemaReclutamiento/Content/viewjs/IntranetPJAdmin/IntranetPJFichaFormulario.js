var PanelFichaFormulario = function () {

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
    PanelFichaFormulario.init();
});