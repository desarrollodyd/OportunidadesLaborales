var PanelFichas = function () {
    var empresas='';
    var listadoEmpleados='';
    var _ListarFichas = function (empresa,sede) {

        if (!$().DataTable) {
            console.warn('Advertencia - datatables.min.js no esta declarado.');
            return;
        }
        var dataForm = { empresa, sede };
    };

    var _inicio = function () {
        selectResponse({
           url: "SQL/TMEMPRListarJson",
           select: "cbo_empresa",
           campoID: "CO_EMPR",
           CampoValor: "DE_NOMB",
           select2: true,
           allOption: false,
           placeholder: "Seleccione Empresa"
        });

    }
    var _componentes = function () {
        // $("#cbo_empresa").change(function(){
        // })
        $(document).on('change','#cbo_empresa',function(){
            empresas=$(this).val();
            var dataForm={
                listaEmpresas:empresas
            }

            responseSimple({
                url: "SQL/TTSEDEListarporEmpresaJson",
                data: JSON.stringify(dataForm),
                refresh: false,
                callBackBeforeSend: function (response) {
                    $("#cbo_sede").html("");
                    $("#cbo_sede").append('<option value="">Cargando...</option>');
                    $("#cbo_sede").attr("disabled", "disabled");
                },
                callBackSuccess: function (response) {
                    console.log(response.data);
                    var respuesta = response.respuesta;
                    if (respuesta) {
                        var data = response.data;
                        $("#cbo_sede").html("");
                        $.each(data, function (index, value) {
                            var children = value.children;
                            $("#cbo_sede").append('<optgroup value="' + value.id + '" label="' + value.text + '">');
                            $.each(children, function (indexCh, valueCh) {
                                $("#cbo_sede").append('<option value="' + valueCh.id + '">' + valueCh.text + '</option>');
                            });
                            $("#cbo_sede").append('</optgroup>');
                            
                        });
                        $("#cbo_sede").removeAttr("disabled");

                        $('#cbo_sede').select2({
                            width: "100%",
                            multiple: true,
                            placeholder:"Seleccione"
                        });
                    }
                }
            });

            //selectResponse({
            //    url: "SQL/TTSEDEListarporEmpresaJson",
            //    select: "cbo_sede",
            //    campoID: "CO_SEDE",
            //    CampoValor: "DE_SEDE",
            //    data:dataForm,
            //    select2: true,
            //    allOption: false,
            // //    placeholder: "Seleccione Empresa"
            // });
            // $("#cbo_sede").select2({
            //    multiple:true,
            //    placeholder:'Seleccione Empresa'
            //})

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

        var dateinicio = new Date(moment().format("MM-DD-YYYY"));
        $('#txt_desde').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })

        $('#txt_hasta').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })

        $('#txt_desdep').datetimepicker({
            format: 'DD-MM-YYYY',
            ignoreReadonly: true,
            allowInputToggle: true,
            defaultDate: dateinicio
        })

        $('#txt_hastap').datetimepicker({
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