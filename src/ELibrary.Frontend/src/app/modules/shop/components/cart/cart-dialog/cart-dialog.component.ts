import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { debounceTime, distinctUntilChanged, Observable, Subject } from 'rxjs';
import { CartService, ShopCommand, ShopCommandObject, ShopCommandRole, ShopCommandType } from '../../..';
import { environment } from '../../../../../../environment/environment';
import { CartBook, CurrencyPipeApplier, redirectPathes } from '../../../../shared';

@Component({
  selector: 'app-cart-dialog',
  templateUrl: './cart-dialog.component.html',
  styleUrl: './cart-dialog.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CartDialogComponent implements OnInit {
  readonly itemHeight = 200;
  readonly amountItemsInView = 3;
  readonly scollSize = 420;

  items$!: Observable<CartBook[]>;
  private inputChangeSubject$ = new Subject<{ cartBook: CartBook, value: number }>();

  get maxAmount() { return environment.maxOrderAmount; }
  get booksUrlPath() { return redirectPathes.client_products; }

  constructor(
    private readonly cartService: CartService,
    private readonly shopCommand: ShopCommand,
    private readonly currencyApplier: CurrencyPipeApplier,
    private readonly dialogRef: MatDialogRef<CartDialogComponent>,
  ) { }

  ngOnInit(): void {
    this.items$ = this.cartService.getCartBooks();

    this.inputChangeSubject$.pipe(
      debounceTime(300),
      distinctUntilChanged((prev, curr) => prev.value === curr.value)
    ).subscribe(({ cartBook, value }) => {
      cartBook = {
        ...cartBook,
        bookAmount: value
      }
      this.shopCommand.dispatchCommand(ShopCommandObject.Cart, ShopCommandType.Update, ShopCommandRole.Client, this, cartBook);
    });
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currencyApplier.applyCurrencyPipe(value);
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
    this.shopCommand.dispatchCommand(ShopCommandObject.Cart, ShopCommandType.Delete, ShopCommandRole.Client, this, cartBook);
  }

  getBookPage(cartBook: CartBook): number {
    return cartBook.book.id;
  }

  makeOrder() {
    this.shopCommand.dispatchCommand(ShopCommandObject.Order, ShopCommandType.Add, ShopCommandRole.Client, this, null, this.dialogRef);
  }
}
