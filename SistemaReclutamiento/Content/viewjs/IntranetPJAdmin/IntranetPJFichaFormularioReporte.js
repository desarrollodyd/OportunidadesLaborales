var PanelFichaFormulario = function () {
    var _inicio = function () {
        var dateinicio = new Date(moment().format("MM-DD-YYYY"));
        $('#txt-fecha').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })

  
        
            var cusUsuario = cumEnvioDet.CumEnvio.CumUsuario;
            var cusEnvioDet = cumEnvioDet;
            var cusEnvio = cumEnvioDet.CumEnvio;
            objEnvioDet = cusEnvioDet;
            objEnvioDet.CumEnvio = cusEnvio;

            if (cusUsuario.cus_id != 0) {

                var fechaActual = moment(new Date()).format("DD/MM/YYYY");
                var fechaEnvio = moment(cusEnvioDet.end_fecha_reg).format("DD/MM/YYYY");
                if (fechaActual != fechaEnvio && cusEnvio.env_estado == 2) {
                    $(".btn_guardar").attr('disabled', true);
                }
                $("#txt-codigo-busqueda").attr('disabled', true);
                $("#txt-dni-busqueda").attr('disabled', true);
                // Llenado de data
                preguntas = cusUsuario.CumUsuPregunta;
                if (preguntas.length > 0) {
                    $.each(preguntas, function (index, value) {
                        var fk_pregunta = value.fk_pregunta;
                        var respuestas = value.CumUsuRespuesta;
                        $.each(respuestas, function (i, val) {
                            if (val.ure_respuesta == 'SI' && val.ure_tipo == 'CERRADA') {
                                // $("#cbo"+fk_pregunta).bootstrapToggle('on');
                                $("#cbo" + fk_pregunta).prop("checked", !$("#cbo" + fk_pregunta).prop("checked"));
                                $("#cbo" + fk_pregunta).val(true);
                            }
                            else {
                                // $("#cbo"+fk_pregunta).bootstrapToggle('off');
                                $("#cbo" + fk_pregunta).val(true);
                            }

                            if (val.ure_tipo == 'ABIERTA' && (val.ure_respuesta == 'SI' || val.ure_respuesta == 'NO')) {
                                if (val.ure_respuesta == 'SI' || val.ure_respuesta == 'NO') {
                                    $("#cbo" + fk_pregunta).attr('data-predesc', value.upr_pregunta);
                                    $("#cbo" + fk_pregunta).attr('data-fkpreg', fk_pregunta);
                                    $("#cbo" + fk_pregunta).attr('data-restipo', val.ure_tipo);
                                    $("#cbo" + fk_pregunta).attr('data-id', val.ure_id);
                                    $("#cbo" + fk_pregunta).attr('data-uprid', value.upr_id);
                                    // $("#cbo"+fk_pregunta).attr('data-detalle','SI');
                                    if (val.ure_respuesta == 'SI') {
                                        // $("#cbo"+fk_pregunta).bootstrapToggle('on');
                                        $("#cbo" + fk_pregunta).prop("checked", !$("#cbo" + fk_pregunta).prop("checked"));
                                        $("#cbo" + fk_pregunta).val(true);
                                        $(".divdetalle" + fk_pregunta).show();
                                    }
                                    else {
                                        // $("#cbo"+fk_pregunta).bootstrapToggle('off');
                                        $("#cbo" + fk_pregunta).val(false);
                                        $(".divdetalle" + fk_pregunta).hide();
                                    }
                                }

                                $("#detalle" + fk_pregunta).val(val.ure_respuesta);

                                $("#detalle" + fk_pregunta).attr('data-predesc', value.upr_pregunta);
                                $("#detalle" + fk_pregunta).attr('data-fkpreg', fk_pregunta);
                                $("#detalle" + fk_pregunta).attr('data-restipo', val.ure_tipo);
                                $("#detalle" + fk_pregunta).attr('data-id', val.ure_id);


                            }
                            else {

                                $("#cbo" + fk_pregunta).attr('data-predesc', value.upr_pregunta);
                                $("#cbo" + fk_pregunta).attr('data-fkpreg', fk_pregunta);
                                $("#cbo" + fk_pregunta).attr('data-restipo', val.ure_tipo);
                                $("#cbo" + fk_pregunta).attr('data-id', val.ure_id);
                                $("#cbo" + fk_pregunta).attr('data-uprid', value.upr_id);
                            }
                        })
                    });
                    $('#usuario_firma').attr('src',basePath+'CumplimientoFiles/CumUsuario/'+cusUsuario.cus_firma);
                }
                else {
                //    $('#usuario_firma').attr('src',BasePath+'CumplimientoFiles/CumUsuario/'+cusUsuario.cus_firma);
                }


                $("#txt-estado").val(cusUsuario.cus_estado);
                $("#txt-id_cus").val(cusUsuario.cus_id);
                // $('#txt-firma').attr('disabled',true);
                $("#txt-fecha").val(moment(cusUsuario.cus_fecha_act).format("DD/MM/YYYY"))

                //Fin llenado de data
                $("#txt-nombres").val(cusUsuario.apellido_pat + " " + cusUsuario.apellido_mat + ", " + cusUsuario.nombre);
                $("#txt-dni").val(cusUsuario.cus_dni);
                $("#txt-celular").val(cusUsuario.celular);
                $("#txt-direccion").val(cusUsuario.direccion);
                $("#txt-area").val(cusUsuario.sede);
                $("#txt-ruc").val(cusUsuario.ruc);
                $("#txt-empresa").val(cusUsuario.empresa);

            }
 
    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();

        },

    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelFichaFormulario.init();
});