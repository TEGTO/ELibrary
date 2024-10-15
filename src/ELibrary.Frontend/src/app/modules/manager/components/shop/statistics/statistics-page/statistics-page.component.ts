import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { BookStatistics, CurrencyPipeApplier, GetBookStatistics, getDefaultGetBookStatistics } from '../../../../../shared';
import { StatisticsService } from '../../../../../shop';

@Component({
  selector: 'app-statistics-page',
  templateUrl: './statistics-page.component.html',
  styleUrl: './statistics-page.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
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
    this.statistics$.pipe(
      tap(x => {
        console.log(x.orderAmountInDays);
      })
    )
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currenctyApplier.applyCurrencyPipe(value);
  }
}
