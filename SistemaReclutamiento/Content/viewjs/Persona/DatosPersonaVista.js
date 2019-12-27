var DatosPersonaVista = function () {
    var _llenarPorcentaje = function () {
        responseSimple({
            url: "Postulante/PostulanteObtenerPorcentajeAvanceJson",
            refresh: false,
            callBackSuccess: function (response) {
                CloseMessages();
                console.log(response);
                $('#porcentajeProgreso').css({ 'width': response.data + '%' });
                $('.progress_wide>span>i').html("")
                $('.progress_wide>span>i').append(response.data + "%")
            }
        });

        $('select option').each(function () {
            $(this).text($(this).text().toUpperCase());
        });
        $("#cboPais").prop("disabled", true);
    }
    var _inicio = function () {
        //$('#porcentajeProgreso').css('width', 80 + '%');
        /*Selects*/
      
        /*Selects*/
        $("[name='per_id']").val(persona.per_id);
        $("[name='pos_id']").val(postulante.pos_id);
        $("[name='fk_postulante']").val(postulante.pos_id);

        $("#persona_nombre").text(persona.per_nombre + " " + persona.per_apellido_pat + " " + persona.per_apellido_mat);
        //Datos de persona
        $("#per_correoelectronico").val(persona.per_correoelectronico);
        $("#per_nombre").val(persona.per_nombre);
        $("#per_apellido_pat").val(persona.per_apellido_pat);
        $("#per_apellido_mat").val(persona.per_apellido_mat);
        $("#per_numdoc").val(persona.per_numdoc);
        $('#per_fechanacimiento').datetimepicker({
            format: 'YYYY/MM/DD',
            ignoreReadonly: true,
            allowInputToggle: true
        });
        var fecha = moment(persona.per_fechanacimiento).format('YYYY/MM/DD');
        console.log(fecha);
        if (fecha != "0000/12/31") {
            $("#per_fechanacimiento").val(moment(persona.per_fechanacimiento).format('YYYY/MM/DD'));
        }
        $("#per_telefono").val(persona.per_telefono);

        if (persona.per_sexo == "") {
            $('#cboSexo').bootstrapToggle('on');
            $("#per_sexo").val("M");
        }
        else{
            if (persona.per_sexo == "M") {
                $('#cboSexo').bootstrapToggle('on');
                $("#per_sexo").val("M");
            }
            else {
                $('#cboSexo').bootstrapToggle('off');
                $("#per_sexo").val("F");
            }
        }
        
        $("#cboSexo").val(persona.per_sexo);
        $("#cbotipoDocumento").val(persona.per_tipodoc);
        $("#pos_celular").val(postulante.pos_celular);
        $("#cbocondicionViv").val(postulante.pos_condicion_viv.toUpperCase());
        $("#pos_direccion").val(postulante.pos_direccion);
        $("#pos_url_perfil").val(postulante.pos_url_perfil);
        $("#cbotipoCalle").val(postulante.pos_tipo_calle.toUpperCase());
        $("#pos_numero_casa").val(postulante.pos_numero_casa);
        $("#cbotipoCasa").val(postulante.pos_tipo_casa.toUpperCase());
        $("#cboestadoCivil").val(postulante.pos_estado_civil.toUpperCase());
        $("#cboBrevete").val(String(postulante.pos_brevete));
        $('#cbotipoDocumento').select2();
        $('#cboNacionalidad').select2();
        $('#cboestadoCivil').select2();
        $('#cbotipoCalle').select2();
        $('#cbocondicionViv').select2();
        $('#cbotipoCasa').select2();
        if (postulante.pos_brevete == "") {
            $('#cboBrevete').bootstrapToggle('off');
            $("#pos_brevete").val("false");
            $('#pos_num_brevete').prop('disabled', true);
        }
        else {
            if (postulante.pos_brevete == true) {
                $('#cboBrevete').bootstrapToggle('on');
                $("#pos_brevete").val(true);
            }
            else {
                $('#cboBrevete').bootstrapToggle('off');
                $("#pos_brevete").val(false);
                $('#pos_num_brevete').prop('disabled', true);
            }
        }

        $("#pos_num_brevete").val(postulante.pos_num_brevete);
        $("#pos_estado").val(postulante.pos_estado);   
        $("#perfil_principal").attr("src", "data:image/gif;base64," + rutaImage);
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);

        if (postulante.fk_nacionalidad > 0) {
            var id = { ubi_id: postulante.fk_nacionalidad };
            responseSimple({
                url: "Ubigeo/UbigeoObtenerDatosporIdJson",
                data: JSON.stringify(id),
                refresh: false,
                callBackSuccess: function (response) {
                    var data = response.data;
                    var mensaje = response.mensaje;
                    selectResponse({
                        url: "Ubigeo/UbigeoListarTodoslosPaisesJson",
                        select: "cboNacionalidad",
                        campoID: "ubi_pais_id",
                        CampoValor: "ubi_nombre",
                        selectVal: data.ubi_pais_id,
                        select2: true,
                        allOption: false
                    });
                    CloseMessages(); 
                }
            });
         
        }
        else {
            selectResponse({
                url: "Ubigeo/UbigeoListarTodoslosPaisesJson",
                select: "cboNacionalidad",
                campoID: "ubi_pais_id",
                CampoValor: "ubi_nombre",
                select2: true,
                allOption: false
            });
        }

        if (ubigeo.ubi_id > 0) {
            selectResponse({
                url: "Ubigeo/UbigeoListarPaisPeruJson",
                select: "cboPais",
                campoID: "ubi_pais_id",
                CampoValor: "ubi_nombre",
                selectVal: ubigeo.ubi_pais_id,
                select2: true,
                allOption: false,
                disabled:false,
            });
            selectResponse({
                url: "Ubigeo/UbigeoListarDepartamentosporPaisJson",
                select: "cboDepartamento",
                data: { ubi_pais_id: ubigeo.ubi_pais_id },
                campoID: "ubi_departamento_id",
                CampoValor: "ubi_nombre",
                selectVal: ubigeo.ubi_departamento_id,
                select2: true,
                allOption: false
            });

            selectResponse({
                url: "Ubigeo/UbigeoListarProvinciasporDepartamentoJson",
                select: "cboProvincia",
                data: { ubi_pais_id: ubigeo.ubi_pais_id, ubi_departamento_id: ubigeo.ubi_departamento_id },
                campoID: "ubi_provincia_id",
                CampoValor: "ubi_nombre",
                selectVal: ubigeo.ubi_provincia_id,
                select2: true,
                allOption: false
            });

            selectResponse({
                url: "Ubigeo/UbigeoListarDistritosporProvinciaJson",
                select: "cboDistrito",
                data: { ubi_pais_id: ubigeo.ubi_pais_id, ubi_departamento_id: ubigeo.ubi_departamento_id, ubi_provincia_id: ubigeo.ubi_provincia_id },
                campoID: "ubi_distrito_id",
                CampoValor: "ubi_nombre",
                selectVal: ubigeo.ubi_distrito_id,
                select2: true,
                allOption: false
            });
        }
        else {
            selectResponse({
                url: "Ubigeo/UbigeoListarPaisPeruJson",
                select: "cboPais",
                campoID: "ubi_pais_id",
                CampoValor: "ubi_nombre",
                select2: true,
                allOption: false,
                disabled:false,
            });
        }
    };
    var _componentes = function () {

        $("#cboSexo").change(function () {
            var check = $(this).prop('checked');
            if (check) {
                $("#per_sexo").val("M");
            }
            else {
                $("#per_sexo").val("F");
            }
        });

        $("#cboBrevete").change(function () {
            var check = $(this).prop('checked');
            if (check) {
                $("#pos_brevete").val(true);
                _objetoForm_frmDatosPersonales.valid();
                $("#pos_num_brevete").rules('add', {
                    required: true,
                    messages: {
                        required: "Nro. de Brevete Obligatorio"
                    }
                });
                $("#pos_num_brevete").prop("disabled", false);
                $("#pos_num_brevete").val(postulante.pos_num_brevete);
            }
            else {
                $("#pos_brevete").val(false);
                $("#pos_num_brevete").prop("disabled", true);
                $("#pos_num_brevete").rules('remove', 'required');
                $("#pos_num_brevete").val("");               
            }
        });

        $("#cboPais").change(function () {
            var ubi_id_pais = $("#cboPais option:selected").val();
            selectResponse({
                url: "Ubigeo/UbigeoListarDepartamentosporPaisJson",
                select: "cboDepartamento",
                data: { ubi_pais_id: ubi_id_pais },
                campoID: "ubi_departamento_id",
                CampoValor: "ubi_nombre",
                select2: true,
                allOption: false
            });
        });

        $("#cboDepartamento").change(function () {
            var ubi_pais_id = $("#cboPais option:selected").val();
            var ubi_departamento_id = $("#cboDepartamento option:selected").val();
            selectResponse({
                url: "Ubigeo/UbigeoListarProvinciasporDepartamentoJson",
                select: "cboProvincia",
                data: { ubi_pais_id: ubi_pais_id, ubi_departamento_id: ubi_departamento_id },
                campoID: "ubi_provincia_id",
                CampoValor: "ubi_nombre",
                select2: true,
                allOption: false
            });

        });

        $("#cboProvincia").change(function () {
            var ubi_pais_id = $("#cboPais option:selected").val();
            var ubi_departamento_id = $("#cboDepartamento option:selected").val();
            var ubi_provincia_id = $("#cboProvincia option:selected").val();

            selectResponse({
                url: "Ubigeo/UbigeoListarDistritosporProvinciaJson",
                select: "cboDistrito",
                data: { ubi_pais_id: ubi_pais_id, ubi_departamento_id: ubi_departamento_id, ubi_provincia_id: ubi_provincia_id },
                campoID: "ubi_distrito_id",
                CampoValor: "ubi_nombre",
                select2: true,
                allOption: false
            });
        });

        $(document).on("click", ".btn_guardar", function (e) {
            $("#frmDatosPersonales-form").submit();
            if (_objetoForm_frmDatosPersonales.valid()) {
                var dataForm = $('#frmDatosPersonales-form').serializeFormJSON();
                responseSimple({
                    url: "Persona/PersonaEditarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
                    callBackSuccess: function () {
                        CloseMessages();
                        _llenarPorcentaje();
                    }
                });
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
            }
        });

        $(document).on("click", ".btn_cancelar", function (e) {
            _objetoForm_frmDatosPersonales.resetForm();
        });

        $('#subir-img-perfil').change(function () {
            var dataForm = new FormData();
            
            var _image = $('#subir-img-perfil')[0].files[0];
            dataForm.append('file', _image);
            dataForm.append('postulanteID', $("#pos_id").val());
            responseFileSimple({
                url: "Postulante/PostulanteSubirFotoJson",
                data: dataForm,
                refresh: false,
                callBackSuccess: function (response) {
                    _llenarPorcentaje();
                    var respuesta = response.respuesta;
                    if (respuesta) {
                        _llenarPorcentaje();
                        readImage(_image, "#perfil_principal");
                        readImage(_image, "#img_layout_post");
                    }
                }
            });
        });
        //Validaciones para numeros y letras

        $("#per_numdoc,#pos_num_brevete,#pos_numero_casa,#pos_celular,#per_telefono").bind('keypress', function (event) {
            var regex = new RegExp("^[0-9]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });   
        $("#per_nombre,#per_apellido_pat,#per_apellido_mat").bind('keypress', function (event) {
            var regex = new RegExp("^[a-zA-Z ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });      
    };
    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmDatosPersonales',
            contenedor: '#frmDatosPersonales-form',
            rules: {
                per_nombre:
                {
                    required: true,

                },
                per_apellido_pat:
                {
                    required: true,

                },
                per_apellido_mat:
                {
                    required: true,
                },
                per_numdoc:
                {
                    required: true,
                    digits: true
                },
                per_fechanacimiento:
                {
                    required: true,

                },
                pos_celular:
                {
                    required: true,
                },            
                per_sexo:
                {
                    required: true,
                },
                ubi_pais_id: {
                    required: true
                },
                ubi_departamento_id: {
                    required: true
                },
                ubi_provincia_id: {
                    required: true
                },
                ubi_distrito_id: {
                    required: true
                },
                fk_nacionalidad:{
                    required: true
                },
                pos_estado_civil: {
                    required: true
                },
                pos_tipo_casa: {
                    required: true
                },
                pos_tipo_calle: {
                    required: true
                },
                pos_condicion_viv: {
                    required: true
                },
                pos_direccion: {
                    required:true
                },
                per_tipodoc: {
                    required: true
                }

            },
            messages: {
                per_nombre:
                {
                    required: 'Nombre Obligatorio',
                },
                per_apellido_pat:
                {
                    required: 'Apellido Paterno Obligatorio',
                },
                per_apellido_mat:
                {
                    required: 'Apellido Materno Obligatorio',
                },
                per_numdoc:
                {
                    required: 'Nro. Documento Obligatorio',
                    digits: 'Solo Números',
                },
                per_fechanacimiento:
                {
                    required: 'Fecha Nacimiento Obligatorio',
                },
                pos_celular:
                {
                    required: 'Nro. de Celular Obligatorio',
                },             
                per_sexo:
                {
                    required: 'Sexo Obligatorio',
                },
                ubi_pais_id: {
                    required: 'País Obligatorio'
                },
                ubi_departamento_id: {
                    required: 'Departamento Obligatorio'
                },
                ubi_provincia_id: {
                    required: 'Provincia Obligatorio'
                },
                ubi_distrito_id: {
                    required: 'Distrito Obligatorio'
                },
                fk_nacionalidad: {
                    required:'Nacionalidad Obligatoria'
                },
                pos_estado_civil: {
                    required: 'Estado Civil Obligatorio'
                },
                pos_tipo_casa: {
                    required: 'Tipo de Vivienda Obligatorio'
                },
                pos_tipo_calle: {
                    required: 'Tipo de Vía Obligatorio'
                },
                pos_condicion_viv: {
                    required: 'Condicion de Vivienda Obligatorio'
                },
                pos_direccion: {
                    required: 'Direccion Obligatoria'
                },
                per_tipodoc: {
                    required:'Tipo de Documento Obligatorio'
                }
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
            _llenarPorcentaje();
        },
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    DatosPersonaVista.init();
});