$(document).ready(function () {
    //$.when(llenarSelect(
    //    basePath + "Consorcio/ConsorcioListarJson", {}, "cboConsorcio", "COD_CONSORCIO", "CONS_DESCRIPCION", empresa.COD_CONSORCIO)).then(function (response, textStatus) {
    //        $("#cboConsorcio").select2();
    //    });


    $("#persona_nombre").text(personaIndex.per_nombre);
    $("#persona_direccion").text(personaIndex.per_direccion);
    $("#persona_apellidos").text(personaIndex.per_apellido_pat +" " +personaIndex.per_apellido_mat);
 
    
    //$(document).on('click', '#btnGuardar', function () {
    //    var validar = $("#frmNuevo");
    //    if (validar.valid()) {
    //        var dataForm = $("#frmNuevo").serializeFormJSON();
    //        var url = basePath + "Empresa/EmpresaEditarJson";
    //        fncRegistrar(dataForm, url, false);
    //    }

    //});

});