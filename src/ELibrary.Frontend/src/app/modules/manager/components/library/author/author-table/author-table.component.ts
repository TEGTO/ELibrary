
import { Component, OnInit, ViewChild } from '@angular/core';
import { map, Observable } from 'rxjs';
import { AuthorService, LibraryCommand, LibraryCommandObject, LibraryCommandType, TableColumn } from '../../../../../library';
import { Author, defaultLibraryFilterRequest, GenericTableComponent, LibraryFilterRequest, LocaleService, LocalizedDatePipe } from '../../../../../shared';

@Component({
  selector: 'app-author-table',
  templateUrl: './author-table.component.html',
  styleUrl: './author-table.component.scss'
})
export class AuthorTableComponent implements OnInit {
  @ViewChild(GenericTableComponent) table!: GenericTableComponent;

  items$!: Observable<Author[]>;
  totalAmount$!: Observable<number>;

  private filterReq: LibraryFilterRequest = defaultLibraryFilterRequest();
  private defaultPagination = { pageIndex: 1, pageSize: 10 };
  columns: TableColumn[] = [
    { header: 'Name', field: 'name' },
    { header: 'Last Name', field: 'lastName' },
    { header: 'Date of Birth', field: 'dateOfBirth', pipe: new LocalizedDatePipe(this.localeService.getLocale()) }
  ];

  constructor(
    private readonly localeService: LocaleService,
    private readonly authorService: AuthorService,
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
    this.totalAmount$ = this.authorService.getItemTotalAmount(this.filterReq);
  }
  private fetchPaginatedItems(pagination: { pageIndex: number, pageSize: number }): void {
    this.updateFilterPagination(pagination);
    this.items$ = this.authorService.getPaginated(this.filterReq).pipe(
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

  createNew(): void {
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Author, LibraryCommandType.Create, this);
  }

  update(item: Author): void {
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Author, LibraryCommandType.Update, this, item);
  }

  delete(item: Author): void {
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Author, LibraryCommandType.Delete, this, item);
  }
}