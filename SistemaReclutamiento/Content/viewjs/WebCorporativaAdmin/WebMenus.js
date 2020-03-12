var WebMenus = function () {
    var _ListarMenus = function () {
        var dataForm={
            menu_id:1,
        }
        responseSimple({
            url: "WebCorporativaAdmin/WebElementoListarxMenuIDJson",
            refresh: false,
            data:JSON.stringify(dataForm),
            callBackSuccess: function (response) {
                var rows=response.data;
                var tr='';
                if(rows[0].elemento!=null){
                    var elemento=rows[0].elemento;
                    $("#TableTipo1").html("");
                    var detalle=rows[0].detalle;
                   
                    $.each(detalle,function(index,value){
                        var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + value.fk_elemento +'" data-tipo='+elemento.fk_tipo_elemento+'><i class="ace-icon fa fa-pencil"></i> </button>' +
                        ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + value.fk_elemento+'" data-tipo='+elemento.fk_tipo_elemento+'><i class="ace-icon fa fa-trash"></i> </button>';
                        var clase_estado = 'success';
                        var estado = "Activo";
                        if (value.detel_estado == "I") {
                            clase_estado = 'danger';
                            estado = "Inactivo";
                        };
                        var posicion = '';
                        var tipo = elemento.fk_tipo_elemento;
                        var clasedetalle = "grey";
                        var clasedetalleboton = "";
                        var columan_imagen = "";
                        var columna_imagen_detalle = "";
                        var detalle = '<div class="action-buttons">' +
                        '<a data-id="' + value.detel_id + '" data-seccion="' + value.detel_id + '" href="javascript:void(0);" class="' + clasedetalle + ' bigger-140 ' + clasedetalleboton+'" title = "Detalle">' +
                        '<i class="ace-icon fa fa-angle-double-up"></i>' +
                        '<span class="sr-only">Detalle</span>' +
                        '</a>' +
                        '</div>';
                        var nombre = value.detel_titulo;
                        var nombre_detalle=value.detel_imagen_detalle;
                        if (value.detel_imagen == "") {
                            nombre = "";
                        }
                        if(value.detel_imagen!=""){
                            nombre=value.detel_imagen;
                        }

                        tr += '<tr class="elem_' + value.fk_elemento + '"  data-id="' + value.detel_id + '" data-orden="' + value.detel_orden 
                        + '"><td class="center">' + detalle + '</td><td><span class="detelem_orden label label-white middle label-default">' 
                        + (index + 1) + '</span> ' + value.detel_titulo + '</td><td class="' + columan_imagen + '">' 
                        + nombre + '</td><td class="' + columna_imagen_detalle + '" >' + nombre_detalle + '</td><td><span class="label label-' 
                        + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';

                    });
                    var columan_imagen = "";
                    var columna_imagen_detalle = "";
                    if (detalle.length > 0) {
                        tr = '<thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' + columan_imagen 
                        + '">Imagen</th><th style="width: 12%;" class="' + columna_imagen_detalle 
                        + '">Detalle</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody class="tbody_detalle_elemento_' 
                        + elemento.elem_id + '">' + tr + '</tbody>';
                        $('#TableTipo1').html(
                          tr);
                    }
                    else {
                        tr = '<thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' 
                        + columan_imagen + '">Imagen</th><th style="width: 12%;" class="' + columna_imagen_detalle 
                        + '">Detalle</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody><tr><td colspan="6"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tr></tbody>';
                        $('#TableTipo1').html('<td colspan="5"><div class="table-detail">' +tr+'</div></td>');
                    }
             
                    

                }
            }
        });
    };
    var _ListarDetalleElementos = function (elemento_id,fk_tipo) {
        var dataForm = {
            elem_id: elemento_id
        };
        responseSimple({
            url: "WebCorporativaAdmin/WebDetalleElementoListarxElementoIDJson",
            data: JSON.stringify(dataForm),
            refresh: false,
            callBackSuccess: function (response) {
                var detalle=response.data;
                var tr='';
                $("#TableTipo"+fk_tipo).html("");
                $.each(detalle,function(index,value){
                    var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + value.fk_elemento +'" data-tipo='+fk_tipo+'><i class="ace-icon fa fa-pencil"></i> </button>' +
                    ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + value.fk_elemento+'" data-tipo='+fk_tipo+'><i class="ace-icon fa fa-trash"></i> </button>';
                    var clase_estado = 'success';
                    var estado = "Activo";
                    if (value.detel_estado == "I") {
                        clase_estado = 'danger';
                        estado = "Inactivo";
                    };
                    var clasedetalle = "grey";
                    var clasedetalleboton = "";
                    var columan_imagen = "";
                    var columna_imagen_detalle = "";
                    var detalle = '<div class="action-buttons">' +
                    '<a data-id="' + value.detel_id + '" data-seccion="' + value.detel_id + '" href="javascript:void(0);" class="' + clasedetalle + ' bigger-140 ' + clasedetalleboton+'" title = "Detalle">' +
                    '<i class="ace-icon fa fa-angle-double-up"></i>' +
                    '<span class="sr-only">Detalle</span>' +
                    '</a>' +
                    '</div>';
                    var nombre = value.detel_titulo;
                    var nombre_detalle=value.detel_imagen_detalle;
                    if (value.detel_imagen == "") {
                        nombre = "";
                    }
                    if(value.detel_imagen!=""){
                        nombre=value.detel_imagen;
                    }

                    tr += '<tr class="elem_' + value.fk_elemento + '"  data-id="' + value.detel_id + '" data-orden="' + value.detel_orden 
                    + '"><td class="center">' + detalle + '</td><td><span class="detelem_orden label label-white middle label-default">' 
                    + (index + 1) + '</span> ' + value.detel_titulo + '</td><td class="' + columan_imagen + '">' 
                    + nombre + '</td><td class="' + columna_imagen_detalle + '" >' + nombre_detalle + '</td><td><span class="label label-' 
                    + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';

                });
                var columan_imagen = "";
                var columna_imagen_detalle = "";
                if (detalle.length > 0) {
                    tr = '<thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' + columan_imagen 
                    + '">Imagen</th><th style="width: 12%;" class="' + columna_imagen_detalle 
                    + '">Detalle</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody class="tbody_detalle_elemento_' 
                    + elemento_id + '">' + tr + '</tbody>';
                    $('#TableTipo'+fk_tipo).html(
                        tr);
                }
                else {
                    tr = '<thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' 
                    + columan_imagen + '">Imagen</th><th style="width: 12%;" class="' + columna_imagen_detalle 
                    + '">Detalle</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody><tr><td colspan="6"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tr></tbody>';
                    $('#TableTipo'+fk_tipo).html('<td colspan="5"><div class="table-detail">' +tr+'</div></td>');
                }
            }
        });
    };
  
    var _componentes = function () {
       $(document).on('click','.btnNuevo',function(){
            var menu_id=$(this).data("id");
            var fk_tipo=$(this).data("tipo");
            $("#fk_tipo").val(fk_tipo);
            $("#menu_id").val(menu_id);
            $("#detel_imagen").val("");
            $("#detel_imagen_detalle").val("");
            /**Ocultando inputs */
            $("#div_parrafo_detalleelemento").html("");
            var input = '<input type="text" name="detel_parrafo" id="detel_parrafo" class="form-control" placeholder="Texto">';
            var textarea = '<textarea name="detel_parrafo" id="detel_parrafo" class="form-control"></textarea>';
            $("#div_parrafo_detalleelemento").html(input);
            $("#tituloModalDetalleElemento").text("Nuevo");
            $("#detel_id").val(0);
            $("#detel_nombre_imagen").text("");
            $("#detel_nombre_imagen_detalle").val("");
            $("#spandetel_detalle").html('<i class="fa fa-upload"></i>  Subir Imagen');
            $("#spandetel").html('<i class="fa fa-upload"></i>  Subir Imagen');
            $("#detel_titulo").val("");
            $("#detel_subtitulo").val("");
            $("#detel_parrafo").val("");
            $("#detel_estado").val("A");
            $("#divdetel").hide();
            $("#divdetel_detalle").hide();
         
            //menu SALAS, DEPARTAMENTO
            if(fk_tipo==1){
                $(".detel-titulo").hide();
                $(".detel-subtitulo").hide();
                $(".detel-parrafo").hide();
                $(".detel-imagen-detalle").show();
            }
            //Menu APUESTAS DEPORTIVAS, SLIDER SUPERIOR
            else if(fk_tipo==2)
            {
                //hide
                $(".detel-subtitulo").hide();
                $(".detel-parrafo").hide();
                $(".detel-imagen-detalle").hide();
                //show
                $(".detel-titulo").show();
            }
            //Menu APUESTAS DEPORTIVAS, IMAGEN INFERIOR
            else if(fk_tipo==3){
                  //hide
                  $(".detel-subtitulo").hide();
                  $(".detel-parrafo").hide();
                  $(".detel-imagen-detalle").hide();
                  $(".detel-titulo").hide();
            }
            //Menu NOTICIAS, SLIDER SUPERIOR
            else if(fk_tipo==4){
                $(".detel-subtitulo").hide();
                $(".detel-parrafo").hide();
                $(".detel-imagen-detalle").hide();
                //show
                $(".detel-titulo").show();
            }
            //Menu NOTICIAS, IMAGEN INFERIOR
            else if(fk_tipo==5){
                //hide
                $(".detel-subtitulo").hide();
                $(".detel-parrafo").hide();
                $(".detel-imagen-detalle").hide();
                $(".detel-titulo").hide();
            }
            //Menu CONOCENOS, SLIDER SUPERIOR
            else if(fk_tipo==6){
                //hide
                $(".detel-parrafo").hide();
                $(".detel-imagen-detalle").hide();
                //show
                $(".detel-titulo").show();
                $(".detel-subtitulo").show();
            }
            //Menu CONOCENOS, SLIDER INFERIOR
            else if(fk_tipo==7){
                //hide
                $(".detel-imagen-detalle").hide();
                //show
                $(".detel-titulo").show();
                $(".detel-subtitulo").show();

                $("#div_parrafo_detalleelemento").html(textarea);
                $('#detel_parrafo').richText({
                    imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                    fileUpload: false, urls: false
                });
                $("a.richText-help").hide();
                $(".detel-parrafo").show();
            }
            $("#modalFormularioDetalleElemento").modal("show");    
       });

       $(document).on('click', '.btn_guardar_detalle_elemento', function () {
        //$("#form_detalle_elemento").submit();
        // var elemento_id = $("#fk_elemento").val();
        var tipo_elemento=   $("#fk_tipo").val();
        if (tipo_elemento == 1) {
            if ($("#detel_imagen").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Selecione Imagen',
                    type: "error"
                });
                return false;
            }
            if ($("#detel_imagen_detalle").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Selecione Imagen para Detalle',
                    type: "error"
                });
                return false;
            }
        }

        if (tipo_elemento == 2||tipo_elemento==4) {
            if ($("#detel_titulo").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Titulo es obligatorio',
                    type: "error"
                });
                return false;
            }
            if ($("#detel_imagen").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Seleccione Imagen',
                    type: "error"
            });
                return false;
            }
        }
        if (tipo_elemento == 3||tipo_elemento==5) {
            if ($("#detel_imagen").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Seleccione Imagen',
                    type: "error"
            });
                return false;
            }
        }

        if (tipo_elemento == 6) {
            if ($("#detel_titulo").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Titulo es obligatorio',
                    type: "error"
                });
                return false;
            }
            if ($("#detel_subtitulo").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Subtitulo es obligatorio',
                    type: "error"
                });
                return false;
            }
            if ($("#detel_imagen").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Selecione Imagen',
                    type: "error"
                });

                return false;
            }
        }

        if (tipo_elemento == 7) {
            if ($("#detel_titulo").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Titulo es obligatorio',
                    type: "error"
                });
                return false;
            }
            if ($("#detel_subtitulo").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Subtitulo es obligatorio',
                    type: "error"
                });
                return false;
            }
            if ($("#detel_parrafo").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Parrafo es obligatorio',
                    type: "error"
                });
                return false;
            }
            if ($("#detel_imagen").val() == "" && $("#tituloModalDetalleElemento").text() == "Nuevo") {
                messageResponse({
                    text: 'Selecione Imagen',
                    type: "error"
                });

                return false;
            }
        }
        var dataForm = new FormData(document.getElementById("form_detalle_elemento"));
        var menu_id=$("#menu_id").val();
        dataForm.append('menu_id', menu_id);
        var url = '';
        if ($("#detel_id").val() == 0) {
            url = 'WebCorporativaAdmin/WebDetalleElementoInsertarJson';
        }
        else {
            url = 'WebCorporativaAdmin/WebDetalleElementoEditarJson';
        }
        responseFileSimple({
            url: url,
            data: dataForm,
            refresh: false,
            callBackSuccess: function (response) {
                if (response.respuesta) {
                    var idInsertado=response.data;
                    //idInsertado es el Elemento al que corresponde el detalle
                     WebMenus.init_ListarDetalleElementos(idInsertado,tipo_elemento);
                    $("#modalFormularioDetalleElemento").modal("hide");
                }
            }
        })
        });
       $(document).on('click','#tabmenu li',function(){
            // for(var i =1;i<=7;i++){
            //     $("TableTipo"+i).html("");
            // }
            var menu_id = $(this).data("menu_id");
            var dataForm={
                menu_id:menu_id
            }
            responseSimple({
                url: "WebCorporativaAdmin/WebElementoListarxMenuIDJson",
                refresh: false,
                data:JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    var rows=response.data;
                    var tr='';
                    if(rows[0]!=null){
                        var elementoSuperior=rows[0].elemento;
                        if(rows[0].detalle!=null&&rows[0].detalle.length>0){
                            $('#TableTipo'+elementoSuperior.fk_tipo_elemento).html("");
                            var detalleSuperior=rows[0].detalle;
                            $.each(detalleSuperior,function(index,value){
                                var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + value.fk_elemento +'" data-tipo='+elementoSuperior.fk_tipo_elemento+'><i class="ace-icon fa fa-pencil"></i> </button>' +
                                ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + value.fk_elemento+'" data-tipo='+elementoSuperior.fk_tipo_elemento+'><i class="ace-icon fa fa-trash"></i> </button>';
                                var clase_estado = 'success';
                                var estado = "Activo";
                                if (value.detel_estado == "I") {
                                    clase_estado = 'danger';
                                    estado = "Inactivo";
                                };
                                var clasedetalle = "grey";
                                var clasedetalleboton = "";
                                var columan_imagen = "";
                                var columna_imagen_detalle = "";
                                var detalle = '<div class="action-buttons">' +
                                '<a data-id="' + value.detel_id + '" data-seccion="' + value.detel_id + '" href="javascript:void(0);" class="' + clasedetalle + ' bigger-140 ' + clasedetalleboton+'" title = "Detalle">' +
                                '<i class="ace-icon fa fa-angle-double-up"></i>' +
                                '<span class="sr-only">Detalle</span>' +
                                '</a>' +
                                '</div>';
                                var nombre = value.detel_titulo;
                                var nombre_detalle=value.detel_imagen_detalle;
                                if (value.detel_imagen == "") {
                                    nombre = "";
                                }
                                if(value.detel_imagen!=""){
                                    nombre=value.detel_imagen;
                                }
                                if(value.detel_imagen_detalle==""){
                                    nombre_detalle=value.detel_subtitulo;
                                }
                                if(value.detel_imagen_detalle!=""){
                                    nombre_detalle=value.detel_imagen_detalle;
                                }
        
                                tr += '<tr class="elem_' + value.fk_elemento + '"  data-id="' + value.detel_id + '" data-orden="' + value.detel_orden 
                                + '"><td class="center">' + detalle + '</td><td><span class="detelem_orden label label-white middle label-default">' 
                                + (index + 1) + '</span> ' + value.detel_titulo + '</td><td class="' + columan_imagen + '">' 
                                + nombre + '</td><td class="' + columna_imagen_detalle + '" >' + nombre_detalle + '</td><td><span class="label label-' 
                                + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
        
                            });
                            var columan_imagen = "";
                            var columna_imagen_detalle = "";
                            if (detalleSuperior.length > 0) {
                                tr = '<thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' + columan_imagen 
                                + '">Imagen</th><th style="width: 12%;" class="' + columna_imagen_detalle 
                                + '">Detalle</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody class="tbody_detalle_elemento_' 
                                + elementoSuperior.elem_id + '">' + tr + '</tbody>';
                                $('#TableTipo'+elementoSuperior.fk_tipo_elemento).html(
                                  tr);
                            }
                            else {
                                tr = '<thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' 
                                + columan_imagen + '">Imagen</th><th style="width: 12%;" class="' + columna_imagen_detalle 
                                + '">Detalle</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody><tr><td colspan="6"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tr></tbody>';
                                $('#TableTipo'+elementoSuperior.fk_tipo_elemento).html('<td colspan="5"><div class="table-detail">' +tr+'</div></td>');
                            }
                        }
                        else{
                            console.log(elementoSuperior.elem_contenido+"no tiene detalle");
                        }
                    }
                    else{
                        console.log("no hay elemento 1");
                    }
                    if(rows[1]!=null){
                        tr='';
                        var elementoInferior=rows[1].elemento;
                        if(rows[1].detalle!=null&&rows[1].detalle.length>0){
                            $('#TableTipo'+elementoInferior.fk_tipo_elemento).html("");
                            var detalleInferior=rows[1].detalle;
                            $.each(detalleInferior,function(index,value){
                                var boton = '<button data-rel="tooltip" title="Editar" class="btn btn-primary  btn-xs btn-round btn-white btn_editar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + value.fk_elemento +'" data-tipo='+elementoInferior.fk_tipo_elemento+'><i class="ace-icon fa fa-pencil"></i> </button>' +
                                ' <button data-rel="tooltip" title="Eliminar" class="btn btn-danger  btn-xs btn-round btn-white btn_eliminar_detalle_elemento" data-id="' + value.detel_id + '" data-elemento_id="' + value.fk_elemento+'" data-tipo='+elementoInferior.fk_tipo_elemento+'><i class="ace-icon fa fa-trash"></i> </button>';
                                var clase_estado = 'success';
                                var estado = "Activo";
                                if (value.detel_estado == "I") {
                                    clase_estado = 'danger';
                                    estado = "Inactivo";
                                };
                                var clasedetalle = "grey";
                                var clasedetalleboton = "";
                                var columan_imagen = "";
                                var columna_imagen_detalle = "";
                                var detalle = '<div class="action-buttons">' +
                                '<a data-id="' + value.detel_id + '" data-seccion="' + value.detel_id + '" href="javascript:void(0);" class="' + clasedetalle + ' bigger-140 ' + clasedetalleboton+'" title = "Detalle">' +
                                '<i class="ace-icon fa fa-angle-double-up"></i>' +
                                '<span class="sr-only">Detalle</span>' +
                                '</a>' +
                                '</div>';
                                var nombre = value.detel_titulo;
                                var nombre_detalle=value.detel_imagen_detalle;
                                if (value.detel_imagen == "") {
                                    nombre = "";
                                }
                                if(value.detel_imagen!=""){
                                    nombre=value.detel_imagen;
                                }
                                if(value.detel_imagen_detalle==""){
                                    nombre_detalle=value.detel_subtitulo;
                                }
                                if(value.detel_imagen_detalle!=""){
                                    nombre_detalle=value.detel_imagen_detalle;
                                }
        
                                tr += '<tr class="elem_' + value.fk_elemento + '"  data-id="' + value.detel_id + '" data-orden="' + value.detel_orden 
                                + '"><td class="center">' + detalle + '</td><td><span class="detelem_orden label label-white middle label-default">' 
                                + (index + 1) + '</span> ' + value.detel_titulo + '</td><td class="' + columan_imagen + '">' 
                                + nombre + '</td><td class="' + columna_imagen_detalle + '" >' + nombre_detalle + '</td><td><span class="label label-' 
                                + clase_estado + ' label-white middle">' + estado + '</span></td><td>' + boton + '</td></tr>';
        
                            });
                            var columan_imagen = "";
                            var columna_imagen_detalle = "";
                            if (detalleInferior.length > 0) {
                                tr = '<thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' + columan_imagen 
                                + '">Imagen</th><th style="width: 12%;" class="' + columna_imagen_detalle 
                                + '">Detalle</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody class="tbody_detalle_elemento_' 
                                + elementoInferior.elem_id + '">' + tr + '</tbody>';
                                $('#TableTipo'+elementoInferior.fk_tipo_elemento).html(
                                  tr);
                            }
                            else {
                                tr = '<thead><tr><th style="width: 5%;"></th><th>Texto</th><th class="' 
                                + columan_imagen + '">Imagen</th><th style="width: 12%;" class="' + columna_imagen_detalle 
                                + '">Detalle</th><th style="width: 10%;">Estado</th><th style="width: 12%;">Acciones</th></tr></thead><tbody><tr><td colspan="6"><div class="alert alert-warning" style="margin-bottom:0px;">No tiene Data ...</div></td></tr></tbody>';
                                $('#TableTipo'+elementoInferior.fk_tipo_elemento).html('<td colspan="5"><div class="table-detail">' +tr+'</div></td>');
                            }
                        }
                        else{
                        console.log(elementoInferior.elem_contenido+"no tiene detalle");
                            
                        }
                    }
                    else{
                        console.log("no hay elemento 2");
                    }
                }
            });
        });
        $(document).on("click", ".btn_eliminar_detalle_elemento", function (e) {
            var det_elemento_id = $(this).data("id");
            var elemento_id = $(this).data("elemento_id");
            var tipo=$(this).data("tipo");
            if (det_elemento_id != "" || det_elemento_id > 0) {
                messageConfirmation({
                    content: '¿Esta seguro que desea ELIMINAR este Detalle de Elemento?',
                    callBackSAceptarComplete: function () {
                        responseSimple({
                            url: "WebDetalleElemento/WebDetalleElementoEliminarJson",
                            data: JSON.stringify({ detel_id: det_elemento_id }),
                            refresh: false,
                            callBackSuccess: function (response) {
                                if (response.respuesta) {
                                    WebMenus.init_ListarDetalleElementos(elemento_id,tipo);
                                }
                            }
                        });
                    }
                });
            }
            else {
                messageResponse({
                    text: "Error no se encontro ID",
                    type: "error"
                })
            }
        });
        $(document).on('click', '.btn_editar_detalle_elemento', function (e) {
            var det_elemento_id = $(this).data("id");
            var tipo=$(this).data("tipo");
            $("#fk_tipo").val(tipo);
            $("#tituloModalDetalleElemento").text("Editar");
            var dataForm = {
                detel_id: det_elemento_id,
            }
            responseSimple({
                url: 'WebDetalleElemento/WebDetalleElementoIdObtenerJson',
                refresh: false,
                data: JSON.stringify(dataForm),
                callBackSuccess: function (response) {
                    var data = response.data;
                    if (response.respuesta) {
                        $("#detel_imagen").val("");
                        $("#detel_imagen_detalle").val("");

                        $("#divdetel").hide();
                        $("#divdetel_detalle").hide();
                        $(".detel-titulo").hide();
                        $(".detel-subtitulo").hide();
                        $(".detel-parrafo").hide();
                        $("#divdetel_detalle").hide();
                        $(".detel-imagen-detalle").hide();

                        $('#detel_id').val(data.detel_id);
                        $('#fk_elemento').val(data.fk_elemento);
                        $("#fk_tipo").val(data.fk_tipo);
                        $("#spandetel").html("");
                        $("#spandetel").append('<i class="fa fa-upload"></i>  Subir Imagen');
                        $("#spandetel_detalle").html("");
                        $("#spandetel_detalle").append('<i class="fa fa-upload"></i>  Subir Detalle');
                        if (data.fk_tipo == 1||data.fk_tipo==2||data.fk_tipo==3||data.fk_tipo==4||data.fk_tipo==5||data.fk_tipo==6||data.fk_tipo==7)
                         {
                            $(".detel-imagen").show();
                            $("#detel_nombre_imagen_modal").text("Nombre: " + data.detel_imagen);
                            $("#detel_nombre_imagen").val(data.detel_imagen);
                            $("#icono_actual_detel").attr("src", basePath+"WebFiles/" + data.detel_imagen);
                            $("#divdetel").show();
                            
                            if(data.fk_tipo==2||data.fk_tipo==4){
                                $(".detel-titulo").show();
                            }
                            else if(data.fk_tipo==6){
                                $(".detel-titulo").show();
                                $(".detel-subtitulo").show();
                                
                            }
                            else if(data.fk_tipo==7){
                                $(".detel-titulo").show();
                                $(".detel-subtitulo").show();
                                $(".detel-parrafo").show();
                                
                            }
                            else if(data.fk_tipo==1){
                                $("#detel_nombre_imagen_detalle_modal").text("Nombre: " + data.detel_imagen_detalle);
                                $("#detel_nombre_imagen_detalle").val(data.detel_imagen_detalle);
                                $("#icono_actual_detel_detalle").attr("src", basePath+"WebFiles/" + data.detel_imagen_detalle);
                                $("#divdetel_detalle").show();
                                $(".detel-imagen-detalle").show();
                            }
                        }
                        $("#div_parrafo_detalleelemento").html("");
                        var input = '<input type="text" name="detel_parrafo" id="detel_parrafo" class="form-control" placeholder="Texto">';
                        var textarea = '<textarea name="detel_parrafo" id="detel_parrafo" class="form-control"></textarea>';
                        $("#div_parrafo_detalleelemento").html(input);

                        if (data.fk_tipo == 6) {
                            $("#div_parrafo_detalleelemento").html(textarea);
                            $("#detel_parrafo").val(data.detel_parrafo);
                            $('#detel_parrafo').richText({
                                imageUpload: false, table: false, removeStyles: false, videoEmbed: false, height: "120",
                                fileUpload: false, urls: false
                            });
                            $("a.richText-help").hide();
                        }

                        
                        $("#detel_estado").val(data.detel_estado);
                        $("#detel_titulo").val(data.detel_titulo);
                        $("#detel_subtitulo").val(data.detel_subtitulo);
                        $("#detel_parrafo").val(data.detel_parrafo);
                        $("#detel_orden").val(data.detel_orden);
                        $('#modalFormularioDetalleElemento').modal('show');
                    }
                },
            })

        });
        $(document).on("change", "#detel_imagen", function () {
            var _image = $('#detel_imagen')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                //console.log(extension);
                if (extension!= "jpg" && extension != "png" && extension != "jpeg") {
                    messageResponse({
                        text: 'Sólo Se Permite formato jpg, png ó jpeg',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 11);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "png" || actualicon == "jpg" || actualicon == 'jpeg') {
                        icon = '<i class="fa fa-file-word-o"></i>';
                    }
                    else {
                        icon = '<i class="fa fa-file-pdf-o"></i>';
                    };
                    $("#spandetel").html("");
                    $("#spandetel").append(icon + " " + nombre + "... ." + actualicon);
                    $("#spandetel").css({ 'font-size': '10px' });
                }
            }
            else {
                $("#spandetel").html("");
                $("#spandetel").append('<i class="fa fa-upload"></i>  Subir Imagen');
            }
        });
        $(document).on("change", "#detel_imagen_detalle", function () {
            var _image = $('#detel_imagen_detalle')[0].files[0];
            if (_image != null) {
                var image_arr = _image.name.split(".");
                var extension = image_arr[1].toLowerCase();
                //console.log(extension);
                if (extension!= "jpg" && extension != "png" && extension != "jpeg") {
                    messageResponse({
                        text: 'Sólo Se Permite formato jpg, png ó jpeg',
                        type: "warning"
                    });
                }
                else {
                    var nombre = image_arr[0].substring(0, 11);
                    var actualicon = image_arr[1].toLowerCase();
                    var icon = "";
                    if (actualicon == "png" || actualicon == "jpg" || actualicon == 'jpeg') {
                        icon = '<i class="fa fa-file-word-o"></i>';
                    }
                    else {
                        icon = '<i class="fa fa-file-pdf-o"></i>';
                    };
                    $("#spandetel_detalle").html("");
                    $("#spandetel_detalle").append(icon + " " + nombre + "... ." + actualicon);
                    $("#spandetel_detalle").css({ 'font-size': '10px' });
                }
            }
            else {
                $("#spandetel_detalle").html("");
                $("#spandetel_detalle").append('<i class="fa fa-upload"></i>  Subir Imagen');
            }
        });
    }
    var _metodos = function () {
       
        validar_Form({
            nameVariable: 'form_elemento',
            contenedor: '#form_elemento',
            rules: {
                fk_tipo_elemento:
                {
                    required: true,

                },
                elem_estado:
                {
                    required: true,

                }
            },
            messages: {
                fk_tipo_elemento:
                {
                    required: 'Campo Obligatorio',
                },
                elem_estado:
                {
                    required: 'Campo Obligatorio',
                }
            }
        });
    }
    return {
        init: function () {
            _componentes();
            _metodos();
            _ListarMenus();
        },
        init_ListarDetalleElementos: function (elemento_id,fk_tipo) {
            _ListarDetalleElementos(elemento_id,fk_tipo);
        },
       
    }
}();
document.addEventListener('DOMContentLoaded', function () {
    WebMenus.init();
})