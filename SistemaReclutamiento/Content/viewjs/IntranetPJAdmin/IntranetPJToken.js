var PanelToken = function () {
    var listaSistemas;
    var sistemas;
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
                            },
                            {
                                data: "usu_id",
                                title: "Nombre",
                                "render":function(value,type,row){
                                    var span=''+row.per_apellido_pat+' '+row.per_apellido_mat+', '+row.per_nombre;
                                    return span.toUpperCase();
                                }
                            },
                            {
                                data: "usu_nombre",
                                title: "Usuario",
                            },
                            {
                                data: "usu_token",
                                title: "Token",
                            },
                        ]
                    })
                }
            }
        });
    };
    var _componentes = function () {
        $(document).on('click','#btn_sincronizar',function(){
            var dataForm=listaSistemas;
            responseSimple({
                url:"IntranetToken/IntranetListarUsuariosSistemasJson",
                refresh:false,
                data:JSON.stringify(dataForm),
                callBackSuccess:function(response){
                    console.log(response.data);
                    var span='';
                    sistemas=response.data;
                    if(sistemas.length>0){
                        span+='<div class="panel panel-default">';
                        $.each(sistemas,function(index,value){

                               
                            span+='<div class="panel-heading"><h4 class="panel-title">'+
                            '<a class="accordion-toggle collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapse'+index+'" aria-expanded="false">'+
                                    '<i class="bigger-110 ace-icon fa fa-angle-right" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>'+
                                    'Listado Sistema '+value[0].sist_nombre+
                                '</a>'+
                            '</h4>'+
                        '</div>'+
                        '<div class="panel-collapse collapse" id="collapse'+index+'" aria-expanded="false" style="height: 0px;">'+
                            '<div class="panel-body">'+
                                'Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident.'+
                            '</div>'+
                        '</div>';
                       
                            //crear acordeon por sistema
                           
                            //primera Lista
                            $.each(value,function(i,val){
                          
                            })
                        })
                        span+='</div>';
                        $("#bloque_usuarios_sistemas").show();
                        $("#bloque_usuarios_postgres").hide();
                        $("#bloque_usuarios_sistemas").html(span);
                    }
                    else{
                        messageResponse({
                            text: "No hay Datos",
                            type: "error"
                        })
                    }
                }
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