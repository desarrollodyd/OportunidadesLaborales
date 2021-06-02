let panelBoletas=function(){
    let _inicio=function(){
        let dateinicio = new Date(moment().format("YYYY"));
        $('#anio_creacion').datetimepicker({
            format: 'YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })
        //carga de salas
        responseSimple({
            url: "sql/TMEMPRListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                let data=response.data
                $.each(data, function (index, value) {
                    $("#cbo_empresa").append(`<option value="${value.CO_EMPR}">${value.DE_NOMB}</option>`);
                });
                $("#cbo_empresa").select2({
                    placeholder: "--Seleccione--", allowClear: true
                });
            }
        });
    }
    let _componentes=function(){
        // $(".nav-tabs a").click(function(){
        //     $(this).tab('show');
        //     console.log("asd")
        //   });
          $('.nav-tabs a').on('shown.bs.tab', function(event){
              console.log(event.target)
              console.log(event.relatedTarget)
            // var x = $(event.target).text();         // active tab
            // var y = $(event.relatedTarget).text();  // previous tab
            // $(".act span").text(x);
            // $(".prev span").text(y);
          });
    }
    let _metodos=function(){

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