let panelConfiguracionBoletas = function () {
    const TIPO_CONFIGURACION="PATH"
    let _inicio=function(){
        let hoy = new Date();
        let fecha_hoy = moment(hoy).format('YYYY-MM-DD');
        $('#anioCreacion').datetimepicker({
            format: 'YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: fecha_hoy,
            maxDate:fecha_hoy
        })
        if(TIPO_CONFIGURACION){
            let dataForm={tipo:TIPO_CONFIGURACION}
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
    let _componentes = function () {
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
                        window.location.reload()
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
                    CloseMessages()
                    if(response.respuesta){
                        zNodes=response.data;
                        $("#mostrarArbolDirectorios").show();
                        $.fn.zTree.init($("#treeDemo2"), {}, zNodes)
                    }
                    messageResponse({
                        text: response.mensaje,
                        type: response.respuesta?'success':'error'
                    })
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
    return {
        init: function () {
            _inicio()
            _componentes()
            _metodos()
        }
    }
}()
document.addEventListener('DOMContentLoaded',function(){
    panelConfiguracionBoletas.init()
})


