import { ChangeDetectionStrategy, Component, Inject, OnInit, signal } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { filter, map, Observable, tap } from 'rxjs';
import { environment } from '../../../../../environment/environment';
import { CartBook, Client, combineDateTime, CommandHandler, CurrencyPipeApplier, DeliveryMethod, getDefaultOrder, getOrderCreateMinDate, getOrderDeliveryMethods, getPaymentMethods, getProductInfoPath, getProductsPath, mapCartBookToOrderBook, minDateValidator, notEmptyString, Order, PaymentMethod, phoneValidator, ValidationMessage } from '../../../shared';
import { CartService, CLIENT_ADD_ORDER_COMMAND_HANDLER, ClientAddOrderCommand, ClientService } from '../../../shop';

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

  get contactClientNameInput() { return this.formGroup.get('contactClientName')! as FormControl; }
  get contactPhoneInput() { return this.formGroup.get('contactPhone')! as FormControl; }
  get deliveryAddressInput() { return this.formGroup.get('address')! as FormControl; }
  get paymentMethodInput() { return this.formGroup.get('payment')! as FormControl; }
  get deliveryMethodInput() { return this.formGroup.get('delivery')! as FormControl; }
  get deliveryDateInput() { return this.formGroup.get('deliveryDate')! as FormControl; }
  get deliveryTimeInput() { return this.formGroup.get('deliveryTime')! as FormControl; }
  get redirectToProductsPagePath() { return getProductsPath(); }
  get paymentMethods() { return getPaymentMethods(); }
  get deliveryMethods() { return getOrderDeliveryMethods(); }
  get minOrderTime() { return environment.minOrderTime; }
  get maxOrderTime() { return environment.maxOrderTime; }

  constructor(
    private readonly validateInput: ValidationMessage,
    private readonly clientService: ClientService,
    private readonly currencyApplier: CurrencyPipeApplier,
    private readonly cartService: CartService,
    @Inject(CLIENT_ADD_ORDER_COMMAND_HANDLER) private readonly addOrderHandler: CommandHandler<ClientAddOrderCommand>
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
    this.formGroup = new FormGroup(
      {
        payment: new FormControl(PaymentMethod.Cash, [Validators.required]),
        deliveryDate: new FormControl(getOrderCreateMinDate(), [Validators.required, minDateValidator(getOrderCreateMinDate())]),
        deliveryTime: new FormControl(getOrderCreateMinDate(), [Validators.required]),
        contactClientName: new FormControl(`${client.name} ${client.middleName} ${client.lastName}`, [Validators.required, notEmptyString, Validators.maxLength(256)]),
        contactPhone: new FormControl(client.phone, [Validators.required, phoneValidator(10, 50)]),
        address: new FormControl(client.address, [Validators.required, notEmptyString, Validators.maxLength(512)]),
        delivery: new FormControl(DeliveryMethod.SelfPickup, [Validators.required]),
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
  getBookPage(cartBook: CartBook): string[] {
    return [`/${getProductInfoPath(cartBook.book.id)}`];
  }
  getTotalPrice(cartBooks: CartBook[]): number {
    return cartBooks.reduce((total, cartBook) => {
      return total + (cartBook.book.price * cartBook.bookAmount);
    }, 0);
  }
  getDeliveryAddressFieldName(): string {
    if (this.deliveryMethodInput.value === DeliveryMethod.AddressDelivery) {
      return "Delivery Address";
    }
    else {
      return "Shop Address";
    }
  }

  makeOrder(books: CartBook[]) {
    if (this.formGroup.valid) {
      const formValues = { ...this.formGroup.value };
      const order: Order = {
        ...getDefaultOrder(),
        contactClientName: formValues.contactClientName,
        contactPhone: formValues.contactPhone,
        deliveryAddress: formValues.address,
        deliveryTime: combineDateTime(formValues.deliveryDate, formValues.deliveryTime),
        paymentMethod: formValues.payment,
        deliveryMethod: formValues.delivery,
        orderBooks: books.map(x => mapCartBookToOrderBook(x))
      };
      const command: ClientAddOrderCommand =
      {
        order: order,
      }
      this.addOrderHandler.dispatch(command);
    }
    else {
      this.formGroup.markAllAsTouched();
    }
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currencyApplier.applyCurrencyPipe(value);
  }
  getCartBookPrice(cartBook: CartBook): number {
    return cartBook.book.price * cartBook.bookAmount;
  }
  onErrorImage(event: Event) {
    const element = event.target as HTMLImageElement;
    element.src = environment.bookCoverPlaceholder;
  }
}
