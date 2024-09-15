import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { AuthorService, GenericTableComponent, LibraryDialogManager } from '../../..';
import { AuthorResponse } from '../../../../shared';
import { AuthorTableComponent } from './author-table.component';

describe('AuthorTableComponent', () => {
  let component: AuthorTableComponent;
  let fixture: ComponentFixture<AuthorTableComponent>;
  let mockDialogManager: jasmine.SpyObj<LibraryDialogManager>;
  let mockAuthorService: jasmine.SpyObj<AuthorService>;

  const mockItems: AuthorResponse[] = [
    { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date('1980-01-01') },
    { id: 2, name: 'Jane', lastName: 'Smith', dateOfBirth: new Date('1990-01-01') }
  ];
  const mockAmount = 2;

  beforeEach(async () => {
    mockDialogManager = jasmine.createSpyObj('LibraryDialogManager', [
      'openAuthorDetailsMenu',
      'openConfirmMenu'
    ]);

    mockAuthorService = jasmine.createSpyObj('AuthorService', [
      'getItemTotalAmount',
      'getAuthorsPaginated',
      'createAuthor',
      'updateAuthor',
      'deleteAuthorById'
    ]);

    mockAuthorService.getItemTotalAmount.and.returnValue(of(mockAmount));
    mockAuthorService.getPaginated.and.returnValue(of(mockItems));

    await TestBed.configureTestingModule({
      declarations: [AuthorTableComponent, GenericTableComponent],
      providers: [
        { provide: LibraryDialogManager, useValue: mockDialogManager },
        { provide: AuthorService, useValue: mockAuthorService }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthorTableComponent);
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
    component.items$ = of(mockItems);
    fixture.detectChanges();

    const genericTable = fixture.debugElement.query(By.directive(GenericTableComponent));
    expect(genericTable).toBeTruthy();

    const componentInstance = genericTable.componentInstance as GenericTableComponent;
    expect(componentInstance.items).toEqual(mockItems);
    expect(componentInstance.totalItemAmount).toBe(mockAmount);
  });

  it('should handle pageChange and update items$', () => {
    const mockItems = [
      { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date('1980-01-01') },
      { id: 2, name: 'Jane', lastName: 'Smith', dateOfBirth: new Date('1990-01-01') }
    ];
    mockAuthorService.getPaginated.and.returnValue(of(mockItems));

    component.pageChange({ pageIndex: 1, pageSize: 10 });
    fixture.detectChanges();

    component.items$.subscribe(items => {
      expect(items).toEqual(mockItems.slice(0, 10));
    });
  });

  it('should open create dialog and call createAuthor on confirmation', () => {
    const mockAuthor: AuthorResponse = { id: 0, name: '', lastName: '', dateOfBirth: new Date() };
    const dialogRef = { afterClosed: () => of(mockAuthor) };
    mockDialogManager.openAuthorDetailsMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'createNew').and.callThrough();

    component.createNew();
    fixture.detectChanges();

    expect(mockDialogManager.openAuthorDetailsMenu).toHaveBeenCalled();
    expect(mockAuthorService.create).toHaveBeenCalled();
  });

  it('should open update dialog and call updateAuthor on confirmation', () => {
    const mockAuthor: AuthorResponse = { id: 1, name: 'Updated Name', lastName: 'Updated LastName', dateOfBirth: new Date('1990-01-01') };
    const dialogRef = { afterClosed: () => of(mockAuthor) };
    mockDialogManager.openAuthorDetailsMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'update').and.callThrough();

    component.update(mockAuthor);
    fixture.detectChanges();

    expect(mockDialogManager.openAuthorDetailsMenu).toHaveBeenCalledWith(mockAuthor);
    expect(mockAuthorService.update).toHaveBeenCalled();
  });

  it('should open confirmation dialog and call deleteAuthorById on confirmation', () => {
    const mockAuthor: AuthorResponse = { id: 1, name: 'John', lastName: 'Doe', dateOfBirth: new Date('1980-01-01') };
    const dialogRef = { afterClosed: () => of(true) };
    mockDialogManager.openConfirmMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'delete').and.callThrough();

    component.delete(mockAuthor);
    fixture.detectChanges();

    expect(mockDialogManager.openConfirmMenu).toHaveBeenCalled();
    expect(mockAuthorService.deleteById).toHaveBeenCalledWith(mockAuthor.id);
  });

  it('should clean up subscriptions on destroy', () => {
    spyOn(component['destroy$'], 'next').and.callThrough();
    spyOn(component['destroy$'], 'complete').and.callThrough();

    component.ngOnDestroy();

    expect(component['destroy$'].next).toHaveBeenCalled();
    expect(component['destroy$'].complete).toHaveBeenCalled();
  });
});
