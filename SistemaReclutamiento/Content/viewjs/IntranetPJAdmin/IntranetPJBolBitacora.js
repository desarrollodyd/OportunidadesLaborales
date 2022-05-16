let panelBitacoraBoletas = function () {
    let _inicio=function(){
        // let dateinicio = new Date(moment().format("YYYY"))
        let hoy = new Date();
        let fecha_hoy = moment(hoy).format('YYYY-MM-DD');
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
    }
    let _componentes = function () {
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
    return {
        init: function () {
            _inicio()
            _componentes()
        }
    }
}()
document.addEventListener('DOMContentLoaded',function(){

    panelBitacoraBoletas.init()
})


