/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnInit, ViewChild } from '@angular/core';
import { Observable, map } from 'rxjs';
import { LibraryCommand, LibraryCommandObject, LibraryCommandType, PublisherService } from '../../../../../library';
import { GenericTableComponent, LibraryFilterRequest, Publisher, defaultLibraryFilterRequest } from '../../../../../shared';

@Component({
  selector: 'app-publisher-table',
  templateUrl: './publisher-table.component.html',
  styleUrl: './publisher-table.component.scss'
})
export class PublisherTableComponent implements OnInit {
  @ViewChild(GenericTableComponent) table!: GenericTableComponent;

  items$!: Observable<Publisher[]>;
  totalAmount$!: Observable<number>;

  private filterReq: LibraryFilterRequest = defaultLibraryFilterRequest();
  private defaultPagination = { pageIndex: 1, pageSize: 10 };
  columns = [
    { header: 'Name', field: 'name' },
  ];

  constructor(
    private readonly publisherService: PublisherService,
    private readonly libraryCommand: LibraryCommand
  ) { }

  ngOnInit(): void {
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
    this.totalAmount$ = this.publisherService.getItemTotalAmount(this.filterReq);
  }
  private fetchPaginatedItems(pagination: { pageIndex: number, pageSize: number }): void {
    this.updateFilterPagination(pagination);
    this.items$ = this.publisherService.getPaginated(this.filterReq).pipe(
      map(items => items.slice(0, pagination.pageSize))
    );
  }

  onPageChange(pagination: { pageIndex: number, pageSize: number }): void {
    this.fetchPaginatedItems(pagination);
  }

  filterChange(req: LibraryFilterRequest): void {
    if (this.table) {
      this.table.resetPagination();
    }
    this.filterReq = { ...req };
    this.fetchTotalAmount();
    this.fetchPaginatedItems(this.defaultPagination);
  }

  createNew() {
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Publisher, LibraryCommandType.Create, this);
  }
  update(item: any) {
    const entity = item as Publisher;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Publisher, LibraryCommandType.Update, this, entity);
  }
  delete(item: any) {
    const entity = item as Publisher;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Publisher, LibraryCommandType.Delete, this, entity);
  }
}
