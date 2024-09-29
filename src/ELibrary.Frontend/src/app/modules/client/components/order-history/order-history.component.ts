import { ChangeDetectionStrategy, Component, Inject, OnInit, signal } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { filter, Observable } from 'rxjs';
import { Client, CommandHandler, CurrencyPipeApplier, getDefaultOrder, getOrderStatusString, minDateValidator, noSpaces, notEmptyString, Order, OrderBook, OrderStatus, PaginatedRequest, redirectPathes, ValidationMessage } from '../../../shared';
import { CLIENT_CANCEL_ORDER_COMMAND_HANDLER, CLIENT_UPDATE_ORDER_COMMAND_HANDLER, ClientCancelOrderCommand, ClientService, ClientUpdateOrderCommand, OrderService } from '../../../shop';

@Component({
  selector: 'app-order-history',
  templateUrl: './order-history.component.html',
  styleUrl: './order-history.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OrderHistoryComponent implements OnInit {
  readonly itemHeight = 200;
  readonly amountItemsInView = 3;
  readonly scrollSizePerObject = 220;
  readonly scollSize = 420;
  pageSize = 10;
  pageSizeOptions: number[] = [10, 20, 30];
  private defaultPagination = { pageIndex: 1, pageSize: 10 };
  private orderPanelStates = new Map<number, ReturnType<typeof signal>>();
  private orderFormGroups = new Map<number, FormGroup>();

  items$!: Observable<Order[]>;
  client$!: Observable<Client>;
  totalAmount$!: Observable<number>;

  get redirectToProductsPagePath() { return redirectPathes.client_products; }

  constructor(
    private readonly validateInput: ValidationMessage,
    private readonly clientService: ClientService,
    private readonly currencyApplier: CurrencyPipeApplier,
    private readonly orderService: OrderService,
    @Inject(CLIENT_UPDATE_ORDER_COMMAND_HANDLER) private readonly updateOrderHandler: CommandHandler<ClientUpdateOrderCommand>,
    @Inject(CLIENT_CANCEL_ORDER_COMMAND_HANDLER) private readonly cancelOrderHandler: CommandHandler<ClientCancelOrderCommand>
  ) { }

  ngOnInit(): void {

    this.client$ = this.clientService.getClient().pipe(
      filter((client): client is Client => client !== null)
    );

    this.totalAmount$ = this.orderService.getOrderTotalAmount();
    this.fetchPaginatedItems(this.defaultPagination);
  }

  onPageChange(event: PageEvent): void {
    const pageIndex = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.fetchPaginatedItems({ pageIndex: pageIndex, pageSize: this.pageSize });
  }
  private fetchPaginatedItems(pagination: { pageIndex: number, pageSize: number }): void {
    const req: PaginatedRequest =
    {
      pageNumber: pagination.pageIndex,
      pageSize: pagination.pageSize
    }
    this.items$ = this.orderService.getPaginatedOrders(req);
  }
  calculateSelectionSize(bookAmount: number): number {
    return this.scrollSizePerObject * bookAmount > this.scollSize ? this.scollSize : this.scrollSizePerObject * bookAmount;
  }
  trackById(index: number, orderBoook: OrderBook): number {
    return orderBoook.bookId;
  }
  getPanelState(order: Order): ReturnType<typeof signal> {
    if (!this.orderPanelStates.has(order.id)) {
      this.orderPanelStates.set(order.id, signal(false));
    }
    return this.orderPanelStates.get(order.id)!;
  }

  getFormGroup(order: Order): FormGroup {
    if (!this.orderFormGroups.has(order.id)) {
      this.initializeForm(order);
    }
    return this.orderFormGroups.get(order.id)!;
  }
  initializeForm(order: Order): void {
    this.orderFormGroups.set(order.id, new FormGroup(
      {
        payment: new FormControl(order.paymentMethod),
        deliveryTime: new FormControl(order.deliveryTime, [Validators.required, minDateValidator(order.deliveryTime)]),
        address: new FormControl(order.deliveryAddress, [Validators.required, notEmptyString, noSpaces, Validators.maxLength(512)]),
      })
    );

    if (!this.isOrderProcessing(order)) {
      this.orderFormGroups.get(order.id)!.disable();
    }
  }
  paymentInput(order: Order) {
    return this.orderFormGroups.get(order.id)!.get('payment')!;
  }
  deliveryAddressInput(order: Order) {
    return this.orderFormGroups.get(order.id)!.get('address')!;
  }
  deliveryTimeInput(order: Order) {
    return this.orderFormGroups.get(order.id)!.get('deliveryTime')!;
  }

  updateOrder(order: Order) {
    const formValues = { ... this.orderFormGroups.get(order.id)!.value };
    const updatedOrder: Order =
    {
      ...getDefaultOrder(),
      id: order.id,
      deliveryAddress: formValues.address,
      deliveryTime: formValues.deliveryTime,
    };
    const command: ClientUpdateOrderCommand =
    {
      order: updatedOrder
    }
    this.updateOrderHandler.dispatch(command);
  }

  deleteOrder(order: Order) {
    const command: ClientCancelOrderCommand =
    {
      order: order
    }
    this.cancelOrderHandler.dispatch(command);
  }

  getBookPage(orderBook: OrderBook): number {
    return orderBook.book.id;
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currencyApplier.applyCurrencyPipe(value);
  }
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  validateInputField(input: AbstractControl<any, any>) {
    return this.validateInput.getValidationMessage(input);
  }

  getOrderBookPrice(orderBook: OrderBook): number {
    return orderBook.book.price * orderBook.bookAmount;
  }
  getOrderStatusString(order: Order): string {
    return getOrderStatusString(order.orderStatus);
  }
  isOrderCanceled(order: Order): boolean {
    return order.orderStatus === OrderStatus.Canceled;
  }
  isOrderProcessing(order: Order): boolean {
    return order.orderStatus === OrderStatus.InProcessing;
  }
  isOrderDelivered(order: Order): boolean {
    return order.orderStatus === OrderStatus.Delivered;
  }
}
