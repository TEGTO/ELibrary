import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '../../../../../../../environment/environment';
import { Book, Client, CurrencyPipeApplier, getClientName, getProductInfoPath, RouteReader, StockBookChange, StockBookOrder, StockBookOrderType, stockBookOrderTypeToString } from '../../../../../shared';
import { BookstockOrderService } from '../../../../../shop';

@Component({
  selector: 'app-book-stock-details',
  templateUrl: './book-stock-details.component.html',
  styleUrl: './book-stock-details.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookStockDetailsComponent implements OnInit {
  readonly itemHeight = 200;
  readonly amountItemsInView = 3;
  readonly scrollSizePerObject = 220;
  readonly scollSize = 420;

  order$!: Observable<StockBookOrder>;

  constructor(
    private readonly stockOrderService: BookstockOrderService,
    private readonly routeReader: RouteReader,
    private readonly route: ActivatedRoute,
    private readonly currenctyApplier: CurrencyPipeApplier,
  ) { }

  ngOnInit(): void {
    this.order$ = this.routeReader.readIdInt(
      this.route,
      (id: number) => this.stockOrderService.getById(id),
    );
  }

  getStringOrderType(type: StockBookOrderType): string {
    return stockBookOrderTypeToString(type);
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currenctyApplier.applyCurrencyPipe(value);
  }
  getBookPage(book: Book): string[] {
    return [`/${getProductInfoPath(book.id)}`];
  }
  calculateSelectionSize(bookAmount: number): number {
    return this.scrollSizePerObject * bookAmount > this.scollSize ? this.scollSize : this.scrollSizePerObject * bookAmount;
  }
  trackById(index: number, stockChange: StockBookChange): number {
    return stockChange.id;
  }
  getClientName(client: Client): string {
    return getClientName(client);
  }
  onErrorImage(event: Event) {
    const element = event.target as HTMLImageElement;
    element.src = environment.bookCoverPlaceholder;
  }
}
