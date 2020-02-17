var PanelToken = function () {
    var listaUsuariosPorsistema;
    var listaSistemas;
    var sistemas;
    var listaUsuariosPostgres;
    var _ListarUsuarios = function () {
        $("#bloque_usuarios_postgres").show();
        $("#bloque_usuarios_sistemas").hide();
        $("#bloque_usuarios_sistemas").html("");
        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        responseSimple({
            url: "IntranetToken/IntranetTokenListarUsuariosJson",
            refresh: false,
            callBackSuccess: function (response) {
                if(response.respuesta){
                    listaSistemas=response.data_sistemas;
                    listaUsuariosPostgres=response.data;
                    simpleDataTable({
                        uniform: false,
                        tableNameVariable: "datatable_usuariosListado",
                        table: "#usuariosListado",
                        tableColumnsData: response.data,
                        tableHeaderCheck: true,
                        tableHeaderCheckIndex: 0,
                        headerCheck: "chk_usuarios",
                        tableColumns: [
                            {
                                data: "usu_id",
                                title: "",
                                "bSortable": false,
                                className: 'align-center',
                                "render": function (value) {
                                    var check = '<input type="checkbox" class="form-check-input-styled-info usuariosListado" data-id="' + value + '" name="chk[]">';
                                    return check;
                                },
                                width: "50px",
                            },
                            {
                                data: "usu_id",
                                title: "Id Usuario",
                                width: "80px",
                            },
                            {
                                data: "usu_id",
                                title: "Nombre",
                                "render":function(value,type,row){
                                    var span=''+row.per_apellido_pat+' '+row.per_apellido_mat+', '+row.per_nombre;
                                    return span.toUpperCase();
                                },
                                width: "300px",
                            },
                            {
                                data: "usu_nombre",
                                title: "Usuario",
                                width: "150px",
                            },
                            {
                                data: "usu_token",
                                title: "Token",
                                width: "535px",
                            },
                        ]
                    })
                }
            }
        });
    };
    var _componentes = function () {
        $(document).on('click','#btn_sincronizar',function(){

            
            let arrayDNIs = [];
            if(listaUsuariosPostgres.length>0){
                
                $.each(listaUsuariosPostgres,function(index,value){
                    arrayDNIs.push(value.per_numdoc);
                })
            }
            var dataForm={
                listaSistemas:listaSistemas,
                listaDNIs:arrayDNIs
            };
          
            responseSimple({
                url:"IntranetToken/IntranetListarUsuariosSistemasJson",
                refresh:false,
                data:JSON.stringify(dataForm),
                callBackSuccess:function(response){
                    var span='';
                    listaUsuariosPorsistema=response.data;
                    sistemas=response.data;
                    if(sistemas.length>0){
                        span+=' <div class="widget-header widget-header-flat widget-header-small">'+
                        '<h4 class="widget-title">Listado Usuarios por Sistemas</h4>'+
                        '<span class="widget-toolbar">'+
                            '<a href="#" class="btn btn-sm btn-warning btn-minier" data-action="settings" id="btn_editar_token"><i class="ace-icon fa fa-file"></i> Modificar Tokens</a>'+
                        '</span>'+
                    '</div>';
                        span+='<div class="panel panel-default">';
                        $.each(sistemas,function(index,value){
                            // var lista=value;
                            // lista=lista.shift();   
                            // console.log(lista);
                            span+='<div class="panel-heading"><h4 class="panel-title">'+
                            '<a class="accordion-toggle collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapse'+index+'" aria-expanded="false">'+
                                    '<i class="bigger-110 ace-icon fa fa-angle-right" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>'+
                                    'Listado Sistema '+value[0].sist_nombre+
                                '</a>'+
                            '</h4>'+
                        '</div>'+
                        '<div class="panel-collapse collapse" id="collapse'+index+'" aria-expanded="false" style="height: 0px;">'+
                            '<div class="panel-body" id="panel'+index+'"><table id="table'+index+'" class="table table-condensed table-striped table-bordered table-hover datatableListado'+index+'"></table>'+
                            '</div>'+
                        '</div>';
                       
                            //crear acordeon por sistema
                           
                            //primera Lista
                        
                        })
                        span+='</div>';
                        $("#bloque_usuarios_sistemas").show();
                        $("#bloque_usuarios_postgres").hide();
                        $("#bloque_usuarios_sistemas").html(span);
                        _crearDatatable(sistemas);
                    }
                    else{
                        messageResponse({
                            text: "No hay Datos",
                            type: "error"
                        })
                    }
                }
            })
        });

        $(document).on('click','#btn_editar_token',function(){
            // console.log(listaSistemas);
            // console.log(listaUsuariosPostgres);
            // console.log(listaUsuariosPorsistema);
            let arrayTokens = [];
            if(listaUsuariosPostgres.length>0){
                var obj="";
                $.each(listaUsuariosPostgres,function(index,value){
                    obj={
                        per_numdoc:value.per_numdoc,
                        per_token:value.token
                    };
                    arrayTokens.push(obj);
                })
            }
            var dataForm={
                listaSistemas:listaSistemas,
                listaTokens:arrayTokens,
            }
            responseSimple({
                url:'IntranetToken/IntranetModificarTokensporSistemaJson',
                data:JSON.stringify(dataForm),
                refresh:false,
                callBackSuccess:function(response){
                    console.log(response);
                }
            })
        });
    };
    var _crearDatatable=function(data){
        $.each(data,function(index,value){
       
            lista=value;
            lista.shift();
            simpleDataTable({
                uniform: false,
                table: "#table"+index,
                tableNameVariable: "datatableListado"+index,
                tableColumnsData: lista,
                tableHeaderCheck: false,
                tableColumns: [
                    {
                        data: "NombreEmpleado",
                        title: "Empleado",
                        width:"300px"
                    },
                    {
                        data: "UsuarioNombre",
                        title: "Usuario",
                        width:"300px"
                    },
                    {
                        data: "Token",
                        title: "Token",
                        width: "300px",
                    },
                ]
            })
        })
    };
    var _metodos = function () {

    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _ListarUsuarios();
            _componentes();
            _metodos();

        },
        init_ListarUsuarios: function () {
            _ListarUsuarios();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelToken.init();
});