let panelBoletas=function(){
    let _inicio=function(){
        // let dateinicio = new Date(moment().format("YYYY"))
        var hoy = new Date();
        var fecha_hoy = moment(hoy).format('YYYY-MM-DD hh:mm A');
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

                    $.each(data, function (index, value) {
                        $("#cboEmpresa").append(`<option value="${value.CO_EMPR}">${value.DE_NOMB}</option>`);
                    });
                    $("#cboEmpresa").select2({
                        placeholder: "--Seleccione--", allowClear: true
                    })
                    $.each(data, function (index, value) {
                        $("#cboEmpresaListar").append(`<option value="${value.CO_EMPR}">${value.DE_NOMB}</option>`);
                    });
                    $("#cboEmpresa").select2({
                        placeholder: "--Seleccione--", allowClear: true
                    })
                    $("#cboEmpresaListar").select2({
                        placeholder: "--Seleccione--", allowClear: true
                    })
                }
            }
        })
    }
    let _componentes=function(){
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
                let url='IntranetPJBoletasGDT/BolProcesarPdf2'
                responseSimple({
                    url: url,
                    data:JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function (response) {
                      if(response.respuesta){
                        llenarDatatableProceso(response.data)
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
            $('#dataTableListarPdf tbody tr input[type=checkbox]:checked').each(function () {
                let obj={
                    emp_co_trab:$(this).data("empcotrab"),
                    emp_ruta_pdf:$(this).data("emprutapdf"),
                    emp_co_empr:$(this).data("empcoempr"),
                    emp_direc_mail:$(this).data("empdiremail")
                }
                arrayEmpleados.push(obj);
            });
            responseSimple({
                url: "IntranetPJBoletasGDT/EnviarBoletasEmailJson",
                refresh: false,
                data: JSON.stringify(arrayEmpleados),
                callBackSuccess: function (response) {
                   console.log(response);
                }
            })
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
    }
    let setting = {	};
    let zNodes =[
        { name:"pNode 01", open:true,
            children: [
                { name:"pNode 11",
                    children: [
                        { name:"leaf node 111"},
                        { name:"leaf node 112"},
                        { name:"leaf node 113"},
                        { name:"leaf node 114"}
                    ]},
                { name:"pNode 12",
                    children: [
                        { name:"leaf node 121"},
                        { name:"leaf node 122"},
                        { name:"leaf node 123"},
                        { name:"leaf node 124"}
                    ]},
                { name:"pNode 13 - no child", isParent:true}
            ]},
        { name:"pNode 02",
            children: [
                { name:"pNode 21", open:true,
                    children: [
                        { name:"leaf node 211"},
                        { name:"leaf node 212"},
                        { name:"leaf node 213"},
                        { name:"leaf node 214"}
                    ]},
                { name:"pNode 22",
                    children: [
                        { name:"leaf node 221"},
                        { name:"leaf node 222"},
                        { name:"leaf node 223"},
                        { name:"leaf node 224"}
                    ]},
                { name:"pNode 23",
                    children: [
                        { name:"leaf node 231"},
                        { name:"leaf node 232"},
                        { name:"leaf node 233"},
                        { name:"leaf node 234"}
                    ]}
            ]},
        { name:"pNode 3 - no child", isParent:true}
    
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
                    title: "Doc. Id.",
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
                        var check = `<input type="checkbox" class="form-check-input-styled-info pdfListado" 
                                        data-empcotrab="${oData.emp_co_trab}" 
                                        data-emprutapdf="${oData.emp_ruta_pdf}" 
                                        data-empcoempr=${oData.emp_co_empr}
                                        data-empdiremail=${oData.emp_direc_mail} name="chk[]">`;
                        return check;
                    },
                    width: "50px",
                },
                {
                    data: "emp_co_trab",
                    title: "Doc. Id.",
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
                    data: "emp_co_trab",
                    title: "Acciones",
                    "render": function (value) {
                        var span = '';
                        var span = `<div class="hidden-sm hidden-xs action-buttons">
                                        <a class="red btn-detalle" href="#" data-id="${value}">
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
                                                    <a href="#" class="tooltip-info btn-detalle" data-id="${value}" data-rel="tooltip" title="View">
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


