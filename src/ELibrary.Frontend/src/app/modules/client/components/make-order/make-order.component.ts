import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { filter, map, Observable, tap } from 'rxjs';
import { CartBook, Client, CurrencyPipeApplier, getDefaultOrder, mapCartBookToOrderBook, minDateValidator, noSpaces, notEmptyString, Order, PaymentMethod, redirectPathes, ValidationMessage } from '../../../shared';
import { CartService, ClientService, ShopCommand, ShopCommandObject, ShopCommandRole, ShopCommandType } from '../../../shop';

@Component({
  selector: 'app-make-order',
  templateUrl: './make-order.component.html',
  styleUrl: './make-order.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MakeOrderComponent implements OnInit {
  readonly itemHeight = 200;
  readonly amountItemsInView = 3;
  readonly scrollSizePerObject = 220;
  readonly scollSize = 420;
  readonly panelOpenState = signal(true);
  formGroup!: FormGroup;

  items$!: Observable<CartBook[]>;
  client$!: Observable<Client>;

  get deliveryAddressInput() { return this.formGroup.get('address')!; }
  get deliveryTimeInput() { return this.formGroup.get('deliveryTime')!; }
  get paymentMethodInput() { return this.formGroup.get('payment')!; }
  get redirectToProductsPagePath() { return redirectPathes.client_products; }

  constructor(
    private readonly validateInput: ValidationMessage,
    private readonly clientService: ClientService,
    private readonly currencyApplier: CurrencyPipeApplier,
    private readonly cartService: CartService,
    private readonly shopCommand: ShopCommand,
  ) { }

  ngOnInit(): void {
    this.items$ = this.cartService.getCartBooks().pipe(
      map(cartBooks => cartBooks.filter(cartBook => cartBook.book.stockAmount > 0))
    );

    this.client$ = this.clientService.getClient().pipe(
      filter((client): client is Client => client !== null),
      tap((client) => {
        this.initializeForm(client);
      })
    );
  }

  initializeForm(client: Client): void {
    const nextDay = new Date();
    nextDay.setDate(nextDay.getDate() + 1);
    nextDay.setHours(0, 0, 0, 0);
    this.formGroup = new FormGroup(
      {
        payment: new FormControl(PaymentMethod.Cash, [Validators.required]),
        deliveryTime: new FormControl(nextDay, [Validators.required, minDateValidator(nextDay)]),
        address: new FormControl(client.address, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(512)]),
      });
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }

  calculateSelectionSize(bookAmount: number): number {
    return this.scrollSizePerObject * bookAmount > this.scollSize ? this.scollSize : this.scrollSizePerObject * bookAmount;
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

  makeOrder(books: CartBook[]) {
    const formValues = { ...this.formGroup.value };
    const order: Order = {
      ...getDefaultOrder(),
      deliveryAddress: formValues.address,
      deliveryTime: formValues.deliveryTime,
      paymentMethod: formValues.payment,
      orderBooks: books.map(x => mapCartBookToOrderBook(x))
    };
    this.shopCommand.dispatchCommand(ShopCommandObject.Order, ShopCommandType.Add, ShopCommandRole.Client, this, order);
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currencyApplier.applyCurrencyPipe(value);
  }
  getCartBookPrice(cartBook: CartBook): number {
    return cartBook.book.price * cartBook.bookAmount;
  }
}
