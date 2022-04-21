let panelBoletas = function () {
    let progress = $.connection.progressHub;
    let connectionId;
    let _inicio=function(){
        // let dateinicio = new Date(moment().format("YYYY"))
        let hoy = new Date();
        let fecha_hoy = moment(hoy).format('YYYY-MM-DD');
        $("#cboQuincena").select2({
            placeholder: "--Seleccione--", allowClear: true
        })
        $('#anioCreacion').datetimepicker({
            format: 'YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: fecha_hoy,
            maxDate:fecha_hoy
        })
        $('#fechaProcesoPdf').datetimepicker({
            format: 'YYYY-MM',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: fecha_hoy,
            maxDate:fecha_hoy
        })
        $('#fechaListar').datetimepicker({
            format: 'YYYY-MM',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: fecha_hoy,
            maxDate:fecha_hoy
        })
        $('#fechaInicio').datetimepicker({
            format: 'YYYY-MM-DD',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: fecha_hoy,
            maxDate:new Date()
        })
        $('#fechaFin').datetimepicker({
            format: 'YYYY-MM-DD',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: fecha_hoy,
            maxDate:new Date()
        })
        $('#fechaProcesoPdfV2').datetimepicker({
            format: 'YYYY-MM',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: fecha_hoy,
            maxDate:fecha_hoy
        })
        //carga de salas
        responseSimple({
            url: "sql/TMEMPRListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                CloseMessages()
                if(response.respuesta){
                    let data=response.data
                    $("#cboEmpresa").append(`<option value="">--Seleccione--<option>`)
                    $("#cboEmpresaListar").append(`<option value="">--Seleccione--<option>`)
                    $("#cboEmpresaV2").append(`<option value="">--Seleccione--<option>`)
                    $.each(data, function (index, value) {
                        $("#cboEmpresa").append(`<option value="${value.CO_EMPR}">${value.DE_NOMB}</option>`);
                    });
                    $("#cboEmpresa").select2({
                        placeholder: "--Seleccione--", allowClear: true
                    })
                    $.each(data, function (index, value) {
                        $("#cboEmpresaV2").append(`<option value="${value.CO_EMPR}">${value.DE_NOMB}</option>`);
                    });
                    $("#cboEmpresaV2").select2({
                        placeholder: "--Seleccione--", allowClear: true
                    })
                    $.each(data, function (index, value) {
                        $("#cboEmpresaListar").append(`<option value="${value.CO_EMPR}">${value.DE_NOMB}</option>`);
                    });
                    $("#cboEmpresaListar").select2({
                        placeholder: "--Seleccione--", allowClear: true
                    })
                }
            }
        })
    }
    let ProgressBarModalBoletas=function (showHide) {
        if (showHide === 'show') {
            $('#mod-progress').modal('show')
            if (arguments.length >= 2) {
                $('#progressBarParagraph').text(arguments[1])
                $('#ProgressBarSpan').text(arguments[2])
    
            } else {
                $('#progressBarParagraph').text('U sdasdas')
            }
            window.ProgressBarActive = true
        } else {
            $('#mod-progress').modal('hide')
            window.ProgressBarActive = false
        }
    }
    let _componentes = function () {
        $(document).on('shown.bs.tab','.nav-tabs a',function(e){
            //   console.log(e.target)
            let tab=$(this).data('tab')
            if(tab=='tab1'){
                
            }
            else if(tab=='tab2'){
                let tipoConfiguracion=$(this).data('tipoconfiguracion')
                if(tipoConfiguracion){
                    let dataForm={tipo:tipoConfiguracion}
                    responseSimple({
                        url: "IntranetPJBoletasGDT/BolConfiguracionObtenerxTipoJson",
                        data:JSON.stringify(dataForm),
                        refresh: false,
                        callBackSuccess: function (response) {
                            CloseMessages()
                            let data=response.data
                            if(data.config_id!=0){
                                $("#config_id").val(data.config_id)
                                $("#config_valor").val(data.config_valor)
                                $("#config_tipo").val(data.config_tipo)
                                $("#config_estado").val(data.config_estado)
                                $("#config_descripcion").val(data.config_descripcion)
                            }
                        }
                    })
                }
            }
            else if(tab=='tab3'){
            }
        })
        $(document).on('click','.btnGuardarPathPrincipal',function(e){
            e.preventDefault()
            $("#formPathPrincipal").submit()
            // let dataForm = $('#login-form').serializeFormJSON();
            if (_objetoForm_formPathPrincipal.valid()) {
                let dataForm = $('#formPathPrincipal').serializeFormJSON()
                let config_id=$("#config_id").val()
                let url=''
                if(config_id==0){
                    url='IntranetPJBoletasGDT/BolConfiguracionInsertarJson'
                }
                else{
                    url='IntranetPJBoletasGDT/BolConfiguracionEditarJson'
                }
                responseSimple({
                    url: url,
                    data:JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        CloseMessages()
                        if(response.idInsertado!=0){
                            config_id=response.idInsertado
                        }
                        $("#config_id").val(config_id)
                    }
                })
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }

        })
        $(document).on('click','.btnCrearDirectorio',function(e){
            e.preventDefault()
            let anio=$("#anioCreacion").val()
            let dataForm={anioCreacion:anio}
            responseSimple({
                url: 'IntranetPJBoletasGDT/CrearDirectorioBoletasGDTJson',
                refresh: false,
                data: JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    // CloseMessages()
                    zNodes=response.data;
                    $("#mostrarArbolDirectorios").show();
                    $.fn.zTree.init($("#treeDemo2"), {}, zNodes)
                }
            })
        })
        $(document).on('click','.btnProcesarPdf',function(e){
            e.preventDefault()
            $("#formProcesarPdf").submit()
            if (_objetoForm_formProcesarPdf.valid()) {
                let dataForm = $('#formProcesarPdf').serializeFormJSON()
                let url='/IntranetPJBoletasGDT/BolProcesarPdf2'
                messageConfirmation({
                    content: '¿Esta seguro de realizar esta acción?',
                    callBackSAceptarComplete: function () {
                        progress.client.AddProgressBoletas = function (message,percentage, hide) {
                            ProgressBarModalBoletas('show', message,percentage)
                            $('#ProgressMessage').width(percentage)
                            if (hide == true) {
                                ProgressBarModalBoletas()
                            }
                        }
                        $.connection.hub.start().done(function () {
                            connectionId = $.connection.hub.id
                            dataForm['connectionId']=connectionId
                            $.ajax({
                                type: "POST",
                                url: basePath+url,
                                cache: false,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify(dataForm),
                                beforeSend: function (xhr) {
                                },
                                success: function (response) {
                                    if(response.respuesta){
                                    llenarDatatableProceso(response.data)
                                    }
                                },
                                error: function (request, status, error) {
                                },
                                complete: function (resul) {
                                }
                            });
                        })
                    }
                });
            }
            else{
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        })
        $(document).on('click','.btnListarData',function(e){
            e.preventDefault()
            $("#formListarPfds").submit()
            if (_objetoForm_formListarPfds.valid()) {
                let dataForm = $('#formListarPfds').serializeFormJSON()
                let url='IntranetPJBoletasGDT/BolListarPdfJson'
                responseSimple({
                    url: url,
                    data:JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                        CloseMessages()
                      if(response.respuesta){
                        $("#divEnvioPDFs").show()
                        llenarDatatablePdfs(response.data)
                      }
                    }
                })
            }
            else{
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        })
        $(document).on('change','#cboEmpresa',function(e){
            let nombreEmpresa=$(this).find(':selected').text()
            $("#nombreEmpresa").val(nombreEmpresa)
        })
        $(document).on('change','#cboEmpresaV2',function(e){
            let nombreEmpresa=$(this).find(':selected').text()
            $("#nombreEmpresaV2").val(nombreEmpresa)
        })
        $(document).on("click", ".chkProcesoPdf", function (e) {
            $('#dataTableProcesoPdf').find('tbody :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "#dataTableProcesoPdf  tbody :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row
            $('.chkProcesoPdf').prop('checked', ($(this).closest('table').find('tbody :checkbox:checked').length == $(this).closest('table').find('tbody :checkbox').length)); //Tira / coloca a seleção no .checkAll
        })

        $(document).on("click", ".chkListarPdf", function (e) {
            $('#dataTableListarPdf').find('tbody :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "#dataTableListarPdf  tbody :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row
            $('.chkListarPdf').prop('checked', ($(this).closest('table').find('tbody :checkbox:checked').length == $(this).closest('table').find('tbody :checkbox').length)); //Tira / coloca a seleção no .checkAll
        })
        $(document).on("click",'.btnEnviarPDFs',function(e){
            console.log(e)
            e.preventDefault()
            let arrayEmpleados = [];
            let nombreEmpresa=$("#cboEmpresaListar").find(':selected').text()
            $('#dataTableListarPdf tbody tr input[type=checkbox]:checked').each(function () {
                let obj={
                    emp_co_trab:$(this).data("empcotrab"),
                    emp_ruta_pdf:$(this).data("emprutapdf"),
                    emp_co_empr:$(this).data("empcoempr"),
                    emp_direc_mail:$(this).data("empdiremail"),
                    emp_periodo:$(this).data("empperiodo"),
                    emp_anio:$(this).data("empanio"),
                    emp_no_trab:$(this).data("empnotrab"),
                    emp_apel_pat:$(this).data("empapelpat"),
                    emp_apel_mat:$(this).data("empapelmat"),
                    nombreEmpresa:nombreEmpresa
                }
                arrayEmpleados.push(obj);
            });
            messageConfirmation({
                content: '¿Esta seguro de realizar esta acción?',
                callBackSAceptarComplete: function () {
                    responseSimple({
                        url: "IntranetPJBoletasGDT/EnviarBoletasEmailJson",
                        refresh: false,
                        data: JSON.stringify(arrayEmpleados),
                        callBackSuccess: function (response) {
                           console.log(response);
                        }
                    })
                }
            });
          
        })
        $(document).on('click','.btnVisualizarPDF',function(e){
            e.preventDefault()
            $("#contenidoBoletaPdf").html('')
            let nombreEmpresa=$("#cboEmpresaListar").find(':selected').text()
            let emp_co_trab=$(this).data("empcotrab")
            let obj={
                emp_co_trab:$(this).data("empcotrab"),
                emp_ruta_pdf:$(this).data("emprutapdf"),
                emp_co_empr:$(this).data("empcoempr"),
                emp_direc_mail:$(this).data("empdiremail"),
                nombreEmpresa:nombreEmpresa
            }
            responseSimple({
                url: "IntranetPJBoletasGDT/VisualizarPdfIntranetAdminJson",
                refresh: false,
                data: JSON.stringify(obj),
                callBackSuccess: function (response) {
                    CloseMessages()
                    if (response.respuesta) {
                        let data = response.data;
                        let fileName=response.fileName
                        $(".ui-pnotify").remove()
                        easyPDF(data,"PDF Empleado : "+ emp_co_trab,fileName)
                        //cargarDialog()
                    //    $("#modalBoleta").modal('show')
                    //     $("#contenidoBoletaPdf").append("<iframe width='100%' height='100%' src='data:application/pdf;base64, " +
                    //         encodeURI(data) + "'></iframe>")


                    //     if (!($('.modal.in').length)) {
                    //         $('.modal-dialog').css({
                    //             top: 0,
                    //             left: 0
                    //         });
                    //     }
                    //     $('#modalBoleta').modal({
                    //     backdrop: false,
                    //     show: true
                    //     });
                    
                    //     $('.modal-dialog').draggable({
                    //     handle: ".modal-header"
                    //     });    
                    }
                }
            })
        })
        $(document).on('click','.close',function(e){
            e.preventDefault()
            $("#modalBoleta").modal('hide')
        })
        $(document).on('click','.btnListarBitacora',function(e){
            e.preventDefault()
            let fechaInicio=$("#fechaInicio").val()
            let fechaFin=$("#fechaFin").val()
            if(fechaInicio==""){
                messageResponse({
                    text: "Fecha Inicio Incorrecta",
                    type: "error"
                })
                return;
            }
            if(fechaFin==""){
                messageResponse({
                    text: "Fecha Fin Incorrecta",
                    type: "error"
                })
                return;
            }
            let dataForm = {
                fechaInicio:fechaInicio+ ' 00:00:00',
                fechaFin:fechaFin+' 23:59:59'
            }
            let url='IntranetPJBoletasGDT/BitacoraListarFiltrosJson'
            responseSimple({
                url: url,
                data:JSON.stringify(dataForm),
                refresh: false,
                callBackSuccess: function (response) {
                    if(response.respuesta){
                        console.log(response.data)
                        llenarDatatableBitacora(response.data)
                    }
                }
            })
        })
        $('body').on('contextmenu', '#pdfview', function(e){ return false; });
        $(document).on('click','.btnProcesarPdfV2',function(e){
            e.preventDefault()
            $("#formProcesarPdfV2").submit()
            if (_objetoForm_formProcesarPdfV2.valid()) {
                let url='/IntranetPJBoletasGDT/BolProcesarPdf'
                let dataForm = new FormData(document.getElementById("formProcesarPdfV2"));
                messageConfirmation({
                    content: '¿Esta seguro de realizar esta acción?',
                    callBackSAceptarComplete: function () {
                        progress.client.AddProgressBoletas = function (message,percentage, hide) {
                            ProgressBarModalBoletas('show', message,percentage)
                            $('#ProgressMessage').width(percentage)
                            if (hide == true) {
                                ProgressBarModalBoletas()
                            }
                        }
                        $.connection.hub.start().done(function () {
                            connectionId = $.connection.hub.id
                            dataForm.append("connectionId",connectionId)
                            // dataForm['connectionId']=connectionId
                            responseFileSimple({
                                url: url,
                                data: dataForm,
                                refresh: false,
                                callBackSuccess: function (response) {
                                    console.log(response);
                                    if(response.respuesta){
                                        llenarDatatableProcesoV2(response.data)
                                    } 
                                },
                                loader:false
                            });
                        })
                    }
                });

               
            }
            else{
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        })
    }
    let _metodos=function(){
        validar_Form({
            nameVariable: 'formPathPrincipal',
            contenedor: '#formPathPrincipal',
            rules: {
                config_descripcion:
                {
                    required: true,

                },
                config_valor:
                {
                    required: true,

                }

            },
            messages: {
                config_descripcion:
                {
                    required: 'Campo Obligatorio',
                },
                config_valor:
                {
                    required: 'Campo Obligatorio',
                }

            }
        })
        validar_Form({
            nameVariable: 'formProcesarPdf',
            contenedor: '#formProcesarPdf',
            rules: {
                empresa:
                {
                    required: true,
                },
                fechaProcesoPdf:
                {
                    required: true,
                },

            },
            messages: {
                empresa:
                {
                    required: 'Campo Obligatorio',
                },
                fechaProcesoPdf:
                {
                    required: 'Campo Obligatorio',
                },
            }
        });
        validar_Form({
            nameVariable: 'formListarPfds',
            contenedor: '#formListarPfds',
            rules: {
                empresaListar:
                {
                    required: true,
                },
                fechaListar:
                {
                    required: true,
                }
            },
            messages: {
                empresaListar:
                {
                    required: 'Campo Obligatorio',
                },
                fechaListar:
                {
                    required: 'Campo Obligatorio',
                },
            }
        });
        validar_Form({
            nameVariable: 'formProcesarPdfV2',
            contenedor: '#formProcesarPdfV2',
            rules: {
                empresa:
                {
                    required: true,
                },
                fechaProcesoPdf:
                {
                    required: true,
                },

            },
            messages: {
                empresa:
                {
                    required: 'Campo Obligatorio',
                },
                fechaProcesoPdf:
                {
                    required: 'Campo Obligatorio',
                },
            }
        });
    }
    let setting = {	};
    let zNodes =[
        // { name:"pNode 01", open:true,
        //     children: [
        //         { name:"pNode 11",
        //             children: [
        //                 { name:"leaf node 111"},
        //                 { name:"leaf node 112"},
        //                 { name:"leaf node 113"},
        //                 { name:"leaf node 114"}
        //             ]},
        //         { name:"pNode 12",
        //             children: [
        //                 { name:"leaf node 121"},
        //                 { name:"leaf node 122"},
        //                 { name:"leaf node 123"},
        //                 { name:"leaf node 124"}
        //             ]},
        //         { name:"pNode 13 - no child", isParent:true}
        //     ]},
        // { name:"pNode 02",
        //     children: [
        //         { name:"pNode 21", open:true,
        //             children: [
        //                 { name:"leaf node 211"},
        //                 { name:"leaf node 212"},
        //                 { name:"leaf node 213"},
        //                 { name:"leaf node 214"}
        //             ]},
        //         { name:"pNode 22",
        //             children: [
        //                 { name:"leaf node 221"},
        //                 { name:"leaf node 222"},
        //                 { name:"leaf node 223"},
        //                 { name:"leaf node 224"}
        //             ]},
        //         { name:"pNode 23",
        //             children: [
        //                 { name:"leaf node 231"},
        //                 { name:"leaf node 232"},
        //                 { name:"leaf node 233"},
        //                 { name:"leaf node 234"}
        //             ]}
        //     ]},
        // { name:"pNode 3 - no child", isParent:true}
    
    ];
    let llenarDatatableProceso=function(data) {
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        let addtabla = $("#contenedorTablaProcesoPdf");
        $(addtabla).empty();
        $(addtabla).append('<table id="dataTableProcesoPdf" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
        simpleDataTable({
            uniform: false,
            tableNameVariable: "datatable_dataTableProcesoPdf",
            table: "#dataTableProcesoPdf",
            tableColumnsData: data,
            tableColumns: [
                {
                    data: "emp_co_trab",
                    title: "Nro. Doc.",
                },
                {
                    data: "emp_tipo_doc",
                    title: "Tipo Doc.",
                },
                {
                    data: "emp_co_trab",
                    title: "Empleado",
                    "render":function(value,row,oData){
                        return oData.emp_apel_pat+ " " + oData.emp_apel_mat+"," + oData.emp_no_trab
                    }
                },
                {
                    data: "emp_direc_mail",
                    title: "Dir. envio",
                },
                {
                    data: "emp_ruta_pdf",
                    title: "Pdf",
                }
            ]
        })
    }
    let llenarDatatablePdfs=function(data) {
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        let addtabla = $("#contenedorTablaListar");
        $(addtabla).empty();
        $(addtabla).append('<table id="dataTableListarPdf" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
        simpleDataTable({
            uniform: false,
            tableNameVariable: "datatable_dataTableListarPdf",
            table: "#dataTableListarPdf",
            tableColumnsData: data,
            tableHeaderCheck: true,
            tableHeaderCheckIndex: 0,
            headerCheck: "chkListarPdf",
            tableColumns: [
                {
                    data: "emp_co_trab",
                    title: "",
                    "bSortable": false,
                    className: 'align-center',
                    "render": function (value,row, oData) {
                        let check = `<input type="checkbox" class="form-check-input-styled-info pdfListado" 
                                        data-empcotrab="${oData.emp_co_trab}" 
                                        data-emprutapdf="${oData.emp_ruta_pdf}" 
                                        data-empcoempr="${oData.emp_co_empr}"
                                        data-empdiremail="${oData.emp_direc_mail}"
                                        data-empperiodo="${oData.emp_periodo}"
                                        data-empanio="${oData.emp_anio}"
                                        data-empnotrab="${oData.emp_no_trab}"
                                        data-empapelpat="${oData.emp_apel_pat}"
                                        data-empapelmat="${oData.emp_apel_mat}"
                                        name="chk[]">`;
                        return check;
                    },
                    width: "50px",
                },
                {
                    data: "emp_co_trab",
                    title: "Nro. Doc.",
                },
                {
                    data: "emp_tipo_doc",
                    title: "Tipo Doc.",
                },
                {
                    data: "emp_co_trab",
                    title: "Empleado",
                    "render":function(value,row,oData){
                        return oData.emp_apel_pat+ " " + oData.emp_apel_mat+"," + oData.emp_no_trab
                    }
                },
                {
                    data: "emp_direc_mail",
                    title: "Dir. envio",
                },
                {
                    data: "emp_ruta_pdf",
                    title: "Pdf",
                },
                {
                    data: "emp_enviado",
                    title: "Nro. Envios",
                },
                {
                    data: null,
                    title: "Acciones",
                    "render": function (value,row, oData) {
                        let span = '';
                        span = `<div class="hidden-sm hidden-xs action-buttons">
                                        <a class="red btnVisualizarPDF" href="#"    
                                            data-empcotrab="${oData.emp_co_trab}" 
                                            data-emprutapdf="${oData.emp_ruta_pdf}" 
                                            data-empcoempr="${oData.emp_co_empr}"
                                            data-empdiremail="${oData.emp_direc_mail}"
                                           >
                                            <i class="ace-icon fa fa-file-pdf-o bigger-130"></i>
                                        </a>
                                    </div>
                                    <div class="hidden-md hidden-lg">
                                        <div class="inline pos-rel">
                                            <button class="btn btn-minier btn-yellow dropdown-toggle" data-toggle="dropdown" data-position="auto">
                                                <i class="ace-icon fa fa-caret-down icon-only bigger-120"></i>
                                            </button>
                                            <ul class="dropdown-menu dropdown-only-icon dropdown-yellow dropdown-menu-right dropdown-caret dropdown-close">
                                                <li>
                                                    <a href="#" class="tooltip-info btnVisualizarPDF" 
                                                    
                                                        data-empcotrab="${oData.emp_co_trab}" 
                                                        data-emprutapdf="${oData.emp_ruta_pdf}" 
                                                        data-empcoempr="${oData.emp_co_empr}"
                                                        data-empdiremail="${oData.emp_direc_mail}"
                                                       
                                                    data-rel="tooltip" title="View">
                                                        <span class="red"><i class="ace-icon fa fa-file-pdf-o bigger-120"></i></span>
                                                    </a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>`
                        return span
                    }
                }

            ]
        })
    }
    let llenarDatatableBitacora=function(data){
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        let addtabla = $("#contenedorTablaBitacora");
        $(addtabla).empty();
        $(addtabla).append('<table id="dataTableBitacora" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
        simpleDataTable({
            uniform: false,
            tableNameVariable: "datatable_dataTableBitacora",
            table: "#dataTableBitacora",
            tableColumnsData: data,
            tableColumns: [
                {
                    data: null,
                    title: "Usuario",
                    "render":function(value,row, oData){
                        return oData.Usuario.usu_nombre
                    }
                },
                {
                    data: null,
                    title: "Fecha",
                    "render":function(value,row, oData){
                        return moment(oData.btc_fecha_reg).format('YYYY-MM-DD hh:mm A')
                    }
                },
                {
                    data: null,
                    title: "Accion",
                    "render":function(value,row, oData){
                        return `${oData.btc_accion}` 
                    }
                    
                },
                {
                    data:null,
                    tittle:"Detalle",
                    "render":function(value,row, oData){
                        return oData.btc_ruta_pdf
                    }
                }

            ]
        })
    }
    let easyPDF=function(_base64, _title,fileName) {
        // HTML definition of dialog elements
        let dialog = `<div style=" background:#000000;
                        width:100%;
                        height:100vh">
                        <div id="pdfDialog" title="${_title}">
                            <label>Page: </label>
                            <label id="pageNum"></label>
                            <label> of </label>
                            <label id="pageLength"></label>
                            <canvas id="pdfview"></canvas>
                        </div>
                    </div>`;
        $("div[id=pdfDialog]").remove();
        $(document.body).append(dialog);
    
        // We need the javascript object of the canvas, not the jQuery reference
        let canvas = document.getElementById('pdfview');
        // Init page count
        let page = 1;
        
    
        // Init page number and the document
        $('#pageNum').text(page);
        RenderPDF(0);
    
        // PDF.js control
        function RenderPDF(pageNumber) {
          let pdfData = atob(_base64);
          pdfjsLib.disableWorker = true;
    
          // Get current global page number, defaults to 1
          displayNum = parseInt($('#pageNum').html())
          pageNumber = parseInt(pageNumber)
    
          let loadingTask = pdfjsLib.getDocument({data: pdfData});
          loadingTask.promise.then(function(pdf) {
              // Gets total page length of pdf
              size = pdf.numPages;
              $('#pageLength').text(size);
              // Handling for changing pages
              if(pageNumber == 1) {
                  pageNumber = displayNum + 1;
              }
              if(pageNumber == -1) {
                  pageNumber = displayNum - 1;
              }
              if(pageNumber == 0) {
                  pageNumber = 1;
              }
          // If the requested page is outside the document bounds
              if(pageNumber > size || pageNumber < 1) {
                  throw "bad page number";
              }
              // Changes the cheeky global to our valid new page number
              $('#pageNum').text(pageNumber)
              pdf.getPage(pageNumber).then(function(page) {
                  let scale = 1.0;
                  let viewport = page.getViewport(scale);
                  let context = canvas.getContext('2d');
                  canvas.height = viewport.height;
                  canvas.width = viewport.width;
                  let renderContext = {
                    canvasContext: context,
                    viewport: viewport
                  };
                  page.render(renderContext);
              });


          }).catch(e => {});
        }
        // Dialog definition
        $( "#pdfDialog" ).dialog({
            // Moves controls to top of dialog
            open: function (event, ui) {
                $(this).before($(this).parent().find('.ui-dialog-buttonpane'));
                // $(event.target).parent().css('background-color','black');
            },
            width: ($(window).width() / 2),
            modal: true,
            // position: ['center',20],
            position: {
                my: "center",
                at: "top",
                of: window,
                collision: "none"
            },
            buttons: {
                "Download": {
                    click: function () {
                        let data = _base64
                        let a = document.createElement('a')
                        a.target = '_self'
                        a.href = "data:application/pdf;base64, " + data
                        a.download = fileName
                        a.click();
                        let objBitacora={
                            btc_vista:getAbsolutePath(),
                            btc_accion:'Descarga Pdf',
                            btc_ruta_pdf:`Descarga de Pdf "${fileName}" desde gestor de contenido`
                        }
                        registrarBitacora(objBitacora)
                    },
                    text: 'Descargar',
                },
                "Confirm": {
                    click: function () {
                        $(this).dialog("close")
                        $("#pdfDialog").remove()
                    },
                    text: 'Cerrar',
                }
            }
        });
    }
    let registrarBitacora=function(data){
        let dataForm=data;
        responseSimple({
            url: "IntranetPJBoletasGDT/GuardarBitacoraSGCJson",
                refresh: false,
                loader:false,
                data:JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    CloseMessages()
                }
        })
    }
    let getAbsolutePath=function() {
        var loc = window.location;
        var pathName = loc.pathname.substring(0, loc.pathname.lastIndexOf('/') + 1);
        return loc.href.substring(0, loc.href.length - ((loc.pathname + loc.search + loc.hash).length - pathName.length));
    }
    let llenarDatatableProcesoV2=function(data) {
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        let addtabla = $("#contenedorTablaProcesoPdfV2");
        $(addtabla).empty();
        $(addtabla).append('<table id="dataTableProcesoPdfV2" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
        simpleDataTable({
            uniform: false,
            tableNameVariable: "datatable_dataTableProcesoPdfV2",
            table: "#dataTableProcesoPdfV2",
            tableColumnsData: data,
            tableColumns: [
                {
                    data: "emp_co_trab",
                    title: "Nro. Doc.",
                },
                {
                    data: "emp_tipo_doc",
                    title: "Tipo Doc.",
                },
                {
                    data: "emp_co_trab",
                    title: "Empleado",
                    "render":function(value,row,oData){
                        return oData.emp_apel_pat+ " " + oData.emp_apel_mat+"," + oData.emp_no_trab
                    }
                },
                {
                    data: "emp_direc_mail",
                    title: "Dir. envio",
                },
                {
                    data: "emp_ruta_pdf",
                    title: "Pdf",
                }
            ]
        })
    }
    return {
        init: function () {
            _inicio()
            _componentes()
            _metodos()
        }
    }
}()
document.addEventListener('DOMContentLoaded',function(){

    panelBoletas.init()
})


