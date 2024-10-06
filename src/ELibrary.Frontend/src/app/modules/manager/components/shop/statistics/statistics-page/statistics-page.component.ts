import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BookStatistics, CurrencyPipeApplier, GetBookStatistics, getDefaultGetBookStatistics } from '../../../../../shared';
import { StatisticsService } from '../../../../../shop';

@Component({
  selector: 'app-statistics-page',
  templateUrl: './statistics-page.component.html',
  styleUrl: './statistics-page.component.scss'
})
export class StatisticsPageComponent implements OnInit {

  statistics$!: Observable<BookStatistics>;

  constructor(
    private readonly currenctyApplier: CurrencyPipeApplier,
    private readonly statisticsService: StatisticsService
  ) { }

  ngOnInit() {
    this.getBookStatistics(getDefaultGetBookStatistics())
  }

  getBookStatistics(getStats: GetBookStatistics) {
    this.statistics$ = this.statisticsService.getBookStatistics(getStats);
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currenctyApplier.applyCurrencyPipe(value);
  }
}
