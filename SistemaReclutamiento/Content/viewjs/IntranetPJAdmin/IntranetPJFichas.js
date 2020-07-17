var PanelFichas = function () {
    var empresas='';
    var listadoEmpleados='';
    var _ListarFichas = function (empresa,sede) {

        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        var dataForm = { empresa, sede };
        // url: "IntranetFichas/IntranetFichasEmviarLink",
        // responseSimple({
        //    url: "SQL/IntranetFichasEmviarLink",
        //    refresh: false,
        //    data: dataForm,
        //    callBackSuccess: function (response) {
        //        simpleDataTable({
        //            uniform: false,
        //            tableNameVariable: "datatable_fichasListado",
        //            table: "#fichasenvioListado",
        //            tableColumnsData: response.data,
        //            tableHeaderCheck: true,
        //            tableHeaderCheckIndex: 0,
        //            headerCheck: "chk_fichas",
        //            tableColumns: [
        //                {
        //                    data: "id",
        //                    title: "",
        //                    "bSortable": false,
        //                    className: 'align-center',
        //                    "render": function (value) {
        //                        var check = '<input type="checkbox" class="form-check-input-styled-info fichasListado" data-id="' + value + '" name="chk[]">';
        //                        return check;
        //                    },
        //                    width: "50px",
        //                },
        //                {
        //                    data: "nombre",
        //                    title: "Nombre Empleado",
        //                },
        //                {
        //                    data: "empresa",
        //                    title: "Empresa",
        //                },
        //                {
        //                    data: "sede",
        //                    title: "Sede",
        //                },
        //                {
        //                    data: "correoCorporativo",
        //                    title: "C.Corporativo",
        //                },
        //                {
        //                    data: "correoPersonal",
        //                    title: "C.Personal",
        //                },
        //            ]
        //        })
        //    }
        // });
    };

    var _inicio = function () {
        selectResponse({
           url: "SQL/TMEMPRListarJson",
           select: "cbo_empresa",
           campoID: "CO_EMPR",
           CampoValor: "DE_NOMB",
           select2: true,
           allOption: false,
        //    placeholder: "Seleccione Empresa"
        });
        $("#cbo_empresa").select2({
            multiple:true,
            placeholder:'Seleccione Empresa'
        })
        // responseSimple({
        //     url: "SQL/TMEMPRListarJson",
        //     refresh: false,
        //     callBackSuccess: function (response) {
        //         console.log(response);
        //     }
        // })
    }
    var _componentes = function () {
        // $("#cbo_empresa").change(function(){
        // })
        $(document).on('change','#cbo_empresa',function(){
            empresas=$(this).val();
            var dataForm={
                listaEmpresas:empresas
            }
            selectResponse({
                url: "SQL/TTSEDEListarporEmpresaJson",
                select: "cbo_sede",
                campoID: "CO_SEDE",
                CampoValor: "DE_SEDE",
                data:dataForm,
                select2: true,
                allOption: false,
             //    placeholder: "Seleccione Empresa"
             });
             $("#cbo_sede").select2({
                multiple:true,
                placeholder:'Seleccione Empresa'
            })
            // responseSimple({
            //     url:'SQL/TTSEDEListarporEmpresaJson',
            //     refresh:false,
            //     data:JSON.stringify(dataForm),
            //     callBackSuccess:function(response){
            //         console.log(response);
            //     }
            // });
        });
        $(document).on('change',"#cbo_sede",function(){
            var sedes=$(this).val();
            var dataForm={
                listaEmpresas:empresas,
                listaSedes:sedes
            }
            responseSimple({
                url:'SQL/PersonaListarFichasJson',
                data:JSON.stringify(dataForm),
                refresh:false,
                callBackSuccess:function(response){
                    if(response.respuesta){
                        listadoEmpleados=response.data;
                        simpleDataTable({
                            uniform: false,
                            tableNameVariable: "datatable_fichasListado",
                            table: "#fichasenvioListado",
                            tableColumnsData: response.data,
                            tableHeaderCheck: true,
                            tableHeaderCheckIndex: 0,
                            headerCheck: "chk_fichas",
                            tableColumns: [
                                {
                                    data: "id",
                                    title: "",
                                    "bSortable": false,
                                    className: 'align-center',
                                    "render": function (value) {
                                        var check = '<input type="checkbox" class="form-check-input-styled-info fichasListado" data-id="' + value + '" name="chk[]">';
                                        return check;
                                    },
                                    width: "50px",
                                },
                                {
                                    data: "nombre",
                                    title: "Nombre Empleado",
                                },
                                {
                                    data: "empresa",
                                    title: "Empresa",
                                },
                                {
                                    data: "sede",
                                    title: "Sede",
                                },
                                {
                                    data: "correoCorporativo",
                                    title: "C.Corporativo",
                                },
                                {
                                    data: "correoPersonal",
                                    title: "C.Personal",
                                },
                                {
                                    data:"id",
                                    tittle:'Estado Correo',
                                    "render":function(value,type,oData){
                                        var span = '';
                                        var correoCorporativo = oData.correoCorporativo;
                                        var correoPersonal=oData.correoPersonal;
                                        var clase='';
                                        var estado='';
                                        if(correoCorporativo==''&&correoPersonal==''){
                                            clase='danger';
                                            estado='No Verificado';
                                        }
                                        else{
                                            clase='success';
                                            estado='verificado';
                                        }
                                     
                                        return '<span class="label label-'+clase+'">'+estado+'</span>';
                                    }
                                }
                            ]
                        });
                    }
                }
            })
            console.log(listadoEmpleados);
        })
    };

    var _metodos = function () {
        $('#txt-firma').ace_file_input({
            no_file: 'sin archivo ...',
            btn_choose: 'escoger',
            btn_change: 'cambiar',
            droppable: false,
            onchange: null,
            thumbnail: false //| true | large
            //whitelist:'gif|png|jpg|jpeg'
            //blacklist:'exe|php'
            //onchange:''
            //
        });

        var dateinicio = new Date(moment().format("MM-DD-YYYY"));
        $('#txt-fecha').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })

    };

    //
    // Return objects assigned to module
    //
    return {
        init: function () {
            _inicio();
            _ListarFichas();
            _componentes();
            _metodos();

        },
        init_ListarFichas: function () {
            _ListarFichas();
        }
    }
}();

// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function () {
    PanelFichas.init();
});