var PanelFichaFormulario = function () {
    var objEnvioDet='';
    var objEnvio='';
    var _componentes = function () {
       
    };

    var _metodos = function () {
        // $('#txt-firma').ace_file_input({
        //     no_file: 'sin archivo ...',
        //     btn_choose: 'escoger',
        //     btn_change: 'cambiar',
        //     droppable: false,
        //     onchange: null,
        //     thumbnail: false //| true | large
        //     //whitelist:'gif|png|jpg|jpeg'
        //     //blacklist:'exe|php'
        //     //onchange:''
        //     //
        // });

        var dateinicio = new Date(moment().format("MM-DD-YYYY"));
        $('#txt-fecha').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })
        $('#txt-dni-busqueda').on('keydown keypress', function (e) {
            if (e.key.length === 1) {
                if ($(this).val().length < 8 && !isNaN(parseFloat(e.key))) {
                    $(this).val($(this).val() + e.key);
                    if ($(this).val().length == 8) {
                        var codigo=$("#txt-codigo-busqueda").val();
                        if(codigo!=''&&codigo!=undefined){
                            var numdoc=$(this).val();
                            var dataForm = {
                                codigo:codigo,
                                numdoc:numdoc,
                                env_id:envio_id
                            };
                            responseSimple({
                                url: "CumUsuario/CumUsuarioObtenerDataporEnvioJson",
                                data: JSON.stringify(dataForm),
                                refresh: false,
                                callBackSuccess: function (response) {
                                    var cusUsuario=response.data.CumEnvio.CumUsuario;
                                    var cusEnvioDet=response.data;
                                    var cusEnvio=response.data.CumEnvio;
                                    objEnvioDet=cusEnvioDet;
                                    objEnvioDet.CumEnvio=cusEnvio;

                                    if(cusUsuario.cus_id!=0){

                                        var fechaActual=moment(new Date()).format("DD/MM/YYYY");
                                        var fechaEnvio=moment(cusEnvioDet.end_fecha_reg).format("DD/MM/YYYY");
                                        if(fechaActual!=fechaEnvio){
                                            $(".btn_guardar").attr('disabled',true);
                                        }
                                        $("#txt-codigo-busqueda").attr('disabled',true);
                                        $("#txt-dni-busqueda").attr('disabled',true);
                                        // Llenado de data
                                        var preguntas=cusUsuario.CumUsuPregunta;
                                        if(preguntas.length>0){
                                            $.each(preguntas,function(index,value){
                                                var fk_pregunta=value.fk_pregunta;
                                                var respuestas=value.CumUsuRespuesta;
                                                $.each(respuestas,function(i,val){
                    
                                               
                                                    if(val.ure_tipo=='ABIERTA'&&(val.ure_respuesta=='SI'||val.ure_respuesta=='NO')){
                                                        if(val.ure_respuesta=='SI'||val.ure_respuesta=='NO'){
                                                            $("#cbo"+fk_pregunta).attr('data-predesc',value.upr_pregunta);
                                                            $("#cbo"+fk_pregunta).attr('data-fkpreg',fk_pregunta);
                                                            $("#cbo"+fk_pregunta).attr('data-restipo',val.ure_tipo);
                                                            $("#cbo"+fk_pregunta).attr('data-id',val.ure_id);
                                                            $("#cbo"+fk_pregunta).attr('data-uprid',value.upr_id);
                                                            if(val.ure_respuesta=='SI'){
                                                                // $("#cbo"+fk_pregunta).bootstrapToggle('on');
                                                                $("#cbo"+fk_pregunta).prop("checked", !$("#cbo"+fk_pregunta).prop("checked"));
                                                                $("#cbo"+fk_pregunta).val(true);
                                                                $(".divdetalle"+fk_pregunta).show();
                                                            }
                                                            else{
                                                                // $("#cbo"+fk_pregunta).bootstrapToggle('off');
                                                                $("#cbo"+fk_pregunta).val(false);
                                                                $(".divdetalle"+fk_pregunta).hide();
                                                            }
                                                        }
                                                      
                                                        $("#detalle"+fk_pregunta).val(val.ure_respuesta);
                    
                                                        $("#detalle"+fk_pregunta).attr('data-predesc',value.upr_pregunta);
                                                        $("#detalle"+fk_pregunta).attr('data-fkpreg',fk_pregunta);
                                                        $("#detalle"+fk_pregunta).attr('data-restipo',val.ure_tipo);
                                                        $("#detalle"+fk_pregunta).attr('data-id',val.ure_id);
                    
                                                        
                                                    }
                                                    else{
                                                        if(val.ure_respuesta=='SI'){
                                                            // $("#cbo"+fk_pregunta).bootstrapToggle('on');
                                                            $("#cbo"+fk_pregunta).prop("checked", !$("#cbo"+fk_pregunta).prop("checked"));
                                                            $("#cbo"+fk_pregunta).val(true);
                                                        }
                                                        else{
                                                            // $("#cbo"+fk_pregunta).bootstrapToggle('off');
                                                            $("#cbo"+fk_pregunta).val(true);
                                                        }
    
    
                                                        $("#cbo"+fk_pregunta).attr('data-predesc',value.upr_pregunta);
                                                        $("#cbo"+fk_pregunta).attr('data-fkpreg',fk_pregunta);
                                                        $("#cbo"+fk_pregunta).attr('data-restipo',val.ure_tipo);
                                                        $("#cbo"+fk_pregunta).attr('data-id',val.ure_id);
                                                        $("#cbo"+fk_pregunta).attr('data-uprid',value.upr_id);
                                                    }
                                                })
                                            });
                                            $('#txt-firma').ace_file_input({
                                                no_file: cusUsuario.cus_firma,
                                                btn_choose: 'cambiar',
                                                btn_change: 'cancelar',
                                                droppable: false,
                                                onchange: null,
                                                thumbnail: false
                                            });
                                        }
                                        else{
                                            $('#txt-firma').ace_file_input({
                                                no_file: 'sin archivo ...',
                                                btn_choose: 'escoger',
                                                btn_change: 'cancelar',
                                                droppable: false,
                                                onchange: null,
                                                thumbnail: false
                                            });
                                        }

                                     
                                        $("#txt-estado").val(cusUsuario.cus_estado);
                                        $("#txt-id_cus").val(cusUsuario.cus_id);
                                        // $('#txt-firma').attr('disabled',true);
                                        $("#txt-fecha").val(moment(cusUsuario.cus_fecha_act).format("DD/MM/YYYY"))
                                        
                                        //Fin llenado de data
                                        $("#txt-nombres").val(cusUsuario.apellido_pat+" "+cusUsuario.apellido_mat+", "+cusUsuario.nombre);
                                        $("#txt-dni").val(cusUsuario.cus_dni);
                                        $("#txt-celular").val(cusUsuario.celular);
                                        $("#txt-direccion").val(cusUsuario.direccion);
                                        $("#txt-area").val(cusUsuario.sede);
                                        $("#txt-ruc").val(cusUsuario.ruc);
                                        $("#txt-empresa").val(cusUsuario.empresa);

                                        //Firma
                                      
                                        $("#fichaSintomatologica").show();

                                    }
                                }
                            });
                        }
                        else{
                            console.log("debe llenar el campo codigo");
                        }
                    }
                }
                return false;
            }
        });
        $(document).on('click','.btn_guardar',function(e){
            e.preventDefault();
            console.log(objEnvioDet);
        })

        $('input[type="checkbox"]').change(function () {
            var check = $(this).prop('checked');
            var tipo=$(this).data('restipo');
            var fk_pregunta=$(this).data('fkpreg');
            if (check) {
                if(tipo=="ABIERTA"){
                    $(".divdetalle"+fk_pregunta).show();
                }
                $("#cboMedicacion").val(true);
                $("#txt-detalle").rules('add', {
                    required: true,
                    messages: {
                        required: "Detalle Obligatorio"
                    }
                });
                $("#txt-detalle").prop('disabled', false);

            }
            else {
                $(".divdetalle"+fk_pregunta).hide();
                $("#cboMedicacion").val(false);
                $("#txt-detalle").prop('disabled', true);
                $("#txt-detalle").val("");
            }
        });
        // $('#txt-codigo-busqueda').on('keydown keypress', function (e) {
        //     if($("#txt-dni-busqueda").val().length == 8){
        //         var e = jQuery.Event("keydown");
        //         e.which = 50;
        //         e.key.length=8; // # Some key code value
        //         $("#txt-dni-busqueda").trigger(e);
        //     }
        // });
    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _componentes();
            _metodos();

        },
        init_ListarFichas: function () {
            _ListarFichas();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelFichaFormulario.init();
});