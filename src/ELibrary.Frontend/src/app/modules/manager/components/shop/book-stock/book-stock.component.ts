/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { map, Observable } from 'rxjs';
import { CommandHandler, GenericTableComponent, LocaleService, LocalizedDatePipe, PaginatedRequest, redirectPathes, stockBookOrderTypeToString } from '../../../../shared';
import { ADD_BOOKSTOCK_ORDER_COMMAND_HANDLER, AddBookStockOrderCommand, BookstockOrderService } from '../../../../shop';

interface StockOrderItem {
  id: number,
  createdAt: Date,
  totalChangeAmount: string,
  type: string,
  clientName: string,
  changes: number,
}
@Component({
  selector: 'app-book-stock',
  templateUrl: './book-stock.component.html',
  styleUrl: './book-stock.component.scss'
})
export class BookStockComponent implements OnInit {
  @ViewChild(GenericTableComponent) table!: GenericTableComponent;

  items$!: Observable<StockOrderItem[]>;
  totalAmount$!: Observable<number>;

  private defaultPagination = { pageIndex: 1, pageSize: 10 };
  columns = [
    { header: 'Id', field: 'id', linkPath: (item: any) => `${redirectPathes.manager_bookstock}/${item.id}` },
    { header: 'Created At', field: 'createdAt', pipe: new LocalizedDatePipe(this.localeService.getLocale()) },
    { header: 'Client', field: 'clientName' },
    { header: 'Type', field: 'type' },
    { header: 'Stock Change', field: 'totalChangeAmount' },
  ];

  constructor(
    private readonly localeService: LocaleService,
    private readonly stockOrderService: BookstockOrderService,
    @Inject(ADD_BOOKSTOCK_ORDER_COMMAND_HANDLER) private readonly bookStockAddHandler: CommandHandler<AddBookStockOrderCommand>
  ) { }

  ngOnInit(): void {
    this.fetchTotalAmount();
    this.fetchPaginatedItems(this.defaultPagination);
  }

  private fetchTotalAmount(): void {
    this.totalAmount$ = this.stockOrderService.getOrderTotalAmount();
  }
  private fetchPaginatedItems(pagination: { pageIndex: number, pageSize: number }): void {
    const req: PaginatedRequest =
    {
      pageNumber: pagination.pageIndex,
      pageSize: pagination.pageSize
    };
    this.items$ = this.stockOrderService.getPaginatedOrders(req).pipe(
      map(orders => orders.slice(0, pagination.pageSize).map(x => ({
        id: x.id,
        createdAt: x.createdAt,
        type: stockBookOrderTypeToString(x.type),
        totalChangeAmount: this.getTotalChangeAmountString(x.totalChangeAmount),
        clientName: x.client.name,
        changes: x.stockBookChanges.length,
      })))
    );
  }

  onPageChange(pagination: { pageIndex: number, pageSize: number }): void {
    this.fetchPaginatedItems(pagination);
  }

  createNew() {
    const command: AddBookStockOrderCommand = {};
    this.bookStockAddHandler.dispatch(command);
  }

  getTotalChangeAmountString(num: number): string {
    return num > 0 ? `+${num}` : `${num}`;
  }
}