$(document).ready(function () {

    //Formulario de LOGIN
    $(document).on('click', '#btnCambio', function () {
        var validar = $("#frmCambio");
        if (validar.valid()) {
            var dataForm = $("#frmCambio").serializeFormJSON();
            var url = basePath + "Login/CambiarPasswordUsuario";
            cambiarPassword(url, dataForm);
        }
    });
    //$('#usu_clave_temp').val(usuario.usu_clave_temp);
   
})
//Validaciones para el Login
function cambiarPassword(url, dataForm) {
    $.ajax({
        type: 'POST',
        url: url,
        data: dataForm,
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
            $("#usu_password").attr('readonly', false);
            $("#usu_password_repetido").attr('readonly', false);
            $("#btnCambio").attr('disabled', false);
        },
        success: function (response) {

            //console.log(response);
            var respuesta = response.respuesta;
            var mensaje = response.mensaje;
            if (respuesta) {
                toastr.success(mensaje, 'Mensaje Servidor');
                setTimeout(function () {
                    window.location.replace(basePath + 'Persona/PersonaIndexVista');
                }, 2000);
            } else {               
               
            $("#usu_password").attr('readonly', true);
            $("#usu_password_repetido").attr('readonly', true);
            $("#btnCambio").attr('disabled', true);
            toastr.warning(mensaje, 'Mensaje Servidor');
            
            }
        }
    });
}
$("#frmCambio")
    .validate({
        rules: {
            usu_password: {
                required: true,
            },

            usu_password_repetido: {
                required: true,
                equalTo:'#usu_password'
            },
        },
        messages: {
            usu_password: {
                required: 'Debe Ingresar una Contraseña'
            },

            usu_password_repetido: {
                required: 'Debe Ingresar una Contraseña',
                equalTo:'Las contraseñas deben ser iguales'
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
