var DatosPersonaVista = function () {
    var _inicio = function () {
        $("[name='per_id']").val(persona.per_id);
        $("[name='pos_id']").val(postulante.pos_id);
        $("[name='fk_postulante']").val(postulante.pos_id);
        //Datos de persona

        $("#per_correoelectronico").val(persona.per_correoelectronico);
        $("#per_nombre").val(persona.per_nombre);
        $("#per_apellido_pat").val(persona.per_apellido_pat);
        $("#per_apellido_mat").val(persona.per_apellido_mat);
        $("#per_numdoc").val(persona.per_numdoc);

        $('#per_fechanacimiento').datetimepicker({
            format: 'DD/MM/YYYY',
            ignoreReadonly: true,
            allowInputToggle: true
        });
        $("#per_fechanacimiento").val(moment(persona.per_fechanacimiento).format('DD/MM/YYYY'));
        $("#per_telefono").val(persona.per_telefono);
        $("#cboSexo").val(persona.per_sexo);
        $("#cbotipoDocumento").val(persona.per_tipodoc);
        $("#pos_celular").val(postulante.pos_celular);
        $("#cbotipoDireccion").val(postulante.pos_tipo_direccion);
        $("#pos_direccion").val(postulante.pos_direccion);
        $("#cbotipoCalle").val(postulante.pos_tipo_calle);
        $("#pos_numero_casa").val(postulante.pos_numero_casa);
        $("#cbotipoCasa").val(postulante.pos_tipo_casa);
        $("#cboestadoCivil").val(postulante.pos_estado_civil);
        $("#cboBrevete").val(String(postulante.pos_brevete));
        $("#pos_num_brevete").val(postulante.pos_num_brevete);
        $("#pos_estado").val(postulante.pos_estado);   
        $("#perfil_principal").attr("src", "data:image/gif;base64," + rutaImage);
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);

        selectResponse({
            url: "Ubigeo/UbigeoListarPaisesJson",
            select: "cboPais",
            campoID: "ubi_pais_id",
            CampoValor: "ubi_nombre",
            selectVal: ubigeo.ubi_pais_id,
            select2: true,
            allOption: false
        });

        if (persona.fk_ubigeo != 0) {
            
        }
        

    };
    var _componentes = function () {

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

        $(document).on("click", ".btn_guardar", function (e) {
            $("#frmDatosPersonales-form").submit();
            if (_objetoForm_frmDatosPersonales.valid()) {
                var dataForm = $('#frmDatosPersonales-form').serializeFormJSON();
                responseSimple({
                    url: "Persona/PersonaEditarJson",
                    data: JSON.stringify(dataForm),
                    refresh: false,
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
            console.log(dataForm, "aaaa");
            dataForm.append('postulanteID', $("#pos_id").val());
            responseFileSimple({
                url: "Postulante/PostulanteSubirFotoJson",
                data: dataForm,
                refresh: false,
                callBackSuccess: function (response) {
                    var respuesta = response.respuesta;
                    if (respuesta) {
                        readImage(_image, "#perfil_principal");
                        readImage(_image, "#img_layout_post");
                    }
                }
            });
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
                per_celular:
                {
                    required: true,

                },
                per_telefono:
                {
                    required: true,

                },
                per_sexo:
                {
                    required: true,
                },
                ubi_distrito_id: {
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
                    digits: 'Solo Numeros',
                },
                per_fechanacimiento:
                {
                    required: 'Fecha Nacimiento Obligatorio',
                },
                per_celular:
                {
                    required: 'Nro. Celular Obligatorio',
                },
                per_telefono:
                {
                    required: 'Nro. Telefono Obligatorio',
                },
                per_sexo:
                {
                    required: '',
                },
                ubi_distrito_id: {
                    required: 'Distrito Obligatorio'
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

        },
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    DatosPersonaVista.init();
});