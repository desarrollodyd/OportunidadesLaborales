$(document).ready(function () {

    //Formulario de LOGIN
    $(document).on('click', '#btnSesion', function () {
        var validar = $("#frmLogin");
        if (validar.valid()) {
            var dataForm = $("#frmLogin").serializeFormJSON();
            var url = basePath + "Login/ValidarLoginJson";
            ValidarLogin(url, dataForm);
        }
    });
    $("input").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('#btnSesion').click();
        }
    });
    $.when(llenarSelect(basePath + "Persona/TipoDocumentoListarJson", {}, "cbotipoDocumento", "tipoDocumentoId", "tipoDocumentoDescripcion", "Seleccione un Tipo de Documento")).then(function (response, textStatus) {
        $("#cbotipoDocumento").select2();
    });
    //Formulario de Registro de Usuario
    $(document).on('click', '#btnGuardar', function () {
        var validar = $("#frmNuevo");
        if (validar.valid()) {
            var dataForm = $("#frmNuevo").serializeFormJSON();
            var url = basePath + "Persona/PersonaInsertarJson";
            fncRegistrar(dataForm, url, true);
        }
    });
})
//Validaciones para el Login
function ValidarLogin(url, dataForm) {
    $.ajax({
        type: 'POST',
        url: url,
        data: dataForm,
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {            
            $.LoadingOverlay("hide");
            $("#usu_login").attr('readonly', false);
            $("#usu_password").attr('readonly', false);
            $("#btnSesion").attr('disabled', false);
        },
        success: function (response) {

            //console.log(response);
            var respuesta = response.respuesta;
            var mensaje = response.mensaje;
            if (respuesta) {
                toastr.success(mensaje, 'Mensaje Servidor');
                setTimeout(function () {
                    window.location.replace(basePath + 'Persona/Index');
                }, 1000);
            } else {
                $("#usu_login").attr('readonly', true);
                $("#usu_password").attr('readonly', true);
                $("#btnSesion").attr('disabled', true);
                toastr.warning(mensaje, 'Mensaje Servidor');
              
            }
        }
    });
}
$("#frmLogin")
    .validate({
        rules: {
            usu_login: {
                required: true,
            },

            usu_password: {
                required: true,
            },
        },
        messages: {
            usu_login: {
                required: 'Debe Ingresar su Correo',
            },

            usu_password: {
                required: 'Debe Ingresar una Constraseña',
            },
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

// Validaciones para el registro
var max_chars = 8;
$('#personaNroDocumento').keydown(function (e) {
    if ($(this).val().length >= max_chars) {
        $(this).val($(this).val().substr(0, max_chars));
    }
});
$('#personaNroDocumento').keyup(function (e) {
    if ($(this).val().length >= max_chars) {
        $(this).val($(this).val().substr(0, max_chars));
    }
});

$('#personaNroDocumento').keyup(function () {
    this.value = (this.value + '').replace(/[^0-9]/g, '');
});

$("#personaNombre").bind('keypress', function (event) {
    var regex = new RegExp("^[a-zA-Z ]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});
$("#personaApellidoPaterno").bind('keypress', function (event) {
    var regex = new RegExp("^[a-zA-Z ]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});
$("#personaApellidoMaterno").bind('keypress', function (event) {
    var regex = new RegExp("^[a-zA-Z ]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});

$("#frmNuevo")
    .validate({
        rules: {
            personaNombre:
            {
                required: true,
            },
            personaApellidoMaterno:
            {
                required: true,
            },
            personaApellidoPaterno:
            {
                required: true,
            },
            personaNroDocumento:
            {
                required: true,
            },
            personaEmail:
            {
                required: true,
            },
            usuarioContrasenia:
            {
                required: true,
            }
        },
        messages: {
            personaNombre:
            {
                required: 'Nombre Obligatorio',
            },
            personaApellidoPaterno:
            {
                required: 'Apellido Paterno Obligatorio',
            },
            personaApellidoMaterno:
            {
                required: 'Apellido Materno Obligatorio',
            },
            personaNroDocumento:
            {
                required: 'Dni Obligatorio',
            },
            personaEmail:
            {
                required: 'Email Obligatorio',
            },
            usuarioContrasenia:
            {
                required: 'Contraseña Obligatoria',
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