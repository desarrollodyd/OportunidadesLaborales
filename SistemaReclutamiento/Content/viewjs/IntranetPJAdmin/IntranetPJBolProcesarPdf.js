let panelBolProcesarPdf = function () {
    let progress = $.connection.progressHub;
    let connectionId;
    let _inicio=function(){
        // let dateinicio = new Date(moment().format("YYYY"))
        let hoy = new Date();
        let fecha_hoy = moment(hoy).format('YYYY-MM-DD');
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
        $('#archivoProceso').ace_file_input({
            no_file: 'sin archivo ...',
            btn_choose: 'escoger',
            btn_change: 'cambiar',
            droppable: false,
            onchange: null,
            thumbnail: false
        });
        responseSimple({
            url:'IntranetPJBoletasGDT/BolEmpresaListarPorUsuarioJson',
            refresh:false,
            callBackSuccess:function(response){
                CloseMessages()
                if(response.respuesta){
                    let data=response.data
                    $("#cboEmpresa").append(`<option value="">--Seleccione--<option>`)
                    $.each(data, function (index, value) {
                        $("#cboEmpresa").append(`<option value="${value.emp_co_ofisis}" data-ruc="${value.emp_rucs}">${value.emp_nomb}</option>`);
                    });
                    $("#cboEmpresa").select2({
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
        $(document).on('change','#cboEmpresa',function(e){
            let nombreEmpresa=$(this).find(':selected').text()
            let rucEmpresa=$(this).find(':selected').data('ruc')
            $("#nombreEmpresa").val(nombreEmpresa)
            $("#rucEmpresa").val(rucEmpresa)
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
                    }
                }
            })
        })
        $(document).on('click','.close',function(e){
            e.preventDefault()
            $("#modalBoleta").modal('hide')
        })
        $('body').on('contextmenu', '#pdfview', function(e){ return false; });
        $(document).off('click', ".btnProcesarPdf")
        $(document).on('click','.btnProcesarPdf',function(e){
            e.preventDefault()
            CloseMessages()
          
            if($("#cboEmpresa").val()==''){
                messageResponse({
                    text: "Debe seleccionar una empresa",
                    type: "warning"
                })
                return false
            }
            if($("#fechaProcesoPdf").val()==''){
                messageResponse({
                    text: "Debe ingresar una fecha valida",
                    type: "warning"
                })
                return false
            }
            let file = $('#archivoProceso')[0].files[0];
            if(file==null){
                messageResponse({
                    text: "Debe seleccionar un archivo .rar",
                    type: "warning"
                })
                return false
            }
            let fileNameArray = file.name.split(".");
            let extension = fileNameArray.pop().toLowerCase();
            if (extension != 'rar') {
                messageResponse({
                    text: 'Sólo Se Permite formato Comprimido .rar',
                    type: "warning"
                });
                return false
            }
            $("#formProcesarPdf").submit()
            let url='/IntranetPJBoletasGDT/BolProcesarPdfV2'
            let dataForm = new FormData(document.getElementById("formProcesarPdf"));
            messageConfirmation({
                content: '¿Esta seguro de realizar esta acción?, la informacion anterior correspondiente a los campos seleccionados seran eliminados',
                callBackSAceptarComplete: function () {
                    progress.client.AddProgressBoletas = function (message,percentage, hide) {
                        console.info(message,percentage,hide);
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
                                    llenarDatatableProceso(response.data)
                                }
                                $('#mod-progress').modal('hide') 
                            },
                            loader:false
                        });
                    })
                }
            });

               
           
        })
    }
    let _metodos=function(){
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
                archivoProceso:{
                    required: true,
                }
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
                archivoProceso:{
                    required: 'Campo Obligatorio',
                }
            }
        });
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
                    data: null,
                    title: "Creado",
                    "render":function(value,row,oData){
                        let spanOculto='<div style="display:none;">'+oData.boletaCreada+'</div>'
                        let style=oData.boletaCreada?"success":"danger"
                        let spanCreado=`<span class="label label-sm label-${style}">${(oData.boletaCreada?"Creado":"No se pudo crear")}</span>`
                        return spanOculto+spanCreado
                    }
                },
                {
                    data: "emp_ruta_pdf",
                    title: "Pdf",
                    "render":function(value,row,oData){
                        return oData.boletaCreada?oData.emp_ruta_pdf:""
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
    panelBolProcesarPdf.init()
})
