import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { Client, getDefaultClient, ValidationMessage } from '../../../../shared';
import { ClientChangeDialogComponent } from './client-change-dialog.component';

describe('ClientChangeDialogComponent', () => {
    let component: ClientChangeDialogComponent;
    let fixture: ComponentFixture<ClientChangeDialogComponent>;
    let mockDialogRef: jasmine.SpyObj<MatDialogRef<ClientChangeDialogComponent>>;
    let mockValidationService: jasmine.SpyObj<ValidationMessage>;

    const mockClient: Client = getDefaultClient();

    beforeEach(async () => {
        mockDialogRef = jasmine.createSpyObj('MatDialogRef', ['close']);
        mockValidationService = jasmine.createSpyObj('ValidationMessage', ['getValidationMessage']);
        mockValidationService.getValidationMessage.and.returnValue({ hasError: false, message: '' });

        await TestBed.configureTestingModule({
            declarations: [ClientChangeDialogComponent],
            imports: [
                ReactiveFormsModule,
                FormsModule,
                NoopAnimationsModule,
                MatDialogModule,
                MatFormFieldModule,
                MatInputModule,
                MatDatepickerModule
            ],
            providers: [
                provideNativeDateAdapter(),
                { provide: MAT_DIALOG_DATA, useValue: mockClient },
                { provide: MatDialogRef, useValue: mockDialogRef },
                { provide: ValidationMessage, useValue: mockValidationService }
            ],
        }).compileComponents();

        fixture = TestBed.createComponent(ClientChangeDialogComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize form with client data', () => {
        expect(component.formGroup).toBeDefined();
        expect(component.nameInput.value).toBe(mockClient.name);
        expect(component.middleNameInput.value).toBe(mockClient.middleName);
        expect(component.lastNameInput.value).toBe(mockClient.lastName);
        expect(component.dateOfBirthInput.value).toEqual(mockClient.dateOfBirth);
        expect(component.addressInput.value).toBe(mockClient.address);
        expect(component.phoneInput.value).toBe(mockClient.phone);
        expect(component.emailInput.value).toBe(mockClient.email);
    });

    it('should return validation message when validating input field', () => {
        component.nameInput.setValue('');
        fixture.detectChanges();

        expect(mockValidationService.getValidationMessage).toHaveBeenCalled();
        expect(component.validateInputField(component.nameInput)).toEqual({ hasError: false, message: '' });
    });

    it('should close dialog with updated client data when form is valid and submitted', () => {
        const updatedClient: Client = {
            ...mockClient,
            name: 'Jane',
            email: 'jane.doe@example.com'
        };

        component.nameInput.setValue(updatedClient.name);
        component.emailInput.setValue(updatedClient.email);
        component.lastNameInput.setValue(updatedClient.name);
        component.phoneInput.setValue("0123456789");
        component.dateOfBirthInput.setValue(new Date(0));
        component.onSubmit();

        expect(mockDialogRef.close).toHaveBeenCalled();
    });

    it('should not close dialog if form is invalid on submission', () => {
        component.nameInput.setValue('');
        component.onSubmit();

        expect(mockDialogRef.close).not.toHaveBeenCalled();
    });

    it('should show error message if form is invalid', () => {
        component.nameInput.setValue('');
        mockValidationService.getValidationMessage.and.returnValue({ hasError: true, message: "required" });
        component.nameInput.markAsTouched();
        component.formGroup.markAsTouched();
        fixture.detectChanges();

        const nameErrorElement = fixture.debugElement.query(By.css('mat-error')).nativeElement;
        expect(nameErrorElement.textContent).toContain('required');
    });

    it('should validate input field when form control is touched', () => {
        const mockControl = component.nameInput;
        mockControl.markAsTouched();

        component.validateInputField(mockControl);

        expect(mockValidationService.getValidationMessage).toHaveBeenCalledWith(mockControl);
    });
});
