import { ApexAxisChartSeries, ApexChart, ApexDataLabels, ApexFill, ApexLegend, ApexMarkers, ApexStroke, ApexTitleSubtitle, ApexTooltip, ApexXAxis, ApexYAxis } from "ng-apexcharts";

// eslint-disable-next-line @typescript-eslint/consistent-type-definitions
export type ChartOptions = {
    series: ApexAxisChartSeries;
    chart: ApexChart;
    title: ApexTitleSubtitle;
    xaxis: ApexXAxis;
    dataLabels: ApexDataLabels;
    yaxis: ApexYAxis;
    fill: ApexFill;
    legend: ApexLegend;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    toolbar: any;
    stroke: ApexStroke;
    markers: ApexMarkers;
    tooltip: ApexTooltip;
    colors: string[];
};