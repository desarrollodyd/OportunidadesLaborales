
    //var data = [{ "image": "abc.png", "para": "Hi..iam rohit.and this 1st card" }, { "image": "sqr.png", "para": "Hi..iam rakesh.and this 2st card" }]
    var html = '<div class="col-md-4">';
    html += '<div class="card">';    
    html += '<h4 class="card-title">Ofertas Laborales</h4>';
    html += '<div class="col-md-4 userimg"> </div>';
    html += '</div>';
    html += '</div>';
  
$(document).ready(function () {
    $.ajax({
        type: 'POST',
        data: {           
            ola_nombre: "",
            fk_ubigeo: 0,
            ola_cod_empresa: "",
            ola_cod_cargo:""
        },
        url: basePath + 'OfertaLaboral/OfertaLaboralListarJson',
        dataType: 'json',
        success: function (data) {    /* Here data length is 5*/
            console.log(data);
            for (var i = 0; i < data.data.length; i++) {
                $('#printcard').append(html);
                var $img = $("<p>" + data.data[i].ola_nombre + "</p>");
                $(".userimg:eq(" + i + ")").append($img);
            }

        }
    });
    $(document).on('click', '.btn_guardar', function () {
        var dataform = $("#frmOfertaLaboral-form").serializeFormJSON();
        $.ajax({
            type: 'POST',
            data: dataform,
            url: basePath + 'OfertaLaboral/OfertaLaboralListarJson',
            dataType: 'json',
            success: function (data) {    /* Here data length is 5*/
                console.log(data);
                $("#printcard").empty();
                for (var i = 0; i < data.data.length; i++) {
                    $('#printcard').append(html);
                    var $img = $("<p>" + data.data[i].ola_nombre + "</p>");
                    $(".userimg:eq(" + i + ")").append($img);
                }

            }
        });
    });

    });
    
