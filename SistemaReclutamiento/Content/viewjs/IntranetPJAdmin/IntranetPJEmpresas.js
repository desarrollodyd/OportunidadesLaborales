let PanelEmpresas = function () {
 
    let _inicio= function(){
        $(".mySelect").append('<option value="">---Seleccione---</option>');
        $(".mySelect").select2({
            multiple: false, placeholder: "--Seleccione--"
        })
        responseSimple({
            url:'IntranetPJBoletasGDT/BolEmpresaListarJson',
            refresh:false,
            callBackSuccess:function(response){
               if(response.respuesta){
                if(response.data.length>0){
                    $.each(response.data, function (index, value) {
                        $("#cboEmpresa").append('<option value="' + value.emp_id + '"  >' + value.emp_nomb + '</option>');
                    });  
                }
               }
            }
        })
    }
    let _metodos= function(){
        $(document).on('change','#cboEmpresa',function(e){
            e.preventDefault()
            let emp_id = $("#cboEmpresa option:selected").val()
            ObtenerInformacionEmpresa(emp_id)
        })
        $(document).on('click','#btnNuevoCertificado',function(e){
            e.preventDefault()
            $("#modalFormularioCertificado").modal('show')
        })
    }
    let ObtenerInformacionEmpresa=function(emp_id){
        let dataForm={
            emp_id:emp_id,
        }
        responseSimple({
            url:'IntranetPJBoletasGDT/BolEmpresaIdObtenerJson',
            refresh:false,
            data: JSON.stringify(dataForm),
            callBackSuccess:function(response){
              if(response.respuesta){
                    $("#divInfoEmpresa").show()
                    let empresa=response.data
                    $("#txtRazonSocial").val(empresa.emp_nomb)
                    $("#txtNombreCorto").val(empresa.emp_nomb_corto)
                    $("#txtRepresentanteLegal").val(empresa.emp_nom_rep_legal)
                    $("#txtRucs").val(empresa.emp_rucs)
                    $("#txtUbicacion").val(empresa.emp_prov+ " - " + empresa.emp_depa+" - "+empresa.emp_pais)
                    var addtabla = $("#divDataTableDetalle");
                    $(addtabla).empty();
                    if(empresa.DetalleCerts.length>0){
                        $(addtabla).append('<table id="dataTableDetalle" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
                        objetodatatable = $("#dataTableDetalle").DataTable({
                            "bDestroy": true,
                            "bSort": false,
                            "scrollCollapse": true,
                            "scrollX": false,
                            "sScrollX": "100%",
                            "paging": true,
                            "autoWidth": false,
                            "bAutoWidth": true,
                            "bProcessing": true,
                            "bDeferRender": true,
                            data: empresa.DetalleCerts,
                            columns: [
                                { 
                                    data: null, title: "Nombre" ,"render":function(value,type,row){
                                        return row.det_nomb_cert
                                    }
                                },
                                {
                                    data: null, title: "Ruta",
                                    "render": function (value,type,row) {
                                        return row.det_ruta_cert
                                    }
                                },
                                {
                                    data: null, title: "En uso",
                                    "render": function (value,type,row) {
                                        return row.det_en_uso
                                    }
                                }
                            ]
                            ,
                            "initComplete": function (settings, json) {
                            },
                            "drawCallback": function (settings) {
                            }
                        });
                    }
                  
              }
              else{
                $("#divInfoEmpresa").hide()
              }
            }
        })
    }
    return {
        init:function(){
            _inicio()
            _metodos()
        }
    }
}()
document.addEventListener("DOMContentLoaded", function () {
    PanelEmpresas.init()
})