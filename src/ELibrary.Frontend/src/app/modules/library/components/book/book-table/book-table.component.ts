import { Component, Inject, LOCALE_ID, OnDestroy, OnInit } from '@angular/core';
import { map, Observable, Subject, takeUntil } from 'rxjs';
import { BookService, LibraryDialogManager } from '../../..';
import { BookResponse, bookToCreateRequest, bookToUpdateRequest, LocalizedDatePipe, PaginatedRequest } from '../../../../shared';

@Component({
  selector: 'book-table',
  templateUrl: './book-table.component.html',
  styleUrl: './book-table.component.scss'
})
export class BookTableComponent implements OnInit, OnDestroy {
  items$!: Observable<{
    id: number,
    title: string,
    publicationDate: Date,
    author: string,
    genre: string
  }[]>;
  totalAmount$!: Observable<number>;
  private destroy$ = new Subject<void>();

  columns = [
    { header: 'Title', field: 'title' },
    { header: 'Publication Date', field: 'publicationDate', pipe: new LocalizedDatePipe(this.locale) },
    { header: 'Author', field: 'author' },
    { header: 'Genre', field: 'genre' }
  ];

  constructor(
    @Inject(LOCALE_ID) private locale: string,
    private readonly dialogManager: LibraryDialogManager,
    private readonly libraryEntityService: BookService) { }

  ngOnInit(): void {
    this.totalAmount$ = this.libraryEntityService.getItemTotalAmount();
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
      pageSize: pageParams.pageSize
    }
    this.items$ = this.libraryEntityService.getBooksPaginated(req).pipe(
      map(books => books.slice(0, pageParams.pageSize).map(x => ({
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
    ).subscribe(book => {
      if (book) {
        let req = bookToCreateRequest(book);
        this.libraryEntityService.createBook(req);
      }
    });
  }
  update(item: any) {
    let entity = item as BookResponse;
    this.dialogManager.openBookDetailsMenu(entity).afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(book => {
      if (book) {
        let req = bookToUpdateRequest(book);
        this.libraryEntityService.updateBook(req);
      }
    });
  }
  delete(item: any) {
    let book = item as BookResponse;
    this.dialogManager.openConfirmMenu().afterClosed().pipe(
      takeUntil(this.destroy$)
    ).subscribe(result => {
      if (result === true) {
        this.libraryEntityService.deleteBookById(book.id);
      }
    });
  }
}