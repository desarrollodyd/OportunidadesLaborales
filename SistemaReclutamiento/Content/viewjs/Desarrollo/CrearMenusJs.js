var MenuVista = function () {

    var _ListarMenus = function () {
        responseSimple({
            url: "Super/MenuListarporTipoJson",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
                var respuesta = response.respuesta;
                var datos = response.data;
                if (respuesta) {
                    $("#tbody_Menu").html("");
                    $.each(datos, function (index, value) {
                        var estado = "";
                        if (value.men_estado == 'A') {
                            estado = "ACTIVO";
                        }
                        else {
                            estado = "INACTIVO";
                        }
                        $("#tbody_Menu").append('<tr><td>' + value.men_descripcion + '</td><td>' + value.men_icono + '</td><td>' + estado + '</td><td><button type="button" data-id="' + value.men_id + '" class="btn btn-danger btn_delete">Eliminar</button><button type="button" data-id="' + value.men_id + '" class="btn btn-default btn_submenu">Agregar Submenu</button></td></tr>');
                    });
                    if (datos.length == 0) {
                        messageResponse({
                            text: "No se Encontraron Registros",
                            type: "warning"
                        });
                    }
                    CloseMessages();
                }
            }
        });
    };
    var _componentes = function () {
        $(document).on("click", ".btn_guardar", function (e) {
            console.log("btn guardar");
            $("#frmMenu-form").submit();
            if (_objetoForm_frmMenu.valid()) {
                var dataForm = $('#frmMenu-form').serializeFormJSON();
                responseSimple({
                    url: "Super/MenuInsertarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {

                        var respuesta = response.respuesta;
                        if (respuesta) {
                            limpiar_form({ contenedor: "#frmMenu-form" });
                            _objetoForm_frmMenu.resetForm();
                            MenuVista.init_ListarMenus();
                        }
                    }
                });
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        });

        $(document).on("click", ".btn_cancelar", function (e) {
            _objetoForm_frmMenu.resetForm();
        });

        $(document).on("click", ".btn_submenu", function () {
            var IdSala = $(this).data("id");
            var url = basePath + "Super/CrearSubMenusView?men_id=" + IdSala;
            window.location.href = url;
        })

        $(document).on("click", ".btn_delete", function (e) {
            var men_id = $(this).data("id");
            if (men_id != "" || men_id > 0) {
                messageConfirmation({
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "Super/MenuEliminarJson",
                            data: JSON.stringify({ men_id: men_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                MenuVista.init_ListarMenus();
                            }
                        });
                    }
                });
            }
            else {
                messageResponse({
                    text: "Error no se encontro ID",
                    type: "error"
                })
            }
        });
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmMenu',
            contenedor: '#frmMenu-form',
            rules: {
                men_descripcion:
                {
                    required: true,

                },
                men_icono:
                {
                    required: true,

                }
               

            },
            messages: {
                men_descripcion:
                {
                    required: 'Campo Obligatorio',
                },
                men_icono:
                {
                    required: 'Campo Obligatorio',
                }
           
            }
        });

    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarMenus();
            _componentes();
            _metodos();
            
        },
        init_ListarMenus: function () {
            _ListarMenus();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
   MenuVista.init();
});