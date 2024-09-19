import { TestBed } from '@angular/core/testing';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorChangeDialogComponent, BookChangeDialogComponent, ConfirmMenuComponent, GenreChangeDialogComponent } from '../..';
import { Author, Book, Genre } from '../../../shared';
import { LibraryDialogManagerService } from './library-dialog-manager.service';

describe('LibraryDialogManagerService', () => {
  let service: LibraryDialogManagerService;
  let mockMatDialog: jasmine.SpyObj<MatDialog>;

  beforeEach(() => {
    mockMatDialog = jasmine.createSpyObj<MatDialog>('MatDialog', ['open']);

    TestBed.configureTestingModule({
      providers: [
        LibraryDialogManagerService,
        { provide: MatDialog, useValue: mockMatDialog }
      ]
    });

    service = TestBed.inject(LibraryDialogManagerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should open confirm menu dialog', () => {
    const dialogRef = {} as MatDialogRef<any>;
    mockMatDialog.open.and.returnValue(dialogRef);

    const result = service.openConfirmMenu();

    expect(mockMatDialog.open).toHaveBeenCalledWith(ConfirmMenuComponent, {
      height: '200px',
      width: '450px'
    });
    expect(result).toBe(dialogRef);
  });

  it('should open book details menu dialog', () => {
    const book: Book = {
      id: 1,
      name: 'Book Title',
      publicationDate: new Date(),
      author: { id: 1, name: 'Author Name', lastName: 'Author LastName', dateOfBirth: new Date() },
      genre: { id: 1, name: 'Genre Name' }
    };
    const dialogRef = {} as MatDialogRef<any>;
    mockMatDialog.open.and.returnValue(dialogRef);

    const result = service.openBookDetailsMenu(book);

    expect(mockMatDialog.open).toHaveBeenCalledWith(BookChangeDialogComponent, {
      height: '460px',
      width: '450px',
      data: book
    });
    expect(result).toBe(dialogRef);
  });

  it('should open author details menu dialog', () => {
    const author: Author = {
      id: 1,
      name: 'Author Name',
      lastName: 'Author LastName',
      dateOfBirth: new Date()
    };
    const dialogRef = {} as MatDialogRef<any>;
    mockMatDialog.open.and.returnValue(dialogRef);

    const result = service.openAuthorDetailsMenu(author);

    expect(mockMatDialog.open).toHaveBeenCalledWith(AuthorChangeDialogComponent, {
      height: '400px',
      width: '450px',
      data: author
    });
    expect(result).toBe(dialogRef);
  });

  it('should open genre details menu dialog', () => {
    const genre: Genre = {
      id: 1,
      name: 'Genre Name'
    };
    const dialogRef = {} as MatDialogRef<any>;
    mockMatDialog.open.and.returnValue(dialogRef);

    const result = service.openGenreDetailsMenu(genre);

    expect(mockMatDialog.open).toHaveBeenCalledWith(GenreChangeDialogComponent, {
      height: '220px',
      width: '450px',
      data: genre
    });
    expect(result).toBe(dialogRef);
  });
});