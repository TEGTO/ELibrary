import { Component, Inject, LOCALE_ID, OnDestroy, OnInit } from '@angular/core';
import { map, Observable, Subject } from 'rxjs';
import { BookService, LibraryCommand, LibraryCommandObject, LibraryCommandType } from '../../..';
import { BookFilterRequest, BookResponse, defaultBookFilterRequest, LocalizedDatePipe } from '../../../../shared';

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
    private readonly bookService: BookService,
    private readonly libraryCommand: LibraryCommand
  ) { }

  ngOnInit(): void {
    this.totalAmount$ = this.bookService.getItemTotalAmount(defaultBookFilterRequest());
    this.pageChange({ pageIndex: 1, pageSize: 10 });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  pageChange(item: any) {
    let pageParams = item as { pageIndex: number, pageSize: number };
    let req: BookFilterRequest = {
      ...defaultBookFilterRequest(),
      pageNumber: pageParams.pageIndex,
      pageSize: pageParams.pageSize,
    }
    this.items$ = this.bookService.getPaginated(req).pipe(
      map(books => books.slice(0, pageParams.pageSize).map(x => ({
        id: x.id,
        title: x.name,
        publicationDate: x.publicationDate,
        author: `${x.author.name} ${x.author.lastName}`,
        genre: x.genre.name
      })))
    );
  }
  createNew() {
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Book, LibraryCommandType.Create, this);
  }
  update(item: any) {
    let entity = item as BookResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Book, LibraryCommandType.Update, this, entity);
  }
  delete(item: any) {
    let entity = item as BookResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Book, LibraryCommandType.Delete, this, entity);
  }
}