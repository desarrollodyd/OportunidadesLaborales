
var PanelDepartamentos = function () {
    var _ListarDepartamentos = function () {
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }

        responseSimple({
            url: "WebDepartamento/WebDepartamentoListarJson",
            refresh: false,
            callBackSuccess: function (response) {
                console.log(response);
                simpleDataTable({
                    uniform: false,
                    tableNameVariable: "datatable-departamentoListado",
                    table: "#departamentosListado",
                    tableColumnsData: response.data,
                    tableColumns: [
                        {
                            data: "dep_id",
                            title: "ID",
                            width: "50px",
                            className: 'align-center',
                        },
                        {
                            data: "dep_nombre",
                            title: "Descripcion",

                        },
                        {
                            data: "dep_imagen",
                            title: "Imagen",
                            "render": function (value) {
                                var img = '';
                                if (value != "") {
                                    img += '<img src="'+basePath+"WebFiles/" + value + '" style="width:50px;height:50px;" />';
                                }
                                return img;
                            },
                        },
                        {
                            data: "dep_imagen_detalle",
                            title: "Detalle",
                            "render": function (value) {
                                var img = '';
                                if (value != "") {
                                    img += '<img src="'+basePath+"WebFiles/" + value + '" style="width:50px;height:50px;" />';
                                }
                                return img;
                            },
                        },
                        {
                            data: "dep_id",
                            title: "Acciones",
                            "render": function (value) {
                                var span = '';
                                var dep_id = value;
                                var span = '<a href="javascript:void(0);"class="btn btn-white btn-success btn-sm btn-round btn_editar" data-id="' + dep_id + '" data-rel="tooltip"title="Editar">Editar</a> <a href="javascript:void(0);"class="btn btn-white btn-danger btn-sm btn-round btn_eliminar" data-id="' + dep_id + '" data-rel="tooltip"title="Eliminar">Eliminar</a>';
                                return span;
                            }
                        }

                    ]
                })
            }
        });
    };
    var _componentes = function () {
        $(document).on('click','#btn_nuevo',function(){
            $("#dep_id").val("0");
            $("#dep_nombre").val("");
            $("#dep_imagen").val("");
            $("#dep_imagen_detalle").val("");
            $("#div_dep_imagen").hide();
            $("#div_dep_imagen_detalle").hide();
            $("#modalFormulario").modal('show');
        });
        $(document).on('click','.btn_editar',function(){
            var dep_id=$(this).data("id");
            var dataForm={
                dep_id:dep_id,
            }
            responseSimple({
                url:"WebDepartamento/WebDepartamentoIdObtenerJson",
                data:JSON.stringify(dataForm),
                refresh:false,
                callBackSuccess:function(response){
                    console.log(response.data)
                    if(response.respuesta){
                        var data=response.data;
                        if(data.dep_imagen!=''){
                            $("#div_dep_imagen").show();
                            $("#dep_imagen_nombre").text(data.dep_imagen);
                            $("#dep_icono_actual").prop('src',basePath+"WebFiles/"+data.dep_imagen);
                        }
                        if(data.dep_imagen_detalle!=''){
                            $("#div_dep_imagen_detalle").show();
                            $("#dep_imagen_detalle_nombre").text(data.dep_imagen);
                            $("#dep_icono_detalle_actual").prop('src',basePath+"WebFiles/"+data.dep_imagen);
                        }
                        $("#dep_id").val(data.dep_id);
                        $("#dep_nombre").val(data.dep_nombre);
                        $("#dep_imagen").val("");
                        $("#dep_imagen_detalle").val("");
                        $("#modalFormulario").modal('show');
                    }
                }
            })
        })
        $(document).on('click','.btn_eliminar',function(){
            var dep_id=$(this).data("id");
            var dataForm={
                dep_id:dep_id,
            }
            responseSimple({
                url:"WebDepartamento/WebDepartamentoEliminarJson",
                data:JSON.stringify(dataForm),
                refresh:false,
                callBackSuccess:function(response){
                    if(response.respuesta){
                        PanelDepartamentos.init_ListarDepartamentos();
                    }
                }
            })
        })
        $(document).on('click','.btn_guardar',function(){
            var dataForm = new FormData(document.getElementById("form_departamentos"));
            var url = "";
            if ($("#dep_id").val() == 0) {
                url = "WebDepartamento/WebDepartamentoInsertarJson";
            }
            else {
                url = "WebDepartamento/WebDepartamentoEditarJson";
            }
            responseFileSimple({
                url: url,
                data: dataForm,
                refresh: false,
                callBackSuccess: function (response) {
                    console.log(response);
                    var respuesta = response.respuesta;
                    if (respuesta) {
                        PanelDepartamentos.init_ListarDepartamentos();
                        $("#modalFormulario").modal("hide");
                    }
                }
            });
        })

        $(document).on("change", "#dep_imagen", function () {
            var _image = $('#dep_imagen')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                if (extension!= "jpg" && extension!= "png") {
                    messageResponse({
                        text: 'Sólo Se Permite formato jpg o png',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 11);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "png" || actualicon == "jpg") {
                        icon = '<i class="fa fa-file-word-o"></i>';
                    }
                    else {
                        icon = '<i class="fa fa-file-pdf-o"></i>';
                    };
                    $("#span_dep_imagen").html("");
                    $("#span_dep_imagen").append(icon + " " + nombre + "... ." + actualicon);
                }
            }
            else {
                $("#span_dep_imagen").html("");
                $("#span_dep_imagen").append('<i class="fa fa-upload"></i>  Subir Icono');
            }
        })
        $(document).on("change", "#dep_imagen_detalle", function () {
            var _image = $('#dep_imagen_detalle')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                if (extension!= "jpg" && extension!= "png") {
                    messageResponse({
                        text: 'Sólo Se Permite formato jpg o png',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 11);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "png" || actualicon == "jpg") {
                        icon = '<i class="fa fa-file-word-o"></i>';
                    }
                    else {
                        icon = '<i class="fa fa-file-pdf-o"></i>';
                    };
                    $("#span_dep_imagen_detalle").html("");
                    $("#span_dep_imagen_detalle").append(icon + " " + nombre + "... ." + actualicon);
                }
            }
            else {
                $("#span_dep_imagen_detalle").html("");
                $("#span_dep_imagen_detalle").append('<i class="fa fa-upload"></i>  Subir Icono');
            }
        })

    };

    var _metodos = function () {
  

    };

 

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarDepartamentos();
            _componentes();
            _metodos();

        },
        init_ListarDepartamentos: function () {
            _ListarDepartamentos();
        },
  
    }
}();
// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelDepartamentos.init();
});