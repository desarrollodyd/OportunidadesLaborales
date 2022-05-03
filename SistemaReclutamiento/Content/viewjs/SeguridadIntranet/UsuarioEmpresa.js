let panelUsuarioEmpresa=function(){
    let _inicio=function(){
        listausuarios()
    }
    let _componentes=function(){
        $(document).on('change','#cboUsuario',function(e){
            e.preventDefault()
            $("#divUsuario").hide()
            let usuario_id=$(this).val()
            if(usuario_id){
                renderizarEmpresas(usuario_id)
            }
            else{
                messageResponse({
                    text: "Debe seleccionar un usuario",
                    type: "error"
                })
            }
        })
        $(document).on('ifChecked', '.myCheck', function (event) {
            let empresa_id=$(this).val()
            let usuario_id=$("#cboUsuario").val()
            let url=basePath+"UsuarioEmpresa/InsertarUsuarioEmpresaJson"
            $.ajax({
                url:url,
                data:JSON.stringify({usuario_id:usuario_id,empresa_id:empresa_id}),
                contentType:"application/json",
                type:"post",
                beforesend:function(){
                    block_general("body")
                },
                complete:function(){
                    unblock("body")
                },
                success:function(response){
                    if(response.respuesta){
                        messageResponse({
                            text: response.mensaje,
                            type: "success"
                        })
                        renderizarEmpresas(usuario_id)
                    }
                    else{
                        messageResponse({
                            text: response.mensaje,
                            type: "error"
                        })
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
        
                }
            })
        })
        $(document).on('ifUnchecked', '.myCheck', function (event) {
            console.log("unchecked")
            let empresa_id=$(this).val()
            let usuario_id=$("#cboUsuario").val()
            let url=basePath+"UsuarioEmpresa/EliminarUsuarioEmpresaJson"
            $.ajax({
                url:url,
                data:JSON.stringify({usuario_id:usuario_id,empresa_id:empresa_id}),
                contentType:"application/json",
                type:"post",
                beforesend:function(){
                    block_general("body")
                },
                complete:function(){
                    unblock("body")
                },
                success:function(response){
                    if(response.respuesta){
                        messageResponse({
                            text: response.mensaje,
                            type: "success"
                        })
                        renderizarEmpresas(usuario_id)
                    }
                    else{
                        messageResponse({
                            text: response.mensaje,
                            type: "error"
                        })
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
        
                }
            })
        })
    }
    let listausuarios=function() {
        let url = basePath + "Usuario/ListadoUsuarioJson"
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({}),
            beforeSend: function () {
                block_general("body")
            },
            complete: function () {
                unblock("body")
            },
            success: function (response) {
                let usuarios = response.data;
                if (response.respuesta) {
                    $("#cboUsuario").html("")
                    $("#cboUsuario").append('<option value="">---Seleccione---</option>')
                    $.each(usuarios, function (index, value) {
                        $("#cboUsuario").append( `  
                            <option value="${value.usu_id}">${value.usu_nombre}</option>
                        `)
                    })
                    $("#cboUsuario").select2({
                        multiple: false, placeholder: "--Seleccione--"
                    })
                }
                else {
                    messageResponse({
                        text: response.mensaje,
                        type: "error"
                    })
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
    
            }
        })
    
    }
    let renderizarEmpresas=function(usuario_id){
        let arrayHtmlEmpresas=[]
        let divInsertar=$("#divEmpresas")
        divInsertar.html("")
        let url=basePath + "UsuarioEmpresa/GetListadoUsuarioEmpresaPorUsuario"
        $.ajax({
            url:url,
            data:JSON.stringify({usuario_id:usuario_id}),
            contentType:"application/json",
            type:"post",
            beforesend:function(){
                block_general("body")
            },
            complete:function(){
                unblock("body")
            },
            success:function(response){
            
                if(response.respuesta){
                    messageResponse({
                        text: response.mensaje,
                        type: "success"
                    })
                    let dataPersona=response.dataPersona
                    let dataUsuario=response.dataUsuario
                    $("#txtUsuario").val(dataUsuario.usu_nombre.toUpperCase())
                    $("#txtPersona").val(`${dataPersona.per_apellido_pat.toUpperCase()} ${dataPersona.per_apellido_mat.toUpperCase()}, ${dataPersona.per_nombre.toUpperCase()}`)
                    $("#txtCorreo").val(dataPersona.per_correoelectronico.toUpperCase())
                    $("#txtTipo").val(dataPersona.per_tipo.toUpperCase())
                    $("#txtEstado").val(dataPersona.per_estado.toUpperCase()=='A'?'ACTIVO':'INACTIVO')
                    let dataEmpresas=response.dataEmpresas
                    $.each(dataEmpresas,function(index,value){
                        let checked=value.seleccionado?"checked":""
                        let htmlTag=`
                        <div  class="col-md-4 col-sm-4"  style="padding-right: 4px; padding-left: 4px; padding-bottom: 4px">
                            <div style="margin-bottom: 0px">
                            <div class="panel-heading" style="background: #ECF9ED;padding: 6px 6px;text-transform: uppercase;">
                                <label>
                                    <input type="checkbox" class="myCheck" ${checked} value="${value.emp_id}"/>
                                    ${value.emp_nomb.trim()}
                                </label>
                            </div>
                            </div>
                        </div>
                        `
                        arrayHtmlEmpresas.push(htmlTag)
                    })
                    divInsertar.html(arrayHtmlEmpresas.join(''))
                    $(".myCheck").iCheck({
                        checkboxClass: 'icheckbox_square-blue',
                        radioClass: 'iradio_square-red',
                        increaseArea: '2%' // optional
                    });
                    $("#divUsuario").show()
                }
                else{
                    messageResponse({
                        text: response.mensaje,
                        type: "error"
                    })
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
    
            }
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
    panelUsuarioEmpresa.init()
})
