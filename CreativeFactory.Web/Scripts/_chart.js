﻿window.onload = function () {
    var r = Raphael("holder"),
        txtattr = { font: "12px sans-serif" };

    var x = [], y = [], y2 = [], y3 = [];
    datesArr = [], countArr = [];

    var zz = $.ajax({
        url: '@Url.Action("GetStats", "Item")',
        type: 'post',
        dataType: 'json',
        async: false,
        success: function (data) {
            datesArr = data.dates;
            countArr = data.count;
        }
    });

    for (var i = 0; i < 1e6; i++) {
        x[i] = i * 10;
        y[i] = (y[i - 1] || 0) + (Math.random() * 7) - 3;
    }

    //var lines = r.linechart(10, 10, 300, 220, datesArr, countArr, { nostroke: false, axis: "0 0 1 1", symbol: "circle", smooth: true, axisxstep: 1, axisystep: 1 }).hoverColumn(function () {
    var lines = r.linechart(10, 10, 300, 220, [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [1, 0, 0, 7, 0, 4, 7, 4], { nostroke: false, axis: "0 0 1 1", symbol: "circle", smooth: true, axisxstep: 1, axisystep: 1 }).hoverColumn(function () {
        this.tags = r.set();

        for (var i = 0, ii = this.y.length; i < ii; i++) {
            this.tags.push(r.tag(this.x, this.y[i], this.values[i], 160, 10).insertBefore(this).attr([{ fill: "#fff" }, { fill: this.symbols[i].attr("fill") }]));
        }
    }, function () {
        this.tags && this.tags.remove();
    });

    //lines.symbols.attr({ r: 6 });


    // lines.lines[0].animate({"stroke-width": 6}, 1000);
    // lines.symbols[0].attr({stroke: "#fff"});
    // lines.symbols[0][1].animate({fill: "#f00"}, 1000);
};