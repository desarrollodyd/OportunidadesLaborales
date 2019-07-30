
    //var data = [{ "image": "abc.png", "para": "Hi..iam rohit.and this 1st card" }, { "image": "sqr.png", "para": "Hi..iam rakesh.and this 2st card" }]
    var html = '<div class="col-md-12">';
    html += '<div class="card">';

    html += '<div class="col-md-4 userimg"> </div>';
    html += '<div class="col-md-8 px-3">';
    html += '<div class="card-block px-3">';
    html += '<h4 class="card-title">sometitle</h4>';
    html += '</div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';
  
   
$(document).ready(function () {
    var mihtml = "<p>";
                $.ajax({
                    type: 'POST',      
                    url: basePath+'OfertaLaboral/OfertaLaboralListarJson',
                    dataType: 'json',
                    success: function (data) {    /* Here data length is 5*/
                        console.log(data.data.length);

                        for (var i = 0; i < data.data.length; i++) {
                            $('#printcard').append(html);                          
                            var $img = $("<p>" + data.data[i].ola_nombre+"</p>");
                            //$img.val(data.data.ola_nombre);
                            $(".userimg:eq(" + i + ")").append($img);
                        }

                    }
                });
        });
  
