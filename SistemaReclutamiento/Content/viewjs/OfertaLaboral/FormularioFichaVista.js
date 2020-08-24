var FormularioFichaVista = function () {
    var usuarioSesion=usuario;
    var postulanteSesion=postulante;
    var personaSesion=persona;
    var _inicio=function(){
        // $("#txt-nombres").val(personaSesion.per_apellido_pat+" "+personaSesion.per_apellido_mat+", "+personaSesion.per_nombre);
        // $("#txt-dni").val(personaSesion.per_numdoc);
        // $("#txt-celular").val(postulanteSesion.pos_celular);
        // $("#txt-direccion").val(postulanteSesion.pos_direccion);
        // _ListardatosFicha(usuarioSesion.usu_id)
        var dataForm={
            fk_usuario:usuarioSesion.usu_id,
            tipo:'POSTULANTE'
        }
        responseSimple({
            url:'CumUsuario/CumEnvioListarJson',
            data:JSON.stringify(dataForm),
            refresh:false,
            callBackSuccess:function(response){
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable_fichasListado",
                    table: "#fichasListado",
                    tableColumnsData: response.data,
                    tableHeaderCheck: true,
                    tableHeaderCheckIndex: 0,
                    headerCheck: "chk_ficha",
                    "scrollX":true,
                    tableColumns: [
                        {
                            data: "env_id",
                            title: "",
                            "bSortable": false,
                            className: 'align-center',
                            "render": function (value, type, oData) {
                                var check = '<input type="checkbox" class="form-check-input-styled-info fichasListado" data-id="' + value +'" name="chk[]">';
                                return check;
                            },
                            width: "50px",
                        },
                        {
                            data: "env_id",
                            title: "ID Registro",
                        },
                        {
                            data: "end_correo_pers",
                            title: "Dir. de Envío",
                        },
                        {
                            data: "env_fecha_reg",
                            title: "Fecha Envío",
                            "render": function (value) {
                                var fecha = moment(value).format('DD-MM-YYYY');
                                return fecha;
                            },
                            width: "120px",
                        },
                        {
                            data: "env_estado",
                            title: 'Estado',
                            "render": function (value, type, oData) {
                                var clase = '';
                                var estado = '';
                                if (value == 1) {
                                    clase = 'danger';
                                    estado = 'Pendiente';
                                }
                                if (value == 2) {
                                    clase = 'success';
                                    estado = 'Completado';
                                }
                                if (value == 3) {
                                    clase = 'warning';
                                    estado = 'Reenviado';
                                }
                                return '<span class="label label-' + clase + '">' + estado + '</span>';
                            }
                        },
                        {
                            data: "env_id",
                            title: 'Accion',
                            "render": function (value, type, oData) {
                                return ' <button class="btn btn-white btn-warning btn-sm btn_download" data-id="' + value + '"><i class="fa fa-download" ></i> Descargar</button>';
                            }
                        }
                    ]
                });
                // $("#fichasListado").DataTable().draw();
            }
        })
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

        $(document).on('click', '.btn_download', function (e) {
            console.log('click');
            var env_id = $(this).data("id");

            let a= document.createElement('a');
            a.target= '_blank';
            a.href= basePath + "FichaSintomatologica/DownloadFdfReporte?env_id=" + env_id;
            a.click();
            // window.open(window.location.href = basePath + "FichaSintomatologica/DownloadFdfReporte?env_id=" + env_id,'_blank');
            
        })
        $(document).on('click','.btn_descargar_todo',function(e){
            e.preventDefault();
            var arrayIds = '';
            $('#fichasListado tbody tr input[type=checkbox]:checked').each(function () {
                // arrayIds.push($(this).data("id"));
                arrayIds+=$(this).data("id")+",";
            });
            arrayIds = arrayIds.substring(0, arrayIds.length - 1);
            // arrayIds=arrayIds.slice(0,-1);
            if(arrayIds.length>0){
                let a= document.createElement('a');
                a.target= '_blank';
                a.href= basePath + "FichaSintomatologica/DownloadPdfReporteMultile?env_ids=" + arrayIds;
                a.click();
                // window.location.href = basePath + "FichaSintomatologica/DownloadPdfReporteMultile?env_ids=" + arrayIds;
            }
            else{
                messageResponse({
                    text: "Debe seleccionar al menos un registro",
                    type: "error"
                })
                return false;
            }
        })
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
        $(document).on("click", ".chk_all", function (e) {
            $('#fichasListado').find('tbody>tr>td :checkbox')
                .prop('checked', this.checked)
                .closest('tr').toggleClass('selected', this.checked);
        })

        $(document).on("click", "#fichasListado>tbody>tr>td :checkbox", function (e) {
            $(this).closest('tr').toggleClass('selected', this.checked); //Classe de seleção na row
            $('.chk_all').prop('checked', ($(this).closest('table').find('tbody>tr>td :checkbox:checked').length == $(this).closest('table').find('tbody>tr>td :checkbox').length)); //Tira / coloca a seleção no .checkAll
        });
    
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
        // __ListardatosFicha: function (usuario_id) {
        //     _ListardatosFicha(usuario_id);
        // },
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