let panelBoletas=function(){
    let tabAcciones=['tab1','tab2','tab3']
    let _inicio=function(){
        let dateinicio = new Date(moment().format("YYYY"))
        $('#anioCreacion').datetimepicker({
            format: 'YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio,
            maxDate:dateinicio
        })
        //carga de salas
        responseSimple({
            url: "sql/TMEMPRListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                CloseMessages()
                if(response.respuesta){
                    let data=response.data
                    $.each(data, function (index, value) {
                        $("#cboEmpresa").append(`<option value="${value.CO_EMPR}">${value.DE_NOMB}</option>`);
                    });
                    $("#cboEmpresa").select2({
                        placeholder: "--Seleccione--", allowClear: true
                    })
                }
            }
        })
    }
    let _componentes=function(){
        // $(".nav-tabs a").click(function(){
        //     $(this).tab('show');
        //     console.log("asd")
        //   });
        //   $('.nav-tabs a').on('shown.bs.tab', function(event){
        //     //   console.log(event.target)
        //     //   console.log(event.relatedTarget)
        //     //   let tab=$(this).data("tab")
        //     //   console.log(tab)
        //     // var x = $(event.target).text();         // active tab
        //     // var y = $(event.relatedTarget).text();  // previous tab
        //     // $(".act span").text(x);
        //     // $(".prev span").text(y);
        //   });
        $(document).on('shown.bs.tab','.nav-tabs a',function(e){
            let tab=$(this).data('tab')
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
                            config_id=idInsertado
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
                    console.log(response)
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
                    required: false,

                },
                config_valor:
                {
                    required: false,

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
        });
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