$(function () {


  // =====================================
  // Profit
  // =====================================
  var profit = {
    series: [
      {
        name: "Profit",
        data: [18, 7, 15, 29, 18, 18, 7, 15, 29, 18, 12, 9],
      },
      {
        name: "Expenses",
        data: [-13, -18, -9, -14, -15, -13, -18, -9, -14, -5, -17, -15],
      },
    ],
    colors: ["var(--bs-primary)", "#fb977d"],
    chart: {
      type: "bar",
      fontFamily: "Plus Jakarta Sans', sans-serif",
      foreColor: "#adb0bb",
      width: "100%",
      height: 350,
      stacked: true,
      toolbar: {
        show: !1,
      },
    },
  
    plotOptions: {
      bar: {
        horizontal: false,
        columnWidth: "35%",
        borderRadius: [6],
        borderRadiusApplication: 'end',
        borderRadiusWhenStacked: 'all'
      },
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      curve: "smooth",
      width: 6,
      colors: ["transparent"],
    },
  
    grid: {
      borderColor: "transparent",
      padding: { top: 0, bottom: -8, left: 20, right: 20 },
    },
    tooltip: {
      theme: "dark",
    },
    toolbar: {
      show: false,
    },
    xaxis: {
      categories: ["Aug", "Sep", "Oct", "Nov", "Dec", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul"],
      axisBorder: {
        show: false,
      },
      axisTicks: {
        show: false,
      },
    },
    legend: {
      show: false,
    },
    fill: {
      opacity: 1,
    },
  };
  
  var chart = new ApexCharts(document.querySelector("#profit"), profit);
  chart.render();


  // =====================================
  // Breakup
  // =====================================
  var grade = {
    series: [5368, 3500, 4106],
    labels: ["5368", "Refferal Traffic", "Oragnic Traffic"],
    chart: {
      height: 170,
      type: "donut",
      fontFamily: "Plus Jakarta Sans', sans-serif",
      foreColor: "#c6d1e9",
    },
  
    tooltip: {
      theme: "dark",
      fillSeriesColor: false,
    },
  
    colors: ["#e7ecf0", "#fb977d", "var(--bs-primary)"],
    dataLabels: {
      enabled: false,
    },
  
    legend: {
      show: false,
    },
  
    stroke: {
      show: false,
    },
    responsive: [
      {
        breakpoint: 991,
        options: {
          chart: {
            width: 150,
          },
        },
      },
    ],
    plotOptions: {
      pie: {
        donut: {
          size: '80%',
          background: "none",
          labels: {
            show: true,
            name: {
              show: true,
              fontSize: "12px",
              color: undefined,
              offsetY: 5,
            },
            value: {
              show: false,
              color: "#98aab4",
            },
          },
        },
      },
    },
  };
  
  var chart = new ApexCharts(document.querySelector("#grade"), grade);
  chart.render();
  



  // =====================================
  // Earning
  // =====================================
  var earning = {
    chart: {
      id: "sparkline3",
      type: "area",
      height: 60,
      sparkline: {
        enabled: true,
      },
      group: "sparklines",
      fontFamily: "Plus Jakarta Sans', sans-serif",
      foreColor: "#adb0bb",
    },
    series: [
      {
        name: "Earnings",
        color: "#8763da",
        data: [25, 66, 20, 40, 12, 58, 20],
      },
    ],
    stroke: {
      curve: "smooth",
      width: 2,
    },
    fill: {
      colors: ["#f3feff"],
      type: "solid",
      opacity: 0.05,
    },

    markers: {
      size: 0,
    },
    tooltip: {
      theme: "dark",
      fixed: {
        enabled: true,
        position: "right",
      },
      x: {
        show: false,
      },
    },
  };
  new ApexCharts(document.querySelector("#earning"), earning).render();

// =====================================
// netsells chart   
// =====================================
var netsells = {
    series: [
        {
            name: "",
            data: [0, 20, 15, 19, 14, 25, 32]
        },
        {
            name: "",
            data: [0, 12, 19, 13, 26, 16, 25]
        },
    ],
    chart: {
        fontFamily: "Plus Jakarta Sans', sans-serif",
        foreColor: "#adb0bb",
        height: 260,
        type: "line",
        toolbar: {
            show: false,
        },
        stacked: false
    },
    legend: {
        show: false,
    },
    stroke: {
        width: 3,
        curve: "smooth",
    },
    grid: {
        borderColor: "var(--bs-border-color)",
        xaxis: {
            lines: {
                show: true
            }
        },
        yaxis: {
            lines: {
                show: true
            },

        },
        padding: {
            top: 0,
            bottom: 0,
            left: 0,
            right: 0
        },
    },
    colors: ["#0085db", "#5AC8FA"],
    fill: {
        type: "gradient",
        gradient: {
            shade: "dark",
            gradientToColors: ["#6993ff"],
            shadeIntensity: 1,
            type: "horizontal",
            opacityFrom: 1,
            opacityTo: 1,
            stops: [0, 100, 100, 100],
        },
    },
    markers: {
        size: 0,
    },
    xaxis: {
        labels: {
            show: true,
        },
        type: 'category',
        categories: [
            "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"
        ],
        axisTicks: {
            show: false,
        },
        axisBorder: {
            show: false,
        },

    },
    yaxis: {
        axisTicks: {
            show: false,
        },
        axisBorder: {
            show: false,
        },
        labels: {
            show: true,
            formatter: function (value) {
                return value + "k";
            }
        },
    },
    tooltip: {
        theme: "dark",
    },
};
new ApexCharts(document.querySelector("#netsells"), netsells).render();




// =====================================
// total-orders chart   
// =====================================

var total_orders = {
    series: [
        {
            name: "Last Year ",
            data: [29, 52, 38, 47, 56, 41, 46],
        },
        {
            name: "This Year ",
            data: [71, 71, 71, 71, 71, 71, 71],
        },
    ],
    chart: {
        fontFamily: "Plus Jakarta Sans', sans-serif",
        type: "bar",
        height: 150,
        stacked: true,
        foreColor: "#707a82",
        toolbar: {
            show: false,
        },
    },
    grid: {
        show: false,
        borderColor: "rgba(0,0,0,0.1)",
        strokeDashArray: 1,
        xaxis: {
            lines: {
                show: false,
            },
        },
        yaxis: {
            lines: {
                show: true,
            },
        },
        padding: {
            top: 0,
            right: 0,
            bottom: 0,
            left: 0
        },
    },
    colors: ["var(--bs-primary)", "#D9D9D9"],
    plotOptions: {
        bar: {
            horizontal: false,
            // barHeight: "90%",
            columnWidth: "26%",
            borderRadius: [3],
            borderRadiusApplication: 'end',
            borderRadiusWhenStacked: 'all'
        },
    },
    dataLabels: {
        enabled: false,
    },
    xaxis: {
        categories: [["M"], ["T"], ["W"], ["T"], ["F"], ["S"], ["S"]],
        axisBorder: {
            show: false,
        },
        axisTicks: {
            show: false,
        },
    },
    yaxis: {
        labels: {
            show: false,
        },
    },
    tooltip: {
        theme: "dark",
    },
    legend: {
        show: false,
    },
};

var chart_column_stacked = new ApexCharts(
    document.querySelector("#total-orders"),
    total_orders
);
chart_column_stacked.render();



// =====================================
// products chart   
// =====================================


var products = {
    color: "#adb5bd",
    series: [70, 18, 12],
    labels: ["2022", "2021", "2020"],
    chart: {
        height: 170,
        type: "donut",
        fontFamily: "Plus Jakarta Sans', sans-serif",
        foreColor: "#adb0bb",
    },
    plotOptions: {
        pie: {
            startAngle: 0,
            endAngle: 360,
            donut: {
                size: '85%',
            },
        },
    },
    stroke: {
        show: false,
    },

    dataLabels: {
        enabled: false,
    },

    legend: {
        show: false,
    },
    colors: ["var(--bs-primary)", "#FB977D", "#5AC8FA"],

    tooltip: {
        theme: "dark",
        fillSeriesColor: false,
    },
};

var chart = new ApexCharts(document.querySelector("#products"), products);
chart.render();


// =====================================
// customers chart   
// =====================================


var options = {
    chart: {
        id: "customers",
        type: "area",
        height: 103,
        sparkline: {
            enabled: true,
        },
        group: 'sparklines',
        fontFamily: "Plus Jakarta Sans', sans-serif",
        foreColor: "#adb0bb",
    },
    series: [
        {
            name: 'monthly earnings',
            color: "var(--bs-primary)",
            data: [25, 66, 20, 40, 12, 30],
        },
    ],
    stroke: {
        curve: "smooth",
        width: 2,
    },
    fill: {
        type: "gradient",
        gradient: {
            shadeIntensity: 0,
            inverseColors: false,
            opacityFrom: 0.05,
            opacityTo: 0,
            stops: [20, 180],
        },
    },


    markers: {
        size: 0,
    },
    tooltip: {
        theme: "dark",
        fixed: {
            enabled: true,
            position: "right",
        },
        x: {
            show: false,
        },
    },
};
new ApexCharts(document.querySelector("#customers"), options).render();



// -----------------------------------------------------------------------
// world map
// -----------------------------------------------------------------------
$("#usa").vectorMap({
    map: "us_aea_en",
    backgroundColor: "transparent",
    zoomOnScroll: false,
    regionStyle: {
        initial: {
            fill: "#c9d6de",
        },
    },
    markers: [
        {
            latLng: [40.71, -74.0],
            name: "LA: 250",
            style: { fill: "var(--bs-info)" },
        },
        {
            latLng: [39.01, -98.48],
            name: "NY: 250",
            style: { fill: "var(--bs-primary)" },
        },
        {
            latLng: [37.38, -122.05],
            name: "KA : 250",
            style: { fill: "var(--bs-danger)" },
        },
    ],
});
})