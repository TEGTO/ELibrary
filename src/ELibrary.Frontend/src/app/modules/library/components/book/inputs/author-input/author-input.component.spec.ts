import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { of } from 'rxjs';
import { AuthorService } from '../../../..';
import { Author, getDefaultAuthor, ValidationMessage } from '../../../../../shared';
import { AuthorInputComponent } from './author-input.component';

describe('AuthorInputComponent', () => {
    let component: AuthorInputComponent;
    let fixture: ComponentFixture<AuthorInputComponent>;
    let authorService: jasmine.SpyObj<AuthorService>;
    let validationMessage: jasmine.SpyObj<ValidationMessage>;

    const mockAuthors: Author[] = [
        { ...getDefaultAuthor(), id: 1, name: 'Author 1' },
        { ...getDefaultAuthor(), id: 2, name: 'Author 1' },
    ];

    beforeEach(async () => {
        const authorServiceSpy = jasmine.createSpyObj('AuthorService', ['getPaginated']);
        const validationMessageSpy = jasmine.createSpyObj('ValidationMessage', ['getValidationMessage']);

        await TestBed.configureTestingModule({
            declarations: [AuthorInputComponent],
            imports: [
                ReactiveFormsModule,
                MatAutocompleteModule
            ],
            providers: [
                { provide: AuthorService, useValue: authorServiceSpy },
                { provide: ValidationMessage, useValue: validationMessageSpy }
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();

        fixture = TestBed.createComponent(AuthorInputComponent);
        component = fixture.componentInstance;
        authorService = TestBed.inject(AuthorService) as jasmine.SpyObj<AuthorService>;
        validationMessage = TestBed.inject(ValidationMessage) as jasmine.SpyObj<ValidationMessage>;

        authorService.getPaginated.and.returnValue(of(mockAuthors));

        component.formGroup = new FormGroup({
            author: new FormControl(null)
        });
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize form control if not present', () => {
        const formGroup = new FormGroup({});
        component.formGroup = formGroup;
        component.ngOnInit();

        expect(formGroup.contains('author')).toBeTrue();
    });

    it('should fetch items on init', fakeAsync(() => {
        component.ngOnInit();
        tick();

        expect(authorService.getPaginated).toHaveBeenCalledWith({
            pageNumber: 1,
            pageSize: component.pageAmount,
            containsName: ''
        });
        expect(component.items.length).toBe(2);
        expect(component.items).toEqual(mockAuthors);
    }));

    it('should update items when value changes', fakeAsync(() => {
        component.ngOnInit();
        component.input.setValue('Author 1');
        tick(100);

        expect(authorService.getPaginated).toHaveBeenCalledWith({
            pageNumber: 1,
            pageSize: component.pageAmount,
            containsName: 'Author 1'
        });
    }));

    it('should display the author name using displayWith', () => {
        const author: Author = { ...getDefaultAuthor(), id: 1, name: 'Test', lastName: 'Author' };
        const displayValue = component.displayWith(author);
        expect(displayValue).toBe('Test Author');
    });

    it('should return empty string if author is undefined in displayWith', () => {
        const displayValue = component.displayWith(undefined);
        expect(displayValue).toBe('');
    });

    it('should return the correct id in trackByAuthor', () => {
        const author: Author = { ...getDefaultAuthor(), id: 1, name: 'Author 1' };
        const trackId = component.trackByAuthor(0, author);
        expect(trackId).toBe(1);
    });

    it('should validate the input field using validateInputField', () => {
        const control = new FormControl();
        validationMessage.getValidationMessage.and.returnValue({
            hasError: true,
            message: 'Error message'
        });

        const validation = component.validateInputField(control);

        expect(validationMessage.getValidationMessage).toHaveBeenCalledWith(control);
        expect(validation.hasError).toBeTrue();
        expect(validation.message).toBe('Error message');
    });

    it('should calculate selection size based on items', () => {
        component.items = mockAuthors;
        const selectionSize = component.selectionSize;
        expect(selectionSize).toBe(component.items.length > component.amountItemsInView
            ? component.amountItemsInView * component.itemHeight
            : component.items.length * component.itemHeight + 5);
    });

    it('should set up scroll listeners', () => {
        const mockScroller = jasmine.createSpyObj('CdkVirtualScrollViewport', ['elementScrolled', 'measureScrollOffset']);
        mockScroller.elementScrolled.and.returnValue(of(0));
        component.scroller = mockScroller;

        component.ngAfterViewInit();

        expect(mockScroller.elementScrolled).toHaveBeenCalled();
    });
});

