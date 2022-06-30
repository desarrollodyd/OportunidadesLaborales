let PanelRemitentes = function () {
    let _listarEmails = function () {

        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        responseSimple({
            url: "IntranetPJBoletasGDT/BolEmailRemitenteListadoJson",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable_emailremitentelistado",
                    table: "#emailRemitenteListado",
                    tableColumnsData: response.data,
                    tableColumns: [
                        {
                            data: "email_nombre",
                            title: "Nombre",

                        },
                        {
                            data: "email_direccion",
                            title: "Direccion",
                        },
                        {
                            data: "email_cantidad_envios",
                            title: "Cantidad Envios",
                        },
                        {
                            data: "email_limite",
                            title: "Limite Envios",
                        },
                        {
                            data: "envios_restantes",
                            title: "Envios Restantes",
                        },
                        {
                            data: "email_ultimo_envio",
                            title: "Ultimo Envio",
                            render: function(value){
                                let fecha=moment(value).format("DD/MM/YYYY")
                                if(fecha!='31/12/1752'){
                                    return moment(value).format("DD/MM/YYYY HH:mm:ss");
                                }
                                return ''
                            }
                        },
                        {
                            data: "email_estado",
                            title: "Estado",
                            "render": function (value) {
                                var estado = value;
                                var mensaje_estado = "";
                                if (estado === 1) {
                                    estado = "success";
                                    mensaje_estado = "Activo";
                                } else {
                                    estado = "danger";
                                    mensaje_estado = "InActivo";
                                }
                                var span = '<span class="label label-sm label-' + estado + ' arrowed arrowed-righ">' + mensaje_estado + '</span>';
                                return span;
                            }
                        },
                        {
                            data: "email_id",
                            title: "Acciones",
                            "render": function (value) {
                                var span = '';
                                var email_id= value;
                                var span = `
                                <div class="hidden-sm hidden-xs action-buttons">
                                    <a class="green btn-editar" href="#" data-id="${email_id}"><i class="ace-icon fa fa-pencil bigger-130"></i></a>
                                    <a class="red btn-eliminar" href="#" data-id="${email_id}"><i class="ace-icon fa fa-trash-o bigger-130"></i></a>
                                </div>
                                <div class="hidden-md hidden-lg">
                                    <div class="inline pos-rel">
                                    <button class="btn btn-minier btn-yellow dropdown-toggle" data-toggle="dropdown" data-position="auto">
                                        <i class="ace-icon fa fa-caret-down icon-only bigger-120"></i>
                                    </button>
                                    <ul class="dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close">
                                        <li>
                                        <a href="#" class="tooltip-success btn-editar" data-id="${email_id}" data-rel="tooltip" title="Edit" >
                                            <span class="green"><i class="ace-icon fa fa-pencil-square-o bigger-120"></i></span></a>
                                        </li>
                                        <li>
                                        <a href="#" class="tooltip-error btn-eliminar" data-id="${email_id}" data-rel="tooltip" title="Delete" ><span class="red">
                                            <i class="ace-icon fa fa-trash-o bigger-120"></i></span></a>
                                        </li>
                                    </ul>
                                    </div>
                                </div>
                                `;
                                return span;
                            }
                        }

                    ]
                })
            }
        });
    };
    let _componentes = function () {

        $(document).on("click", "#btn_nuevo", function (e) {
            $("#email_id").val(0);
            $("#tituloModal").text("Nuevo");
            _limpiarCampos()
            $("#modalFormulario").modal("show");
        });

        $(document).off('click', ".btn-guardar")
        $(document).on('click', ".btn-guardar", function (e) {
            $("#form_emailremitente").submit();
            if (_objetoForm_form_emailremitente.valid()) {
                var dataForm = $('#form_emailremitente').serializeFormJSON();
                var url = "";
                if ($("#email_id").val() == 0) {
                    url = "IntranetPJBoletasGDT/BolEmailRemitenteInsertarJson";
                }
                else {
                    url = "IntranetPJBoletasGDT/BolEmailRemitenteEditarJson";
                }
                responseSimple({
                    url: url,
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        console.log(response);
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            PanelRemitentes.init__listarEmails();
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
            var email_id = $(this).data("id");
            _limpiarCampos()
            var dataForm = { email_id: email_id };
            responseSimple({
                url: "IntranetPJBoletasGDT/BolEmailRemitenteIdObtenerJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    console.log(response);
                    if (response.respuesta) {
                        let emailRemitente=response.data
                        $("#tituloModal").text("Editar");
                        $("#email_id").val(emailRemitente.email_id);
                        $("#email_nombre").val(emailRemitente.email_nombre);
                        $("#email_direccion").val(emailRemitente.email_direccion);
                        $("#email_password").val(emailRemitente.email_password);
                        $("#email_ssl").val(emailRemitente.email_ssl.toString());
                        $("#email_smtp").val(emailRemitente.email_smtp);
                        $("#email_puerto").val(emailRemitente.email_puerto);
                        $("#email_estado").val(emailRemitente.email_estado);
                        $("#email_limite").val(emailRemitente.email_limite);
                        $("#modalFormulario").modal("show");
                    }
                }
            })
        })

        $(document).on("click", ".btn-eliminar", function (e) {
            var email_id = $(this).data("id");
            if (email_id != "" || email_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de ELIMINAR este remitente?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "IntranetPJBoletasGDT/BolEmailRemitenteEliminarJson",
                            data: JSON.stringify({ email_id: email_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                PanelRemitentes.init__listarEmails()
                                //refresh(true);
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

    let _metodos = function () {
        validar_Form({
            nameVariable: 'form_emailremitente',
            contenedor: '#form_emailremitente',
            rules: {
                email_nombre:
                {
                    required: true,
                },
                email_direccion:
                {
                    required: true,
                },
                email_password: {
                    required: true,
                },
                email_ssl: {
                    required: true,
                },
                email_smtp: {
                    required: true,
                },
                email_puerto: {
                    required: true,
                },
                email_estado: {
                    required: true,
                },
                email_limite: {
                    required: true,
                }

            },
            messages: {
                email_nombre:
                {
                    required: 'Campo Obligatorio',
                },
                email_direccion:
                {
                    required: 'Campo Obligatorio',
                },
                email_password:
                {
                    required: 'Campo Obligatorio',
                },
                email_ssl:
                {
                    required: 'Campo Obligatorio',
                },
                email_smtp:
                {
                    required: 'Campo Obligatorio',
                },
                email_puerto:
                {
                    required: 'Campo Obligatorio',
                },
                email_estado:
                {
                    required: 'Campo Obligatorio',
                },
                email_limite:
                {
                    required: 'Campo Obligatorio',
                }
            }
        });

    };
    let _limpiarCampos=function(){
        $("#email_id").val(0);
        $("#email_nombre").val("");
        $("#email_direccion").val("");
        $("#email_password").val("");
        $("#email_ssl").val("true");
        $("#email_smtp").val("smtp.gmail.com");
        $("#email_puerto").val(587);
        $("#email_estado").val(1);
        $("#email_limite").val(500);
    }
    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _listarEmails();
            _componentes();
            _metodos();

        },
        init__listarEmails: function () {
            _listarEmails();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelRemitentes.init();
});