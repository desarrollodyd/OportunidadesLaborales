var PanelRoles = function () {
    var _ListarRoles = function () {

        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        responseSimple({
            url: "Rol/GetListadoRol",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable-roleslistado",
                    table: "#rolesListado",
                    tableColumnsData: response.data,
                    tableHeaderCheck: true,
                    tableHeaderCheckIndex: 0,
                    headerCheck: "chk_actividades",
                    tableColumns: [
                        {
                            data: "WEB_RolID",
                            title: "ID",
                            width: "50px",
                            className: 'align-center',
                        },
                        {
                            data: "WEB_RolNombre",
                            title: "Nombre",
                        },
                        {
                            data: "WEB_RolDescripcion",
                            title: "Descripcion",
                        },
                        {
                            data: "WEB_RolEstado",
                            title: "Estado",
                            "render": function (value) {
                                let estado = value;
                                let mensaje_estado = "";
                                if (estado === '1') {
                                    estado = "success";
                                    mensaje_estado = "Activo";
                                } else {
                                    estado = "danger";
                                    mensaje_estado = "Inactivo";
                                }
                                var span = '<span class="label label-sm label-' + estado + ' arrowed arrowed-righ">' + mensaje_estado + '</span>';
                                return span;
                            }
                        },
                        {
                            data: "WEB_RolID",
                            title: "Acciones",
                            "render": function (value) {
                                let span = '';
                                let WEB_RolID = value;
                                span = `<a class="btn btn-sm btn-primary btn-editar" data-id=${WEB_RolID}>Editar</a> <a class="btn btn-sm btn-danger btn-eliminar" data-id=${WEB_RolID}>Eliminar</a>`;
                                return span;
                            }
                        }

                    ]
                })
            }
        });
    };
    var _componentes = function () {

        $(document).on("click", "#btn_nuevo", function (e) {
            $("#WEB_RolID").val(0);
            $("#tituloModalRoles").text("Nuevo");
            _objetoForm_form_roles.resetForm();
            $("#WEB_RolNombre").val("");
            $("#WEB_RolDescripcion").val("");
            $("#WEB_RolEstado").val(1);
            $("#modalFormulario").modal("show");
        });

        // $(document).off('click', ".btn-guardar")
        $(document).on('click', ".btn-guardar", function (e) {
            $("#form_roles").submit();
            if (_objetoForm_form_roles.valid()) {
                var dataForm = $('#form_roles').serializeFormJSON();
                var url = "";
                if ($("#WEB_RolID").val() == 0) {
                    url = "Rol/GuardarRol";
                }
                else {
                    url = "Rol/ActualizarRol";
                }
                responseSimple({
                    url: url,
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            PanelRoles.init_ListarRoles();
                            $("#modalFormulario").modal("hide");
                        }
                    }
                });
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        })
        $(document).on("click", ".btn-editar", function () {
            let WEB_RolID = $(this).data("id");
            _objetoForm_form_roles.resetForm();
            let dataForm = { WEB_RolID: WEB_RolID };
            responseSimple({
                url: "Rol/GetRolId",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    console.log(response);
                    if (response.respuesta) {
                        let data=response.data
                        $("#tituloModalRoles").text("Editar");

                        $("#WEB_RolID").val(data.WEB_RolID);
                        $("#WEB_RolNombre").val(data.WEB_RolNombre);
                        $("#WEB_RolDescripcion").val(data.WEB_RolDescripcion);
                        $("#WEB_RolEstado").val(data.WEB_RolEstado);
                        $("#modalFormulario").modal("show");
                    }
                }
            })
        })

        $(document).on("click", ".btn-eliminar", function (e) {
            var WEB_RolID = $(this).data("id");
            if (WEB_RolID != "" || WEB_RolID > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR este Rol?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "Rol/DeleteRolId",
                            data: JSON.stringify({ rolId: WEB_RolID }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                PanelRoles.init_ListarRoles();
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
            nameVariable: 'form_roles',
            contenedor: '#form_roles',
            rules: {
                WEB_RolNombre:
                {
                    required: true,

                },
                WEB_RolDescripcion:
                {
                    required: true,

                },
                WEB_RolEstado: {
                    required: true,
                }

            },
            messages: {
                WEB_RolNombre:
                {
                    required: 'Campo Obligatorio',
                },
                WEB_RolDescripcion:
                {
                    required: 'Campo Obligatorio',
                },
                WEB_RolEstado:
                {
                    required: 'Campo Obligatorio',
                }

            }
        });

    };
  
    return {
        init: function () {
            _ListarRoles();
            _componentes();
            _metodos();

        },
        init_ListarRoles: function () {
            _ListarRoles();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelRoles.init();
});