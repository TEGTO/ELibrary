import { CurrencyPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { debounceTime, distinctUntilChanged, Observable, Subject } from 'rxjs';
import { CartService, ShopCommand, ShopCommandObject, ShopCommandType } from '../../..';
import { environment } from '../../../../../../environment/environment';
import { CartBook, LocaleService } from '../../../../shared';

@Component({
  selector: 'app-cart-dialog',
  templateUrl: './cart-dialog.component.html',
  styleUrl: './cart-dialog.component.scss'
})
export class CartDialogComponent implements OnInit {
  currencyPipe!: CurrencyPipe;
  readonly itemHeight = 200;
  readonly amountItemsInView = 3;
  readonly scollSize = 420;


  items$!: Observable<CartBook[]>;
  private inputChangeSubject$ = new Subject<{ cartBook: CartBook, value: number }>();

  get maxAmount() { return environment.maxOrderAmount; }

  constructor(
    private readonly localeService: LocaleService,
    private readonly cartService: CartService,
    private readonly shopCommand: ShopCommand
  ) { }

  ngOnInit(): void {
    this.currencyPipe = new CurrencyPipe(this.localeService.getLocale(), this.localeService.getCurrency());
    this.items$ = this.cartService.getCartBooks();

    this.inputChangeSubject$.pipe(
      debounceTime(300),
      distinctUntilChanged((prev, curr) => prev.value === curr.value)
    ).subscribe(({ cartBook, value }) => {
      cartBook = {
        ...cartBook,
        bookAmount: value
      }
      this.shopCommand.dispatchCommand(ShopCommandObject.Cart, ShopCommandType.Update, this, cartBook);
    });
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currencyPipe ? this.currencyPipe.transform(value) : value;
  }

  calculateSelectionSize(): number {
    return this.scollSize;
  }

  trackById(index: number, cartBook: CartBook): number {
    return cartBook.bookId;
  }
  getPrice(cartBook: CartBook): number {
    return cartBook.book.price * cartBook.bookAmount;
  }
  getTotalPrice(cartBooks: CartBook[]): number {
    return cartBooks.reduce((total, cartBook) => {
      return total + (cartBook.book.price * cartBook.bookAmount);
    }, 0);
  }

  onInputChange(cartBook: CartBook, event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const value = +inputElement.value;
    this.inputChangeSubject$.next({ cartBook, value });
  }
  deleteCartBook(cartBook: CartBook) {
    this.shopCommand.dispatchCommand(ShopCommandObject.Cart, ShopCommandType.Delete, this, cartBook);
  }

}
