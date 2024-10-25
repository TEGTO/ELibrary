/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { AuthorService, CreateAuthorCommand, LibraryDialogManager } from '../../..';
import { Author, getDefaultAuthor } from '../../../../shared';
import { CreateAuthorCommandHandlerService } from './create-author-command-handler.service';

describe('CreateAuthorCommandHandlerService', () => {
    let service: CreateAuthorCommandHandlerService;
    let dialogManagerMock: jasmine.SpyObj<LibraryDialogManager>;
    let authorServiceMock: jasmine.SpyObj<AuthorService>;

    beforeEach(() => {
        dialogManagerMock = jasmine.createSpyObj('LibraryDialogManager', ['openAuthorDetailsMenu']);
        authorServiceMock = jasmine.createSpyObj('AuthorService', ['create']);

        TestBed.configureTestingModule({
            providers: [
                CreateAuthorCommandHandlerService,
                { provide: LibraryDialogManager, useValue: dialogManagerMock },
                { provide: AuthorService, useValue: authorServiceMock },
            ]
        });

        service = TestBed.inject(CreateAuthorCommandHandlerService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should call create when author is confirmed', () => {
        const mockAuthor: Author = { ...getDefaultAuthor(), name: 'John Doe' };

        dialogManagerMock.openAuthorDetailsMenu.and.returnValue({
            afterClosed: () => of(mockAuthor),
        } as any);

        service.dispatch({} as CreateAuthorCommand);

        expect(dialogManagerMock.openAuthorDetailsMenu).toHaveBeenCalled();
        expect(authorServiceMock.create).toHaveBeenCalledWith(jasmine.objectContaining({ name: 'John Doe' }));
    });

    it('should not call create if dialog is canceled', () => {
        dialogManagerMock.openAuthorDetailsMenu.and.returnValue({
            afterClosed: () => of(null),
        } as any);

        service.dispatch({} as CreateAuthorCommand);

        expect(dialogManagerMock.openAuthorDetailsMenu).toHaveBeenCalled();
        expect(authorServiceMock.create).not.toHaveBeenCalled();
    });
});
