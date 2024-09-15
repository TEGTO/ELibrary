import { Component, OnDestroy } from '@angular/core';
import { map, Observable, Subject } from 'rxjs';
import { GenreService, LibraryCommand, LibraryCommandObject, LibraryCommandType } from '../../..';
import { defaultLibraryFilterRequest, GenreResponse, LibraryFilterRequest } from '../../../../shared';

@Component({
  selector: 'genre-table',
  templateUrl: './genre-table.component.html',
  styleUrl: './genre-table.component.scss'
})
export class GenreTableComponent implements OnDestroy {
  items$!: Observable<GenreResponse[]>;
  totalAmount$!: Observable<number>;
  private destroy$ = new Subject<void>();

  columns = [
    { header: 'Name', field: 'name' },
  ];

  constructor(
    private readonly genreService: GenreService,
    private readonly libraryCommand: LibraryCommand
  ) { }

  ngOnInit(): void {
    this.totalAmount$ = this.genreService.getItemTotalAmount(defaultLibraryFilterRequest());
    this.pageChange({ pageIndex: 1, pageSize: 10 });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  pageChange(item: any) {
    let pageParams = item as { pageIndex: number, pageSize: number };
    let req: LibraryFilterRequest = {
      ...defaultLibraryFilterRequest(),
      pageNumber: pageParams.pageIndex,
      pageSize: pageParams.pageSize,
    }
    this.items$ = this.genreService.getPaginated(req).pipe(
      map(items => items.slice(0, pageParams.pageSize))
    );
  }
  createNew() {
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Genre, LibraryCommandType.Create, this);
  }
  update(item: any) {
    let entity = item as GenreResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Genre, LibraryCommandType.Update, this, entity);
  }
  delete(item: any) {
    let entity = item as GenreResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Genre, LibraryCommandType.Delete, this, entity);
  }
}