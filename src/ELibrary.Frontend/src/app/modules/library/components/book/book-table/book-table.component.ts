import { Component, OnDestroy, OnInit } from '@angular/core';
import { map, Observable, Subject, takeUntil, tap } from 'rxjs';
import { BookService, LibraryDialogManager } from '../../..';
import { BookResponse, bookToCreateRequest, bookToUpdateRequest, PaginatedRequest } from '../../../../shared';

@Component({
  selector: 'book-table',
  templateUrl: './book-table.component.html',
  styleUrl: './book-table.component.scss'
})
export class BookTableComponent implements OnInit, OnDestroy {
  pageSize: number = 10;

  items$!: Observable<{
    id: number,
    title: string,
    publicationDate: Date,
    author: string,
    genre: string
  }[]>;
  private destroy$ = new Subject<void>();

  columns = [
    { header: 'Title', field: 'title' },
    { header: 'Publication Date', field: 'publicationDate' },
    { header: 'Author', field: 'author' },
    { header: 'Genre', field: 'genre' }
  ];

  constructor(private readonly dialogManager: LibraryDialogManager, private readonly libraryEntityService: BookService) { }

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
    this.items$ = this.libraryEntityService.getBooksPaginated(req).pipe(
      map(books => books.map(x => ({
        id: x.id,
        title: x.title,
        publicationDate: x.publicationDate,
        author: `${x.author.name} ${x.author.lastName}`,
        genre: x.genre.name
      })))
    );
  }
  createNew() {
    let entity: BookResponse = {
      id: 0,
      title: "",
      publicationDate: new Date(),
      author: {
        id: 0,
        name: "",
        lastName: "",
        dateOfBirth: new Date(),
      },
      genre: {
        id: 0,
        name: ""
      }
    }
    this.dialogManager.openBookDetailsMenu(entity).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(author => {
      if (author) {
        let req = bookToCreateRequest(author);
        this.libraryEntityService.createBook(req);
      }
    });
  }
  update(item: any) {
    let entity = item as BookResponse;
    this.dialogManager.openBookDetailsMenu(entity).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(author => {
      if (author) {
        let req = bookToUpdateRequest(author);
        this.libraryEntityService.updateBook(req);
      }
    });
  }
  delete(item: any) {
    let author = item as BookResponse;
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      tap(result => {
        if (result === true) {
          this.libraryEntityService.deleteBookById(author.id);
        }
      }),
      takeUntil(this.destroy$)
    ).subscribe();
  }
}