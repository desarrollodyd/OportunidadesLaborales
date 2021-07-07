let PanelBoletas=function(){
    let modal = document.getElementById("modalBoleta");
    let inicio=function(){
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
        $('#fechaProceso').datetimepicker({
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
                        title: "Acciones",
                        "render": function (value, row, oData) {
                            var span = `
                                                        <a href="#" class="btn btn-primary button radius alert btnVisualizarPDF2" 
                                                        data-empcotrab="${oData.emp_co_trab}" 
                                                        data-emprutapdf="${oData.emp_ruta_pdf}" 
                                                        data-empcoempr="${oData.emp_co_empr}"
                                                        data-empdiremail="${oData.emp_direc_mail}"
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
            let fechaListar=$("#fechaProceso").val()
            if(empresaListar==""){
                messageResponse({
                    text: "Debe seleccionar una empresa",
                    type: "error"
                })
                return;
            }
            if(fechaListar==""){
                messageResponse({
                    text: "Fecha Incorrecta",
                    type: "error"
                })
                return;
            }
            let nombreEmpresaListar=$("#cboEmpresas").find(':selected').text()
            let empleado=persona.per_numdoc
            let dataForm={
                empresaListar:empresaListar,
                fechaListar:fechaListar,
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
                                    title: "Acciones",
                                    "render": function (value,row, oData) {
                                        var span = `
                                                        <a href="#" class="btn btn-primary button radius alert btnVisualizarPDF2" 
                                                        data-empcotrab="${oData.emp_co_trab}" 
                                                        data-emprutapdf="${oData.emp_ruta_pdf}" 
                                                        data-empcoempr="${oData.emp_co_empr}"
                                                        data-empdiremail="${oData.emp_direc_mail}"
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
                        $("body").addClass("openModal");
                        modal.style.display = "block";
                        modal.style.zIndex = 10000;
                        $("#contenidoBoletaPdf").append("<iframe width='100%' height='100%' src='data:application/pdf;base64, " +
                            encodeURI(data) + "'></iframe>")
                        // let pdfWindow = window.open("")
                        // pdfWindow.document.write(
                        //     "<iframe width='100%' height='100%' src='data:application/pdf;base64, " +
                        //     encodeURI(data) + "'></iframe>"
                        // )
                      
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