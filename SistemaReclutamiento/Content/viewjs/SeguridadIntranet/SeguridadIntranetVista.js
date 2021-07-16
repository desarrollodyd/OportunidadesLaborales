let panelSeguridad=function(){
    let colors = ['primary', 'warning', 'success', 'danger','pink','inverse','default']
    let icons = ['cutlery', 'star', 'trophy', 'bug','leaf','beer','flask']
    let _inicio=function(){
        llenarSelectSeguridad(basePath + "RolUsuario/ListadoRolUsuario", {}, "cboRol_", "WEB_RolID", "WEB_RolNombre", rolid)
        permisosMenuDis(0)
    }
    let _componentes=function(){
        $(document).on('shown.bs.tab','.nav-tabs a',function(e){
            //   console.log(e.target)
            e.preventDefault()
            let tab=$(this).data('tab')
            if(tab=='tab1'){
            }
            else if(tab=='tab2'){
                ListadoRolestab()
            }
            else if(tab=='tab3'){
                listausuarios()
            }
        })
        $(document).on('change', '#tab3 select', function (event) {
            event.preventDefault()
            let idusuario = jQuery(this).data("usuid")
            let idRol = jQuery(this).val()
            if (idRol != "") {
                let data = { WEB_RolID: idRol, UsuarioID: idusuario }
                let url = basePath + "RolUsuario/GuardarRolUsuario"
                $.ajax({
                    url: url,
                    type: "POST",
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    beforeSend: function () {
                        block_general("body")
                    },
                    success: function (response) {
                         respuesta = response.respuesta
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
                        unblock("body")
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                    }
                })
            } else {
                messageResponse({
                    text: "Seleccione un Rol,para Registrar",
                    type: "warning"
                })
            }
        
        })
        $(document).on('change','#cboRol_',function(e){
            e.preventDefault()
            let rolid_ = $(this).val()
            if (rolid_) {
                permisosMenuDis(rolid_)
            }
        })
        $(this).off('ifChecked', '#libody input')
        $(document).on('ifChecked', '#libody input', function (event) {

            let idRol = $("#cboRol_").val()
            let idPermNombre = jQuery(this).val()
            let dataTitulo = jQuery(this).data("tit")
            let data = { WEB_RolID: idRol, WEB_PMeDataMenu: idPermNombre, WEB_PMeNombre: dataTitulo, WEB_PMeEstado: 1 }
            let url = basePath + "Seguridadintranet/AgregarPermisoMenu"
            let principal = jQuery(this).data("principal")
            //console.log(data)
            if (principal == "1") {
                console.log("entro 1")
                $.ajax({
                    url: url,
                    type: "POST",
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    beforeSend: function () {
                       block_general("body")
                    },
                    success: function (response) {
                        let respuesta = response.respuesta
                        if (respuesta === true) {
                            // $('.nav-list li[data-menu1="' + idPermNombre + '"]').removeClass('oculto');
                            messageResponse({
                                text: 'Se Asigno Permiso',
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
                      unblock("body")
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        mensaje = false
                    }
                })
            } else {
                console.log("entro 2")
            }
        })

        $(document).on('ifUnchecked', '#libody input', function (event) {
            let idRol = $("#cboRol_").val()
            let idPermNombre = jQuery(this).val()
            let dataTitulo = jQuery(this).data("tit")
            let data = { WEB_RolID: idRol, WEB_PMeDataMenu: idPermNombre }
            let url = basePath + "Seguridadintranet/QuitarPermisoMenu"
            let principal = jQuery(this).data("principal")
            if (principal == "1") {
                $.ajax({
                    url: url,
                    type: "POST",
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    beforeSend: function () {
                       block_general("body")
                    },
                    success: function (response) {
                        let respuesta = response.respuesta
                        if (respuesta === true) {
                            // $('.nav-list li[data-menu1="' + idPermNombre + '"]').addClass('oculto');
                            messageResponse({
                                text: 'Se Quitó Permiso',
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
                      unblock("body")
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        mensaje = false
                    }
                })
            } else {

            }
        })
        $(document).on('click', '#listaRoles li', function (event) {
            var id = $(this).data("id");
            var nombreRol = $(this).find("a").text();
            console.log(nombreRol)
            $("#nombreRolSpan").text(nombreRol);
            ListadoPermisoRol(id);
        });
        $(document).on('ifChecked', '#tab2 input:checkbox', function (event) {

            var todos = jQuery(this).data("todos");
            var permiso = [];
            var toaster = "";
            if (todos == "2") {
                $("#lblesperando").show();
                $("#lblcheck").hide();
                $.LoadingOverlay("show");
                setTimeout(function () {
                    $("#tab2").iCheck("destroy");
                    $('#tab2 ul.task-list').each(function () {
                        $(this).find('input:checkbox:not(:checked)').each(function () {
                            var idinputdata = jQuery(this).data("id");
                            var ids = idinputdata.split("_");
                            var idrolcheck = ids[0];
                            var controladorcheck = ids[1];
                            var idpermisocheck = ids[2];
                            permiso.push({ WEB_RolID: idrolcheck, WEB_PermID: idpermisocheck });
                            jQuery(this).click();
                            var cant = $("#cant_" + controladorcheck).text();
                            $("#cant_" + controladorcheck).text(parseInt(cant) + 1);
                        });
        
                    });
                    $('input[data-todos="1"]').prop('checked', true);
                    $("#tab2").iCheck({
                        checkboxClass: 'icheckbox_square-blue',
                        radioClass: 'iradio_square-red',
                        increaseArea: '2%' // optional
                    });
                    toaster = 0;
                    $(".badge.pull-right").css("border-bottom", "0px");
                    $.LoadingOverlay("hide");
                    if (permiso.length > 0) {
                        var url = basePath + "Seguridadintranet/AgregarPermisoRol";
                        DataPostWithoutChangePermiso(url, permiso, toaster);
                    }
                    $("#lblesperando").hide();
                    $("#lblcheck").show();
        
                }, 100);
        
        
        
            }
            else {
                var id = jQuery(this).data("id");
                var ids = id.split("_");
                var idrol = ids[0];
                var controlador = ids[1];
                var idpermiso = ids[2];
                if (todos == "0") {
                    permiso.push({ WEB_RolID: idrol, WEB_PermID: idpermiso });
                    var cant = $("#cant_" + controlador).text();
                    $("#cant_" + controlador).text(parseInt(cant) + 1);
                    var nocheked = $('#' + controlador + ' ul.task-list input:checkbox:not(:checked)').length;
                    if (nocheked == "0") {
                        $("#check_control_" + controlador).iCheck("destroy");
                        $("#check_control_" + controlador).click();
                        $("#check_control_" + controlador).iCheck({
                            checkboxClass: 'icheckbox_square-blue',
                            radioClass: 'iradio_square-red',
                            increaseArea: '2%' // optional
                        });
                    }
                    toaster = 2;
                }
                if (todos == "1") {
                    $("#" + controlador).iCheck("destroy");
                    $('#' + controlador + ' ul.task-list input:checkbox:not(:checked)').each(function () {
                        var idinputdata = jQuery(this).data("id");
                        var ids = idinputdata.split("_");
                        var idrolcheck = ids[0];
                        var controladorcheck = ids[1];
                        var idpermisocheck = ids[2];
                        permiso.push({ WEB_RolID: idrolcheck, WEB_PermID: idpermisocheck });
                        jQuery(this).click();
                        var cant = $("#cant_" + controladorcheck).text();
                        $("#cant_" + controladorcheck).text(parseInt(cant) + 1);
        
                    });
                    $("#" + controlador).iCheck({
                        checkboxClass: 'icheckbox_square-blue',
                        radioClass: 'iradio_square-red',
                        increaseArea: '2%' // optional
                    });
                    toaster = 1;
                }
        
                if (permiso.length > 0) {
                    var url = basePath + "Seguridadintranet/AgregarPermisoRol";
                    DataPostWithoutChangePermiso(url, permiso, toaster);
                }
            };
        })
        $(document).on('ifUnchecked', '#tab2 input:checkbox', function (event) {

            var todos = jQuery(this).data("todos");
            var permiso = [];
            var toaster = "";
            if (todos == "2") {
                $("#lblesperando").show();
                $("#lblcheck").hide();
                $.LoadingOverlay("show");
                setTimeout(function () {
        
                    $("#tab2").iCheck("destroy");
                    $('#tab2 ul.task-list').each(function () {
                        $(this).find('input:checkbox:checked').each(function () {
                            var idinputdata = jQuery(this).data("id");
                            var ids = idinputdata.split("_");
                            var idrolcheck = ids[0];
                            var controladorcheck = ids[1];
                            var idpermisocheck = ids[2];
                            permiso.push({ WEB_RolID: idrolcheck, WEB_PermID: idpermisocheck });
                            jQuery(this).click();
                            var cant = $("#cant_" + controladorcheck).text();
                            $("#cant_" + controladorcheck).text(parseInt(cant) - 1);
                        });
        
                    });
                    $('input[data-todos="1"]').prop('checked', false);
                    $("#tab2").iCheck({
                        checkboxClass: 'icheckbox_square-blue',
                        radioClass: 'iradio_square-red',
                        increaseArea: '2%' // optional
                    });
                    toaster = 3;
        
                    $(".badge.pull-right").css("border-bottom", "2px solid red");
                    $.LoadingOverlay("hide");
                    if (permiso.length > 0) {
                        var url = basePath + "Seguridadintranet/QuitarPermisoRol";
                        DataPostWithoutChangePermiso(url, permiso, toaster);
                    }
                    $("#lblesperando").hide();
                    $("#lblcheck").show();
                }, 100);
        
        
        
            }
            else {
                var id = jQuery(this).data("id");
                var ids = id.split("_");
                var idrol = ids[0];
                var controlador = ids[1];
                var idpermiso = ids[2];
                if (todos == "0") {
                    permiso.push({ WEB_RolID: idrol, WEB_PermID: idpermiso });
                    var cant = $("#cant_" + controlador).text();
                    $("#cant_" + controlador).text(parseInt(cant) - 1);
                    var estado = $("#check_control_" + controlador).is(':checked');
                    if (estado == true) {
                        $("#check_control_" + controlador).iCheck("destroy");
                        $("#check_control_" + controlador).click();
                        $("#check_control_" + controlador).iCheck({
                            checkboxClass: 'icheckbox_square-blue',
                            radioClass: 'iradio_square-red',
                            increaseArea: '2%' // optional
                        });
                    }
                    toaster = 5;
                }
                if (todos == "1") {
                    $("#" + controlador).iCheck("destroy");
                    $('#' + controlador + ' ul.task-list input:checkbox:checked').each(function () {
                        var idinputdata = jQuery(this).data("id");
                        var ids = idinputdata.split("_");
                        var idrolcheck = ids[0];
                        var controladorcheck = ids[1];
                        var idpermisocheck = ids[2];
                        permiso.push({ WEB_RolID: idrolcheck, WEB_PermID: idpermisocheck });
                        jQuery(this).click();
                        var cant = $("#cant_" + controladorcheck).text();
                        $("#cant_" + controladorcheck).text(parseInt(cant) - 1);
        
                    });
                    $("#" + controlador).iCheck({
                        checkboxClass: 'icheckbox_square-blue',
                        radioClass: 'iradio_square-red',
                        increaseArea: '2%' // optional
                    });
                    toaster = 4;
                };
        
                if (permiso.length > 0) {
                    var url = basePath + "Seguridadintranet/QuitarPermisoRol";
                    DataPostWithoutChangePermiso(url, permiso, toaster);
                }
            };
        
        
            //console.log(permiso)
        });
    }
    let DataPostWithoutChangePermiso=function(url, data, toaster) {

        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json",
            beforeSend: function () {
                block_general("body")
            },
            success: function (response) {
                var respuesta = response.respuesta;
                //console.log(respuesta,"ase")
                if (respuesta === true) {
                    if (toaster == 0) {
                        messageResponse({
                            text: "Se Asigno Todos los Permisos",
                            type: "success"
                        })
                        
                    }
                    if (toaster == 1) {
                        messageResponse({
                            text: "Se Asigno Todos los Permisos de Bloque",
                            type: "success"
                        })
                    }
                    if (toaster == 2) {
                        messageResponse({
                            text: "Se Asigno Permiso",
                            type: "success"
                        })
                    }
    
                    if (toaster == 3) {
                        messageResponse({
                            text: "Se Quito Todos los Permisos",
                            type: "success"
                        })
                    }
    
                    if (toaster == 4) {
                        messageResponse({
                            text: "Se Quito Todos los Permisos del Bloque",
                            type: "success"
                        })
                    }
    
                    if (toaster == 5) {
                        messageResponse({
                            text: "Se Quito el Permiso Asignado",
                            type: "success"
                        })
                    }
    
                } else {
                    messageResponse({
                        text: response.mensaje,
                        type: "success"
                    })
                }
            },
            complete: function () {
                unblock('body')
                totales();
            },
            error: function (jqXHR, textStatus, errorThrown) {
    
            }
        });
    
    }
    let ListadoPermisoRol=function(rolid) {
        var url = basePath + "Seguridadintranet/ListadoControladorPermisos";
        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify({ rolid: rolid }),
            contentType: "application/json",
            beforeSend: function () {
                block_general("body")
                $("#contenidopermisos").hide();
                $("#alertapermisos").text("Cargando Permisos del Rol ...");
                $("#alertapermisos").show();
    
            },
            success: function (response) {
                var controlador = response.controlador;
                var permisosControlador = response.listaPermisoControlador;
                var permisosRol = response.listaPermisosRol;
    
                if (controlador) {
                    $("#bodyPermisosRoles").html("");
    
                    $.each(controlador, function (index, value) {
    
                        var permisosLista = "";
                        var cantPerm = "0";
                        var permisosChek = "0";
                        $.each(permisosControlador, function (index, valuePC) {
                            var check = "";
                            if (value.WEB_PermControlador == valuePC.WEB_PermControlador) {
                                cantPerm = Number(cantPerm) + 1;
                                var nombrePermiso = valuePC.WEB_PermNombreR ? valuePC.WEB_PermNombreR : valuePC.WEB_PermNombre;
    
                                $.each(permisosRol, function (key, valuePR) {
                                    if (valuePR.WEB_RolID == rolid && valuePR.WEB_PermID == valuePC.WEB_PermID) {
                                        check = "checked";
                                        permisosChek = Number(permisosChek) + 1;
                                    }
                                });
    
                                permisosLista += '<li class="task-list-item">' +
                                    '<div class="checkbox"><label>' +
                                    '<input id="' + valuePC.WEB_PermID + '_' + rolid + '" ' + check + ' data-todos="0" data-id="' + rolid + "_" + value.WEB_PermControlador + "_" + valuePC.WEB_PermID + '"  type="checkbox" class="task-list-item-checkbox"/> ' +
                                    nombrePermiso +
                                    '</label></div>' +
                                    '</li>';
                            };
                        });
                        var todos = "";
                        if (Number(cantPerm) > 0) {
                            if (cantPerm == permisosChek) {
                                todos = "checked";
                            };
                        }
                        var border = "";
                        if (permisosChek != cantPerm) {
                            border = 'border-bottom:2px solid red';
                        }
                        $("#bodyPermisosRoles").append('<div class="col-md-4" style="padding-right: 4px;padding-left: 4px;padding-bottom: 4px;">' +
                            '<div class= "panel panel-success">' +
                            '<div class="panel-heading">' +
                            '<h4 class="panel-title">' +
                            '<a style="font-size: 14px;" class="collapsed" data-toggle="collapse"  href="#' + value.WEB_PermControlador + '">' +
                            value.WEB_PermControlador.replace("Controller", "") +
                            '<span class="icon icon-arrow-down" style="margin-top: -3px;"></span>' +
                            '<span class="badge pull-right" style="margin-right: 5px;margin-top: 0px;width: 46px;' + border + '">' +
                            '<span id="cant_' + value.WEB_PermControlador + '"  >' + permisosChek + '</span> / ' + cantPerm + '</span>' +
                            '</a>' +
                            '</h4>' +
                            '</div>' +
                            '<div id="' + value.WEB_PermControlador + '" class="panel-collapse collapse">' +
                            '<div class="panel-body taskPanel" style="text-transform: uppercase;padding-top: 5px;">' +
                            '<div class="row"><div class="checkbox"> ' +
                            '<label><input type="checkbox" ' + todos + ' id="check_control_' + value.WEB_PermControlador + '" data-todos="1" data-id="' + rolid + "_" + value.WEB_PermControlador + '_" class="task-list-item-checkbox" /> Seleccionar Todos</label>' +
                            '</div></div>' +
                            '<hr style="margin-top:0px;margin-bottom:10px" />' +
                            '<ul class="task-list">' + permisosLista +
                            '</ul>' +
                            '</div>' +
                            '</div>' +
                            '</div>' +
                            '</div>');
                    });
    
                    totales();
                }
                else {
                    messageResponse({
                        text: response.mensaje,
                        type: "error"
                    });
                }
    
            },
            complete: function () {
                $("#contenidopermisos").show();
                $("#alertapermisos").hide();
                $("ul.task-list").mCustomScrollbar({
                    autoHideScrollbar: true,
                    scrollbarPosition: "outside",
                    theme: "dark",
                    setHeight: "210px"
                });
    
                $("#tab2").iCheck({
                    checkboxClass: 'icheckbox_square-blue',
                    radioClass: 'iradio_square-red',
                    increaseArea: '2%' // optional
                });
                unblock('body')
            },
            error: function (jqXHR, textStatus, errorThrown) {
    
            }
        });
    }
    let totales=function() {
        var totalno = $('#tab2 ul.task-list input:checkbox:not(:checked)').length;
        var total = $('#tab2 ul.task-list input:checkbox').length;
        var totalsi = $('#tab2 ul.task-list input:checkbox:checked').length;
        $("#fullall").iCheck("destroy");
        if (totalno == "0") {
            $("#fullall").prop('checked', true);
            $("#divfaltanTotal").hide();
    
        } else {
            $("#fullall").prop('checked', false);
            $("#totalPermisosSpanFaltan").text(totalno);
            $("#divfaltanTotal").show();
        }
        $("#fullall").iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-red',
            increaseArea: '2%' // optional
        });
        $("#totalPermisosSpan").text(totalsi + "/" + total);
    }
    let ListadoRolestab=function() {
        var url = basePath + "Seguridadintranet/ListadoRolesSeguridadPermiso";
        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify({}),
            contentType: "application/json",
            beforeSend: function () {
                block_general("body")
            },
            success: function (response) {
                var roles = response.roles;
                if (roles) {
    
                    $("#listaRoles").html("");
                    $.each(roles, function (index, value) {
                        $("#listaRoles").append( `  
                        <li class="" data-toggle="tab" data-id="${value.WEB_RolID}">
                            <a href="#">
                                <i class="pink ace-icon fa fa-tachometer bigger-110"></i>
                                ${value.WEB_RolNombre}
                            </a>
                        </li>`)
                    })
                }
                else {
                    messageResponse({
                        text: response.mensaje,
                        type: "error"
                    })
                }
            },
            complete: function () {
                unblock('body')
            },
            error: function (jqXHR, textStatus, errorThrown) {
    
            }
        });
    }
    let llenarSelectSeguridad=function(url, data, select, dataId, dataValor, selectVal) {

        if (!url) {
            messageResponse({
                text: 'No se Declaro Url',
                type: "success"
            })
            return false
        }
        let mensaje = true
        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json",
            beforeSend: function () {
                $("#" + select).html("")
                $("#" + select).append('<option value="">Cargando...</option>')
                $("#" + select).attr("disabled", "disabled")
            },
            success: function (response) {
                let datos = response.data
                let mensaje = response.mensaje
                if (datos.length > 0) {
                    $("#" + select).html("")
                    $("#" + select).append('<option value="">--Seleccione--</option>')
                    if (selectVal == "allOption") {
                        $("#" + select).append('<option value="0">Todos</option>')
                    }
                    $.each(datos, function (index, value) {
                        let selected = ""
                        if ($.isArray(selectVal)) {
                            if (objectFindByKey(selectVal, dataId, value[dataId]) != null) {
                                selected = "selected='selected'"
                            }
                        } else {
    
                            if (value[dataId] === selectVal) {
                                selected = "selected='selected'"
                            }
                        }
                        $("#" + select).append(`<option value="${value[dataId]}" ${selected}>${value[dataValor]}</option>`)
    
                    })
                    $("#" + select).removeAttr("disabled")
                } else {
                    toastr.error("No Hay Data  en " + select, "Mensaje Servidor")
                }
                if (mensaje !== "") {
                    messageResponse({
                        text: mensaje,
                        type: "success"
                    })
                }
            },
            complete: function () {
                //$.LoadingOverlay("hide")
            },
            error: function (jqXHR, textStatus, errorThrown) {
                mensaje = false
                
            }
        })
        return mensaje
    }
    let permisosMenuDis=function(rol) {
        if (rol != 0) {
            rol = $("#cboRol_").val()
        }
        else {
            rol = rolid
        }
    
        let data = { rolId: rol }
        let url = basePath + "SeguridadIntranet/ListadoMenusRolId"
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            beforeSend: function () {
                block_general("body")
            },
            complete: function () {
                unblock("body")
            },
            success: function (response) {
                let respuesta = response.dataResultado
                if (response.mensaje) {
                    toastr.error(response.mensaje, "Mensaje Servidor")
                }
                if (respuesta) {
                    $("#libody").html("")
                    let menus = []
                    $.each(respuesta, function (index, value) {
                        menus.push(value.WEB_PMeDataMenu)
                    })
                    $(".cabecera").each(function (i) {
                        let aleatorio = Math.round(Math.random()*6)
                        let total = $(".cabecera").length - 1
                        let element = $(this)
                        let menu = element.data('menu1')
                        let titulo = element.data('titulo')
                        let existeMenu_ = jQuery.inArray(menu, menus)
                        let check = ""
                        if (existeMenu_ >= 0) {
                            check = "checked"
                        } else {
                            check = ""
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
                            })
                        }
            
                    })
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
    
            }
        })
        
    
    }
    let listausuarios=function() {
        let url = basePath + "Rolusuario/ListadoTableUsuarioAsignarRol"
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({}),
            beforeSend: function () {
                block_general("body")
            },
            complete: function () {
                unblock("body")
            },
            success: function (response) {
                let roles = response.roles
                let usuarios = response.usuarios
                let rolUsuarios = response.rolUsuarios
                console.log(response)
                $("#tableUsuRol").on('page.dt', function () {
                    $('.dataTables_scrollBody').css('height', '250px')
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
                                let selectedOptions = ""
                                selectedOptions += '<option value="">--Seleccione--</option>'
                                $.each(roles, function (keyr, valuer) {
                                    let seleccion = ""
                                    $.each(rolUsuarios, function (key, value2) {
                                        if (value2.UsuarioID == o && value2.WEB_RolID == valuer.WEB_RolID) {
                                            seleccion = "selected"
                                        }
                                    })
    
                                    selectedOptions += `<option ${seleccion} value="${valuer.WEB_RolID}">${valuer.WEB_RolNombre}</option>`
                                })
    
                                return `<select data-usuid="${o}" style="font-weight: bolder;" class="form-control input-sm selectEmp">${selectedOptions}</select>`
                            }
                        }
                    ]
                })
    
                $('table').css('width', '100%')
                $('.dataTables_scrollHeadInner').css('width', '100%')
                $('.dataTables_scrollFootInner').css('width', '100%')
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
    
            }
        })
    
    }
    return {
        init: function () {
            _inicio()
            _componentes()
        }
    }
}()
document.addEventListener('DOMContentLoaded',function(){
    panelSeguridad.init()
})