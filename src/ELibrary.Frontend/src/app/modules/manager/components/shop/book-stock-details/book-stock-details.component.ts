import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, map, Observable, of, switchMap } from 'rxjs';
import { Book, CurrencyPipeApplier, RedirectorService, StockBookChange, StockBookOrder, StockBookOrderType, stockBookOrderTypeToString } from '../../../../shared';
import { BookstockOrderService } from '../../../../shop';

@Component({
  selector: 'app-book-stock-details',
  templateUrl: './book-stock-details.component.html',
  styleUrl: './book-stock-details.component.scss'
})
export class BookStockDetailsComponent implements OnInit {
  readonly itemHeight = 200;
  readonly amountItemsInView = 3;
  readonly scrollSizePerObject = 220;
  readonly scollSize = 420;

  order$!: Observable<StockBookOrder>;

  constructor(
    private readonly stockOrderService: BookstockOrderService,
    private readonly route: ActivatedRoute,
    private readonly currenctyApplier: CurrencyPipeApplier,
    private readonly redirectService: RedirectorService,
  ) { }

  ngOnInit(): void {
    this.order$ = this.route.paramMap.pipe(
      map(params => params.get('id')),
      switchMap(id => {
        if (!id) {
          this.redirectService.redirectToHome();
          return of();
        }

        const intId = parseInt(id, 10);
        if (isNaN(intId)) {
          this.redirectService.redirectToHome();
          return of();
        }

        return this.stockOrderService.getById(intId).pipe(
          catchError(() => {
            this.redirectService.redirectToHome();
            return of();
          })
        );
      })
    );
  }

  getStringOrderType(type: StockBookOrderType): string {
    return stockBookOrderTypeToString(type);
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currenctyApplier.applyCurrencyPipe(value);
  }
  getBookPage(book: Book): number {
    return book.id;
  }
  calculateSelectionSize(bookAmount: number): number {
    return this.scrollSizePerObject * bookAmount > this.scollSize ? this.scollSize : this.scrollSizePerObject * bookAmount;
  }
  trackById(index: number, stockChange: StockBookChange): number {
    return stockChange.id;
  }
}
