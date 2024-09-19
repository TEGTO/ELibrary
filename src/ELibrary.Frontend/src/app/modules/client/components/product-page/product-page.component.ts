import { CurrencyPipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { map, Observable } from 'rxjs';
import { BookService } from '../../../library';
import { Book, BookFilterRequest, defaultBookFilterRequest, LocaleService } from '../../../shared';

@Component({
  selector: 'app-product-page',
  templateUrl: './product-page.component.html',
  styleUrl: './product-page.component.scss'
})
export class ProductPageComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  items$!: Observable<Book[]>;
  totalAmount$!: Observable<number>;

  currencyPipe!: CurrencyPipe;
  pageSize = 8;
  pageSizeOptions: number[] = [8, 16, 32];
  private defaultPagination = { pageIndex: 1, pageSize: 8 };
  private filterReq: BookFilterRequest = defaultBookFilterRequest();

  constructor(
    private readonly bookService: BookService,
    private readonly localeService: LocaleService,
  ) { }

  ngOnInit(): void {
    this.currencyPipe = new CurrencyPipe(this.localeService.getLocale(), this.localeService.getCurrency());
    this.fetchTotalAmount();
    this.fetchPaginatedItems(this.defaultPagination);
  }

  private updateFilterPagination(pagination: { pageIndex: number, pageSize: number }): void {
    this.filterReq = {
      ...this.filterReq,
      pageNumber: pagination.pageIndex,
      pageSize: pagination.pageSize
    };
  }

  private fetchTotalAmount(): void {
    this.totalAmount$ = this.bookService.getItemTotalAmount(this.filterReq);
  }
  private fetchPaginatedItems(pagination: { pageIndex: number, pageSize: number }): void {
    this.updateFilterPagination(pagination);
    this.items$ = this.bookService.getPaginated(this.filterReq).pipe(
      map(books => books.slice(0, pagination.pageSize))
    );
  }

  filterChange(req: BookFilterRequest): void {
    this.resetPagination();
    this.filterReq = { ...req };
    this.fetchTotalAmount();
    this.fetchPaginatedItems(this.defaultPagination);
  }

  onPageChange(event: PageEvent): void {
    const pageIndex = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.fetchPaginatedItems({ pageIndex: pageIndex, pageSize: this.pageSize });
  }
  resetPagination() {
    if (this.paginator) {
      this.paginator.pageIndex = this.defaultPagination.pageIndex;
      this.pageSize = this.defaultPagination.pageSize;
    }
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  applyCurrencyPipe(value: any): any {
    return this.currencyPipe ? this.currencyPipe.transform(value) : value;
  }
}
