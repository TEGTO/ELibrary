import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, map, Observable, of, switchMap } from 'rxjs';
import { BookService } from '../../../library';
import { Book, CoverType, CurrencyPipeApplier, getDefaultBook, getStringCoverType, RedirectorService } from '../../../shared';
import { ShopCommand, ShopCommandObject, ShopCommandType } from '../../../shop';

@Component({
  selector: 'app-product-info',
  templateUrl: './product-info.component.html',
  styleUrl: './product-info.component.scss'
})
export class ProductInfoComponent implements OnInit {
  bookAdded = false;

  book$!: Observable<Book>;

  constructor(
    private readonly bookService: BookService,
    private readonly shopCommand: ShopCommand,
    private readonly route: ActivatedRoute,
    private readonly redirectService: RedirectorService,
    private readonly currenctyApplier: CurrencyPipeApplier
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
    this.shopCommand.dispatchCommand(ShopCommandObject.Cart, ShopCommandType.Add, this, book);
    this.bookAdded = true;
  }
}
