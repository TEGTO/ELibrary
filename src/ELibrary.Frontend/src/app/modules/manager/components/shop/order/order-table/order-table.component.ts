/* eslint-disable @typescript-eslint/no-explicit-any */
import { CurrencyPipe } from '@angular/common';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { map, Observable } from 'rxjs';
import { CommandHandler, GenericTableComponent, getClientName, getOrderStatusString, LocaleService, LocalizedDatePipe, Order, PaginatedRequest, redirectPathes } from '../../../../../shared';
import { MANAGER_ORDER_DETAILS_COMMAND_HANDLER, ManagerOrderDetailsCommand, OrderService } from '../../../../../shop';

interface OrderItem {
  id: number,
  createdAt: Date,
  totalPrice: number,
  deliveryTime: Date,
  orderStatus: string,
  clientName: string,
  order: Order
}
@Component({
  selector: 'app-order-table',
  templateUrl: './order-table.component.html',
  styleUrl: './order-table.component.scss'
})
export class OrderTableComponent implements OnInit {
  @ViewChild(GenericTableComponent) table!: GenericTableComponent;

  items$!: Observable<OrderItem[]>;
  totalAmount$!: Observable<number>;

  private defaultPagination = { pageIndex: 1, pageSize: 10 };
  columns = [
    { header: 'Id', field: 'id', linkPath: (item: any) => `${redirectPathes.manager_orders}/${item.id}` },
    { header: 'Created At', field: 'createdAt', pipe: new LocalizedDatePipe(this.localeService.getLocale()) },
    { header: 'Total Price', field: 'totalPrice', pipe: new CurrencyPipe(this.localeService.getLocale(), this.localeService.getCurrency()) },
    { header: 'Delivery Time', field: 'deliveryTime', pipe: new LocalizedDatePipe(this.localeService.getLocale()), pipeArgs: [true] },
    { header: 'Order Status', field: 'orderStatus' },
    { header: 'Client', field: 'clientName' },
  ];

  constructor(
    private readonly localeService: LocaleService,
    private readonly orderService: OrderService,
    @Inject(MANAGER_ORDER_DETAILS_COMMAND_HANDLER) private readonly managerOrderDetailsHandler: CommandHandler<ManagerOrderDetailsCommand>,
  ) { }

  ngOnInit(): void {
    this.fetchTotalAmount();
    this.fetchPaginatedItems(this.defaultPagination);
  }

  private fetchTotalAmount(): void {
    this.totalAmount$ = this.orderService.getOrderTotalAmount();
  }
  private fetchPaginatedItems(pagination: { pageIndex: number, pageSize: number }): void {
    const req: PaginatedRequest =
    {
      pageNumber: pagination.pageIndex,
      pageSize: pagination.pageSize
    };
    this.items$ = this.orderService.getPaginatedOrders(req).pipe(
      map(orders => orders.slice(0, pagination.pageSize).map(x => ({
        id: x.id,
        createdAt: x.createdAt,
        totalPrice: x.totalPrice,
        deliveryTime: x.deliveryTime,
        orderStatus: getOrderStatusString(x.orderStatus),
        clientName: getClientName(x.client),
        order: x
      })))
    );
  }

  onPageChange(pagination: { pageIndex: number, pageSize: number }): void {
    this.fetchPaginatedItems(pagination);
  }

  update(orderItem: OrderItem) {
    const command: ManagerOrderDetailsCommand = {
      order: orderItem.order
    };
    this.managerOrderDetailsHandler.dispatch(command);
  }
}
