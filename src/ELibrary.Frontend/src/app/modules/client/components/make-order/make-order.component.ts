import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { filter, Observable, Subject } from 'rxjs';
import { CartBook, Client, CurrencyPipeApplier, ValidationMessage } from '../../../shared';
import { CartService, ClientService } from '../../../shop';

@Component({
  selector: 'app-make-order',
  templateUrl: './make-order.component.html',
  styleUrl: './make-order.component.scss'
})
export class MakeOrderComponent implements OnInit, OnDestroy {
  readonly itemHeight = 200;
  readonly amountItemsInView = 3;
  readonly scollSize = 420;

  items$!: Observable<CartBook[]>;
  client$!: Observable<Client>;
  private destroy$ = new Subject<void>();

  constructor(
    private readonly validateInput: ValidationMessage,
    private readonly clientService: ClientService,
    private readonly currencyApplier: CurrencyPipeApplier,
    private readonly cartService: CartService,
  ) { }

  ngOnInit(): void {

    this.items$ = this.cartService.getCartBooks();

    this.client$ = this.clientService.getClient().pipe(
      filter((client): client is Client => client !== null)
    );
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }

  calculateSelectionSize(): number {
    return this.scollSize;
  }

  trackById(index: number, cartBook: CartBook): number {
    return cartBook.bookId;
  }
  getBookPage(cartBook: CartBook): number {
    return cartBook.book.id;
  }
  getTotalPrice(cartBooks: CartBook[]): number {
    return cartBooks.reduce((total, cartBook) => {
      return total + (cartBook.book.price * cartBook.bookAmount);
    }, 0);
  }

  makeOrder() {
    // this.shopCommand.dispatchCommand(ShopCommandObject.Order, ShopCommandType.Add, this, null, this.dialogRef);
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currencyApplier.applyCurrencyPipe(value);
  }
  getCartBookPrice(cartBook: CartBook): number {
    return cartBook.book.price * cartBook.bookAmount;
  }
}
