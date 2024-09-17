/* eslint-disable @typescript-eslint/no-explicit-any */
import { CurrencyPipe } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { map, Observable, Subject } from 'rxjs';
import { BookService, LibraryCommand, LibraryCommandObject, LibraryCommandType } from '../../../../../library';
import { BookFilterRequest, BookResponse, defaultBookFilterRequest, LocaleService, LocalizedDatePipe } from '../../../../../shared';

@Component({
  selector: 'app-book-table',
  templateUrl: './book-table.component.html',
  styleUrl: './book-table.component.scss'
})
export class BookTableComponent implements OnInit, OnDestroy {
  items$!: Observable<{
    id: number,
    name: string,
    publicationDate: Date,
    author: string,
    genre: string,
    publisher: string,
  }[]>;
  totalAmount$!: Observable<number>;
  private destroy$ = new Subject<void>();

  columns = [
    { header: 'Name', field: 'name' },
    { header: 'Publication Date', field: 'publicationDate', pipe: new LocalizedDatePipe(this.localeService.getLocale()) },
    { header: 'Author', field: 'author' },
    { header: 'Genre', field: 'genre' },
    { header: 'Publisher', field: 'publisher' },
    { header: 'Price', field: 'price', pipe: new CurrencyPipe(this.localeService.getLocale(), this.localeService.getCurrency()) },
    { header: 'Stock Amount', field: 'stockAmount' },
  ];

  constructor(
    private readonly localeService: LocaleService,
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
    const pageParams = item as { pageIndex: number, pageSize: number };
    const req: BookFilterRequest = {
      ...defaultBookFilterRequest(),
      pageNumber: pageParams.pageIndex,
      pageSize: pageParams.pageSize,
    }
    this.items$ = this.bookService.getPaginated(req).pipe(
      map(books => books.slice(0, pageParams.pageSize).map(x => ({
        id: x.id,
        name: x.name,
        publicationDate: x.publicationDate,
        price: x.price,
        coverType: x.coverType,
        pageAmount: x.pageAmount,
        stockAmount: x.stockAmount,
        author: `${x.author.name} ${x.author.lastName}`,
        genre: x.genre.name,
        publisher: x.publisher.name
      })))
    );
  }
  createNew() {
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Book, LibraryCommandType.Create, this);
  }
  update(item: any) {
    const entity = item as BookResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Book, LibraryCommandType.Update, this, entity);
  }
  delete(item: any) {
    const entity = item as BookResponse;
    this.libraryCommand.dispatchCommand(LibraryCommandObject.Book, LibraryCommandType.Delete, this, entity);
  }
}