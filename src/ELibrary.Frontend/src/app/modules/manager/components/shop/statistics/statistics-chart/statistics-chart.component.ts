import { Component, Input } from '@angular/core';
import { ChartType } from 'ng-apexcharts';
import { ChartOptions } from '../../../../../shared';

@Component({
  selector: 'app-statistics-chart',
  templateUrl: './statistics-chart.component.html',
  styleUrl: './statistics-chart.component.scss'
})
export class StatisticsChartComponent {
  @Input({ required: true }) data!: Map<Date, number>;

  chartOptions!: Partial<ChartOptions>;
  multiMonth = false;

  initChartOptions(data: Map<Date, number>): boolean {
    const dateRange = this.getDateRange(data);
    this.multiMonth = this.isMultiMonth(dateRange.start, dateRange.end);

    const formattedData = this.multiMonth
      ? this.aggregateDataByMonth(data)
      : this.formatDataByDay(data);

    this.chartOptions = {
      chart: {
        type: 'area' as ChartType,
        height: 280,
        stacked: false,
        animations: { enabled: false },
        zoom: {
          type: "x",
          enabled: true,
          autoScaleYaxis: true
        },
        toolbar: {
          autoSelected: "zoom",
        }
      },
      stroke: { width: [4], curve: ['smooth'] },
      fill: {
        type: 'gradient',
        gradient: {
          shadeIntensity: 1,
          inverseColors: false,
          opacityFrom: 0.45,
          opacityTo: 0.05,
          stops: [20, 100, 100, 100],
        },
      },
      series: [{ name: "Order Amount", data: formattedData }],
      dataLabels: { enabled: false },
      markers: { size: 0 },
      xaxis: {
        type: 'datetime',
        labels: {
          formatter: this.labelFormatter.bind(this)
        },
        tooltip: { enabled: false },
      },
      yaxis: {
        show: true,
        title: { text: "Order Amount" },
      },
      tooltip: {
        x: { formatter: this.tooltipformatter.bind(this) },
        marker: { show: true, fillColors: ['#40abfc'] },
      },
    };
    return true;
  }

  private getDateRange(data: Map<Date, number>) {
    const dates = Array.from(data.keys());
    const start = new Date(Math.min(...dates.map(d => d.getTime())));
    const end = new Date(Math.max(...dates.map(d => d.getTime())));
    return { start, end };
  }

  private isMultiMonth(start: Date, end: Date): boolean {
    return (
      start.getFullYear() !== end.getFullYear() ||
      start.getMonth() !== end.getMonth()
    );
  }

  private formatDataByDay(data: Map<Date, number>): { x: number, y: number }[] {
    return Array.from(data.entries()).map(([date, value]) => ({
      x: date.getTime(),
      y: value,
    }));
  }

  private aggregateDataByMonth(data: Map<Date, number>): { x: number, y: number }[] {
    const monthlyTotals = new Map<number, number>();

    data.forEach((value, date) => {
      const monthKey = new Date(date.getFullYear(), date.getMonth()).getTime();
      const existingValue = monthlyTotals.get(monthKey) ?? 0;
      monthlyTotals.set(monthKey, existingValue + value);
    });

    return Array.from(monthlyTotals.entries()).map(([timestamp, value]) => ({
      x: timestamp,
      y: value,
    }));
  }

  labelFormatter(value: string, timestamp?: number) {
    const date = new Date(timestamp ?? parseInt(value));
    return this.multiMonth
      ? date.toLocaleString('en-EN', { year: 'numeric', month: 'short' })
      : `${date.getDate()} ${date.toLocaleString('en-EN', { month: 'short' })}`;
  }
  tooltipformatter(val: number): string {
    const date = new Date(val);
    return this.multiMonth
      ? date.toLocaleString('en-EN', { year: 'numeric', month: 'short' })
      : `${date.getDate()} ${date.toLocaleString('en-EN', { month: 'short' })}`;
  }
}
