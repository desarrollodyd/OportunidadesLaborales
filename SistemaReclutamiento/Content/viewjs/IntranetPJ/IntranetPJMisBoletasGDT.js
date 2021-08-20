let PanelBoletas=function(){
    let modal = document.getElementById("modalBoleta");
    let objBitacora={}
    let inicio=function(){
        objBitacora={
            btc_vista:getAbsolutePath(),
            btc_accion:'ACCESO URL'
        }
        registrarBitacora(objBitacora)
        $('#scroll').toggle('hide');
        let hoy = new Date();
        let fecha_hoy = moment(hoy).format('YYYY-MM-DD hh:mm A');
        $("#cboEmpresas").html('')
        $("#cboEmpresas").append('<option value="">--Seleccione--</option>')
        if (listaEmpresas) {
            $.each(listaEmpresas, function (index, value) {
                $("#cboEmpresas").append(`<option value="${value.CO_EMPR}">${value.DE_NOMB}</option>`)
            })
            $("#cboEmpresas").select2()
        }
        $('#fechaProcesoInicio').datetimepicker({
            format: 'YYYY-MM',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: fecha_hoy,
            maxDate:fecha_hoy
        })
        $('#fechaProcesoFin').datetimepicker({
            format: 'YYYY-MM',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: fecha_hoy,
            maxDate:fecha_hoy
        })
        if (listaBoletasActuales) {
            simpleDataTable({
                uniform: false,
                tableNameVariable: "datatable_boletasListado",
                table: "#boletasListado",
                tableColumnsData: listaBoletasActuales,
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
                        "render": function (value, row, oData) {
                            return oData.emp_apel_pat + " " + oData.emp_apel_mat + "," + oData.emp_no_trab
                        }
                    },
                    {
                        data: "emp_direc_mail",
                        title: "Dir. envio",
                    },
                    {
                        data: null,
                        title: "Fecha Doc.",
                        "render": function (value, row, oData) {
                            return oData.emp_anio+"/"+oData.emp_periodo
                        }
                    },
                    {
                        data: null,
                        title: "Acciones",
                        "render": function (value, row, oData) {
                            var span = `
                                                        <a href="#" class="btn btn-primary button radius alert btnVisualizarPDF2" 
                                                        data-empcotrab="${oData.emp_co_trab}" 
                                                        data-emprutapdf="${oData.emp_ruta_pdf}" 
                                                        data-empcoempr="${oData.emp_co_empr}"
                                                        data-empdiremail="${oData.emp_direc_mail}"
                                                        data-nombreempresa="${oData.nombreEmpresa}"
                                                        data-rel="tooltip" title="View">
                                                        Ver
                                                    </a>
                                                                `
                            return span
                        }
                    }

                ]
            })
        }
    }   
    let componentes=function(){
        $(document).on('click','.btnBuscar',function(e){
            e.preventDefault()

            if (!$().DataTable) {
                console.warn('Advertencia - datatables.min.js no esta declarado.');
                return;
            }
            let empresaListar=$("#cboEmpresas").val()
            let fechaProcesoInicio=$("#fechaProcesoInicio").val()
            let fechaProcesoFin=$("#fechaProcesoFin").val()
            if(empresaListar==""){
                messageResponse({
                    text: "Debe seleccionar una empresa",
                    type: "error"
                })
                return;
            }
            if(fechaProcesoInicio==""){
                messageResponse({
                    text: "Fecha Inicio Incorrecta",
                    type: "error"
                })
                return;
            }
            if(fechaProcesoFin==""){
                messageResponse({
                    text: "Fecha Fin Incorrecta",
                    type: "error"
                })
                return;
            }
            let nombreEmpresaListar=$("#cboEmpresas").find(':selected').text()
            let empleado=persona.per_numdoc
            let dataForm={
                empresaListar:empresaListar,
                fechaProcesoInicio:fechaProcesoInicio,
                fechaProcesoFin:fechaProcesoFin,
                nombreEmpresaListar:nombreEmpresaListar,
                empleado:empleado
            }
            
            responseSimple({
                url: "IntranetPJBoletasGDT/BolListarporEmpleadoJson",
                refresh: false,
                data:JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    if(response.respuesta){
                        let datos=response.data
                        simpleDataTable({
                            uniform: false,
                            tableNameVariable: "datatable_boletasListado",
                            table: "#boletasListado",
                            tableColumnsData: datos,
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
                                    title: "Fecha Doc.",
                                    "render": function (value, row, oData) {
                                        return oData.emp_anio+"/"+oData.emp_periodo
                                    }
                                },
                                {
                                    data: null,
                                    title: "Acciones",
                                    "render": function (value,row, oData) {
                                        var span = `
                                                        <a href="#" class="btn btn-primary button radius alert btnVisualizarPDF2" 
                                                        data-empcotrab="${oData.emp_co_trab}" 
                                                        data-emprutapdf="${oData.emp_ruta_pdf}" 
                                                        data-empcoempr="${oData.emp_co_empr}"
                                                        data-empdiremail="${oData.emp_direc_mail}"
                                                        data-nombreempresa="${nombreEmpresaListar}"
                                                        data-rel="tooltip" title="View">
                                                        Ver
                                                    </a>
                                                                `
                                        return span
                                    }
                                }
                
                            ]
                        })
                        objBitacora={
                            btc_vista:'IntranetPJBoletasGDT/BolListarporEmpleadoJson',
                            btc_accion:'BUSQUEDA BOLETAS'
                        }
                        registrarBitacora(objBitacora)
                    }
                }
            });
        })
        $(document).on('click','.btnVisualizarPDF',function(e){
            e.preventDefault()
            let nombreEmpresa=$("#cboEmpresas").find(':selected').text()
            
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
                    if (response.respuesta) {
                        let data = response.data;
                        let file = response.fileName;
                        let a = document.createElement('a');
                        a.target = '_self';
                        a.href = "data:application/pdf;base64, " + data;
                        a.download = file;
                        a.click();
                    }
                }
            })
        })
        $(document).on('click','.btnVisualizarPDF2',function(e){
            e.preventDefault()
            $("#contenidoBoletaPdf").html('')
            // let nombreEmpresa=$("#cboEmpresas").find(':selected').text()
            let nombreEmpresa=$(this).data("nombreempresa").trim()
            let emp_co_trab=$(this).data("empcotrab")
            console.log(nombreEmpresa)
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
                    if (response.respuesta) {
                        let data = response.data;
                        let fileName = response.fileName;
                        $(".ui-pnotify").remove()
                        easyPDF(data,"PDF Empleado : "+ emp_co_trab,fileName)
                        // $("body").addClass("openModal");
                        // modal.style.display = "block";
                        // modal.style.zIndex = 10000;
                        // $("#contenidoBoletaPdf").append("<iframe width='100%' height='100%' src='data:application/pdf;base64, " +
                        //     encodeURI(data) + "'></iframe>")
                        objBitacora={
                            btc_vista:'IntranetPJBoletasGDT/VisualizarPdfIntranetAdminJson',
                            btc_accion:'VISUALIZACION PDF'
                        }
                        registrarBitacora(objBitacora)
                      
                    }
                }
            })
        })
        $(document).on('click', 'span.close', function () {
            modal.style.display = "none";
            $("body").removeClass("openModal");
        })
        window.onclick = function (event) {
            if (event.target == modal) {
                modal.style.display = "none";
                $("body").removeClass("openModal");
            }
        }
        $(document).on("click", "#btn_hide_show", function () {
            $('#scroll').toggle('slow');
        });
    }
    let easyPDF=function(_base64, _title,fileName) {
        // HTML definition of dialog elements
        let dialog = `<div style=" background:#ffffff;
                        width:100%;
                        ">
                        <div id="pdfDialog" title="${_title}">
                            <label>Page: </label> <label id="pageNum"></label><label> of </label><label id="pageLength"></label>
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
                $(event.target).parent().css('position', 'absolute');
                $(event.target).parent().css('top', '0px');
            },
            width: ($(window).width() / 2),
            modal: true,
            position: {
                my: "center top",
                at: "center top",
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
            url: "IntranetPJBoletasGDT/GuardarBitacoraJson",
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
    return {
        init:function(){
            inicio()
            componentes()
        }
    } 
}()
document.addEventListener('DOMContentLoaded',function(){
    PanelBoletas.init()
})