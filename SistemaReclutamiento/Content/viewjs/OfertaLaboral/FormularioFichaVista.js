var FormularioFichaVista = function () {

    var _ListardatosFicha = function () {

    }

    var _componentes = function () {
       

        var dateinicio = new Date(moment().format("MM-DD-YYYY"));
        $('#txt-fecha').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmFichaFormulario',
            contenedor: '#frmFichaFormulario-form',
            rules: {

            },
            messages: {

            }
        });
    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListardatosFicha();
            _componentes();
            _metodos();
        },
        __ListardatosFicha: function () {
            _ListardatosFicha();
        },
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    FormularioFichaVista.init();
});