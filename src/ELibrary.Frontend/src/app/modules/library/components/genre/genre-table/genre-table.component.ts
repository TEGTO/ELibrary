import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject, takeUntil, tap } from 'rxjs';
import { GenreService, LibraryDialogManager } from '../../..';
import { GenreResponse, genreToCreateRequest, genreToUpdateRequest, PaginatedRequest } from '../../../../shared';

@Component({
  selector: 'genre-table',
  templateUrl: './genre-table.component.html',
  styleUrl: './genre-table.component.scss'
})
export class GenreTableComponent implements OnInit, OnDestroy {
  pageSize: number = 10;

  items$!: Observable<GenreResponse[]>;
  private destroy$ = new Subject<void>();

  columns = [
    { header: 'Name', field: 'name' }
  ];

  constructor(private readonly dialogManager: LibraryDialogManager, private readonly libraryEntityService: GenreService) { }

  ngOnInit(): void {
    this.pageChange(1);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  pageChange(item: any) {
    let pageIndex = item as number;
    let req: PaginatedRequest = {
      pageNumber: pageIndex,
      pageSize: this.pageSize
    }
    this.items$ = this.libraryEntityService.getGenresPaginated(req);
  }
  createNew() {
    let genre: GenreResponse = {
      id: 0,
      name: "",
    }
    this.dialogManager.openGenreDetailsMenu(genre).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(genre => {
      if (genre) {
        let req = genreToCreateRequest(genre);
        this.libraryEntityService.createGenre(req);
      }
    });
  }
  update(item: any) {
    let genre = item as GenreResponse;
    this.dialogManager.openGenreDetailsMenu(genre).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(author => {
      if (author) {
        let req = genreToUpdateRequest(author);
        this.libraryEntityService.updateGenre(req);
      }
    });
  }
  delete(item: any) {
    let genre = item as GenreResponse;
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      tap(result => {
        if (result === true) {
          this.libraryEntityService.deleteGenreById(genre.id);
        }
      }),
      takeUntil(this.destroy$)
    ).subscribe();
  }
}
