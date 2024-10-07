/* eslint-disable @typescript-eslint/no-explicit-any */
import { CurrencyPipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { map, Observable } from 'rxjs';
import { BookService, CREATE_BOOK_COMMAND_HANDLER, CreateBookCommand, DELETE_BOOK_COMMAND_HANDLER, DeleteBookCommand, UPDATE_BOOK_COMMAND_HANDLER, UpdateBookCommand } from '../../../../../library';
import { Book, BookFilterRequest, CommandHandler, GenericTableComponent, defaultBookFilterRequest as getDefaultBookFilterRequest, getProductInfoPath, LocaleService, LocalizedDatePipe } from '../../../../../shared';

interface BookItem {
  id: number;
  name: string;
  publicationDate: Date;
  author: string;
  genre: string;
  publisher: string;
}
@Component({
  selector: 'app-book-table',
  templateUrl: './book-table.component.html',
  styleUrl: './book-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookTableComponent implements OnInit {
  @ViewChild(GenericTableComponent) table!: GenericTableComponent;

  items$!: Observable<BookItem[]>;
  totalAmount$!: Observable<number>;

  private filterReq: BookFilterRequest = getDefaultBookFilterRequest();
  private defaultPagination = { pageIndex: 1, pageSize: 10 };
  columns = [
    { header: 'Name', field: 'name', linkPath: (item: any) => getProductInfoPath(item.id) },
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
    @Inject(CREATE_BOOK_COMMAND_HANDLER) private readonly createHandler: CommandHandler<CreateBookCommand>,
    @Inject(UPDATE_BOOK_COMMAND_HANDLER) private readonly updateHandler: CommandHandler<UpdateBookCommand>,
    @Inject(DELETE_BOOK_COMMAND_HANDLER) private readonly deleteHandler: CommandHandler<DeleteBookCommand>,
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
    this.totalAmount$ = this.bookService.getItemTotalAmount(this.filterReq);
  }
  private fetchPaginatedItems(pagination: { pageIndex: number, pageSize: number }): void {
    this.updateFilterPagination(pagination);
    this.items$ = this.bookService.getPaginated(this.filterReq).pipe(
      map(books => books.slice(0, pagination.pageSize).map(x => ({
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

  onPageChange(pagination: { pageIndex: number, pageSize: number }): void {
    this.fetchPaginatedItems(pagination);
  }

  filterChange(req: BookFilterRequest): void {
    if (this.table) {
      this.table.resetPagination();
    }
    this.filterReq = { ...req };
    this.fetchTotalAmount();
    this.fetchPaginatedItems(this.defaultPagination);
  }

  createNew() {
    const command: CreateBookCommand = {};
    this.createHandler.dispatch(command);
  }
  update(item: any) {
    const command: UpdateBookCommand = {
      book: item as Book
    };
    this.updateHandler.dispatch(command);
  }
  delete(item: any) {
    const command: DeleteBookCommand = {
      book: item as Book
    };
    this.deleteHandler.dispatch(command);
  }
}