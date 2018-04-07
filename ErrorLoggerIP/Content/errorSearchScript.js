function errorSearch(n) {
    var input, filter, table, tr, td, i;
    switch (n) {
        case 0:
            input = document.getElementById("logLevelInput");
            break;
        case 1:
            input = document.getElementById("errDescinput");
            break;
        case 2:
            input = document.getElementById("timestampInput");
            break;
        case 3:
            input = document.getElementById("exceptionInput");
            break;
        default:
            input = document.getElementById("logLevelInput");
            break;
    }
    for (var i = 0 ; i < 4 ; i++) {
        if(i !=n )
            clearThis(i)
    }
    filter = input.value.toUpperCase();
    table = document.getElementById("ErrorTable");
    tr = table.getElementsByTagName("tr");

    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[n];
        if (td) {
            if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

/*Function to clear other text box*/
function clearThis(n) {
    var input;
    switch (n) {
        case 0:
            input = document.getElementById("logLevelInput");
            break;
        case 1:
            input = document.getElementById("errDescinput");
            break;
        case 2:
            input = document.getElementById("timestampInput");
            break;
        case 3:
            input = document.getElementById("exceptionInput");
            break;
        
    }
    input.value = ""
}

function drawPiechart (info,debug,error,performace,fatal) {
    var myCanvas = document.getElementById("ErrorCanvas");
    var myLegend = document.getElementById("ErrorLegend");
    myCanvas.width = 300;
    myCanvas.height = 300;

    var myVinyls = {
        "Info": info,
        "Debug": debug,
        "Error": error,
        "Performace": performace,
        "Fatal":fatal
    };

    var myPiechart = new Piechart(
    {
        canvas: myCanvas,
        data: myVinyls,
        colors: ["#ff7518", "#ea4335", "#fbbc05", "#4285f4", "#34a853"],
        legend: myLegend
    }
    );
    myPiechart.draw();
};

function drawPieSlice(ctx, centerX, centerY, radius, startAngle, endAngle, color) {
    ctx.fillStyle = color;
    ctx.beginPath();
    ctx.moveTo(centerX, centerY);
    ctx.arc(centerX, centerY, radius, startAngle, endAngle);
    ctx.closePath();
    ctx.fill();
}
var Piechart = function (options) {
    this.options = options;
    this.canvas = options.canvas;
    this.ctx = this.canvas.getContext("2d");
    this.colors = options.colors;

    this.draw = function () {
        var total_value = 0;
        var color_index = 0;
        for (var categ in this.options.data) {
            var val = this.options.data[categ];
            total_value += val;
        }//getting total value

        var start_angle = 0;
        for (categ in this.options.data) {
            val = this.options.data[categ];
            var slice_angle = 2 * Math.PI * val / total_value;

            drawPieSlice(
                this.ctx,
                this.canvas.width / 2,
                this.canvas.height / 2,
                Math.min(this.canvas.width / 2, this.canvas.height / 2),
                start_angle,
                start_angle + slice_angle,
                this.colors[color_index % this.colors.length]
            );

            start_angle += slice_angle;
            color_index++;
        }
        if (this.options.legend) {
            color_index = 0;
            var legendHTML = "";
            for (categ in this.options.data) {
                val = this.options.data[categ];
                var percentage = Math.round(100 * val / total_value);
                legendHTML += "<div><span style='display:inline-block;width:20px;background-color:" + this.colors[color_index++] + ";'>&nbsp;</span> " +"<b>" +categ +"</b>"+ " = " + "<b>"+percentage + "% ("+this.options.data[categ] + ")"+"</b>"+"</div>";
            }
            this.options.legend.innerHTML = legendHTML;
        }
    }
}
