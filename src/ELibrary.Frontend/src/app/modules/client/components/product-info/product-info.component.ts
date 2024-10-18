import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environment/environment';
import { BookService } from '../../../library';
import { Book, CommandHandler, CoverType, CurrencyPipeApplier, getStringCoverType, RouteReader } from '../../../shared';
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
    private readonly routeReader: RouteReader,
    private readonly route: ActivatedRoute,
    private readonly currenctyApplier: CurrencyPipeApplier,
    @Inject(CART_ADD_BOOK_COMMAND_HANDLER) private readonly addBookToCartHandler: CommandHandler<CartAddBookCommand>
  ) { }

  ngOnInit(): void {
    this.book$ = this.routeReader.readIdInt(
      this.route,
      (id: number) => this.bookService.getById(id),
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
  onErrorImage(event: Event) {
    const element = event.target as HTMLImageElement;
    element.src = environment.bookCoverPlaceholder;
  }
}
