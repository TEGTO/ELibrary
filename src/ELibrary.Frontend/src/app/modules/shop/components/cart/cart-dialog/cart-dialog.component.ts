import { ChangeDetectionStrategy, Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { debounceTime, distinctUntilChanged, Observable, Subject, takeUntil } from 'rxjs';
import { CartService, CLIENT_ADD_ORDER_COMMAND_HANDLER, ClientAddOrderCommand, DELETE_CART_BOOK_COMMAND_HANDLER, DeleteCartBookCommand, UPDATE_CART_BOOK_COMMAND_HANDLER, UpdateCartBookCommand } from '../../..';
import { environment } from '../../../../../../environment/environment';
import { Book, CartBook, CommandHandler, CurrencyPipeApplier, getProductInfoPath, getProductsPath } from '../../../../shared';

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

  get booksUrlPath() { return getProductsPath(); }

  constructor(
    private readonly cartService: CartService,
    private readonly currencyApplier: CurrencyPipeApplier,
    @Inject(UPDATE_CART_BOOK_COMMAND_HANDLER) private readonly updateCartBookHanlder: CommandHandler<UpdateCartBookCommand>,
    @Inject(DELETE_CART_BOOK_COMMAND_HANDLER) private readonly deleteCartBookHandler: CommandHandler<DeleteCartBookCommand>,
    @Inject(CLIENT_ADD_ORDER_COMMAND_HANDLER) private readonly clientAddOrderHandler: CommandHandler<ClientAddOrderCommand>,
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

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  getMaxAmount(book: Book): number {
    return environment.maxOrderAmount;
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
        const command: UpdateCartBookCommand = {
          cartBook: { ...cartBook, bookAmount: value }
        }
        this.updateCartBookHanlder.dispatch(command);
      });
    }

    subject.next(value);
  }

  deleteCartBook(cartBook: CartBook) {
    const command: DeleteCartBookCommand =
    {
      cartBook: cartBook
    }
    this.deleteCartBookHandler.dispatch(command);
  }

  getBookPage(cartBook: CartBook): string[] {
    return [`/${getProductInfoPath(cartBook.book.id)}`];
  }
  makeOrder() {
    const command: ClientAddOrderCommand =
    {
      order: undefined,
      matDialogRef: this.dialogRef
    }
    this.clientAddOrderHandler.dispatch(command);
  }
  checkIfInStock(book: Book): boolean {
    return book.stockAmount > 0;
  }
}
