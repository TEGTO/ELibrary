import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { ChartType } from 'ng-apexcharts';
import { ChartOptions } from '../../../../../shared';

@Component({
  selector: 'app-statistics-chart',
  templateUrl: './statistics-chart.component.html',
  styleUrl: './statistics-chart.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StatisticsChartComponent {
  @Input({ required: true }) data!: Map<Date, number>;

  chartOptions!: Partial<ChartOptions>;
  multiMonth = false;
  hourly = false;

  initChartOptions(data: Map<Date, number>): boolean {
    const dateRange = this.getDateRange(data);
    this.multiMonth = this.isMultiMonth(dateRange.start, dateRange.end);
    this.hourly = this.isHourly(dateRange.start, dateRange.end);

    let formattedData;
    if (this.multiMonth) {
      formattedData = this.aggregateDataByMonth(data);
    } else if (this.hourly) {
      formattedData = this.formatDataByHour(data);
    } else {
      formattedData = this.formatDataByDay(data);
    }

    formattedData = this.sortFormattedData(formattedData);

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
    return start.getFullYear() !== end.getFullYear() || start.getMonth() !== end.getMonth();
  }

  private isHourly(start: Date, end: Date): boolean {
    const msInDay = 24 * 60 * 60 * 1000;
    return (end.getTime() - start.getTime()) <= msInDay;
  }

  private formatDataByDay(data: Map<Date, number>): { x: number, y: number }[] {
    return Array.from(data.entries()).map(([date, value]) => ({
      x: date.getTime(),
      y: value,
    }));
  }

  private formatDataByHour(data: Map<Date, number>): { x: number, y: number }[] {
    const hourlyData = new Map<number, number>();

    data.forEach((value, date) => {
      const hourKey = new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours()).getTime();
      const existingValue = hourlyData.get(hourKey) ?? 0;
      hourlyData.set(hourKey, existingValue + value);
    });

    return Array.from(hourlyData.entries()).map(([timestamp, value]) => ({
      x: timestamp,
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

  private labelFormatter(value: string, timestamp?: number) {
    const date = new Date(timestamp ?? parseInt(value));
    return this.getForrmattedString(date);
  }

  private tooltipformatter(val: number): string {
    const date = new Date(val);
    return this.getForrmattedString(date);
  }

  private getForrmattedString(date: Date) {
    let labelText;
    if (this.multiMonth) {
      labelText = date.toLocaleString('en-EN', { year: 'numeric', month: 'short' });
    } else if (this.hourly) {
      labelText = `${date.getHours()}:00 | ${date.getDate()} ${date.toLocaleString('en-EN', { month: 'short' })}`;
    } else {
      labelText = `${date.getDate()} ${date.toLocaleString('en-EN', { month: 'short' })}`;
    }
    return labelText;
  }

  private sortFormattedData(formattedData: { x: number, y: number }[]): { x: number, y: number }[] {
    return formattedData.sort((n1, n2) => {
      if (n1.x > n2.x) {
        return 1;
      }

      if (n1.x < n2.x) {
        return -1;
      }

      return 0;
    });
  }
}

