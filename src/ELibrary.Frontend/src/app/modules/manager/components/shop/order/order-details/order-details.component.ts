import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '../../../../../../../environment/environment';
import { Client, combineDateTime, CommandHandler, CurrencyPipeApplier, getClientName, getCreatedOrderMinDate, getDeliveryMethodsString, getOrderPaymentMethodString, getOrderStatusString, getOrderUpdateStatuses, getProductInfoPath, minDateValidator, noSpaces, notEmptyString, Order, OrderBook, OrderStatus, RouteReader, ValidationMessage } from '../../../../../shared';
import { MANAGER_CANCEL_ORDER_COMMAND_HANDLER, MANAGER_UPDATE_ORDER_COMMAND_HANDLER, ManagerCancelOrderCommand, ManagerUpdateOrderCommand, OrderService } from '../../../../../shop';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrl: './order-details.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OrderDetailsComponent implements OnInit {
  readonly itemHeight = 200;
  readonly scrollSizePerObject = 200;
  readonly scollSize = 420;
  private formGroup!: FormGroup;

  order$!: Observable<Order>;

  get deliveryDateInput() { return this.formGroup.get('deliveryDate')! as FormControl; }
  get deliveryTimeInput() { return this.formGroup.get('deliveryTime')! as FormControl; }
  get deliveryAddressInput() { return this.formGroup.get('address')! as FormControl; }
  get orderStatusInput() { return this.formGroup.get('orderStatus')! as FormControl; }
  get minOrderTime() { return environment.minOrderTime; }
  get maxOrderTime() { return environment.maxOrderTime; }

  constructor(
    private readonly validateInput: ValidationMessage,
    private readonly orderService: OrderService,
    private readonly currenctyApplier: CurrencyPipeApplier,
    private readonly routeReader: RouteReader,
    private readonly route: ActivatedRoute,
    @Inject(MANAGER_CANCEL_ORDER_COMMAND_HANDLER) private readonly managerCancelOrderHandler: CommandHandler<ManagerCancelOrderCommand>,
    @Inject(MANAGER_UPDATE_ORDER_COMMAND_HANDLER) private readonly managerUpdateOrderHandler: CommandHandler<ManagerUpdateOrderCommand>
  ) { }

  ngOnInit(): void {
    this.order$ = this.routeReader.readIdInt(
      this.route,
      (id: number) => this.orderService.managerGetOrderById(id),
    );
  }

  getFormGroup(order: Order): FormGroup {
    if (!this.formGroup) {
      this.formGroup = new FormGroup(
        {
          orderStatus: new FormControl(order.orderStatus, [Validators.required]),
          deliveryDate: new FormControl(order.deliveryTime, [Validators.required, minDateValidator(getCreatedOrderMinDate(order))]),
          deliveryTime: new FormControl(order.deliveryTime, [Validators.required]),
          address: new FormControl(order.deliveryAddress, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(512)]),
        });
    }
    if (this.isOrderCanceled(order) || this.isOrderCompleted(order)) {
      this.formGroup.disable();
    }

    return this.formGroup;
  }

  updateOrder(order: Order) {
    const formValues = { ...this.formGroup.value };
    const updatedOrder: Order = {
      ...order,
      deliveryAddress: formValues.address,
      deliveryTime: combineDateTime(formValues.deliveryDate, formValues.deliveryTime),
      orderStatus: formValues.orderStatus,
    };
    const command: ManagerUpdateOrderCommand = {
      order: updatedOrder
    };
    this.managerUpdateOrderHandler.dispatch(command);
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currenctyApplier.applyCurrencyPipe(value);
  }
  getBookPage(orderBook: OrderBook): string[] {
    return [`/${getProductInfoPath(orderBook.bookId)}`];
  }
  calculateSelectionSize(bookAmount: number): number {
    return this.scrollSizePerObject * bookAmount > this.scollSize ? this.scollSize : this.scrollSizePerObject * bookAmount;
  }
  trackById(index: number, orderBoook: OrderBook): number {
    return orderBoook.bookId;
  }
  getClientName(client: Client): string {
    return getClientName(client);
  }
  getOrderBookPrice(orderBook: OrderBook): number {
    return orderBook.book.price * orderBook.bookAmount;
  }
  getDeliveryMethodString(order: Order) {
    return getDeliveryMethodsString(order.deliveryMethod);
  }
  getOrderStatusString(order: Order): string {
    return getOrderStatusString(order.orderStatus);
  }
  getOrderPaymentMethodString(order: Order): string {
    return getOrderPaymentMethodString(order.paymentMethod);
  }
  cancelOrder(order: Order) {
    const command: ManagerCancelOrderCommand =
    {
      order: order
    }
    this.managerCancelOrderHandler.dispatch(command);
  }
  orderUnchangeable(order: Order): boolean {
    return this.isOrderCanceled(order) || this.isOrderCompleted(order);
  }
  isOrderCanceled(order: Order): boolean {
    return order.orderStatus === OrderStatus.Canceled;
  }
  isOrderCompleted(order: Order) {
    return order.orderStatus === OrderStatus.Completed;
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }
  getOrderStatusesInString() {
    return getOrderUpdateStatuses();
  }
  onErrorImage(event: Event) {
    const element = event.target as HTMLImageElement;
    element.src = environment.bookCoverPlaceholder;
  }
}
