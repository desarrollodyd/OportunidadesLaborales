let panelUsuarioEmpresa=function(){
    let _inicio=function(){
        console.log('inicio')
    }
    return {
        init: function () {
            _inicio()
        }
    }
}()
document.addEventListener('DOMContentLoaded',function(){
    panelUsuarioEmpresa.init()
})
