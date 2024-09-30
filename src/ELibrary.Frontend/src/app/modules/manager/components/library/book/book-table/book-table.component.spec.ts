/* eslint-disable @typescript-eslint/no-explicit-any */
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { BookService, LibraryDialogManager } from '../../../../../library';
import { Book, GenericTableComponent, getDefaultBook } from '../../../../../shared';
import { BookTableComponent } from './book-table.component';

describe('BookTableComponent', () => {
  let component: BookTableComponent;
  let fixture: ComponentFixture<BookTableComponent>;
  let mockDialogManager: jasmine.SpyObj<LibraryDialogManager>;
  let mockBookService: jasmine.SpyObj<BookService>;

  const mockItems: Book[] = [
    getDefaultBook(),
    getDefaultBook(),
  ];
  const mockAmount = 2;

  beforeEach(async () => {
    mockDialogManager = jasmine.createSpyObj('LibraryDialogManager', [
      'openBookDetailsMenu',
      'openConfirmMenu'
    ]);

    mockBookService = jasmine.createSpyObj('BookService', [
      'getItemTotalAmount',
      'getBooksPaginated',
      'createBook',
      'updateBook',
      'deleteBookById'
    ]);

    mockBookService.getItemTotalAmount.and.returnValue(of(mockAmount));
    mockBookService.getPaginated.and.returnValue(of(mockItems));

    await TestBed.configureTestingModule({
      declarations: [BookTableComponent, GenericTableComponent],
      providers: [
        { provide: LibraryDialogManager, useValue: mockDialogManager },
        { provide: BookService, useValue: mockBookService }
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

  it('should initialize and call pageChange on ngOnInit', () => {
    spyOn(component, 'onPageChange');
    component.ngOnInit();
    expect(component.onPageChange).toHaveBeenCalledWith({ pageIndex: 1, pageSize: 10 });
  });

  it('should bind data to generic-table component', () => {
    component.items$ = of(mockItems.map(item => ({
      id: item.id,
      name: item.name,
      publicationDate: item.publicationDate,
      author: `${item.author.name} ${item.author.lastName}`,
      genre: item.genre.name,
      publisher: item.publisher.name
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
      publisher: item.publisher.name
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
        publisher: item.publisher.name
      })));
    });
  });

  it('should open create dialog and call createBook on confirmation', () => {
    const mockBook: Book = getDefaultBook();
    const dialogRef = { afterClosed: () => of(mockBook) };
    mockDialogManager.openBookDetailsMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'createNew').and.callThrough();

    component.createNew();
    fixture.detectChanges();

    expect(mockDialogManager.openBookDetailsMenu).toHaveBeenCalled();
    expect(mockBookService.create).toHaveBeenCalled();
  });

  it('should open update dialog and call updateBook on confirmation', () => {
    const mockBook: Book = getDefaultBook();
    const dialogRef = { afterClosed: () => of(mockBook) };
    mockDialogManager.openBookDetailsMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'update').and.callThrough();

    component.update(mockBook);
    fixture.detectChanges();

    expect(mockDialogManager.openBookDetailsMenu).toHaveBeenCalledWith(mockBook);
    expect(mockBookService.update).toHaveBeenCalled();
  });

  it('should open confirmation dialog and call deleteBookById on confirmation', () => {
    const mockBook: Book = mockItems[0];
    const dialogRef = { afterClosed: () => of(true) };
    mockDialogManager.openConfirmMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'delete').and.callThrough();

    component.delete(mockBook);
    fixture.detectChanges();

    expect(mockDialogManager.openConfirmMenu).toHaveBeenCalled();
    expect(mockBookService.deleteById).toHaveBeenCalledWith(mockBook.id);
  });
});