function GetDetailPerMonth() {
    debugger;
    var month = $("#month").val();
    var year = $("#AnneeSelector").val();
    window.open("/Home/PrintSalesDetail?Month=" + month + "&Year=" + year, '_blank');
}
$(document).ready(function () {
    $(".ItemSelect").select2({
        theme: "classic"
    });
    // Set new default font family and font color to mimic Bootstrap's default styling
    Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
    Chart.defaults.global.defaultFontColor = '#858796';
    var linear = document.getElementById("CharVentesParMois");
    var myChart = new Chart(linear, {
        type: 'line',
        data: {
        },
        options: {
            maintainAspectRatio: false,
            layout: {
                padding: {
                    left: 10,
                    right: 25,
                    top: 25,
                    bottom: 0
                }
            },
            scales: {
                xAxes: [{
                    time: {
                        unit: 'month'
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false
                    },
                    ticks: {
                        maxTicksLimit: 6
                    },
                    maxBarThickness: 25,
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        max: 0,
                        maxTicksLimit: 5,
                        padding: 10,
                        // Include a dollar sign in the ticks
                        callback: function (value, index, values) {

                            return (value + " TND");
                        }
                    },
                    gridLines: {
                        color: "rgb(234, 236, 244)",
                        zeroLineColor: "rgb(234, 236, 244)",
                        drawBorder: false,
                        borderDash: [2],
                        zeroLineBorderDash: [2]
                    }
                }],
            },
            legend: {
                display: false
            },
            tooltips: {
                titleMarginBottom: 10,
                titleFontColor: '#6e707e',
                titleFontSize: 14,
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
                callbacks: {
                    label: function (tooltipItem, chart) {

                        var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                        return datasetLabel + tooltipItem.yLabel;
                    }
                }
            },
        }
    });
    var year = $("#AnneeSelector").val();
    $.ajax({
        url: "/Home/GetSalePerMonthStats",
        data: { year: year },
        success: function (ResultData) {

            var newfirstChar = 0;
            var maxValue = 0;
            for (var i = 0; i < 12; i++) {
                if (parseInt(ResultData[i]) > maxValue) {
                    maxValue = parseInt(ResultData[i]);
                }

            }
            var lenghtMax = String(maxValue).length;
            var firstChar = String(maxValue)[0];
            if (firstChar < 9) {
                newfirstChar = parseInt(firstChar) + 1;
                for (var j = 0; j < lenghtMax - 1; j++) {
                    newfirstChar = newfirstChar * 10;
                }

            }
            else {
                newfirstChar = 1;
                for (var j = 0; j < lenghtMax; j++) {
                    newfirstChar = newfirstChar * 10;
                }
            }

            var data = {
                labels: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Decembre"],
                datasets: [{
                    label: "",
                    lineTension: 0.3,
                    backgroundColor: "rgba(78, 115, 223, 0.05)",
                    borderColor: "rgba(78, 115, 223, 1)",
                    pointRadius: 3,
                    pointBackgroundColor: "rgba(78, 115, 223, 1)",
                    pointBorderColor: "rgba(78, 115, 223, 1)",
                    pointHoverRadius: 3,
                    pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
                    pointHoverBorderColor: "rgba(78, 115, 223, 1)",
                    pointHitRadius: 10,
                    pointBorderWidth: 2,
                    data: ResultData,
                }],
            };
            myChart.data = data;
            var options = {
                maintainAspectRatio: false,
                layout: {
                    padding: {
                        left: 10,
                        right: 25,
                        top: 25,
                        bottom: 0
                    }
                },
                scales: {
                    xAxes: [{
                        time: {
                            unit: 'month'
                        },
                        gridLines: {
                            display: false,
                            drawBorder: false
                        },
                        ticks: {
                            maxTicksLimit: 6
                        },
                        maxBarThickness: 25,
                    }],
                    yAxes: [{
                        ticks: {
                            min: 0,
                            max: maxValue,
                            maxTicksLimit:10,
                            padding: 5,
                            // Include a dollar sign in the ticks
                            callback: function (value, index, values) {
                                return  number_format(value,3)
                          }
                        },
                        gridLines: {
                            color: "rgb(234, 236, 244)",
                            zeroLineColor: "rgb(234, 236, 244)",
                            drawBorder: false,
                            borderDash: [5],
                            zeroLineBorderDash: [5]
                        }
                    }],
                },
                legend: {
                    display: false
                },
                tooltips: {
                    titleMarginBottom: 10,
                    titleFontColor: '#6e707e',
                    titleFontSize: 14,
                    backgroundColor: "rgb(255,255,255)",
                    bodyFontColor: "#858796",
                    borderColor: '#dddfeb',
                    borderWidth: 1,
                    xPadding: 15,
                    yPadding: 15,
                    displayColors: false,
                    caretPadding: 10,
                    callbacks: {
                        label: function (tooltipItem, chart) {
                            var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                            return datasetLabel + tooltipItem.yLabel;
                        }
                    }
                },
            };
            myChart.options = options;
            myChart.update();
        },
    });


    var bar = document.getElementById("CharClientParMois");
    var barChart = new Chart(bar, {
        type: 'bar',
        data: {},
        options: {
            maintainAspectRatio: false,
            layout: {
                padding: {
                    left: 10,
                    right: 25,
                    top: 25,
                    bottom: 0
                }
            },
            scales: {
                xAxes: [{
                    time: {
                        unit: 'month'
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false
                    },
                    ticks: {
                        maxTicksLimit: 6
                    },
                    maxBarThickness: 25,
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        max: 20,
                        maxTicksLimit: 5,
                        padding: 10,
                        // Include a dollar sign in the ticks
                        callback: function (value, index, values) {

                            return (value + " m");
                        }
                    },
                    gridLines: {
                        color: "rgb(234, 236, 244)",
                        zeroLineColor: "rgb(234, 236, 244)",
                        drawBorder: false,
                        borderDash: [2],
                        zeroLineBorderDash: [2]
                    }
                }],
            },
            legend: {
                display: false
            },
            tooltips: {
                titleMarginBottom: 10,
                titleFontColor: '#6e707e',
                titleFontSize: 14,
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
                callbacks: {
                    label: function (tooltipItem, chart) {

                        var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                        return datasetLabel + tooltipItem.yLabel;
                    }
                }
            },
        }
    });


    $.ajax({
        url: "/Home/GetCustomerStatsPerMonth",
        data: { year: year },
        success: function (ResultData) {

            var newfirstChar = 0;
            var maxValue = 0;
            var client = [];
            var Qte = [];
            for (var key in ResultData) {
                client.push(key);
                Qte.push(ResultData[key]);
                if ((ResultData[key]) > maxValue) {
                    maxValue = (ResultData[key]);
                }
            }

            var lenghtMax = String(maxValue).length;
            var firstChar = String(maxValue)[0];
            if (firstChar < 9) {
                newfirstChar = parseInt(firstChar) + 1;
                for (var j = 0; j < lenghtMax - 1; j++) {
                    newfirstChar = newfirstChar * 10;
                }

            }
            else {
                newfirstChar = 1;
                for (var j = 0; j < lenghtMax; j++) {
                    newfirstChar = newfirstChar * 10;
                }
            }
            var data = {
                labels: client,
                datasets: [{
                    label: "Quantité: ",
                    backgroundColor: "#4e73df",
                    hoverBackgroundColor: "#2e59d9",
                    borderColor: "#4e73df",
                    data: Qte,
                }]
            };
            barChart.data = data;
            var options = {
                maintainAspectRatio: false,
                layout: {
                    padding: {
                        left: 10,
                        right: 25,
                        top: 25,
                        bottom: 0
                    }
                },
                scales: {
                    xAxes: [{
                        time: {
                            unit: 'month'
                        },
                        gridLines: {
                            display: false,
                            drawBorder: false
                        },
                        ticks: {
                            maxTicksLimit: 6
                        },
                        maxBarThickness: 25,
                    }],
                    yAxes: [{
                        ticks: {
                            min: 0,
                            max: maxValue,
                            maxTicksLimit: 5,
                            padding: 10,
                            // Include a dollar sign in the ticks
                            callback: function (value, index, values) {

                                return (value + " m");
                            }
                        },
                        gridLines: {
                            color: "rgb(234, 236, 244)",
                            zeroLineColor: "rgb(234, 236, 244)",
                            drawBorder: false,
                            borderDash: [2],
                            zeroLineBorderDash: [2]
                        }
                    }],
                },
                legend: {
                    display: false
                },
                tooltips: {
                    titleMarginBottom: 10,
                    titleFontColor: '#6e707e',
                    titleFontSize: 14,
                    backgroundColor: "rgb(255,255,255)",
                    bodyFontColor: "#858796",
                    borderColor: '#dddfeb',
                    borderWidth: 1,
                    xPadding: 15,
                    yPadding: 15,
                    displayColors: false,
                    caretPadding: 10,
                    callbacks: {
                        label: function (tooltipItem, chart) {

                            var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                            return datasetLabel + tooltipItem.yLabel;
                        }
                    }
                },
            };
            barChart.options = options;
            barChart.update();
        },
    });
    debugger;
    var vente = Number($("#venteYear").val().replace(",",".")).toFixed(3).replace(/\d(?=(\d{3})+\.)/g, '$&,');  // 12,345.67
    $("#venteAnnee").text(vente);
    $("#venteAnnee").text(vente);
    $("#AnneeSelector").change(function () {
        GetSalePerMonthStats(this.value, myChart);
        GetCustomerStatsPerMonth(this.value, barChart);
        GetSale(this.value);
    });
});
function GetSalePerMonthStats(year, myChart) {
    $.ajax({
        url: "/Home/GetSalePerMonthStats",
        data: { year: year },
        success: function (ResultData) {
            debugger;
            var newfirstChar = 0;
            var maxValue = Math.max.apply(null, ResultData);
            var s = Math.ceil(maxValue)
            var lenghtMax = String(maxValue).length;
            var firstChar = String(maxValue)[0];
            if (firstChar < 9) {
                newfirstChar = parseInt(firstChar) + 1;
                for (var j = 0; j < lenghtMax - 1; j++) {
                    newfirstChar = newfirstChar * 10;
                }

            }
            else {
                newfirstChar = 1;
                for (var j = 0; j < lenghtMax; j++) {
                    newfirstChar = newfirstChar * 10;
                }
            }



            var data = {
                labels: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Decembre"],
                datasets: [{
                    label: "",
                    lineTension: 0.3,
                    backgroundColor: "rgba(78, 115, 223, 0.05)",
                    borderColor: "rgba(78, 115, 223, 1)",
                    pointRadius: 3,
                    pointBackgroundColor: "rgba(78, 115, 223, 1)",
                    pointBorderColor: "rgba(78, 115, 223, 1)",
                    pointHoverRadius: 3,
                    pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
                    pointHoverBorderColor: "rgba(78, 115, 223, 1)",
                    pointHitRadius:1,
                    pointBorderWidth: 2,
                    data: ResultData,
                }],
            };
            myChart.data = data;
            var options = {
                maintainAspectRatio: false,
                layout: {
                    padding: {
                        left: 10,
                        right: 25,
                        top: 25,
                        bottom: 0
                    }
                },
                scales: {
                    xAxes: [{
                        time: {
                            unit: 'month'
                        },
                        gridLines: {
                            display: false,
                            drawBorder: false
                        },
                        ticks: {
                            maxTicksLimit: 6
                        },
                        maxBarThickness: 25,
                    }],
                    yAxes: [{
                        ticks: {
                            min: 0,
                            max: Math.ceil(maxValue),
                            maxTicksLimit: Math.ceil(maxValue),
                            padding: 10,
                            // Include a dollar sign in the ticks
                            callback: function (value, index, values) {
                                return number_format(value, 3);
                            }
                        },
                        gridLines: {
                            color: "rgb(234, 236, 244)",
                            zeroLineColor: "rgb(234, 236, 244)",
                            drawBorder: false,
                            borderDash: [2],
                            zeroLineBorderDash: [2]
                        }
                    }],
                },
                legend: {
                    display: false
                },
                tooltips: {
                    titleMarginBottom: 10,
                    titleFontColor: '#6e707e',
                    titleFontSize: 14,
                    backgroundColor: "rgb(255,255,255)",
                    bodyFontColor: "#858796",
                    borderColor: '#dddfeb',
                    borderWidth: 1,
                    xPadding: 15,
                    yPadding: 15,
                    displayColors: false,
                    caretPadding: 10,
                    callbacks: {
                        label: function (tooltipItem, chart) {

                            var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                            return datasetLabel + tooltipItem.yLabel;
                        }
                    }
                },
            };
            myChart.options = options;
            myChart.update();

        },
    });
}
function GetCustomerStatsPerMonth(year, barChart) {
    $.ajax({
        url: "/Home/GetCustomerStatsPerMonth",
        data: { year: year },
        success: function (ResultData) {
            var newfirstChar = 0;
            var maxValue = 0;
            var client = [];
            var Qte = [];
            for (var key in ResultData) {
                client.push(key);
                Qte.push(ResultData[key]);
                if ((ResultData[key]) > maxValue) {
                    maxValue = (ResultData[key]);
                }
            }

            var lenghtMax = String(maxValue).length;
            var firstChar = String(maxValue)[0];
            if (firstChar < 9) {
                newfirstChar = parseInt(firstChar) + 1;
                for (var j = 0; j < lenghtMax - 1; j++) {
                    newfirstChar = newfirstChar * 10;
                }

            }
            else {
                newfirstChar = 1;
                for (var j = 0; j < lenghtMax; j++) {
                    newfirstChar = newfirstChar * 10;
                }
            }
            var data = {
                labels: client,
                datasets: [{
                    label: "Quantité: ",
                    backgroundColor: "#4e73df",
                    hoverBackgroundColor: "#2e59d9",
                    borderColor: "#4e73df",
                    data: Qte,
                }]
            };
            barChart.data = data;
            var options = {
                maintainAspectRatio: false,
                layout: {
                    padding: {
                        left: 10,
                        right: 25,
                        top: 25,
                        bottom: 0
                    }
                },
                scales: {
                    xAxes: [{
                        time: {
                            unit: 'month'
                        },
                        gridLines: {
                            display: false,
                            drawBorder: false
                        },
                        ticks: {
                            maxTicksLimit: 6
                        },
                        maxBarThickness: 25,
                    }],
                    yAxes: [{
                        ticks: {
                            min: 0,
                            max: maxValue,
                            maxTicksLimit: 5,
                            padding: 10,
                            // Include a dollar sign in the ticks
                            callback: function (value, index, values) {

                                return (value + " m");
                            }
                        },
                        gridLines: {
                            color: "rgb(234, 236, 244)",
                            zeroLineColor: "rgb(234, 236, 244)",
                            drawBorder: false,
                            borderDash: [2],
                            zeroLineBorderDash: [2]
                        }
                    }],
                },
                legend: {
                    display: false
                },
                tooltips: {
                    titleMarginBottom: 10,
                    titleFontColor: '#6e707e',
                    titleFontSize: 14,
                    backgroundColor: "rgb(255,255,255)",
                    bodyFontColor: "#858796",
                    borderColor: '#dddfeb',
                    borderWidth: 1,
                    xPadding: 15,
                    yPadding: 15,
                    displayColors: false,
                    caretPadding: 10,
                    callbacks: {
                        label: function (tooltipItem, chart) {

                            var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                            return datasetLabel + tooltipItem.yLabel;
                        }
                    }
                },
            };
            barChart.options = options;
            barChart.update();
        },
    });
}
function GetSale(year) {
    $.ajax({
        url: "/Home/GetSale",
        data: { year: year },
        success: function (result) {

            var vente = (Number(result.VenteAnnee)).toFixed(3).replace(/\d(?=(\d{3})+\.)/g, '$&,');  // 12,345.67
            $("#venteAnnee").text(vente);
            $("#MoisPourcent").text(result.MoisPourcent + " %");
        },
        error: function () { }
    });
}

function number_format(number, decimals, dec_point, thousands_sep) {
    // *     example: number_format(1234.56, 2, ',', ' ');
    // *     return: '1 234,56'
    number = (number + '').replace(',', '').replace(' ', '');
    var n = !isFinite(+number) ? 0 : +number,
        prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
        sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
        dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
        s = '',
        toFixedFix = function (n, prec) {
            var k = Math.pow(10, prec);
            return '' + Math.round(n * k) / k;
        };
    // Fix for IE parseFloat(0.55).toFixed(0) = 0;
    s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
    if (s[0].length > 3) {
        s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
    }
    if ((s[1] || '').length < prec) {
        s[1] = s[1] || '';
        s[1] += new Array(prec - s[1].length + 1).join('0');
    }
    return s.join(dec);
}