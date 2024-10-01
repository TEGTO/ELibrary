import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { of } from 'rxjs';
import { GenreInputComponent, GenreService } from '../../../..';
import { Genre, ValidationMessage } from '../../../../../shared';

describe('GenreInputComponent', () => {
    let component: GenreInputComponent;
    let fixture: ComponentFixture<GenreInputComponent>;
    let genreService: jasmine.SpyObj<GenreService>;
    let validationMessage: jasmine.SpyObj<ValidationMessage>;

    const mockGenres: Genre[] = [
        { id: 1, name: 'Genre 1' },
        { id: 2, name: 'Genre 2' }
    ];

    beforeEach(async () => {
        const genreServiceSpy = jasmine.createSpyObj('GenreService', ['getPaginated']);
        const validationMessageSpy = jasmine.createSpyObj('ValidationMessage', ['getValidationMessage']);

        await TestBed.configureTestingModule({
            declarations: [GenreInputComponent],
            imports: [
                ReactiveFormsModule,
                MatAutocompleteModule
            ],
            providers: [
                { provide: GenreService, useValue: genreServiceSpy },
                { provide: ValidationMessage, useValue: validationMessageSpy }
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();

        fixture = TestBed.createComponent(GenreInputComponent);
        component = fixture.componentInstance;
        genreService = TestBed.inject(GenreService) as jasmine.SpyObj<GenreService>;
        validationMessage = TestBed.inject(ValidationMessage) as jasmine.SpyObj<ValidationMessage>;

        genreService.getPaginated.and.returnValue(of(mockGenres));

        component.formGroup = new FormGroup({
        });
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize form control if not present', () => {
        const formGroup = new FormGroup({});
        component.formGroup = formGroup;
        component.ngOnInit();

        expect(formGroup.contains('genre')).toBeTrue();
    });

    it('should fetch items on init', fakeAsync(() => {
        component.ngOnInit();
        tick();

        expect(genreService.getPaginated).toHaveBeenCalledWith({
            pageNumber: 1,
            pageSize: component.pageAmount,
            containsName: ''
        });
        expect(component.items.length).toBe(2);
        expect(component.items).toEqual(mockGenres);
    }));

    it('should update items when value changes', fakeAsync(() => {
        component.ngOnInit();
        component.input.setValue('Genre 1');
        tick(100);

        expect(genreService.getPaginated).toHaveBeenCalledWith({
            pageNumber: 1,
            pageSize: component.pageAmount,
            containsName: 'Genre 1'
        });
    }));

    it('should display the genre name using displayWith', () => {
        const genre: Genre = { id: 1, name: 'Test Genre' };
        const displayValue = component.displayWith(genre);
        expect(displayValue).toBe('Test Genre');
    });

    it('should return empty string if genre is undefined in displayWith', () => {
        const displayValue = component.displayWith(undefined);
        expect(displayValue).toBe('');
    });

    it('should return the correct id in trackByGenre', () => {
        const genre: Genre = { id: 1, name: 'Genre 1' };
        const trackId = component.trackByGenre(0, genre);
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
        component.items = mockGenres;
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
