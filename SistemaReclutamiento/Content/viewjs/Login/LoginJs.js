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
    $('#usu_clave_temp').val(usuario.usu_clave_temp);
    //$("input").keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        e.preventDefault();
    //        $('#btnSesion').click();
    //    }
    //});

     //Formulario de Registro de Usuario

    $('#cbotipoDocumento').change(function (e) {
        if ($(this).val() === "") {
            $('#per_numdoc').prop("disabled", true);
        } else {
            $('#per_numdoc').prop("disabled", false);
        }
    })  
    $(document).on('click', '#btnGuardar', function () {

        var validar = $("#frmNuevo");
        if (validar.valid()) {
            if ($('#cbotipoDocumento').val().trim() === '') {
                alert('Debe seleccionar un Tipo de Documento');

            } else {
                var dataForm = $("#frmNuevo").serializeFormJSON();
                var url = basePath + "Persona/PersonaInsertarJson";
                fncRegistrar(dataForm, url, true);
            }

            
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
                    window.location.replace(basePath + 'Persona/PersonaIndexVista');
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
$('#per_numdoc').keydown(function (e) {
    if ($(this).val().length >= max_chars) {
        $(this).val($(this).val().substr(0, max_chars));
    }
});
$('#per_numdoc').keyup(function (e) {
    if ($(this).val().length >= max_chars) {
        $(this).val($(this).val().substr(0, max_chars));
    }
});

$('#per_numdoc').keyup(function () {
    this.value = (this.value + '').replace(/[^0-9]/g, '');
});

$("#per_nombre").bind('keypress', function (event) {
    var regex = new RegExp("^[a-zA-Z ]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});
$("#per_apellido_pat").bind('keypress', function (event) {
    var regex = new RegExp("^[a-zA-Z ]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
});
$("#per_apellido_mat").bind('keypress', function (event) {
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
            per_numdoc:
            {
                required: true,
            },
            per_correoelectronico:
            {
                required: true,
            },
            usu_contrasenia:
            {
                required: true,
            }
        },
        messages: {
            per_nombre:
            {
                required: 'Nombre Obligatorio',
            },
            per_apellido_pat:
            {
                required: 'Apellido Paterno Obligatorio',
            },
            per_apellido_mat:
            {
                required: 'Apellido Materno Obligatorio',
            },
            per_numdoc:
            {
                required: 'Dni Obligatorio',
            },
            per_correoelectronico:
            {
                required: 'Email Obligatorio',
            },
            usu_contrasenia:
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