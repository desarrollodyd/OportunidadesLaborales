var PanelFichas = function () {
    var empresas='';
    var listadoEmpleados='';

    var _inicio = function () {
        responseSimple({
            url: "SQL/TMEMPRListarJson",
            refresh: false,
            notify:false,
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
                                    "render": function (value, type, oData) {
                                        var correoCorporativo = oData.correoCorporativo;
                                        var correoPersonal = oData.correoPersonal;
                                        var check = '<input type="checkbox" class="form-check-input-styled-info fichasListado" data-id="' + value + '|' + correoCorporativo + ' | ' + correoPersonal +'" name="chk[]">';
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

        $(document).on('click', '.btn_enviarFichas', function () {
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
                        tableHeaderCheck: true,
                        tableHeaderCheckIndex: 0,
                        headerCheck: "chk_enviosIDE",
                        tableColumns: [
                            {
                                data: "env_id",         
                                title: "",
                                "bSortable": false,
                                className: 'align-center',
                                "render": function (value) {
                                    var check = '<input type="checkbox" class="form-check-input-styled-info fichasestadoListado" data-id="' + value + '" name="chk[]">';
                                    return check;
                                },
                                width: "50px",
                            },
                            {
                                data: "per_nombre",
                                title: "Nombre Empleado",
                                "render": function (value, type, oData) {
                                    var nombre = oData.per_apellido_pat +' ' + oData.per_apellido_mat + ', ' + oData.per_nombre;
                                    return nombre;
                                },
                            },
                            {
                                data: "cus_correo",
                                title: "Correo",
                            },
                            {
                                data: "env_fecha_reg",
                                title: "Fecha",
                                "render": function (value) {
                                    var fecha = moment(value).format('DD-MM-YYYY');
                                    return fecha;
                                },
                                width: "120px",
                            },
                            {
                                data: "env_estado",
                                title: 'Estado',
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
                                title: 'Accion',
                                "render": function (value, type, oData) {
                                    return '<button class="btn btn-white btn-primary btn-sm btn_reenviar" data-id="' + value + '"><i class="ace-icon fa fa-envelope-o" ></i> Reenviar</button>' +
                                        ' <button class="btn btn-white btn-warning btn-sm btn_download" data-id="' + value + '"><i class="ace-icon fa fa-file-word-o" ></i> Descargar</button>';
                                }
                            }
                        ]
                    });
                }
            })
        });

        $(document).on("click", ".chk_enviosIDE", function (e) {
            $('#fichasestadoListado').find('tbody :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "#fichasestadoListado  tbody :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row
            $('.chk_enviosIDE').prop('checked', ($(this).closest('table').find('tbody :checkbox:checked').length == $(this).closest('table').find('tbody :checkbox').length)); //Tira / coloca a seleção no .checkAll
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
                url: "IntranetPjAdmin/IntranetFichasPostulanteListarJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    simpleDataTable({
                        uniform: false,
                        tableNameVariable: "datatable-fichaspostulantelistadop",
                        table: "#fichaspostulanteListadop",
                        tableColumnsData: response.data,
                        tableHeaderCheck: true,
                        tableHeaderCheckIndex: 0,
                        headerCheck: "chk_enviosIDP",
                        tableColumns: [
                            {
                                data: "env_id",
                                title: "",
                                "bSortable": false,
                                className: 'align-center',
                                "render": function (value) {
                                    var check = '<input type="checkbox" class="form-check-input-styled-info fichaspostulanteListadop" data-id="' + value + '" name="chk[]">';
                                    return check;
                                },
                                width: "50px",
                            },
                            {
                                data: "nombre",
                                title: "Nombre Empleado",
                                "render": function (value, type, oData) {
                                    var nombre = oData.per_apellido_pat + ' '+ oData.per_apellido_mat + ', ' + oData.per_nombre;

                                    return nombre;
                                }
                            },
                            {
                                data: "end_correo_pers",
                                title: "Correo",
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
                                title: 'Estado',
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
                                title: 'Accion',
                                "render": function (value, type, oData) {
                                    return '<button class="btn btn-white btn-primary btn-sm btn_reenviar" data-id="' + value + '"><i class="ace-icon fa fa-envelope-o" ></i> Reenviar</button>' +
                                        ' <button class="btn btn-white btn-warning btn-sm btn_download" data-id="' + value + '"><i class="ace-icon fa fa-file-word-o" ></i> Descargar</button>';
                                }
                            }
                        ]
                    });
                }
            })
        });

        $(document).on("click", ".chk_enviosIDP", function (e) {
            $('#fichaspostulanteListadop').find('tbody :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "#fichaspostulanteListadop  tbody :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row
            $('.chk_enviosIDP').prop('checked', ($(this).closest('table').find('tbody :checkbox:checked').length == $(this).closest('table').find('tbody :checkbox').length)); //Tira / coloca a seleção no .checkAll
        });

        $(document).on('click', '.btn_reenviar', function () {
            var envioid = $(this).data("id");
            messageConfirmation({
                content: '¿Esta seguro de Reenviar Ficha Sintomatológica?',
                callBackSAceptarComplete: function () {
                    var dataForm = { envioID: envioid };
                    responseSimple({
                        url: "IntranetPjAdmin/ReEnviarJson",
                        data: JSON.stringify(dataForm),
                        refresh: false,
                        callBackSuccess: function (response) {
                            if ($("#profile3").is(":visible")) {
                                $(".btn_buscarFichas").click();
                            }
                            else {
                                $(".btn_buscarFichasp").click();
                            }
                            
                        }
                    })
                }
            });

        });

        $(document).on('click', '#tabpostulanted', function () {
            responseSimple({
                url: "IntranetPjAdmin/listaPostulantes",
                data: JSON.stringify({}),
                refresh: false,
                callBackSuccess: function (response) {
                    simpleDataTable({
                        uniform: false,
                        tableNameVariable: "datatable_fichapostulanteListado",
                        table: "#fichapostulanteListado",
                        tableColumnsData: response.data,
                        tableHeaderCheck: true,
                        tableHeaderCheckIndex: 0,
                        headerCheck: "chk_fichasp",
                        tableColumns: [
                            {
                                data: "usu_id",
                                title: "",
                                "bSortable": false,
                                className: 'align-center',
                                "render": function (value, type, oData) {
                                    var correoPersonal = oData.per_correoelectronico;
                                    var dni = oData.per_num_doc;
                                    var check = '<input type="checkbox" class="form-check-input-styled-info fichapostulanteListado" data-id="' + value + ' | ' + dni + ' | ' + correoPersonal + '" name="chk[]">';
                                    return check;
                                },
                                width: "50px",
                            },
                            {
                                data: "nombre",
                                title: "Nombre Empleado",
                                "render": function (value, type, oData) {
                                    var nombre = oData.per_apellido_pat + ', ' + oData.per_nombre;
                                   
                                    return nombre;
                                }
                            },
                            {
                                data: "per_correoelectronico",
                                title: "C.Personal",
                            },
                            {
                                data: "usu_id",
                                tittle: 'Estado Correo',
                                "render": function (value, type, oData) {
                                    var correoPersonal = oData.per_correoelectronico;
                                    var clase = '';
                                    var estado = '';
                                    if (correoPersonal == '') {
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
            })

        });

        $(document).on("click", ".chk_fichasp", function (e) {
            $('#fichapostulanteListado').find('tbody :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "#fichapostulanteListado  tbody :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row
            $('.chk_fichasp').prop('checked', ($(this).closest('table').find('tbody :checkbox:checked').length == $(this).closest('table').find('tbody :checkbox').length)); //Tira / coloca a seleção no .checkAll
        });

        $(document).on('click', '.btn_enviarFichasP', function () {
            let arrayUsuarios = [];
            $('#fichapostulanteListado tbody tr input[type=checkbox]:checked').each(function () {
                arrayUsuarios.push($(this).data("id"));
            });

            if (arrayUsuarios.length > 0) {
                messageConfirmation({
                    content: '¿Esta seguro de Enviar Fichas Sintomatológicas a los empleados Seleccionados?',
                    callBackSAceptarComplete: function () {
                        var dataForm = { listaPostulantes: arrayUsuarios };
                        responseSimple({
                            url: "IntranetPjAdmin/EnviarPJson",
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
                    text: "Debe Seleccionar al menos un Postulante",
                    type: "error"
                });
            }

        });
        $(document).on('click','.btn_descargarTodosPostulantes',function(e){
            e.preventDefault();
            var arrayIds = '';
            $('#fichaspostulanteListadop tbody tr input[type=checkbox]:checked').each(function () {
                // arrayIds.push($(this).data("id"));
                arrayIds+=$(this).data("id")+",";
            });
            arrayIds = arrayIds.substring(0, arrayIds.length - 1);
            // arrayIds=arrayIds.slice(0,-1);
            if(arrayIds.length>0){
                let a= document.createElement('a');
                a.target= '_blank';
                a.href= basePath + "FichaSintomatologica/DownloadPdfReporteMultile?env_ids=" + arrayIds;
                a.click();
                // window.location.href = basePath + "FichaSintomatologica/DownloadPdfReporteMultile?env_ids=" + arrayIds;
            }
            else{
                messageResponse({
                    text: "Debe seleccionar al menos un registro",
                    type: "error"
                })
                return false;
            }
        })
        $(document).on('click','.btn_descargarTodosEmpleados',function(e){
            e.preventDefault();
            var arrayIds = '';
            $('#fichasestadoListado tbody tr input[type=checkbox]:checked').each(function () {
                // arrayIds.push($(this).data("id"));
                arrayIds+=$(this).data("id")+",";
            });
            arrayIds = arrayIds.substring(0, arrayIds.length - 1);
            // arrayIds=arrayIds.slice(0,-1);
            if(arrayIds.length>0){
                let a= document.createElement('a');
                a.target= '_blank';
                a.href= basePath + "FichaSintomatologica/DownloadPdfReporteMultile?env_ids=" + arrayIds;
                a.click();
                // window.location.href = basePath + "FichaSintomatologica/DownloadPdfReporteMultile?env_ids=" + arrayIds;
            }
            else{
                messageResponse({
                    text: "Debe seleccionar al menos un registro",
                    type: "error"
                })
                return false;
            }
        })
        $(document).on('click','.btn_download',function(e){
            e.preventDefault();
            console.log('click');
            var env_id = $(this).data("id");
            let a= document.createElement('a');
            a.target= '_blank';
            a.href= basePath + "FichaSintomatologica/DownloadFdfReporte?env_id=" + env_id;;
            a.click();
            // window.location.href = basePath + "FichaSintomatologica/DownloadFdfReporte?env_id=" + env_id;
        })
        
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