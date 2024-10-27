import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { BookFilterRequest, defaultBookFilterRequest, ValidationMessage } from '../../../../shared';
import { BookFilterComponent } from './book-filter.component';

describe('BookFilterComponent', () => {
    let component: BookFilterComponent;
    let fixture: ComponentFixture<BookFilterComponent>;
    let mockValidationMessage: jasmine.SpyObj<ValidationMessage>;

    beforeEach(async () => {
        mockValidationMessage = jasmine.createSpyObj<ValidationMessage>(['getValidationMessage']);
        mockValidationMessage.getValidationMessage.and.returnValue({ hasError: false, message: "" });

        await TestBed.configureTestingModule({
            imports: [
                ReactiveFormsModule,
                MatCheckboxModule,
                MatRadioModule,
                MatFormFieldModule,
                MatInputModule,
                MatDatepickerModule,
                MatExpansionModule,
                NoopAnimationsModule,
                MatNativeDateModule
            ],
            declarations: [BookFilterComponent],
            providers: [
                { provide: ValidationMessage, useValue: mockValidationMessage }
            ]
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(BookFilterComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize form with default values', () => {
        component.ngOnInit();
        fixture.detectChanges();

        expect(component.formGroup).toBeDefined();
        const formValue = component.formGroup.value;
        const defaultRequest = defaultBookFilterRequest();

        expect(formValue.containsName).toEqual(defaultRequest.containsName);
        expect(formValue.minPrice).toBeNull();
        expect(formValue.maxPrice).toBeNull();
        expect(formValue.minPageAmount).toBeNull();
        expect(formValue.maxPageAmount).toBeNull();
    });

    it('should emit filterChange when form values change', () => {
        spyOn(component.filterChange, 'emit');

        component.formGroup.patchValue({
            containsName: 'Test Book',
            minPrice: 10,
            maxPrice: 50
        });

        fixture.detectChanges();
        component.onFilterChange();

        const expectedRequest: BookFilterRequest = {
            containsName: 'Test Book',
            pageNumber: 0,
            pageSize: 0,
            publicationFrom: null,
            publicationTo: null,
            minPrice: 10,
            maxPrice: 50,
            coverType: 0,
            onlyInStock: false,
            minPageAmount: null,
            maxPageAmount: null,
            authorId: null,
            genreId: null,
            publisherId: null,
            sorting: 0,
        };

        expect(component.filterChange.emit).toHaveBeenCalledWith(expectedRequest);
    });

    it('should not emit filterChange if form is invalid', () => {
        spyOn(component.filterChange, 'emit');

        // Set invalid values
        component.formGroup.patchValue({
            minPrice: -10,  // Invalid price
            maxPrice: 50
        });

        component.onFilterChange();
        fixture.detectChanges();

        expect(component.filterChange.emit).not.toHaveBeenCalled();
    });

    it('should validate form fields correctly', () => {
        const minPriceControl = component.minPriceInput;
        const maxPriceControl = component.maxPriceInput;

        minPriceControl.setValue(-10);
        maxPriceControl.setValue(50);

        fixture.detectChanges();

        expect(minPriceControl.invalid).toBeTrue();
        expect(maxPriceControl.valid).toBeTrue();
    });

    it('should set panel open state correctly when expansion panel is opened/closed', () => {
        const expansionPanel = fixture.debugElement.query(By.css('mat-expansion-panel'));

        expansionPanel.triggerEventHandler('opened', {});
        fixture.detectChanges();
        expect(component.panelOpenState()).toBeTrue();

        expansionPanel.triggerEventHandler('closed', {});
        fixture.detectChanges();
        expect(component.panelOpenState()).toBeFalse();
    });

    it('should unsubscribe from valueChanges on destroy', () => {
        spyOn(component["destroy$"], 'next');
        spyOn(component["destroy$"], 'complete');

        component.ngOnDestroy();

        expect(component["destroy$"].next).toHaveBeenCalled();
        expect(component["destroy$"].complete).toHaveBeenCalled();
    });
});
