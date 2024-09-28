import { ChangeDetectionStrategy, Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { debounceTime, distinctUntilChanged, Observable, Subject, takeUntil } from 'rxjs';
import { CartService, ShopCommand, ShopCommandObject, ShopCommandRole, ShopCommandType } from '../../..';
import { environment } from '../../../../../../environment/environment';
import { Book, CartBook, CurrencyPipeApplier, redirectPathes } from '../../../../shared';

@Component({
  selector: 'app-cart-dialog',
  templateUrl: './cart-dialog.component.html',
  styleUrls: ['./cart-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CartDialogComponent implements OnInit, OnDestroy {
  readonly itemHeight = 200;
  readonly amountItemsInView = 3;
  readonly scollSize = 420;

  items$!: Observable<CartBook[]>;
  private inputChangeSubjectMap = new Map<number, Subject<number>>();
  private destroy$ = new Subject<void>();

  get booksUrlPath() { return redirectPathes.client_products; }

  constructor(
    private readonly cartService: CartService,
    private readonly shopCommand: ShopCommand,
    private readonly currencyApplier: CurrencyPipeApplier,
    private readonly dialogRef: MatDialogRef<CartDialogComponent>,
  ) { }

  ngOnInit(): void {
    this.items$ = this.cartService.getCartBooks();
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }


  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currencyApplier.applyCurrencyPipe(value);
  }

  calculateSelectionSize(): number {
    return this.scollSize;
  }

  getMaxAmount(book: Book): number {
    return environment.maxOrderAmount > book.stockAmount ? book.stockAmount : environment.maxOrderAmount;
  }

  trackById(index: number, cartBook: CartBook): number {
    return cartBook.bookId;
  }

  getPrice(cartBook: CartBook): number {
    return cartBook.book.price * cartBook.bookAmount;
  }

  getTotalPrice(cartBooks: CartBook[]): number {
    return cartBooks.reduce((total, cartBook) => {
      if (cartBook.book.stockAmount > 0) {
        return total + (cartBook.book.price * cartBook.bookAmount);
      }
      return total;
    }, 0);
  }

  onInputChange(cartBook: CartBook, event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const value = +inputElement.value;

    let subject = this.inputChangeSubjectMap.get(cartBook.bookId);
    if (!subject) {
      subject = new Subject<number>();
      this.inputChangeSubjectMap.set(cartBook.bookId, subject);

      subject.pipe(
        takeUntil(this.destroy$),
        debounceTime(300),
        distinctUntilChanged()
      ).subscribe((value: number) => {
        const updatedCartBook = { ...cartBook, bookAmount: value };
        this.shopCommand.dispatchCommand(ShopCommandObject.Cart, ShopCommandType.Update, ShopCommandRole.Client, this, updatedCartBook);
      });
    }

    subject.next(value);
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
