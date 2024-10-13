/* eslint-disable @typescript-eslint/no-explicit-any */
import { ChangeDetectionStrategy, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { Observable, map } from 'rxjs';
import { CREATE_PUBLISHER_COMMAND_HANDLER, CreatePublisherCommand, DELETE_PUBLISHER_COMMAND_HANDLER, DeletePublisherCommand, PublisherService, UPDATE_PUBLISHER_COMMAND_HANDLER, UpdatePublisherCommand } from '../../../../../library';
import { CommandHandler, GenericTableComponent, LibraryFilterRequest, Publisher, defaultLibraryFilterRequest } from '../../../../../shared';

@Component({
  selector: 'app-publisher-table',
  templateUrl: './publisher-table.component.html',
  styleUrl: './publisher-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
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
    @Inject(CREATE_PUBLISHER_COMMAND_HANDLER) private readonly createHandler: CommandHandler<CreatePublisherCommand>,
    @Inject(UPDATE_PUBLISHER_COMMAND_HANDLER) private readonly updateHandler: CommandHandler<UpdatePublisherCommand>,
    @Inject(DELETE_PUBLISHER_COMMAND_HANDLER) private readonly deleteHandler: CommandHandler<DeletePublisherCommand>,
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
    const command: CreatePublisherCommand = {};
    this.createHandler.dispatch(command);
  }
  update(item: any) {
    const command: UpdatePublisherCommand = {
      publisher: item as Publisher
    };
    this.updateHandler.dispatch(command);
  }
  delete(item: any) {
    const command: DeletePublisherCommand = {
      publisher: item as Publisher
    };
    this.deleteHandler.dispatch(command);
  }
}
