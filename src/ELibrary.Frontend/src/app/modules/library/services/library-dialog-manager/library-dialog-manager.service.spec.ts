/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthorChangeDialogComponent, BookChangeDialogComponent, GenreChangeDialogComponent, PublisherChangeDialogComponent } from '../..';
import { Author, Book, Genre, getDefaultAuthor, getDefaultBook, getDefaultGenre, getDefaultPublisher, Publisher } from '../../../shared';
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

    it('should open book details menu dialog', () => {
        const book: Book = getDefaultBook();
        const dialogRef = {} as MatDialogRef<any>;
        mockMatDialog.open.and.returnValue(dialogRef);

        const result = service.openBookDetailsMenu(book);

        expect(mockMatDialog.open).toHaveBeenCalledWith(BookChangeDialogComponent, {
            maxHeight: '660px',
            width: '650px',
            data: book
        });
        expect(result).toBe(dialogRef);
    });

    it('should open author details menu dialog', () => {
        const author: Author = getDefaultAuthor();
        const dialogRef = {} as MatDialogRef<any>;
        mockMatDialog.open.and.returnValue(dialogRef);

        const result = service.openAuthorDetailsMenu(author);

        expect(mockMatDialog.open).toHaveBeenCalledWith(AuthorChangeDialogComponent, {
            maxHeight: '400px',
            width: '450px',
            data: author
        });
        expect(result).toBe(dialogRef);
    });

    it('should open genre details menu dialog', () => {
        const genre: Genre = getDefaultGenre();
        const dialogRef = {} as MatDialogRef<any>;
        mockMatDialog.open.and.returnValue(dialogRef);

        const result = service.openGenreDetailsMenu(genre);

        expect(mockMatDialog.open).toHaveBeenCalledWith(GenreChangeDialogComponent, {
            maxHeight: '230px',
            width: '450px',
            data: genre
        });
        expect(result).toBe(dialogRef);
    });

    it('should open publisher details menu dialog', () => {
        const publisher: Publisher = getDefaultPublisher();
        const dialogRef = {} as MatDialogRef<any>;
        mockMatDialog.open.and.returnValue(dialogRef);

        const result = service.openPublisherDetailsMenu(publisher);

        expect(mockMatDialog.open).toHaveBeenCalledWith(PublisherChangeDialogComponent, {
            maxHeight: '230px',
            width: '450px',
            data: publisher
        });
        expect(result).toBe(dialogRef);
    });
});