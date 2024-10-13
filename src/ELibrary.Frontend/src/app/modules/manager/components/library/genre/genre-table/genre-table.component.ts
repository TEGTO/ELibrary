/* eslint-disable @typescript-eslint/no-explicit-any */
import { ChangeDetectionStrategy, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { map, Observable } from 'rxjs';
import { CREATE_GENRE_COMMAND_HANDLER, CreateGenreCommand, DELETE_GENRE_COMMAND_HANDLER, DeleteGenreCommand, GenreService, UPDATE_GENRE_COMMAND_HANDLER, UpdateGenreCommand } from '../../../../../library';
import { CommandHandler, defaultLibraryFilterRequest, GenericTableComponent, Genre, LibraryFilterRequest } from '../../../../../shared';

@Component({
  selector: 'app-genre-table',
  templateUrl: './genre-table.component.html',
  styleUrl: './genre-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GenreTableComponent implements OnInit {
  @ViewChild(GenericTableComponent) table!: GenericTableComponent;

  items$!: Observable<Genre[]>;
  totalAmount$!: Observable<number>;

  private filterReq: LibraryFilterRequest = defaultLibraryFilterRequest();
  private defaultPagination = { pageIndex: 1, pageSize: 10 };
  columns = [
    { header: 'Name', field: 'name' },
  ];

  constructor(
    private readonly genreService: GenreService,
    @Inject(CREATE_GENRE_COMMAND_HANDLER) private readonly createHandler: CommandHandler<CreateGenreCommand>,
    @Inject(UPDATE_GENRE_COMMAND_HANDLER) private readonly updateHandler: CommandHandler<UpdateGenreCommand>,
    @Inject(DELETE_GENRE_COMMAND_HANDLER) private readonly deleteHandler: CommandHandler<DeleteGenreCommand>,
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
    const command: CreateGenreCommand = {};
    this.createHandler.dispatch(command);
  }
  update(item: any) {
    const command: UpdateGenreCommand = {
      genre: item as Genre
    };
    this.updateHandler.dispatch(command);
  }
  delete(item: any) {
    const command: DeleteGenreCommand = {
      genre: item as Genre
    };
    this.deleteHandler.dispatch(command);
  }
}