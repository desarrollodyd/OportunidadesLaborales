var OfertaLaboralVista = function () {

    var _inicio = function () {
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);
        selectResponse({
            url: "Ubigeo/UbigeoListarPaisesJson",
            select: "cboPais",
            campoID: "ubi_pais_id",
            CampoValor: "ubi_nombre",
            select2: true,
            allOption: false,
            placeholder: "PAIS"
        });
    };

    var _ListarOfertas = function () {
        var dataForm = $('#frmOfertaLaboral-form').serializeFormJSON();
        responseSimple({
            url: "OfertaLaboral/OfertaLaboralListarJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                var data = response.data;
                var respuesta = response.respuesta;
                $("#ofertasContenido").html("");
                if (respuesta) {
                    console.log(data);
                    $.each(data, function (index, value) {
                        $("#ofertasContenido").append('<div class="col-md-4 col-sm-4 col-xs-12 profile_details">'+
                                                            '<div class="well profile_view">'+
                                                               '<div class="col-md-12 col-sm-12 col-xs-12" style="text-align: center;">'+
                                                                    '<h3 class="brief" style="margin: 0px !important;"><i>'+value.ola_nombre+'</i></h3>'+
                                                                    '<h6 style="margin: 0px !important;">direccion</h6>'+
                                                                    '<div class=" col-xs-12" style="padding-bottom: 10px;">'+
                                                                        '<div class="ln_solid" style="margin-top: 10px !important;"></div>'+
                            '<button type="button" class="btn btn-primary btn-sm" style="font-size: 15px !important;">  Detalle </button>' +
                            '<button type="button" id=postular' + value.ola_id + ' data-id=' + value.ola_id + ' class="btn btn-success btn-sm btn_postular" style="font-size: 15px !important;"> Postula</button>' +
                                                                    '</div>'+
                                                                '</div>'+
                                                                '<div class="col-xs-12 bottom text-center">'+
                                                                     '<div class="cold-md-12 col-xs-12 col-sm-12 emphasis">'+
                                                                          '<p class="ratings" style="text-align: center;">'+
                                                                              '<a>Publicado hace 4 dias</a>'+
                                                                              '<a href="#" style="float: right;"><span class="fa fa-star-o"></span></a>'+
                                                                          '</p>'+
                                                                     '</div>'+
                                                                '</div>'+
                                                            '</div>'+
                                                      '</div >');
                    });

                    if (data.length == 0) {
                        CloseMessages();
                        messageResponse({
                            text: "No se Encontraron Ofertas",
                            type: "warning"
                        });
                    }
                }
            }
        });
        /*Postular a ofertas laborales*/
        $(document).on("click", ".btn_postular", function (e) {
           var data = { fk_oferta_laboral:$(this).data("id") };
            responseSimple({
                url: "Postulante/PostulanteMigrarDataJson",
                data: JSON.stringify(data),
                refresh: true,
                callBackSuccess: function (response) {
                    console.log(response);
                }
                });
        });
        /*Fin de Postulacion*/

    };
    var _componentes = function () {

        $(document).on("click", ".btn_filtrar", function (e) {
            $("#frmOfertaLaboral-form").submit();
            if (_objetoForm_frmOfertaLaboral.valid()) {
                OfertaLaboralVista.__ListarOfertas();
            } else {
                messageResponse({
                    text: "Complete los campos Obligatorios",
                    type: "error"
                })
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
                allOption: false,
                placeholder:"DEPARTAMENTO"
            });

            $("#cboProvincia").html('<option value="">PROVINCIA</option>');
            if ($('#cboProvincia').hasClass('select2-hidden-accessible')) {
                $('#cboProvincia').select2('destroy');
            }
            $("#cboDistrito").html('<option value="">DISTRITO</option>');
            if ($('#cboDistrito').hasClass('select2-hidden-accessible')) {
                $('#cboDistrito').select2('destroy');
            }
            if (ubi_id_pais != "") {
                $("#cboPais").rules('remove', 'required');
                $("#cboDepartamento").rules('remove', 'required');
                $("#cboProvincia").rules('remove', 'required');
                $("#cboDistrito").rules('remove', 'required');
            }
          
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
                allOption: false,
                placeholder: "PROVINCIA"
            });
            $("#cboDistrito").html('<option value="">DISTRITO</option>');
            if ($('#cboDistrito').hasClass('select2-hidden-accessible')) {
                $('#cboDistrito').select2('destroy');
            }
            if (ubi_departamento_id != "") {
                $("#cboPais").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboDepartamento").rules('remove', 'required');
                $("#cboProvincia").rules('remove', 'required');
                $("#cboDistrito").rules('remove', 'required');
            }
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
                allOption: false,
                placeholder: "DISTRITO"
            });
            if (ubi_provincia_id != "") {
                $("#cboPais").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboDepartamento").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboProvincia").rules('remove', 'required');
                $("#cboDistrito").rules('remove', 'required');
            }
          
        });

        $("#cboDistrito").change(function () {
            var ubi_distrito_id = $("#cboDistrito option:selected").val();
            if (ubi_distrito_id != "") {
                $("#cboPais").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboDepartamento").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboProvincia").rules('add', {
                    required: true,
                    messages: {
                        required: "Pais Obligatorio"
                    }
                });
                $("#cboDistrito").rules('remove', 'required');
            }
        });
    };

    var _metodos = function () {
        validar_Form({
            nameVariable: 'frmOfertaLaboral',
            contenedor: '#frmOfertaLaboral-form',
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
            _ListarOfertas();
            _metodos();
        },
        __ListarOfertas: function () {
            _ListarOfertas();
        },
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    OfertaLaboralVista.init();
});