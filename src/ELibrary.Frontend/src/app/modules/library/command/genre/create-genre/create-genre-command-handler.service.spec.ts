/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { CreateGenreCommand, GenreService, LibraryDialogManager } from '../../..';
import { Genre, getDefaultGenre, mapGenreToCreateGenreRequest } from '../../../../shared';
import { CreateGenreCommandHandlerService } from './create-genre-command-handler.service';

describe('CreateGenreCommandHandlerService', () => {
  let service: CreateGenreCommandHandlerService;
  let dialogManagerMock: jasmine.SpyObj<LibraryDialogManager>;
  let genreServiceMock: jasmine.SpyObj<GenreService>;

  beforeEach(() => {
    dialogManagerMock = jasmine.createSpyObj('LibraryDialogManager', ['openGenreDetailsMenu']);
    genreServiceMock = jasmine.createSpyObj('GenreService', ['create']);

    TestBed.configureTestingModule({
      providers: [
        CreateGenreCommandHandlerService,
        { provide: LibraryDialogManager, useValue: dialogManagerMock },
        { provide: GenreService, useValue: genreServiceMock },
      ]
    });

    service = TestBed.inject(CreateGenreCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call create when genre is confirmed', () => {
    const mockGenre: Genre = { ...getDefaultGenre(), name: 'Science Fiction' };
    const expectedRequest = mapGenreToCreateGenreRequest(mockGenre);

    dialogManagerMock.openGenreDetailsMenu.and.returnValue({
      afterClosed: () => of(mockGenre),
    } as any);

    service.dispatch({} as CreateGenreCommand);

    expect(dialogManagerMock.openGenreDetailsMenu).toHaveBeenCalled();
    expect(genreServiceMock.create).toHaveBeenCalledWith(jasmine.objectContaining(expectedRequest));
  });

  it('should not call create if dialog is canceled', () => {
    dialogManagerMock.openGenreDetailsMenu.and.returnValue({
      afterClosed: () => of(null),
    } as any);

    service.dispatch({} as CreateGenreCommand);

    expect(dialogManagerMock.openGenreDetailsMenu).toHaveBeenCalled();
    expect(genreServiceMock.create).not.toHaveBeenCalled();
  });
});