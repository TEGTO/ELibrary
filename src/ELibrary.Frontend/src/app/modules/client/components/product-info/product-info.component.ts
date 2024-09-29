import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, map, Observable, of, switchMap } from 'rxjs';
import { BookService } from '../../../library';
import { Book, CommandHandler, CoverType, CurrencyPipeApplier, getDefaultBook, getStringCoverType, RedirectorService } from '../../../shared';
import { CART_ADD_BOOK_COMMAND_HANDLER, CartAddBookCommand } from '../../../shop';

@Component({
  selector: 'app-product-info',
  templateUrl: './product-info.component.html',
  styleUrl: './product-info.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductInfoComponent implements OnInit {
  bookAdded = false;

  book$!: Observable<Book>;

  constructor(
    private readonly bookService: BookService,
    private readonly route: ActivatedRoute,
    private readonly redirectService: RedirectorService,
    private readonly currenctyApplier: CurrencyPipeApplier,
    @Inject(CART_ADD_BOOK_COMMAND_HANDLER) private readonly addBookToCartHandler: CommandHandler<CartAddBookCommand>
  ) { }

  ngOnInit(): void {
    this.book$ = this.route.paramMap.pipe(
      map(params => params.get('id')),
      switchMap(id => {
        if (!id) {
          this.redirectService.redirectToHome();
          return of(getDefaultBook());
        }

        const intId = parseInt(id, 10);
        if (isNaN(intId)) {
          this.redirectService.redirectToHome();
          return of(getDefaultBook());
        }

        return this.bookService.getById(intId).pipe(
          catchError(() => {
            this.redirectService.redirectToHome();
            return of(getDefaultBook());
          })
        );
      })
    );
  }

  getStringCoverType(coverType: CoverType): string {
    return getStringCoverType(coverType);
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currenctyApplier.applyCurrencyPipe(value);
  }

  checkIfInStock(book: Book): boolean {
    return book.stockAmount > 0;
  }

  addBookToCart(book: Book) {
    const command: CartAddBookCommand =
    {
      book: book
    }
    this.addBookToCartHandler.dispatch(command);
    this.bookAdded = true;
  }
}
