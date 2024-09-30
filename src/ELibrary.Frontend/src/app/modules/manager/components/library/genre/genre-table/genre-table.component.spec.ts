/* eslint-disable @typescript-eslint/no-explicit-any */

import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { CREATE_GENRE_COMMAND_HANDLER, CreateGenreCommand, DELETE_GENRE_COMMAND_HANDLER, DeleteGenreCommand, GenreService, UPDATE_GENRE_COMMAND_HANDLER, UpdateGenreCommand } from '../../../../../library';
import { CommandHandler, GenericTableComponent, Genre, getDefaultGenre } from '../../../../../shared';
import { GenreTableComponent } from './genre-table.component';

describe('GenreTableComponent', () => {
  let component: GenreTableComponent;
  let fixture: ComponentFixture<GenreTableComponent>;
  let mockGenreService: jasmine.SpyObj<GenreService>;
  let mockCreateGenreCommandHandler: jasmine.SpyObj<CommandHandler<CreateGenreCommand>>;
  let mockUpdateGenreCommandHandler: jasmine.SpyObj<CommandHandler<UpdateGenreCommand>>;
  let mockDeleteGenreCommandHandler: jasmine.SpyObj<CommandHandler<DeleteGenreCommand>>;

  const mockItems = [
    getDefaultGenre(),
    getDefaultGenre()
  ];
  const mockAmount = 100;

  beforeEach(async () => {
    mockGenreService = jasmine.createSpyObj('GenreService', [
      'getItemTotalAmount',
      'getPaginated',
      'createGenre',
      'updateGenre',
      'deleteGenreById'
    ]);

    mockCreateGenreCommandHandler = jasmine.createSpyObj<CommandHandler<CreateGenreCommand>>([
      'dispatch',
    ]);

    mockUpdateGenreCommandHandler = jasmine.createSpyObj<CommandHandler<UpdateGenreCommand>>([
      'dispatch',
    ]);

    mockDeleteGenreCommandHandler = jasmine.createSpyObj<CommandHandler<DeleteGenreCommand>>([
      'dispatch',
    ]);

    mockGenreService.getItemTotalAmount.and.returnValue(of(mockAmount));
    mockGenreService.getPaginated.and.returnValue(of(mockItems));

    await TestBed.configureTestingModule({
      imports: [GenericTableComponent, BrowserAnimationsModule],
      declarations: [GenreTableComponent],
      providers: [
        { provide: GenreService, useValue: mockGenreService },
        { provide: CREATE_GENRE_COMMAND_HANDLER, useValue: mockCreateGenreCommandHandler },
        { provide: UPDATE_GENRE_COMMAND_HANDLER, useValue: mockUpdateGenreCommandHandler },
        { provide: DELETE_GENRE_COMMAND_HANDLER, useValue: mockDeleteGenreCommandHandler }
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

  it('should initialize and fetch paginated items', () => {
    spyOn<any>(component, 'fetchPaginatedItems');
    component.ngOnInit();
    expect(component["fetchPaginatedItems"]).toHaveBeenCalledWith({ pageIndex: 1, pageSize: 10 });
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
    const mockItems = [getDefaultGenre()];
    mockGenreService.getPaginated.and.returnValue(of(mockItems));

    component.onPageChange({ pageIndex: 1, pageSize: 10 });
    fixture.detectChanges();

    component.items$.subscribe(items => {
      expect(items).toEqual(mockItems.slice(0, 10));
    });
  });

  it('should dispatch create genre command', () => {

    component.createNew();
    fixture.detectChanges();

    expect(mockCreateGenreCommandHandler.dispatch).toHaveBeenCalled();
  });

  it('should dispatch update genre command', () => {
    const mockGenre: Genre = getDefaultGenre();

    component.update(mockGenre);
    fixture.detectChanges();

    expect(mockUpdateGenreCommandHandler.dispatch).toHaveBeenCalled();
  });

  it('should dispatch delete genre command', () => {
    const mockGenre: Genre = getDefaultGenre();

    component.delete(mockGenre);
    fixture.detectChanges();

    expect(mockDeleteGenreCommandHandler.dispatch).toHaveBeenCalled();
  });
});