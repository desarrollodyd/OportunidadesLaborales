let panelSeguridad=function(){
    let colors = ['primary', 'warning', 'success', 'danger','pink','inverse','default'];
    let icons = ['cutlery', 'star', 'trophy', 'bug','leaf','beer','flask'];
    let _inicio=function(){
        permisosMenuDis(0)
    }
    let _componentes=function(){
        $(document).on('shown.bs.tab','.nav-tabs a',function(e){
            //   console.log(e.target)
            let tab=$(this).data('tab')
            if(tab=='tab1'){
            }
            else if(tab=='tab2'){
            }
            else if(tab=='tab3'){
                listausuarios()
            }
        })
        $(document).on('change', '#tab3 select', function (event) {
            event.preventDefault()
            let idusuario = jQuery(this).data("usuid");
            let idRol = jQuery(this).val();
            if (idRol != "") {
                let data = { WEB_RolID: idRol, UsuarioID: idusuario }
                let url = basePath + "RolUsuario/GuardarRolUsuario";
                $.ajax({
                    url: url,
                    type: "POST",
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    beforeSend: function () {
                        block_general("body");
                    },
                    success: function (response) {
                         respuesta = response.respuesta;
                        if (respuesta === true) {
                            messageResponse({
                                text: response.mensaje,
                                type: "success"
                            })
                        } else {
                            messageResponse({
                                text: response.mensaje,
                                type: "error"
                            })
                        }
                    },
                    complete: function () {
                        unblock("body");
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                    }
                });
            } else {
                messageResponse({
                    text: "Seleccione un Rol,para Registrar",
                    type: "warning"
                })
            }
        
        });
    }
    let _metodos=function(){

    }
    let permisosMenuDis=function(rol) {
        if (rol != 0) {
            rol = $("#cboRol_").val();
        }
        else {
            rol = rolid;
        }
    
        let data = { rolId: rol }
        let url = basePath + "SeguridadIntranet/ListadoMenusRolId";
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            beforeSend: function () {
                block_general("body");
            },
            complete: function () {
                unblock("body");
            },
            success: function (response) {
                let respuesta = response.dataResultado;
                if (response.mensaje) {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
                if (respuesta) {
                    $("#libody").html("")
                    let menus = [];
                    $.each(respuesta, function (index, value) {
                        menus.push(value.WEB_PMeDataMenu);
                    });
                    $(".cabecera").each(function (i) {
                        let aleatorio = Math.round(Math.random()*6);
                        let total = $(".cabecera").length - 1;
                        let element = $(this);
                        let menu = element.data('menu1');
                        let titulo = element.data('titulo');
                        let existeMenu_ = jQuery.inArray(menu, menus);
                        let check = "";
                        if (existeMenu_ >= 0) {
                            check = "checked";
                        } else {
                            check = "";
                        }
                        $("#libody").append(`
                            <div class="timeline-items">
                                <div class="timeline-item clearfix">
                                    <div class="timeline-info">
                                        <i class="timeline-indicator ace-icon fa fa-${icons[aleatorio]} btn btn-${colors[aleatorio]} no-hover"></i>
                                    </div>
            
                                    <div class="widget-box clearfix">
                                        <div class="widget-body">
                                            <div class="widget-main">
                                               ${titulo}
                                                <div class="pull-right">
                                                    <!-- <i class="ace-icon fa fa-clock-o bigger-110"></i>-->
                                                    <input type="checkbox" data-tit="${titulo}" data-principal="1" value="${menu}"  ${check} name="square-checkbox">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        `)
                        if (total == i) {
                            $("#libody").iCheck({
                                checkboxClass: 'icheckbox_square-blue',
                                radioClass: 'iradio_square-red',
                                increaseArea: '2%' // optional
                            });
                        }
            
                    });
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
    
            }
        })
        
    
    }
    let listausuarios=function() {
        let url = basePath + "Rolusuario/ListadoTableUsuarioAsignarRol";
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({}),
            beforeSend: function () {
                block_general("body");
            },
            complete: function () {
                unblock("body");
            },
            success: function (response) {
                let roles = response.roles
                let usuarios = response.usuarios
                let rolUsuarios = response.rolUsuarios
                console.log(response)
                $("#tableUsuRol").on('page.dt', function () {
                    $('.dataTables_scrollBody').css('height', '250px');
                }).DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "paging": true,
                    "scrollX": false,
                    "sScrollX": "100%",
                    "scrollCollapse": true,
                    "bProcessing": true,
                    "bDeferRender": true,
                    "autoWidth": false,
                    "bAutoWidth": true,
                    "lengthMenu": [[10, 50, 200, -1], [10, 50, 200, "All"]],
                    "pageLength": 10,
                    data: usuarios,
                    columns: [
                        { data: "usu_nombre", title: "Usuario", "width": "250px" },
                        { data: null, title: "Nombre Empleado" ,render: function(value,type,oData){
                                return `${oData.per_apellido_pat} ${oData.per_apellido_mat}, ${oData.per_nombre}` 
                            }
                        },
                        {
                            data: "usu_id", title: "Rol", "width": "150px",
                            "render": function (o) {
                                let selectedOptions = "";
                                selectedOptions += '<option value="">--Seleccione--</option>';
                                $.each(roles, function (keyr, valuer) {
                                    let seleccion = "";
                                    $.each(rolUsuarios, function (key, value2) {
                                        if (value2.UsuarioID == o && value2.WEB_RolID == valuer.WEB_RolID) {
                                            seleccion = "selected";
                                        }
                                    });
    
                                    selectedOptions += '<option ' + seleccion + '  value="' + valuer.WEB_RolID + '">' + valuer.WEB_RolNombre + '</option>';
                                });
    
                                return '<select data-usuid="' + o + '" style="font-weight: bolder;" class="form-control input-sm selectEmp">' + selectedOptions + '</select>';
                            }
                        }
                    ]
                });
    
                $('table').css('width', '100%');
                $('.dataTables_scrollHeadInner').css('width', '100%');
                $('.dataTables_scrollFootInner').css('width', '100%');
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
    
            }
        });
    
    };
    return {
        init: function () {
            _inicio()
            _componentes()
            _metodos()
        }
    }
}()
document.addEventListener('DOMContentLoaded',function(){
    panelSeguridad.init()
})