import { ChangeDetectionStrategy, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { map, Observable, tap } from 'rxjs';
import { environment } from '../../../../../environment/environment';
import { BookService } from '../../../library';
import { Book, BookFilterRequest, CommandHandler, CurrencyPipeApplier, defaultBookFilterRequest } from '../../../shared';
import { CART_ADD_BOOK_COMMAND_HANDLER, CartAddBookCommand } from '../../../shop';

@Component({
  selector: 'app-product-page',
  templateUrl: './product-page.component.html',
  styleUrl: './product-page.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductPageComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  items$!: Observable<Book[]>;
  totalAmount$!: Observable<number>;

  bookAddedMap: Record<string, boolean> = {};
  pageSize = 8;
  pageSizeOptions: number[] = [8, 16, 32];
  private readonly defaultPagination = { pageIndex: 1, pageSize: 8 };
  private filterReq: BookFilterRequest = defaultBookFilterRequest();

  constructor(
    private readonly bookService: BookService,
    private readonly currenctyApplier: CurrencyPipeApplier,
    @Inject(CART_ADD_BOOK_COMMAND_HANDLER) private readonly addBookToCartHandler: CommandHandler<CartAddBookCommand>
  ) { }

  ngOnInit(): void {
    this.fetchTotalAmount();
    this.fetchPaginatedItems(this.defaultPagination);

    this.items$.pipe(
      tap(
        items => {
          items.forEach(book => {
            this.bookAddedMap[book.id] = false;
          });
        }));
  }

  addBookToCart(book: Book) {
    const command: CartAddBookCommand =
    {
      book: book
    }
    this.addBookToCartHandler.dispatch(command);
    this.bookAddedMap[book.id] = true;
  }

  private updateFilterPagination(pagination: { pageIndex: number, pageSize: number }): void {
    this.filterReq = {
      ...this.filterReq,
      pageNumber: pagination.pageIndex,
      pageSize: pagination.pageSize
    };
  }

  private fetchTotalAmount(): void {
    this.totalAmount$ = this.bookService.getItemTotalAmount(this.filterReq);
  }
  private fetchPaginatedItems(pagination: { pageIndex: number, pageSize: number }): void {
    this.updateFilterPagination(pagination);
    this.items$ = this.bookService.getPaginated(this.filterReq).pipe(
      map(books => books.slice(0, pagination.pageSize))
    );
  }

  filterChange(req: BookFilterRequest): void {
    this.resetPagination();
    this.filterReq = { ...req };
    this.fetchTotalAmount();
    this.fetchPaginatedItems(this.defaultPagination);
  }

  onPageChange(event: PageEvent): void {
    const pageIndex = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.fetchPaginatedItems({ pageIndex: pageIndex, pageSize: this.pageSize });
  }
  resetPagination() {
    if (this.paginator) {
      this.paginator.pageIndex = this.defaultPagination.pageIndex;
      this.pageSize = this.defaultPagination.pageSize;
    }
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currenctyApplier.applyCurrencyPipe(value);
  }

  getBookPage(book: Book): string {
    return book.id.toString();
  }
  checkIfInStock(book: Book): boolean {
    return book.stockAmount > 0;
  }
  onErrorImage(event: Event) {
    const element = event.target as HTMLImageElement;
    element.src = environment.bookCoverPlaceholder;
  }
}
