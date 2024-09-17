/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject, map } from 'rxjs';
import { LibraryCommand, LibraryCommandObject, LibraryCommandType, PublisherService } from '../../../../../library';
import { LibraryFilterRequest, PublisherResponse, defaultLibraryFilterRequest } from '../../../../../shared';

@Component({
  selector: 'app-publisher-table',
  templateUrl: './publisher-table.component.html',
  styleUrl: './publisher-table.component.scss'
})
export class PublisherTableComponent implements OnInit, OnDestroy {
  items$!: Observable<PublisherResponse[]>;
  totalAmount$!: Observable<number>;
  private destroy$ = new Subject<void>();

  columns = [
    { header: 'Name', field: 'name' },
  ];

  constructor(
    private readonly publisherService: PublisherService,
    private readonly libraryCommand: LibraryCommand
  ) { }

  ngOnInit(): void {
    this.totalAmount$ = this.publisherService.getItemTotalAmount(defaultLibraryFilterRequest());
    this.pageChange({ pageIndex: 1, pageSize: 10 });
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  pageChange(item: any) {
    const pageParams = item as { pageIndex: number, pageSize: number };
    const req: LibraryFilterRequest = {
      ...defaultLibraryFilterRequest(),
      pageNumber: pageParams.pageIndex,
      pageSize: pageParams.pageSize,
    }
    this.items$ = this.publisherService.getPaginated(req).pipe(
      map(items => items.slice(0, pageParams.pageSize))
    );
  }
  createNew() {
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Publisher, LibraryCommandType.Create, this);
  }
  update(item: any) {
    const entity = item as PublisherResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Publisher, LibraryCommandType.Update, this, entity);
  }
  delete(item: any) {
    const entity = item as PublisherResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Publisher, LibraryCommandType.Delete, this, entity);
  }
}
