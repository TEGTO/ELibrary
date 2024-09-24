import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { filter, Observable } from 'rxjs';
import { Client, CurrencyPipeApplier, Order, PaginatedRequest, redirectPathes, ValidationMessage } from '../../../shared';
import { ClientService, OrderService, ShopCommand } from '../../../shop';

@Component({
  selector: 'app-order-history',
  templateUrl: './order-history.component.html',
  styleUrl: './order-history.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OrderHistoryComponent implements OnInit {

  items$!: Observable<Order[]>;
  client$!: Observable<Client>;
  totalAmount$!: Observable<number>;

  pageSize = 10;
  pageSizeOptions: number[] = [10, 20, 30];
  private defaultPagination = { pageIndex: 1, pageSize: 10 };

  get redirectToProductsPagePath() { return redirectPathes.client_products; }


  constructor(
    private readonly validateInput: ValidationMessage,
    private readonly clientService: ClientService,
    private readonly currencyApplier: CurrencyPipeApplier,
    private readonly shopCommand: ShopCommand,
    private readonly orderService: OrderService,
  ) { }


  ngOnInit(): void {

    this.client$ = this.clientService.getClient().pipe(
      filter((client): client is Client => client !== null),
      // tap((client) => {
      // })
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
}
