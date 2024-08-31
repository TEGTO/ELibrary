import { Component, OnDestroy } from '@angular/core';
import { Observable, Subject, takeUntil, tap } from 'rxjs';
import { GenreService, LibraryDialogManager } from '../../..';
import { GenreResponse, genreToCreateRequest, genreToUpdateRequest, PaginatedRequest } from '../../../../shared';

@Component({
  selector: 'genre-table',
  templateUrl: './genre-table.component.html',
  styleUrl: './genre-table.component.scss'
})
export class GenreTableComponent implements OnDestroy {
  items$!: Observable<GenreResponse[]>;
  private destroy$ = new Subject<void>();

  columns = [
    { header: 'Name', field: 'name' },
  ];

  constructor(private readonly dialogManager: LibraryDialogManager, private readonly libraryEntityService: GenreService) { }

  ngOnInit(): void {
    this.pageChange({ pageIndex: 1, pageSize: 10 });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  pageChange(item: any) {
    let pageParams = item as { pageIndex: number, pageSize: number };
    let req: PaginatedRequest = {
      pageNumber: pageParams.pageIndex,
      pageSize: pageParams.pageSize + 1
    }
    this.items$ = this.libraryEntityService.getGenresPaginated(req);
  }
  createNew() {
    let entity: GenreResponse = {
      id: 0,
      name: "",
    }
    this.dialogManager.openGenreDetailsMenu(entity).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(genre => {
      if (genre) {
        let req = genreToCreateRequest(genre);
        this.libraryEntityService.createGenre(req);
      }
    });
  }
  update(item: any) {
    let entity = item as GenreResponse;
    this.dialogManager.openGenreDetailsMenu(entity).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(genre => {
      if (genre) {
        let req = genreToUpdateRequest(genre);
        this.libraryEntityService.updateGenre(req);
      }
    });
  }
  delete(item: any) {
    let entity = item as GenreResponse;
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      tap(result => {
        if (result === true) {
          this.libraryEntityService.deleteGenreById(entity.id);
        }
      }),
      takeUntil(this.destroy$)
    ).subscribe();
  }
}
