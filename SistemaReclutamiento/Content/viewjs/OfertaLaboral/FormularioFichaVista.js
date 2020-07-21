var FormularioFichaVista = function () {
    var usuarioSesion=usuario;
    var postulanteSesion=postulante;
    var personaSesion=persona;
    var _inicio=function(){
        $("#txt-nombres").val(personaSesion.per_apellido_pat+" "+personaSesion.per_apellido_mat+", "+personaSesion.per_nombre);
        $("#txt-dni").val(personaSesion.per_numdoc);
        $("#txt-celular").val(postulanteSesion.pos_celular);
        $("#txt-direccion").val(postulanteSesion.pos_direccion);
        _ListardatosFicha(usuarioSesion.usu_id)
    }
    var _ListardatosFicha = function (usuario_id) {
        var dataForm={
            fk_usuario:usuario_id
        }
        responseSimple({
            url:'CumUsuario/CumUsuarioFkUsuarioObtenerJson',
            refresh:false,
            data:JSON.stringify(dataForm),
            callBackSuccess:function(response){
                if(response.respuesta){
                    var data=response.data;
                    if(data.cus_id>0){
                        var preguntas=data.CumUsuPregunta;

                        $.each(preguntas,function(index,value){
                            var fk_pregunta=value.fk_pregunta;
                            var respuestas=value.CumUsuRespuesta;
                            $.each(respuestas,function(i,val){

                                if(val.ure_respuesta=='SI'){
                                    $("#cbo"+fk_pregunta).bootstrapToggle('on');
                                    $("#cbo"+fk_pregunta).val(true);
                                }
                                else{
                                    $("#cbo"+fk_pregunta).bootstrapToggle('off');
                                    $("#cbo"+fk_pregunta).val(true);
                                }
                                if(val.ure_tipo=='ABIERTA'&&(val.ure_respuesta=='SI'||val.ure_respuesta=='NO')){
                                    if(val.ure_respuesta=='SI'||val.ure_respuesta=='NO'){
                                        $("#cbo"+fk_pregunta).attr('data-predesc',value.upr_pregunta);
                                        $("#cbo"+fk_pregunta).attr('data-fkpreg',fk_pregunta);
                                        $("#cbo"+fk_pregunta).attr('data-restipo',val.ure_tipo);
                                        $("#cbo"+fk_pregunta).attr('data-id',val.ure_id);
                                        $("#cbo"+fk_pregunta).attr('data-uprid',value.upr_id);
                                        if(val.ure_respuesta=='SI'){
                                            $("#cbo"+fk_pregunta).bootstrapToggle('on');
                                            $("#cbo"+fk_pregunta).val(true);
                                            $(".divdetalle"+fk_pregunta).show();
                                        }
                                        else{
                                            $("#cbo"+fk_pregunta).bootstrapToggle('off');
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
                                    $("#cbo"+fk_pregunta).attr('data-predesc',value.upr_pregunta);
                                    $("#cbo"+fk_pregunta).attr('data-fkpreg',fk_pregunta);
                                    $("#cbo"+fk_pregunta).attr('data-restipo',val.ure_tipo);
                                    $("#cbo"+fk_pregunta).attr('data-id',val.ure_id);
                                    $("#cbo"+fk_pregunta).attr('data-uprid',value.upr_id);
                                }
                            })
                        })
                        $("#txt-estado").val(data.cus_estado);
                        $("#txt-id_cus").val(data.cus_id);
                        $('#txt-firma').attr('disabled',true);
                        $("#txt-fecha").val(moment(data.cus_fecha_act).format("DD/MM/YYYY"))
                    }
                    else{
                        $("#txt-fecha").val(moment(data.cus_fecha_reg).format("DD/MM/YYYY"))
                    }
                }
                else{

                }

            }
        })
    }

    var _componentes = function () {


        // var dateinicio = new Date(moment().format("MM-DD-YYYY"));
        $('#txt-fecha').datetimepicker({
            format: 'DD-MM-YYYY',
            // ignoreReadonly: true,
            allowInputToggle: true,

        }).attr('readonly','readonly');

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
        $(document).on('click','.btn_guardar',function(e){
            e.preventDefault();
            if($("#txt-dni").val()=='' || $("#txt-direccion").val()=='' || $("#txt-celular").val()==''){
                messageResponse({
                    text: "Complete los Campos Obligatorios",
                    type: "error"
                })
                return false;
            }
            var objUsuario={
                cus_id:$("#txt-id_cus").val(),
                cus_dni:$("#txt-dni").val(),
                cus_direccion:$("#txt-direccion").val(),
                cus_celular:$("#txt-celular").val(),
                cus_tipo:$("#txt-tipo").val(),
                cus_firma_act:$("#txt-firma_act").val(),
                cus_estado:$("#txt-estado").val(),
            }
            var fileName=$('#txt-firma').val();
            // var fileName = e.target.files[0].name;
            objUsuario.cus_firma=fileName;
            var divsPreguntas = $(".pregunta");
            var arrayPreguntas=[];

            $(divsPreguntas).each(function(e,val){
                var arrayRespuestas=[];
                var respuesta='';
                var orden=0;
                if($(this).prop('checked')){
                    respuesta='SI';
                    orden=1;
                }
                else{
                    respuesta='NO';
                    orden=2;
                }
                var objetoPregunta={
                    upr_id:$(this).data('uprid'),
                    upr_pregunta:$(this).data('predesc'),
                    fk_pregunta:$(this).data('fkpreg'),
                }
                var objetoRespuesta={
                    ure_id:$(this).data("id"),
                    ure_respuesta:respuesta,
                    ure_tipo:$(this).data('restipo'),
                    ure_orden:orden,
                }
                arrayRespuestas.push(objetoRespuesta);
                if($(this).data('restipo')=='ABIERTA'){
                   
                    if(respuesta!='SI' && respuesta!='NO'){
                        console.log($("#detalle"+$(this).data('fkpreg')).data("id"));
                        console.log($("#detalle"+$(this).data('fkpreg')).val());
                        console.log($("#detalle"+$(this).data('fkpreg')).data('restipo'));
                        var objetoRespuestaAbierta={
                            ure_id:$("#detalle"+$(this).data('fkpreg')).data("id"),
                            // ure_respuesta:$("#detalle"+$(this).data('fkpreg')).val(),
                            ure_respuesta:'',
                            ure_tipo:$("#detalle"+$(this).data('fkpreg')).data('restipo'),
                            ure_orden:3
                        }
                        arrayRespuestas.push(objetoRespuestaAbierta);
                    }
                }
                objetoPregunta.CumUsuRespuesta=arrayRespuestas;
                arrayPreguntas.push(objetoPregunta);
            })
            objUsuario.CumUsuPregunta=arrayPreguntas;
            if(objUsuario.cus_id==0){//Nuevo Usuario
                var dataForm=new FormData();
                dataForm.append('usuario',JSON.stringify(objUsuario));
                var file = $('#txt-firma')[0].files[0];
                if(file!=null){
                    dataForm.append('file', file);
                    responseFileSimple({
                        url:'CumUsuario/CumFichaInsertarJson',
                        data:dataForm,
                        refresh:false,
                        callBackSuccess:function(response){
                            if(response.respuesta){
                                window.location.reload();
                            }
                        }
                    })
                }
                else{
                    messageResponse({
                        text: "Debe Seleccionar Un Archivo Adjunto",
                        type: "error"
                    })
                }
            }
            else{
                var dataForm={
                    usuario:objUsuario,
                }
                responseSimple({
                    url:'CumUsuario/CumFichaEditarJson',
                    refresh:false,
                    data:JSON.stringify(dataForm),
                    callBackSuccess:function(response){
                        if(response.respuesta){
                            window.localtion.reload();
                        }
                    }
                })
            }


        })
        $(document).on('click','#myTab',function(e){
            e.preventDefault();
            console.log("click");
        })
        $(document).on('click','#myTab2',function(e){
            // e.preventDefault();
    
            console.log("click2");
        })
        $(document).on('click','#btn_tab',function(e){
           
            $("#ficha_tab").addClass("active");
            $("#ficha_tab").addClass("in");
            // $('#myTab2')[0].click();
            $('#myTab2').addClass("active");
            $('#myTab').removeClass("active");
            $("#lista_ficha_tab").removeClass("active");
            $("#lista_ficha_tab").removeClass("in");
            // $('#myTab2').trigger('click'); 
        })
    
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmFichaFormulario',
            contenedor: '#frmFichaFormulario-form',
            rules: {

            },
            messages: {

            }
        });
    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();
            _componentes();
            _metodos();
        },
        __ListardatosFicha: function (usuario_id) {
            _ListardatosFicha(usuario_id);
        },
    }
}();

function appendFormdata(FormData, data, name){
    name = name || '';
    if (typeof data === 'object'){
        $.each(data, function(index, value){
            if (name == ''){
                appendFormdata(FormData, value, index);
            } else {
                appendFormdata(FormData, value, name + '['+index+']');
            }
        })
    } else {
        FormData.append(name, data);
    }
}
// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    FormularioFichaVista.init();
});