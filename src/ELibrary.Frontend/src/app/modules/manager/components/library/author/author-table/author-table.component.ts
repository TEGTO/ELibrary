/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnDestroy, OnInit } from '@angular/core';
import { map, Observable, Subject } from 'rxjs';
import { AuthorService, LibraryCommand, LibraryCommandObject, LibraryCommandType, TableColumn } from '../../../../../library';
import { AuthorResponse, defaultLibraryFilterRequest, LibraryFilterRequest, LocaleService, LocalizedDatePipe } from '../../../../../shared';

@Component({
  selector: 'app-author-table',
  templateUrl: './author-table.component.html',
  styleUrl: './author-table.component.scss'
})
export class AuthorTableComponent implements OnInit, OnDestroy {
  items$!: Observable<AuthorResponse[]>;
  totalAmount$!: Observable<number>;
  private destroy$ = new Subject<void>();

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
    this.totalAmount$ = this.authorService.getItemTotalAmount(defaultLibraryFilterRequest());
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
    this.items$ = this.authorService.getPaginated(req).pipe(
      map(items => items.slice(0, pageParams.pageSize))
    );
  }
  createNew() {
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Author, LibraryCommandType.Create, this);
  }
  update(item: any) {
    const entity = item as AuthorResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Author, LibraryCommandType.Update, this, entity);
  }
  delete(item: any) {
    const entity = item as AuthorResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Author, LibraryCommandType.Delete, this, entity);
  }
}