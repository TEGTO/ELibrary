import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { GenericTableComponent, GenreService, LibraryDialogManager } from '../../..';
import { GenreResponse } from '../../../../shared';
import { GenreTableComponent } from './genre-table.component';

describe('GenreTableComponent', () => {
  let component: GenreTableComponent;
  let fixture: ComponentFixture<GenreTableComponent>;
  let mockDialogManager: jasmine.SpyObj<LibraryDialogManager>;
  let mockGenreService: jasmine.SpyObj<GenreService>;

  const mockItems = [{ id: 1, name: 'Fiction' }, { id: 2, name: 'Non-Fiction' }];
  const mockAmount = 100;

  beforeEach(async () => {
    mockDialogManager = jasmine.createSpyObj('LibraryDialogManager', [
      'openGenreDetailsMenu',
      'openConfirmMenu'
    ]);

    mockGenreService = jasmine.createSpyObj('GenreService', [
      'getItemTotalAmount',
      'getGenresPaginated',
      'createGenre',
      'updateGenre',
      'deleteGenreById'
    ]);

    mockGenreService.getItemTotalAmount.and.returnValue(of(mockAmount));
    mockGenreService.getGenresPaginated.and.returnValue(of(mockItems));

    await TestBed.configureTestingModule({
      declarations: [GenreTableComponent, GenericTableComponent],
      providers: [
        { provide: LibraryDialogManager, useValue: mockDialogManager },
        { provide: GenreService, useValue: mockGenreService }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GenreTableComponent);
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
    const mockItems = [{ id: 1, name: 'Fiction' }, { id: 2, name: 'Non-Fiction' }];
    mockGenreService.getGenresPaginated.and.returnValue(of(mockItems));

    component.pageChange({ pageIndex: 1, pageSize: 10 });
    fixture.detectChanges();

    component.items$.subscribe(items => {
      expect(items).toEqual(mockItems.slice(0, 10));
    });
  });

  it('should open create dialog and call createGenre on confirmation', () => {
    const mockGenre: GenreResponse = { id: 0, name: '' };
    const dialogRef = { afterClosed: () => of(mockGenre) };
    mockDialogManager.openGenreDetailsMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'createNew').and.callThrough();

    component.createNew();
    fixture.detectChanges();

    expect(mockDialogManager.openGenreDetailsMenu).toHaveBeenCalledWith(mockGenre);
    expect(mockGenreService.createGenre).toHaveBeenCalled();
  });

  it('should open update dialog and call updateGenre on confirmation', () => {
    const mockGenre: GenreResponse = { id: 1, name: 'Updated Name' };
    const dialogRef = { afterClosed: () => of(mockGenre) };
    mockDialogManager.openGenreDetailsMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'update').and.callThrough();

    component.update(mockGenre);
    fixture.detectChanges();

    expect(mockDialogManager.openGenreDetailsMenu).toHaveBeenCalledWith(mockGenre);
    expect(mockGenreService.updateGenre).toHaveBeenCalled();
  });

  it('should open confirmation dialog and call deleteGenreById on confirmation', () => {
    const mockGenre: GenreResponse = { id: 1, name: 'Fiction' };
    const dialogRef = { afterClosed: () => of(true) };
    mockDialogManager.openConfirmMenu.and.returnValue(dialogRef as any);

    spyOn(component, 'delete').and.callThrough();

    component.delete(mockGenre);
    fixture.detectChanges();

    expect(mockDialogManager.openConfirmMenu).toHaveBeenCalled();
    expect(mockGenreService.deleteGenreById).toHaveBeenCalledWith(mockGenre.id);
  });

  it('should clean up subscriptions on destroy', () => {
    spyOn(component['destroy$'], 'next').and.callThrough();
    spyOn(component['destroy$'], 'complete').and.callThrough();

    component.ngOnDestroy();

    expect(component['destroy$'].next).toHaveBeenCalled();
    expect(component['destroy$'].complete).toHaveBeenCalled();
  });
});