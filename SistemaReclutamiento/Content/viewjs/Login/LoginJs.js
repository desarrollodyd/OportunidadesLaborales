$(document).ready(function () {
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
})

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

            console.log(response);
            var respuesta = response.respuesta;
            var mensaje = response.mensaje;
            if (respuesta) {
                toastr.success(mensaje, 'Mensaje Servidor');
                setTimeout(function () {
                    window.location.replace(basePath + 'Home/Index');
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
            usuLogin: {
                required: true,
            },

            usuPassword: {
                required: true,
            },
        },
        messages: {
            usuLogin: {
                required: 'Debe Ingresar su Correo',
            },

            usuPassword: {
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