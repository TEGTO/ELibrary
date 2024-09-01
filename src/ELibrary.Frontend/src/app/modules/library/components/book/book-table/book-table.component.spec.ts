import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { BookService, GenericTableComponent, LibraryDialogManager } from '../../..';
import { BookResponse } from '../../../../shared';
import { BookTableComponent } from './book-table.component';

describe('BookTableComponent', () => {
  let component: BookTableComponent;
  let fixture: ComponentFixture<BookTableComponent>;
  let mockDialogManager: jasmine.SpyObj<LibraryDialogManager>;
  let mockBookService: jasmine.SpyObj<BookService>;

  const mockItems: BookResponse[] = [
    { id: 1, title: 'Book One', publicationDate: new Date('2022-01-01'), author: { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date() }, genre: { id: 1, name: 'Fiction' } },
    { id: 2, title: 'Book Two', publicationDate: new Date('2023-01-01'), author: { id: 2, name: 'Jane', lastName: 'Smith', dateOfBirth: new Date() }, genre: { id: 2, name: 'Non-Fiction' } }
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
    mockBookService.getBooksPaginated.and.returnValue(of(mockItems));

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
    spyOn(component, 'pageChange');
    component.ngOnInit();
    expect(component.pageChange).toHaveBeenCalledWith({ pageIndex: 1, pageSize: 10 });
  });

  it('should bind data to generic-table component', () => {
    component.items$ = of(mockItems.map(item => ({
      id: item.id,
      title: item.title,
      publicationDate: item.publicationDate,
      author: `${item.author.name} ${item.author.lastName}`,
      genre: item.genre.name
    })));
    fixture.detectChanges();

    const genericTable = fixture.debugElement.query(By.directive(GenericTableComponent));
    expect(genericTable).toBeTruthy();

    const componentInstance = genericTable.componentInstance as GenericTableComponent;
    expect(componentInstance.items).toEqual(mockItems.map(item => ({
      id: item.id,
      title: item.title,
      publicationDate: item.publicationDate,
      author: `${item.author.name} ${item.author.lastName}`,
      genre: item.genre.name
    })));
    expect(componentInstance.totalItemAmount).toBe(mockAmount);
  });

  it('should handle pageChange and update items$', () => {
    mockBookService.getBooksPaginated.and.returnValue(of(mockItems));

    component.pageChange({ pageIndex: 1, pageSize: 10 });
    fixture.detectChanges();

    component.items$.subscribe(items => {
      expect(items).toEqual(mockItems.slice(0, 10).map(x => ({
        id: x.id,
        title: x.title,
        publicationDate: x.publicationDate,
        author: `${x.author.name} ${x.author.lastName}`,
        genre: x.genre.name
      })));
    });
  });

  it('should open create dialog and call createBook on confirmation', () => {
    const mockBook: BookResponse = {
      id: 0,
      title: '',
      publicationDate: new Date(),
      author: { id: 0, name: '', lastName: '', dateOfBirth: new Date() },
      genre: { id: 0, name: '' }
    };
    const dialogRef = { afterClosed: () => of(mockBook) };
    mockDialogManager.openBookDetailsMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'createNew').and.callThrough();

    component.createNew();
    fixture.detectChanges();

    expect(mockDialogManager.openBookDetailsMenu).toHaveBeenCalledWith(mockBook);
    expect(mockBookService.createBook).toHaveBeenCalled();
  });

  it('should open update dialog and call updateBook on confirmation', () => {
    const mockBook: BookResponse = {
      id: 1,
      title: 'Updated Title',
      publicationDate: new Date('2023-01-01'),
      author: { id: 1, name: 'Updated Name', lastName: 'Updated LastName', dateOfBirth: new Date() },
      genre: { id: 1, name: 'Updated Genre' }
    };
    const dialogRef = { afterClosed: () => of(mockBook) };
    mockDialogManager.openBookDetailsMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'update').and.callThrough();

    component.update(mockBook);
    fixture.detectChanges();

    expect(mockDialogManager.openBookDetailsMenu).toHaveBeenCalledWith(mockBook);
    expect(mockBookService.updateBook).toHaveBeenCalled();
  });

  it('should open confirmation dialog and call deleteBookById on confirmation', () => {
    const mockBook: BookResponse = mockItems[0];
    const dialogRef = { afterClosed: () => of(true) };
    mockDialogManager.openConfirmMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'delete').and.callThrough();

    component.delete(mockBook);
    fixture.detectChanges();

    expect(mockDialogManager.openConfirmMenu).toHaveBeenCalled();
    expect(mockBookService.deleteBookById).toHaveBeenCalledWith(mockBook.id);
  });

  it('should clean up subscriptions on destroy', () => {
    spyOn(component['destroy$'], 'next').and.callThrough();
    spyOn(component['destroy$'], 'complete').and.callThrough();

    component.ngOnDestroy();

    expect(component['destroy$'].next).toHaveBeenCalled();
    expect(component['destroy$'].complete).toHaveBeenCalled();
  });
});