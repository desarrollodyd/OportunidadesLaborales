var PanelFichas = function () {
    var empresas='';
    var listadoEmpleados='';

    var _inicio = function () {
        responseSimple({
            url: "SQL/TMEMPRListarJson",
            refresh: false,
            callBackBeforeSend: function (response) {
                $("#cbo_empresa").html("");
                $("#cbo_empresa").append('<option value="">Cargando...</option>');
                $("#cbo_empresa").attr("disabled", "disabled");
            },
            callBackSuccess: function (response) {
                console.log(response.data);
                var respuesta = response.respuesta;
                if (respuesta) {
                    var data = response.data;
                    $("#cbo_empresa").html("");
                    $.each(data, function (index, value) {
                        $("#cbo_empresa").append('<option value="' + value.CO_EMPR + '">' + value.DE_NOMB + '</option>');
                    });
                    $("#cbo_empresa").removeAttr("disabled");
                    $("#cbo_empresa").val("");
                    $('#cbo_empresa').select2({
                        width: "100%",
                        multiple: true,
                        placeholder: "Seleccione Empresa"
                    });
                }
            }
        });

        $('#cbo_sede').select2({
            width: "100%",
            multiple: true,
            placeholder: "Seleccione Sede"
        });
    }
    var _componentes = function () {
        // $("#cbo_empresa").change(function(){
        // })
        $(document).on('change','#cbo_empresa',function(){
            empresas=$(this).val();
            var dataForm={
                listaEmpresas:empresas
            }
            console.log(empresas)

            if (empresas == null) {
                $('#cbo_sede').html("");
                $('#cbo_sede').select2({ placeholder: 'Seleccione Sede' }).val('').trigger('change');
                console.log("primero")
            }
            else {
                if (empresas.length > 0) {
                    responseSimple({
                        url: "SQL/TTSEDEListarporEmpresaJson",
                        data: JSON.stringify(dataForm),
                        refresh: false,
                        callBackBeforeSend: function (response) {
                            $("#cbo_sede").html("");
                            $("#cbo_sede").append('<option value="">Cargando...</option>');
                            $("#cbo_sede").attr("disabled", "disabled");
                        },
                        callBackSuccess: function (response) {
                            console.log(response.data);
                            var respuesta = response.respuesta;
                            if (respuesta) {
                                var data = response.data;
                                $("#cbo_sede").html("");
                                $.each(data, function (index, value) {
                                    var children = value.children;
                                    $("#cbo_sede").append('<optgroup value="' + value.id + '" label="' + value.text + '">');
                                    $.each(children, function (indexCh, valueCh) {
                                        $("#cbo_sede").append('<option value="' + valueCh.id + '">' + valueCh.text + '</option>');
                                    });
                                    $("#cbo_sede").append('</optgroup>');

                                });
                                $("#cbo_sede").removeAttr("disabled");

                                $('#cbo_sede').select2({
                                    width: "100%",
                                    multiple: true,
                                    placeholder: "Seleccione Sede"
                                });
                            }
                        }
                    });
                }
                else {
                    $('#cbo_sede').html("");
                    $('#cbo_sede').select2({ placeholder: 'Seleccione Sede' }).val('').trigger('change');
                }
            }
            
        });

        $(document).on('change', "#cbo_sede", function () {
            var sedes = $(this).val();
            var dataForm = {
                listaEmpresas: empresas,
                listaSedes: sedes
            }

            if (sedes == null || empresas == null) {
                var table = $('#fichasenvioListado').DataTable();
                table
                    .clear()
                    .draw();
                return false;
            }

            responseSimple({
                url: 'SQL/PersonaListarFichasJson',
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if (response.respuesta) {
                        listadoEmpleados = response.data;
                        simpleDataTable({
                            uniform: false,
                            tableNameVariable: "datatable_fichasListado",
                            table: "#fichasenvioListado",
                            tableColumnsData: response.data,
                            tableHeaderCheck: true,
                            tableHeaderCheckIndex: 0,
                            headerCheck: "chk_fichas",
                            tableColumns: [
                                {
                                    data: "id",
                                    title: "",
                                    "bSortable": false,
                                    className: 'align-center',
                                    "render": function (value) {
                                        var check = '<input type="checkbox" class="form-check-input-styled-info fichasListado" data-id="' + value + '" name="chk[]">';
                                        return check;
                                    },
                                    width: "50px",
                                },
                                {
                                    data: "nombre",
                                    title: "Nombre Empleado",
                                },
                                {
                                    data: "empresa",
                                    title: "Empresa",
                                },
                                {
                                    data: "sede",
                                    title: "Sede",
                                },
                                {
                                    data: "correoCorporativo",
                                    title: "C.Corporativo",
                                },
                                {
                                    data: "correoPersonal",
                                    title: "C.Personal",
                                },
                                {
                                    data: "id",
                                    tittle: 'Estado Correo',
                                    "render": function (value, type, oData) {
                                        var span = '';
                                        var correoCorporativo = oData.correoCorporativo;
                                        var correoPersonal = oData.correoPersonal;
                                        var clase = '';
                                        var estado = '';
                                        if (correoCorporativo == '' && correoPersonal == '') {
                                            clase = 'danger';
                                            estado = 'No Verificado';
                                        }
                                        else {
                                            clase = 'success';
                                            estado = 'verificado';
                                        }

                                        return '<span class="label label-' + clase + '">' + estado + '</span>';
                                    }
                                }
                            ]
                        });
                    }
                }
            })
            //console.log(listadoEmpleados);
        });

        $(document).on("click", ".chk_fichas", function (e) {
            $('#fichasenvioListado').find('tbody :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "#fichasenvioListado  tbody :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row
            $('.chk_fichas').prop('checked', ($(this).closest('table').find('tbody :checkbox:checked').length == $(this).closest('table').find('tbody :checkbox').length)); //Tira / coloca a seleção no .checkAll
        });

        $(document).on('click', '#btn_enviarFichas', function () {
            let arrayUsuarios = [];
            $('#fichasenvioListado tbody tr input[type=checkbox]:checked').each(function () {
                arrayUsuarios.push($(this).data("id"));
            });

            if (arrayUsuarios.length > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de Enviar Fichas Sintomatológicas a los empleados Seleccionados?',
                    callBackSAceptarComplete: function () {
                        var dataForm = { listaEmpleados: arrayUsuarios };
                        responseSimple({
                            url: "IntranetPjAdmin/EnviarJson",
                            data: JSON.stringify(dataForm),
                            refresh: false,
                            callBackSuccess: function (response) {
                                
                            }
                        })
                    }
                });
            }
            else {
                messageResponse({
                    text: "Debe Seleccionar al menos un Empleado",
                    type: "error"
                });
            }

        });

        $(document).on('click', '.btn_buscarFichas', function () {
            var desde = $("#txt_desde").val();
            var hasta = $("#txt_hasta").val();
            var estado = $("#cboestado").val();

            if (desde == "") {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
                return false;
            }

            if (hasta == "") {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
                return false;
            }

            if (estado == "") {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
                return false;
            }

            var dataForm = { desde,hasta,estado };
            responseSimple({
                url: "IntranetPjAdmin/IntranetFichasEmpleadoListarJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    simpleDataTable({
                        uniform: false,
                        tableNameVariable: "datatable_fichasestadoListado",
                        table: "#fichasestadoListado",
                        tableColumnsData: response.data,
                        tableHeaderCheck: false,
                        tableColumns: [
                            {
                                data: "fk_usuario",
                                title: "ID Usuario",
                            },
                            {
                                data: "correoCorporativo",
                                title: "C.Corporativo",
                            },
                            {
                                data: "correoPersonal",
                                title: "C.Personal",
                            },
                            {
                                data: "env_fecha_reg",
                                title: "Fecha",
                                "render": function (value) {
                                    var fecha = moment(value).format('YYYY-MM-DD');
                                    return fecha;
                                },
                                width: "120px",
                            },
                            {
                                data: "env_estado",
                                tittle: 'Estado',
                                "render": function (value, type, oData) {
                                    var clase = '';
                                    var estado = '';
                                    if (value == 1) {
                                        clase = 'danger';
                                        estado = 'Pendiente';
                                    }
                                    if (value == 2) {
                                        clase = 'success';
                                        estado = 'Completado';
                                    }
                                    if (value == 3) {
                                        clase = 'warning';
                                        estado = 'Reenviado';
                                    }
                                    return '<span class="label label-' + clase + '">' + estado + '</span>';
                                }
                            },
                            {
                                data: "env_id",
                                tittle: 'Accion',
                                "render": function (value, type, oData) {
                                    return '<button class="btn btn-primary btn_download" data-id="' + value.env_id+'"><i class="ace-icon fa fa-word align-top bigger-125" ></i >Descargar</button>';
                                }
                            }
                        ]
                    });
                }
            })
        });

        $(document).on('click', '.btn_buscarFichasp', function () {
            var desde = $("#txt_desdep").val();
            var hasta = $("#txt_hastap").val();
            var estado = $("#cboestadop").val();


            if (desde == "") {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
                return false;
            }

            if (hasta == "") {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
                return false;
            }

            if (estado == "") {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
                return false;
            }

            var dataForm = { desde, hasta, estado };
            responseSimple({
                url: "IntranetPjAdmin/IntranetFichasEmpleadoListarJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    simpleDataTable({
                        uniform: false,
                        tableNameVariable: "datatable_fichasestadoListado",
                        table: "#fichasestadoListado",
                        tableColumnsData: response.data,
                        tableHeaderCheck: false,
                        tableColumns: [
                            {
                                data: "fk_usuario",
                                title: "ID Usuario",
                            },
                            {
                                data: "correoCorporativo",
                                title: "C.Corporativo",
                            },
                            {
                                data: "correoPersonal",
                                title: "C.Personal",
                            },
                            {
                                data: "env_fecha_reg",
                                title: "Fecha",
                                "render": function (value) {
                                    var fecha = moment(value).format('YYYY-MM-DD');
                                    return fecha;
                                },
                                width: "120px",
                            },
                            {
                                data: "env_estado",
                                tittle: 'Estado',
                                "render": function (value, type, oData) {
                                    var clase = '';
                                    var estado = '';
                                    if (value == 1) {
                                        clase = 'danger';
                                        estado = 'Pendiente';
                                    }
                                    if (value == 2) {
                                        clase = 'success';
                                        estado = 'Completado';
                                    }
                                    if (value == 3) {
                                        clase = 'warning';
                                        estado = 'Reenviado';
                                    }
                                    return '<span class="label label-' + clase + '">' + estado + '</span>';
                                }
                            },
                            {
                                data: "env_id",
                                tittle: 'Accion',
                                "render": function (value, type, oData) {
                                    return '<button class="btn btn-primary btn_download" data-id="' + value.env_id + '"><i class="ace-icon fa fa-word align-top bigger-125" ></i >Descargar</button>';
                                }
                            }
                        ]
                    });
                }
            })
        });
    };

    var _metodos = function () {

        var dateinicio = new Date(moment().format("MM-DD-YYYY"));
        $('#txt_desde').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })

        $('#txt_hasta').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })

        $('#txt_desdep').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })

        $('#txt_hastap').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })
    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();
            _componentes();
            _metodos();
        },
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelFichas.init();
});