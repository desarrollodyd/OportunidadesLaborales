var EducacionBasicaVista = function () {
    var _inicio = function () {
        $("[name='per_id']").val(persona.per_id);
        $("[name='pos_id']").val(postulante.pos_id);
        $("[name='fk_postulante']").val(postulante.pos_id);
        $("#persona_nombre").text(persona.per_nombre + " " + persona.per_apellido_pat + " " + persona.per_apellido_mat);
        $("#perfil_principal").attr("src", "data:image/gif;base64," + rutaImage);
        $("#img_layout_post").attr("src", "data:image/gif;base64," + rutaImage);
        $("#pos_nombre_referido").val(postulante.pos_nombre_referido);

        if (postulante.pos_cv != "") {
            $("#cvnombre").text(postulante.pos_cv);
            $("#cvfecha").text(moment(postulante.pos_fecha_act).format("DD-MM-YYYY HH:MM:SS A"));
            $("#divCV").show();
        }

        if (postulante.pos_referido == "") {
            $('#cboReferido').bootstrapToggle('off');
            $("#pos_referido").val(false);
        }
        else {
            if (postulante.pos_referido == true) {
                $('#cboReferido').bootstrapToggle('on');
                $("#pos_referido").val(true);
            }
            else {
                $('#cboReferido').bootstrapToggle('off');
                $("#pos_referido").val(false);
            }
        }

    };

    var downloadArchivo = function () {

        var xhr = new XMLHttpRequest();
        xhr.open('POST', basePath + 'Postulante/DescargarArchivo', true);
        xhr.responseType = 'arraybuffer';
        xhr.onload = function () {
            if (this.status === 200) {
                var filename = "";
                var disposition = xhr.getResponseHeader('Content-Disposition');
                if (disposition === null) {
                    messageResponse({
                        text: "No tiene Permisos",
                        type: "error"
                    });
                }
                if (disposition && disposition.indexOf('attachment') !== -1) {
                    var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                    var matches = filenameRegex.exec(disposition);
                    if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
                }
                var type = xhr.getResponseHeader('Content-Type');

                var blob = new Blob([this.response], { type: type });
                if (typeof window.navigator.msSaveBlob !== 'undefined') {
                    // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                    window.navigator.msSaveBlob(blob, filename);
                }
                else {
                    var URL = window.URL || window.webkitURL;
                    var downloadUrl = URL.createObjectURL(blob);

                    if (filename) {
                        // use HTML5 a[download] attribute to specify filename
                        var a = document.createElement("a");
                        // safari doesn't support this yet
                        if (typeof a.download === 'undefined') {
                            window.location = downloadUrl;
                        }
                        else {


                            a.href = downloadUrl;
                            a.download = filename;
                            document.body.appendChild(a);
                            a.click();
                        }
                    }
                    else {
                        window.location = downloadUrl;
                    }
                    setTimeout(function () { URL.revokeObjectURL(downloadUrl); }, 100); // cleanup
                }
            }
            else {
                aaaa = this;
                messageResponse({
                    text: "Error al Descargar",
                    type: "error"
                });

                // Handle Error Here
            }
        };
        xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
        xhr.send();


    };

    var _componentes = function () {

        $("#frm_cv").on("click", function () {
            EducacionBasicaVista.DescargaArchivo();
        });

        $("#pos_cv").change(function () {
            var _image = $('#pos_cv')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                console.log(extension);
                if (extension != "pdf" && extension != "doc" && extension != "docx") {
                    messageResponse({
                        text: 'Solo Se Permite formato word o pdf',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 8);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "doc" || actualicon == "docx") {
                        icon = '<i class="fa fa-file-word-o"></i>';
                    }
                    else {
                        icon = '<i class="fa fa-file-pdf-o"></i>';
                    };
                    $("#spancv").html("");
                    $("#spancv").append(icon + " " + nombre + "...");
                }
            }
            else {
                $("#spancv").html("");
                $("#spancv").append('<i class="fa fa-upload"></i>  Subir CV');
            }
        });

        $("#cboReferido").change(function () {
            var check = $(this).prop('checked');
            if (check) {
                $("#pos_referido").val(true);
                _objetoForm_frmInformacionAdicional.valid();
                $("#pos_nombre_referido").rules('add', {
                    required: true,
                    messages: {
                        required: "Referido Obligatorio"
                    }
                });
                $("#pos_nombre_referido").prop("disabled", false);
                $("#pos_nombre_referido").val(postulante.pos_nombre_referido);
                $("#pos_cv").rules('remove', 'required');
            }
            else {
                $("#pos_referido").val(false);
                $("#pos_nombre_referido").prop("disabled", true);
                $("#pos_nombre_referido").rules('remove', 'required');
                $("#pos_nombre_referido").val("");
                $("#pos_cv").rules('add', {
                    required: true,
                    messages: {
                        required: "Subir CV Obligatorio"
                    }
                });
            }
        });

        $(document).on("click", ".btn_guardar", function (e) {
            $("#frmInformacionAdicional-form").submit();
            if (_objetoForm_frmInformacionAdicional.valid()) {
                var dataForm = new FormData(document.getElementById("frmInformacionAdicional-form"));
                responseFileSimple({
                    url: "Postulante/PostulanteInsertarInformacionAdicionalJson",
                    data: dataForm,
                    refresh: false,
                    callBackSuccess: function (response) {
                        var respuesta = response.respuesta;
                        if (respuesta) {
                            $("#spancv").html("");
                            $("#spancv").append('<i class="fa fa-upload"></i>  Subir CV');
                        }
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
            _objetoForm_frmInformacionAdicional.resetForm();
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
            nameVariable: 'frmInformacionAdicional',
            contenedor: '#frmInformacionAdicional-form',
            rules: {
                pos_referido:
                {
                    required: true,

                },

            },
            messages: {
                pos_referido:
                {
                    required: 'Referido Obligatorio',
                },
 
            }
        });

        if (postulante.pos_referido) {
            $("#pos_cv").rules('remove', 'required');
            $("#pos_nombre_referido").prop("disabled", false);
        }
        else {
            $("#pos_nombre_referido").prop("disabled", true);
            $("#pos_cv").rules('add', {
                required: true,
                messages: {
                    required: "Subir CV Obligatorio"
                }
            });
        }
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
        DescargaArchivo: function () {
            downloadArchivo();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    EducacionBasicaVista.init();
});