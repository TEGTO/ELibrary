import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { environment } from '../../../../../../environment/environment';
import { ValidationMessageService, getDefaultStockBookChange } from '../../../../shared';
import { BookStockReplenishmentComponent } from './book-stock-replenishment.component';

describe('BookStockReplenishmentComponent', () => {
    let component: BookStockReplenishmentComponent;
    let fixture: ComponentFixture<BookStockReplenishmentComponent>;
    let mockValidationMessageService: jasmine.SpyObj<ValidationMessageService>;
    let mockDialogRef: jasmine.SpyObj<MatDialogRef<BookStockReplenishmentComponent>>;

    beforeEach(async () => {
        mockValidationMessageService = jasmine.createSpyObj('ValidationMessageService', ['getValidationMessage']);
        mockDialogRef = jasmine.createSpyObj('MatDialogRef', ['close']);

        await TestBed.configureTestingModule({
            declarations: [BookStockReplenishmentComponent],
            imports: [
                ReactiveFormsModule,
                MatDialogModule,
                MatFormFieldModule,
                MatInputModule,
                NoopAnimationsModule
            ],
            providers: [
                { provide: ValidationMessageService, useValue: mockValidationMessageService },
                { provide: MatDialogRef, useValue: mockDialogRef }
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(BookStockReplenishmentComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should add a form group on initialization', () => {
        expect(component.items.length).toBe(1);
        expect(component.items[0].get('amount')).toBeDefined();
    });

    it('should add a form group when addFormGroup is called', () => {
        component.addFormGroup();
        expect(component.items.length).toBe(2);
    });

    it('should delete the specified form group when deleteFormGroup is called', () => {
        component.addFormGroup();
        const formGroupToDelete = component.items[0];

        component.deleteFormGroup(formGroupToDelete);

        expect(component.items.length).toBe(1);
        expect(component.items).not.toContain(formGroupToDelete);
    });

    it('should return the maximum allowed amount', () => {
        expect(component.maxAmount).toBe(environment.maxOrderAmount);
    });

    it('should validate input field and return validation message', () => {
        const mockControl = new FormControl('', Validators.required);
        mockValidationMessageService.getValidationMessage.and.returnValue({ hasError: true, message: 'Required field' });

        const formGroup = new FormGroup({ amount: mockControl });
        const result = component.validateInputField('amount', formGroup);

        expect(result.hasError).toBeTrue();
        expect(result.message).toBe('Required field');
    });

    it('should close dialog with valid data on submitForms', () => {
        component.addFormGroup();
        component.items[0].get('amount')?.setValue(5);
        component.items[0].addControl('book', new FormControl({ id: 1, title: 'Test Book' }));
        component.items[1].get('amount')?.setValue(10);
        component.items[1].addControl('book', new FormControl({ id: 2, title: 'Another Book' }));

        component.submitForms();

        expect(mockDialogRef.close).toHaveBeenCalledWith([
            { ...getDefaultStockBookChange(), book: { id: 1, title: 'Test Book' }, changeAmount: 5 },
            { ...getDefaultStockBookChange(), book: { id: 2, title: 'Another Book' }, changeAmount: 10 }
        ]);
    });

    it('should not close the dialog if the forms are invalid on submitForms', () => {
        component.items[0].get('amount')?.setValue(null);
        component.submitForms();

        expect(mockDialogRef.close).not.toHaveBeenCalled();
    });

    it('should mark all forms as touched if forms are invalid on submitForms', () => {
        component.addFormGroup();
        component.items[0].get('amount')?.setValue(null);
        component.submitForms();

        component.items.forEach(form => {
            expect(form.touched).toBeTrue();
        });
    });
});
