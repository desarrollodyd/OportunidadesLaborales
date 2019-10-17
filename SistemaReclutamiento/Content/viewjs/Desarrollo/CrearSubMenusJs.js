var SubMenuVista = function () {
    var men_id = menu.men_id;
    $("#fk_menu").val(men_id);
    var _ListarSubMenus = function () {
        var data = { men_id: men_id };
        responseSimple({
            url: "Super/SubMenuListarporMenuJson",
            refresh: false,
            data: JSON.stringify(data),
            callBackSuccess: function (response) {
                console.log(response);
                var respuesta = response.respuesta;
                var datos = response.data;
                if (respuesta) {
                    $("#tbody_SubMenu").html("");
                    $.each(datos, function (index, value) {
                        var estado = "";
                        if (value.snu_estado == 'A') {
                            estado = "ACTIVO";
                        }
                        else {
                            estado = "INACTIVO";
                        }
                        $("#tbody_SubMenu").append('<tr><td>' + value.snu_descripcion + '</td><td>' + value.snu_icono + '</td><td>' + value.snu_url + '</td><td>' + estado + '</td><td><button type="button" data-id="' + value.snu_id + '" class="btn btn-danger btn_delete">Editar Estado</button></td></tr>');
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
            $("#frmSubMenu-form").submit();
            if (_objetoForm_frmSubMenu.valid()) {
                var dataForm = $('#frmSubMenu-form').serializeFormJSON();
                responseSimple({
                    url: "Super/SubMenuInsertarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {

                        var respuesta = response.respuesta;
                        if (respuesta) {
                            limpiar_form({ contenedor: "#frmSubMenu-form" });
                            _objetoForm_frmSubMenu.resetForm();
                            SubMenuVista.init_ListarSubMenus();
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
            _objetoForm_frmSubMenu.resetForm();
        });

        $(document).on("click", ".btn_atras", function () {
           
            var url = basePath + "Super/CrearMenusView";
            window.location.href = url;
        })

        //$(document).on("click", ".btn_delete", function (e) {
        //    var men_id = $(this).data("id");
        //    if (men_id != "" || id > 0) {
        //        messageConfirmation({
        //            callBackSAceptarComplete: function () {
        //                responseSimple({
        //                    url: "Super/MenuEliminarJson",
        //                    data: JSON.stringify({ men_id: men_id }),
        //                    refresh: false,
        //                    callBackSuccess: function (response) {
        //                        EducacionBasicaVista.init_ListarEducacionBasica();
        //                    }
        //                });
        //            }
        //        });
        //    }
        //    else {
        //        messageResponse({
        //            text: "Error no se encontro ID",
        //            type: "error"
        //        })
        //    }
        //});
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmSubMenu',
            contenedor: '#frmSubMenu-form',
            rules: {
                snu_descripcion:
                {
                    required: true,

                },
                snu_icono:
                {
                    required: true,
                },
                url:
                {
                    required: true,
                }


            },
            messages: {
                snu_descripcion:
                {
                    required: 'Campo Obligatorio',
                },
                snu_icono:
                {
                    required: 'Campo Obligatorio',
                },
                snu_url:
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
            _ListarSubMenus();
            _componentes();
            _metodos();

        },
        init_ListarSubMenus: function () {
            _ListarSubMenus();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    SubMenuVista.init();
});