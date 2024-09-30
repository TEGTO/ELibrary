/* eslint-disable @typescript-eslint/no-explicit-any */

import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { BookService, CREATE_BOOK_COMMAND_HANDLER, CreateBookCommand, DELETE_BOOK_COMMAND_HANDLER, DeleteBookCommand, UPDATE_BOOK_COMMAND_HANDLER, UpdateBookCommand } from '../../../../../library';
import { Book, CommandHandler, GenericTableComponent, getDefaultBook } from '../../../../../shared';
import { BookTableComponent } from './book-table.component';

describe('BookTableComponent', () => {
  let component: BookTableComponent;
  let fixture: ComponentFixture<BookTableComponent>;
  let mockBookService: jasmine.SpyObj<BookService>;
  let mockCreateBookCommandHandler: jasmine.SpyObj<CommandHandler<CreateBookCommand>>;
  let mockUpdateBookCommandHandler: jasmine.SpyObj<CommandHandler<UpdateBookCommand>>;
  let mockDeleteBookCommandHandler: jasmine.SpyObj<CommandHandler<DeleteBookCommand>>;

  const mockItems: Book[] = [
    getDefaultBook(),
    getDefaultBook(),
  ];
  const mockAmount = 2;

  beforeEach(async () => {
    mockBookService = jasmine.createSpyObj('BookService', [
      'getItemTotalAmount',
      'getPaginated',
      'createBook',
      'updateBook',
      'deleteBookById'
    ]);

    mockCreateBookCommandHandler = jasmine.createSpyObj<CommandHandler<CreateBookCommand>>([
      'dispatch',
    ]);

    mockUpdateBookCommandHandler = jasmine.createSpyObj<CommandHandler<UpdateBookCommand>>([
      'dispatch',
    ]);

    mockDeleteBookCommandHandler = jasmine.createSpyObj<CommandHandler<DeleteBookCommand>>([
      'dispatch',
    ]);

    mockBookService.getItemTotalAmount.and.returnValue(of(mockAmount));
    mockBookService.getPaginated.and.returnValue(of(mockItems));

    await TestBed.configureTestingModule({
      imports: [GenericTableComponent, BrowserAnimationsModule],
      declarations: [BookTableComponent],
      providers: [
        { provide: BookService, useValue: mockBookService },
        { provide: CREATE_BOOK_COMMAND_HANDLER, useValue: mockCreateBookCommandHandler },
        { provide: UPDATE_BOOK_COMMAND_HANDLER, useValue: mockUpdateBookCommandHandler },
        { provide: DELETE_BOOK_COMMAND_HANDLER, useValue: mockDeleteBookCommandHandler }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BookTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize and fetch paginated items', () => {
    spyOn<any>(component, 'fetchPaginatedItems');
    component.ngOnInit();
    expect(component["fetchPaginatedItems"]).toHaveBeenCalledWith({ pageIndex: 1, pageSize: 10 });
  });

  it('should bind data to generic-table component', () => {
    component.items$ = of(mockItems.map(item => ({
      id: item.id,
      name: item.name,
      publicationDate: item.publicationDate,
      author: `${item.author.name} ${item.author.lastName}`,
      genre: item.genre.name,
      publisher: item.publisher.name,
      price: item.price,
      coverType: item.coverType,
      pageAmount: item.pageAmount,
      stockAmount: item.stockAmount,
    })));
    fixture.detectChanges();

    const genericTable = fixture.debugElement.query(By.directive(GenericTableComponent));
    expect(genericTable).toBeTruthy();

    const componentInstance = genericTable.componentInstance as GenericTableComponent;
    expect(componentInstance.items).toEqual(mockItems.map(item => ({
      id: item.id,
      name: item.name,
      publicationDate: item.publicationDate,
      author: `${item.author.name} ${item.author.lastName}`,
      genre: item.genre.name,
      publisher: item.publisher.name,
      price: item.price,
      coverType: item.coverType,
      pageAmount: item.pageAmount,
      stockAmount: item.stockAmount,
    })));
    expect(componentInstance.totalItemAmount).toBe(mockAmount);
  });

  it('should handle pageChange and update items$', () => {
    mockBookService.getPaginated.and.returnValue(of(mockItems));

    component.onPageChange({ pageIndex: 1, pageSize: 10 });
    fixture.detectChanges();

    component.items$.subscribe(items => {
      expect(items).toEqual(mockItems.slice(0, 10).map(item => ({
        id: item.id,
        name: item.name,
        publicationDate: item.publicationDate,
        author: `${item.author.name} ${item.author.lastName}`,
        genre: item.genre.name,
        publisher: item.publisher.name,
        price: item.price,
        coverType: item.coverType,
        pageAmount: item.pageAmount,
        stockAmount: item.stockAmount,
      })));
    });
  });

  it('should dispatch create book command', () => {

    component.createNew();
    fixture.detectChanges();

    expect(mockCreateBookCommandHandler.dispatch).toHaveBeenCalled();
  });

  it('should dispatch update book command', () => {
    const mockBook: Book = getDefaultBook();

    component.update(mockBook);
    fixture.detectChanges();

    expect(mockUpdateBookCommandHandler.dispatch).toHaveBeenCalled();
  });

  it('should dispatch delete book command', () => {
    const mockBook: Book = mockItems[0];

    component.delete(mockBook);
    fixture.detectChanges();

    expect(mockDeleteBookCommandHandler.dispatch).toHaveBeenCalled();
  });
});