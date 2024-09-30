/* eslint-disable @typescript-eslint/no-explicit-any */
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { AuthorService, CREATE_AUTHOR_COMMAND_HANDLER, CreateAuthorCommand, DELETE_AUTHOR_COMMAND_HANDLER, DeleteAuthorCommand, UPDATE_AUTHOR_COMMAND_HANDLER, UpdateAuthorCommand } from '../../../../../library';
import { Author, CommandHandler, GenericTableComponent, getDefaultAuthor } from '../../../../../shared';
import { AuthorTableComponent } from './author-table.component';

describe('AuthorTableComponent', () => {
    let component: AuthorTableComponent;
    let fixture: ComponentFixture<AuthorTableComponent>;
    let mockAuthorService: jasmine.SpyObj<AuthorService>;
    let mockCreateAuthorCommandHandler: jasmine.SpyObj<CommandHandler<CreateAuthorCommand>>;
    let mockUpdateAuthorCommandHandler: jasmine.SpyObj<CommandHandler<UpdateAuthorCommand>>;
    let mockDeleteAuthorCommandHandler: jasmine.SpyObj<CommandHandler<DeleteAuthorCommand>>;

    const mockItems: Author[] = [
        getDefaultAuthor(),
        getDefaultAuthor(),
    ];
    const mockAmount = 2;

    beforeEach(async () => {
        mockAuthorService = jasmine.createSpyObj('AuthorService', [
            'getItemTotalAmount',
            'getPaginated',
            'createAuthor',
            'updateAuthor',
            'deleteAuthorById'
        ]);

        mockCreateAuthorCommandHandler = jasmine.createSpyObj<CommandHandler<CreateAuthorCommand>>([
            'dispatch',
        ]);

        mockUpdateAuthorCommandHandler = jasmine.createSpyObj<CommandHandler<UpdateAuthorCommand>>([
            'dispatch',
        ]);

        mockDeleteAuthorCommandHandler = jasmine.createSpyObj<CommandHandler<DeleteAuthorCommand>>([
            'dispatch',
        ]);

        mockAuthorService.getItemTotalAmount.and.returnValue(of(mockAmount));
        mockAuthorService.getPaginated.and.returnValue(of(mockItems));

        await TestBed.configureTestingModule({
            imports: [GenericTableComponent, BrowserAnimationsModule],
            declarations: [AuthorTableComponent],
            providers: [
                { provide: AuthorService, useValue: mockAuthorService },
                { provide: CREATE_AUTHOR_COMMAND_HANDLER, useValue: mockCreateAuthorCommandHandler },
                { provide: UPDATE_AUTHOR_COMMAND_HANDLER, useValue: mockUpdateAuthorCommandHandler },
                { provide: DELETE_AUTHOR_COMMAND_HANDLER, useValue: mockDeleteAuthorCommandHandler }
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
        mockAuthorService.getPaginated.and.returnValue(of(mockItems));

        component.onPageChange({ pageIndex: 1, pageSize: 10 });
        fixture.detectChanges();

        component.items$.subscribe(items => {
            expect(items).toEqual(mockItems);
        });
    });

    it('should dispatch create author command', () => {

        component.createNew();
        fixture.detectChanges();

        expect(mockCreateAuthorCommandHandler.dispatch).toHaveBeenCalled();
    });

    it('should dispatch update author command', () => {
        const mockAuthor: Author = getDefaultAuthor();

        component.update(mockAuthor);
        fixture.detectChanges();

        expect(mockUpdateAuthorCommandHandler.dispatch).toHaveBeenCalled();
    });

    it('should dispatch delete author command', () => {
        const mockAuthor: Author = mockItems[0];

        component.delete(mockAuthor);
        fixture.detectChanges();

        expect(mockDeleteAuthorCommandHandler.dispatch).toHaveBeenCalled();
    });
});