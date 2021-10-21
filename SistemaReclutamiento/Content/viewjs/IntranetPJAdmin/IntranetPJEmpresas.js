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
                        $("#cboEmpresa").append('<option value="' + value.emp_id + '" data-codofisis="'+value.emp_co_ofisis+'">' + value.emp_nomb + '</option>');
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
            $("#det_empr_id").val(emp_id)
            ObtenerInformacionEmpresa(emp_id)
        })
        $(document).on('click','#btnNuevoCertificado',function(e){
            e.preventDefault()
            $("#modalFormularioCertificado").modal('show')
        })
        $(document).on('click','#btnSincronizar',function(e){
            e.preventDefault()
            responseSimple({
                url:'IntranetPJBoletasGDT/BolEmpresaSincronizarJson',
                refresh:false,
                callBackSuccess:function(response){
                    console.log(response)
                }
            })
        })
        $(document).on('click','#btnGuardarCertificado',function(e){
            e.preventDefault();
            
            let selected = $("#cboEmpresa").find('option:selected');
            let codofisis = selected.data('codofisis')
            let empresa=selected.text()
            let emp_id = selected.val()

            let file = $('#certificado')[0].files[0];
            if(file==null){
                messageResponse({
                    text: "Debe Seleccionar Un Archivo",
                    type: "error"
                })
                return false;
            }
            else{
              
                let image_arr = file.name.split(".");
                let extension = image_arr.pop().toLowerCase();
                url='IntranetPJBoletasGDT/BolDetCertEmpresaInsertarJson';
                let dataForm=new FormData();
                dataForm.append('file',file);
                dataForm.append('emp_co_ofisis',codofisis)
                dataForm.append('emp_nomb',empresa)
                dataForm.append('det_empr_id',$("#det_empr_id").val())
                dataForm.append('det_pass_cert',$("#det_pass_cert").val())
                if (extension != 'pfx') {
                    messageResponse({
                        text: 'Sólo Se Permite formato PFX(.pfx)',
                        type: "warning"
                    });
                  
                }
                else {
                    responseFileSimple({
                        url:url,
                        data:dataForm,
                        refresh:false,
                        callBackSuccess:function(response){
                            if(response.respuesta){
                                ObtenerInformacionEmpresa(emp_id)
                                $("#modalFormularioCertificado").modal('hide')
                            }
                        }
                    })
                }
            }
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
                        $("#divInfoCertificados").show()
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
                                        let select='<select class="form-control input-sm selectEnUso" data-id="'+row.det_empr_id+'" style="width:100%">'
                                        if(row.det_en_uso==1){
                                            select+='<option value="1" selected>SI</option><option value="0">NO</option>'
                                        }
                                        else{
                                            select+='<option value="1">SI</option><option value="0" selected>NO</option>'
                                        }
                                        select+='</select>'
                                        return select
                                    }
                                }
                            ]
                        });
                    }
                    else {
                        $("#divInfoCertificados").hide()
                    }
              }
              else{
                $("#divInfoEmpresa").hide()
                $("#divInfoCertificados").hide()
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