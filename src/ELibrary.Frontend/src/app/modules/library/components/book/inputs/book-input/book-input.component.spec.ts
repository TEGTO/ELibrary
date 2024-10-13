import { CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { ComponentFixture, fakeAsync, TestBed, tick } from "@angular/core/testing";
import { FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { of } from "rxjs";
import { BookService } from "../../../..";
import { Book, defaultBookFilterRequest, getDefaultBook, ValidationMessage } from "../../../../../shared";
import { BookInputComponent } from "./book-input.component";

describe('BookInputComponent', () => {
    let component: BookInputComponent;
    let fixture: ComponentFixture<BookInputComponent>;
    let bookService: jasmine.SpyObj<BookService>;
    let validationMessage: jasmine.SpyObj<ValidationMessage>;

    const mockBooks: Book[] = [
        { ...getDefaultBook(), id: 1, name: "Book 1" },
        { ...getDefaultBook(), id: 2, name: "Book 2" }
    ];

    beforeEach(async () => {
        const bookServiceSpy = jasmine.createSpyObj('BookService', ['getPaginated']);
        const validationMessageSpy = jasmine.createSpyObj('ValidationMessage', ['getValidationMessage']);

        await TestBed.configureTestingModule({
            declarations: [BookInputComponent],
            imports: [
                ReactiveFormsModule,
                MatAutocompleteModule
            ],
            providers: [
                { provide: BookService, useValue: bookServiceSpy },
                { provide: ValidationMessage, useValue: validationMessageSpy }
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();

        fixture = TestBed.createComponent(BookInputComponent);
        component = fixture.componentInstance;
        bookService = TestBed.inject(BookService) as jasmine.SpyObj<BookService>;
        validationMessage = TestBed.inject(ValidationMessage) as jasmine.SpyObj<ValidationMessage>;

        bookService.getPaginated.and.returnValue(of(mockBooks));

        component.formGroup = new FormGroup({});
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize form control if not present', () => {
        const formGroup = new FormGroup({});
        component.formGroup = formGroup;
        component.ngOnInit();

        expect(formGroup.contains('book')).toBeTrue();
    });

    it('should fetch items on init', fakeAsync(() => {
        component.ngOnInit();
        tick();

        expect(bookService.getPaginated).toHaveBeenCalledWith({
            ...defaultBookFilterRequest(),
            pageNumber: 1,
            pageSize: component.pageAmount,
            containsName: '',
        });
        expect(component.items.length).toBe(2);
        expect(component.items).toEqual(mockBooks);
    }));

    it('should update items when value changes', fakeAsync(() => {
        component.ngOnInit();
        component.input.setValue('Book 1');
        tick(100);

        expect(bookService.getPaginated).toHaveBeenCalledWith({
            ...defaultBookFilterRequest(),
            pageNumber: 1,
            pageSize: component.pageAmount,
            containsName: 'Book 1'
        });
    }));

    it('should display the book name using displayWith', () => {
        const book: Book = { ...getDefaultBook(), id: 1, name: 'Test Book' };
        const displayValue = component.displayWith(book);
        expect(displayValue).toBe('Test Book');
    });

    it('should return empty string if book is undefined in displayWith', () => {
        const displayValue = component.displayWith(undefined);
        expect(displayValue).toBe('');
    });

    it('should return the correct id in trackByBook', () => {
        const book: Book = { ...getDefaultBook(), id: 1, name: 'Book 1' };
        const trackId = component.trackByBook(0, book);
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
        component.items = mockBooks;
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
