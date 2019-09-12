var basePath = $("#BasePath").text();

///*SIDEBAR*/

//var CURRENT_URL = window.location.href.split('#')[0].split('?')[0],
//    $BODY = $('body'),
//    $MENU_TOGGLE = $('#menu_toggle'),
//    $SIDEBAR_MENU = $('#sidebar-menu'),
//    $SIDEBAR_FOOTER = $('.sidebar-footer'),
//    $LEFT_COL = $('.left_col'),
//    $RIGHT_COL = $('.right_col'),
//    $NAV_MENU = $('.nav_menu'),
//    $FOOTER = $('footer');
//function init_sidebar() {
//    // TODO: This is some kind of easy fix, maybe we can improve this
//    var setContentHeight = function () {
//        // reset height
//        $RIGHT_COL.css('min-height', $(window).height());

//        var bodyHeight = $BODY.outerHeight(),
//            footerHeight = $BODY.hasClass('footer_fixed') ? -10 : $FOOTER.height(),
//            leftColHeight = $LEFT_COL.eq(1).height() + $SIDEBAR_FOOTER.height(),
//            contentHeight = bodyHeight < leftColHeight ? leftColHeight : bodyHeight;

//        // normalize content
//        contentHeight -= $NAV_MENU.height() + footerHeight;

//        $RIGHT_COL.css('min-height', contentHeight);
//    };

//    $SIDEBAR_MENU.find('a').on('click', function (ev) {
//        console.log('clicked - sidebar_menu');
//        var $li = $(this).parent();

//        if ($li.is('.active')) {
//            $li.removeClass('active active-sm');
//            $('ul:first', $li).slideUp(function () {
//                setContentHeight();
//            });
//        } else {
//            // prevent closing menu if we are on child menu
//            if (!$li.parent().is('.child_menu')) {
//                $SIDEBAR_MENU.find('li').removeClass('active active-sm');
//                $SIDEBAR_MENU.find('li ul').slideUp();
//            } else {
//                if ($BODY.is(".nav-sm")) {
//                    $li.parent().find("li").removeClass("active active-sm");
//                    $li.parent().find("li ul").slideUp();
//                }
//            }
//            $li.addClass('active');

//            $('ul:first', $li).slideDown(function () {
//                setContentHeight();
//            });
//        }
//    });

//    // toggle small or large menu 
//    $MENU_TOGGLE.on('click', function () {
//        console.log('clicked - menu toggle');

//        if ($BODY.hasClass('nav-md')) {
//            $SIDEBAR_MENU.find('li.active ul').hide();
//            $SIDEBAR_MENU.find('li.active').addClass('active-sm').removeClass('active');
//        } else {
//            $SIDEBAR_MENU.find('li.active-sm ul').show();
//            $SIDEBAR_MENU.find('li.active-sm').addClass('active').removeClass('active-sm');
//        }

//        $BODY.toggleClass('nav-md nav-sm');

//        setContentHeight();

//        $('.dataTable').each(function () { $(this).dataTable().fnDraw(); });
//    });

//    // check active menu
//    $SIDEBAR_MENU.find('a[href="' + CURRENT_URL + '"]').parent('li').addClass('current-page');

//    $SIDEBAR_MENU.find('a').filter(function () {
//        return this.href == CURRENT_URL;
//    }).parent('li').addClass('current-page').parents('ul').slideDown(function () {
//        setContentHeight();
//    }).parent().addClass('active');

//    // recompute content when resizing
//    $(window).smartresize(function () {
//        setContentHeight();
//    });

//    setContentHeight();

//    // fixed sidebar
//    if ($.fn.mCustomScrollbar) {
//        $('.menu_fixed').mCustomScrollbar({
//            autoHideScrollbar: true,
//            theme: 'minimal',
//            mouseWheel: { preventDefault: true }
//        });
//    }
//};
//$(document).ready(function () {
//    init_sidebar();
//});
///*END SIDEBAR*/


$.ajaxSetup({
    error: function (xmlHttpRequest, textStatus, errorThrow) {
        messageResponse({
            message: errorThrow,
            type: "error"
        });
    },
    statusCode: {
        404: function () {
            messageResponse({
                message: "No Se encuentra la Direccion.(404)",
                type: "error"
            });
        },
        405: function () {
            messageResponse({
                message: "Metodo no Permitido.(GET,POST,PUT,DELETE)(405)",
                type: "error"
            });
        },
        500: function () {
            messageResponse({
                message: "Error Interno.(500)",
                type: "error"
            });
        }
    }
});

//////////////////////////////////////////////////////

function refresh(obj) {
    //console.warn("estado_refresh", estate);
    var defaults = {
        estate: null,
        time: 2500,

    };
    var opciones = $.extend({}, defaults, obj);
    setTimeout(function () {
        window.location.reload(opciones.estate);
    }, opciones.time);
}

function redirect(obj) {
    var defaults = {
        site: null,
        time: 2500,

    };
    var opciones = $.extend({}, defaults, obj);
    setTimeout(function () {
        window.location.href = basePath + opciones.site;
    }, opciones.time);
    
}

//////////////////////////////////
function block_general(block) {
    // $.LoadingOverlay("show");
    $(block).LoadingOverlay("show", {
        background: "rgba(51, 51, 51, 0.5)"
    });
}

function unblock(block) {
    $.LoadingOverlay("hide");
}

////////////////////////////////////////////////////////////
function messageResponse(obj) {
    var defaults = {
        text: null,
        type: null,
        delay: 2500,
        title: null,
        addclass: null
    };
    //type success,warning,info,error ; addclass:dark
    var opciones = $.extend({}, defaults, obj);
    switch (opciones.type) {
        case 'warning':
            opciones.type = null;
            break;
    };
    new PNotify({
        title: 'Mensaje Respuesta',
        text: opciones.text,
        type: opciones.type,
        styling: 'bootstrap3',
        delay: opciones.delay,
        addclass: opciones.addclass
    });
}

function CloseMessages() {
    PNotify.removeAll();
}

function messageConfirmation(obj) {
    var defaults = {
        title: 'Confirmacion',
        content: '¿Seguro que desea proceder?',
        icon: 'fa fa-warning',
        callBackSAceptarComplete: null,
        callBackSCCerraromplete: null,
    };
    var opciones = $.extend({}, defaults, obj);
    $.confirm({
        title: opciones.title,
        theme: 'dark',
        icon: opciones.icon,
        content: opciones.content,
        buttons: {
            Aceptar: function () {
                if (opciones.callBackSAceptarComplete != null) {
                    opciones.callBackSAceptarComplete();
                }
            },
            Cerrar: function () {
                if (opciones.callBackSCCerraromplete != null) {
                    opciones.callBackSCCerraromplete();
                }
            },
            
        }
    });
}
/////////////////////////////////////////////////////////////////

function limpiar_form(obj) {
    var defaults = {
        contenedor: null,
    }

    var opciones = $.extend({}, defaults, obj);
    if (opciones.contenedor == null) {
        console.warn('Advertencia - contenedor no fue declarado.');
        return;
    };

    $(opciones.contenedor + " input:visible,select:visible,textarea:visible").val("");
    $(opciones.contenedor + ' select').val(null).trigger("change");
}

////////////////////////////////////////////////////////////////////


function responseSimple(obj) {
    var defaults = {
        url: null,
        type: "POST",
        data: [],
        refresh: true,
        redirect: false,
        redirectUrl: null,
        callBackBeforeSend: null,
        callBackSComplete: null,
        callBackSuccess: function () {
            console.warn("funcion vacio finalizada");
        },
        time: 2000,
        loader: true
    }

    var opciones = $.extend({}, defaults, obj);
    if (opciones.url == null) {
        console.warn('Advertencia - url no fue declarado.');
        return;
    };

    var url = basePath + opciones.url;
    $.ajax({
        url: url,
        type: opciones.type,
        contentType: "application/json",
        data: opciones.data,
        beforeSend: function () {
            if (opciones.loader) {
                block_general("body");
            }
            if (opciones.callBackBeforeSend != null) {
                opciones.callBackBeforeSend();
            }
        },
        complete: function () {
            if (opciones.loader) {
                unblock("body");
            }
            if (opciones.callBackSComplete != null) {
                opciones.callBackSComplete();
            }
        },
        success: function (response) {
            var respuesta = response.respuesta;
            var mensaje = response.mensaje;

            if (respuesta) {
                messageResponse({
                    text: mensaje,
                    type: "success"
                });
            } else {
                messageResponse({
                    text: mensaje,
                    type: "error"
                });
            };
            if (opciones.redirect) {
                if (opciones.redirectUrl == null) {
                    console.warn('Advertencia - redirectUrl no fue declarado.');
                    return;
                };
                setTimeout(function () {
                    redirect({ site: opciones.redirectUrl, time: 0 });
                }, opciones.time);

            } else {
                if (opciones.refresh) {
                    setTimeout(function () {
                        refresh({ estate: true, time: 0 });
                    }, opciones.time);
                } else {
                    opciones.callBackSuccess(response);
                }
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            unblock("body");
            console.warn('Message :', errorThrow);
        }
    });
}

function responseFileSimple(obj) {
    var defaults = {
        url: null,
        type: "POST",
        method:"POST",
        data: [],
        refresh: true,
        redirect: false,
        redirectUrl: null,
        callBackBeforeSend: null,
        callBackSComplete: null,
        callBackSuccess: function () {
            console.warn("funcion vacio finalizada");
        },
        time: 2000,
        loader: true
    }

    var opciones = $.extend({}, defaults, obj);
    if (opciones.url == null) {
        console.warn('Advertencia - url no fue declarado.');
        return;
    };

    var url = basePath + opciones.url;
    $.ajax({
        url: url,
        type: opciones.type,
        method:opciones.method,
        data: opciones.data,
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function () {
            if (opciones.loader) {
                block_general("body");
            }
            if (opciones.callBackBeforeSend != null) {
                opciones.callBackBeforeSend();
            }
        },
        complete: function () {
            if (opciones.loader) {
                unblock("body");
            }
            if (opciones.callBackSComplete != null) {
                opciones.callBackSComplete();
            }
        },
        success: function (response) {
            var respuesta = response.respuesta;
            var mensaje = response.mensaje;

            if (respuesta) {
                messageResponse({
                    text: mensaje,
                    type: "success"
                });

                if (opciones.redirect) {
                    if (opciones.redirectUrl == null) {
                        console.warn('Advertencia - redirectUrl no fue declarado.');
                        return;
                    };
                    setTimeout(function () {
                        redirect({ site: opciones.redirectUrl, time: 0 });
                    }, opciones.time);

                } else {
                    if (opciones.refresh) {
                        setTimeout(function () {
                            refresh({ estate: true, time: 0 });
                        }, opciones.time);
                    } else {
                        opciones.callBackSuccess(response);
                    }
                }

            } else {
                messageResponse({
                    text: mensaje,
                    type: "error"
                });
            };

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            unblock("body");
            console.warn('Message :', xmlHttpRequest);
        }
    });
}

////////////////////////////////////////

function validar_Form(obj) {
    if (!$().validate) {
        console.warn('Advertencia - validate.min.js no esta definido.');
        return;
    }
    // Initialize
    var defaults = {
        contenedor: null,
        nameVariable: "",
        ignore: 'input[type=hidden], .select2-search__field',
        rules: {},
        messages: {}
    }

    var opciones = $.extend({}, defaults, obj);

    if (opciones.contenedor == null) {
        console.warn('Advertencia - contenedor no esta definido.');
        return;
    }

    var objt = "_objetoForm";
    this[objt + '_' + opciones.nameVariable] = $(opciones.contenedor).validate({
        ignore: opciones.ignore, // ignore hidden fields
        errorClass: 'validation-invalid-label',
        successClass: 'validation-valid-label',
        validClass: 'validation-valid-label',
        highlight: function (element, errorClass) {
            $(element).addClass(errorClass);
        },
        unhighlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
        },
        success: function (label, element, errorClass) {
            //label.removeClass('validation-invalid-label');
            //label.addClass('validation-valid-label').text('Correcto.'); // remove to hide Success message
            //$(element).removeClass(errorClass);
        },
        // Different components require proper error label placement
        errorPlacement: function (error, element) {
          
            // Unstyled checkboxes, radios
            if (element.parents().hasClass('form-check')) {
                error.appendTo(element.parents('.form-check').parent());
            }

            // Input with icons and Select2
            else if (element.parents().hasClass('form-group-feedback') || element.hasClass('select2-hidden-accessible')) {
                error.appendTo(element.parent());
            }

            // Input group, styled file input
            else if (element.parent().is('.uniform-uploader, .uniform-select') || element.parents().hasClass('input-group')) {
                error.appendTo(element.parent().parent());
            }

            // Input group, styled file input
            else if (element.parent().is('.uniform-uploader, .uniform-select') || element.parents().hasClass('input-group')) {
                error.appendTo(element.parent().parent());
            }

            // Other elements
            else {
                error.insertAfter(element);
            }
        },

        submitHandler: function (form) { // for demo
            return false;
        },
        rules: opciones.rules,
        messages: opciones.messages
    });

}

////////////////////////////////////////////////////////////////

function selectResponse(obj) {
    // Initialize
    var defaults = {
        url: null,
        data: {},
        select: null,
        campoID: null,
        CampoValor: null,
        selectVal: null,
        select2: false,
        allOption: false,
        placeholder:"Seleccione"
    }

    var opciones = $.extend({}, defaults, obj);

    if (opciones.url==null) {
        messageResponse({
            text: "No se Declaro Url",
            type: "warning"
        });
        return false;
    }

    $.ajax({
        url: basePath+ opciones.url,
        type: "POST",
        data: JSON.stringify(opciones.data),
        contentType: "application/json",
        beforeSend: function () {
            if ($("#" + opciones.select).hasClass('select2-hidden-accessible')) {
                $("#" + opciones.select).select2('destroy');
            }
            $("#" + opciones.select).html("");
            $("#" + opciones.select).append('<option value="">Cargando...</option>');
            $("#" + opciones.select).attr("disabled", "disabled");
            //$.LoadingOverlay("show");
        },
        success: function (response) {
            var datos = response.data;
            var mensaje = response.mensaje;
            $("#" + opciones.select).html("");
            $("#" + opciones.select).append('<option value="">' + opciones.placeholder + '</option>');
            if (datos.length > 0) {
                if (opciones.allOption) {
                    $("#" + opciones.select).append('<option value="0">Todos</option>');
                }
                $.each(datos, function (index, value) {
                    var selected = "";
                   
                    if ($.isArray(opciones.selectVal)) {
                        if (objectFindByKey(opciones.selectVal, opciones.campoID, value[opciones.campoID]) != null) {
                            selected = "selected='selected'";
                        };
                    } else {

                        if (value[opciones.campoID] === opciones.selectVal) {
                            selected = "selected='selected'";
                        };
                    }
                    $("#" + opciones.select).append('<option value="' + value[opciones.campoID] + '"    ' + selected + '>' + value[opciones.CampoValor] + '</option>');

                });
                $("#" + opciones.select).removeAttr("disabled");
                if (opciones.select2) {
                    $("#" + opciones.select).select2();
                }
                
            } else {
                $("#" + opciones.select).removeAttr("disabled");
                messageResponse({
                    text: "No Hay Registros",
                    type: "warning"
                });
            }
            if (mensaje !== "") {
                messageResponse({
                    text: mensaje,
                    type: "success"
                });
            }
        },
        complete: function () {
            //$.LoadingOverlay("hide");
        },
    });

}

//////////////////////////////////////////////////////////////////

function readImage(input,contenedor) {

    if (input != null && contenedor != null) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(contenedor).attr('src', e.target.result);
        }
        reader.readAsDataURL(input);
    }
    else {
        console.warn("input o contenedor vacio(s)");
    }   
    console.warn("input o contenedor vacio(s)");
}

//////////////////////////////////////////////////////////////////

$.fn.serializeFormJSON = function () {

    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}


function fncRegistrar(dataForm, url, resetform, url_redirect) {
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(dataForm),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            var respuesta = response.respuesta;
            if (respuesta === true) {
                toastr.success("Se Registro Correctamente", "Mensaje Servidor");
                if (url_redirect != "") {
                    setTimeout(function () {
                        window.location.replace(basePath + url_redirect);
                    }, 2000);

                }
                if (resetform) {
                    $("#frmNuevo")[0].reset();
                    $('select').prop('selectedIndex', 0).change();
                }
            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        }
    });
}
//////////////////////////////////////////////////////////////////////////////

function llenarSelect(url, data, select, dataId, dataValor, selectVal) {

    if (!url) {
        toastr.error("No se Declaro Url", "Mensaje Servidor");
        return false;
    }
    var mensaje = true;
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            $("#" + select).html("");
            $("#" + select).append('<option value="">Cargando...</option>');
            $("#" + select).attr("disabled", "disabled");
            //$.LoadingOverlay("show");
        },
        success: function (response) {
            var datos = response.data;
            var mensaje = response.mensaje;
            if (datos.length > 0) {
              
                $("#" + select).html("");
                $("#" + select).append('<option value="">--Seleccione--</option>');
                if (selectVal == "allOption") {
                    $("#" + select).append('<option value="0">Todos</option>');
                }
                $.each(datos, function (index, value) {
                    var selected = "";
                    if ($.isArray(selectVal)) {
                        if (objectFindByKey(selectVal, dataId, value[dataId]) != null) {
                            selected = "selected='selected'";
                        };
                    } else {

                        if (value[dataId] === selectVal) {
                            selected = "selected='selected'";
                        };
                    }
                    $("#" + select).append('<option value="' + value[dataId] + '"    ' + selected + '>' + value[dataValor] + '</option>');

                });
                $("#" + select).removeAttr("disabled");
            } else {
                toastr.success("No Hay Registros", "Mensaje Servidor");
            }
            if (mensaje !== "") {
                toastr.error(mensaje, "Mensaje Servidor");
            }
        },
        complete: function () {
            //$.LoadingOverlay("hide");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mensaje = false;

        }
    });
    return mensaje;
}
