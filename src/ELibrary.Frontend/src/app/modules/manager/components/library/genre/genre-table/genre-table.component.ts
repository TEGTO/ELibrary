/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component, OnInit, ViewChild } from '@angular/core';
import { map, Observable } from 'rxjs';
import { GenreService, LibraryCommand, LibraryCommandObject, LibraryCommandType } from '../../../../../library';
import { defaultLibraryFilterRequest, GenericTableComponent, GenreResponse, LibraryFilterRequest } from '../../../../../shared';

@Component({
  selector: 'app-genre-table',
  templateUrl: './genre-table.component.html',
  styleUrl: './genre-table.component.scss'
})
export class GenreTableComponent implements OnInit {
  @ViewChild(GenericTableComponent) table!: GenericTableComponent;

  items$!: Observable<GenreResponse[]>;
  totalAmount$!: Observable<number>;

  private filterReq: LibraryFilterRequest = defaultLibraryFilterRequest();
  private defaultPagination = { pageIndex: 1, pageSize: 10 };
  columns = [
    { header: 'Name', field: 'name' },
  ];

  constructor(
    private readonly genreService: GenreService,
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
    this.totalAmount$ = this.genreService.getItemTotalAmount(this.filterReq);
  }
  private fetchPaginatedItems(pagination: { pageIndex: number, pageSize: number }): void {
    this.updateFilterPagination(pagination);
    this.items$ = this.genreService.getPaginated(this.filterReq).pipe(
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
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Genre, LibraryCommandType.Create, this);
  }
  update(item: any) {
    const entity = item as GenreResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Genre, LibraryCommandType.Update, this, entity);
  }
  delete(item: any) {
    const entity = item as GenreResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Genre, LibraryCommandType.Delete, this, entity);
  }
}