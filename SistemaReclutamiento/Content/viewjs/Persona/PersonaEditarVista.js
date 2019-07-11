$(document).ready(function () {

    $('#myDatepicker').datetimepicker({
        format: 'DD/MM/YYYY',
        ignoreReadonly: true,
        allowInputToggle: true
    });
    $("#per_id").val(persona.per_id);
    $("#per_nombre").val(persona.per_nombre);
    $("#per_apellido_pat").val(persona.per_apellido_pat);
    $("#per_apellido_mat").val(persona.per_apellido_mat);
    $("#per_direccion").val(persona.per_direccion);
    $("#per_numdoc").val(persona.per_numdoc);
    $("#per_fechanacimiento").val(moment(persona.per_fechanacimiento).format('DD/MM/YYYY'));   
    $("#per_celular").val(persona.per_celular);
    $("#per_telefono").val(persona.per_telefono);
    $("#cboSexo").val(persona.per_sexo);
    $("#cbotipoDocumento").val(persona.per_tipodoc);
    $("#per_correoelectronico").val(persona.per_correoelectronico);

    $(document).on('click', '#btnGuardar', function () {
        var validar = $("#frmNuevo");
        if (validar.valid()) {
            var dataForm = $("#frmNuevo").serializeFormJSON();
            var url = basePath + "Persona/PersonaEditarJson";
            fncRegistrar(dataForm, url, false);
        }

    });

});
//var max_chars_empresa = 2;

//$('#COD_EMPRESA').keydown(function (e) {
//    if ($(this).val().length >= max_chars_empresa) {
//        $(this).val($(this).val().substr(0, max_chars_empresa));
//    }
//});

//$('#COD_EMPRESA').keyup(function (e) {
//    if ($(this).val().length >= max_chars_empresa) {
//        $(this).val($(this).val().substr(0, max_chars_empresa));
//    }
//});
$("#frmNuevo")
    .validate({
        rules: {
            per_nombre:
            {
                required: true,

            },
            per_apellido_pat:
            {
                required: true,

            },
            per_apellido_mat:
            {
                required: true,

            },
            per_direccion:
            {
                required: true,

            },
            per_numdoc:
            {
                required: true,

            },
            per_fechanacimiento:
            {
                required: true,

            },
            per_celular:
            {
                required: true,

            },
            per_telefono:
            {
                required: true,

            }
        },
        messages: {
            per_nombre:
            {
                required: 'REQUERIDO',
            },
            per_apellido_pat:
            {
                required: 'REQUERIDO',
            },
            per_apellido_mat:
            {
                required: 'REQUERIDO',
            },
            per_direccion:
            {
                required: 'REQUERIDO',
            },
            per_numdoc:
            {
                required: 'REQUERIDO',
            },
            per_fechanacimiento:
            {
                required: 'REQUERIDO',
            },
            per_celular:
            {
                required:'REQUERIDO',
            },
            per_telefono:
            {
                required: 'REQUERIDO',
            }

        },
        errorPlacement: function (error, element) {
            if (element.is(":radio") || element.is(":checkbox")) {
                element.closest('.option-group').after(error);
            }
            else {
                error.insertAfter(element);
            }
        }
    });