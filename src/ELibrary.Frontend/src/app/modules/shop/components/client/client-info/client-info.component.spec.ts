import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UPDATE_CLIENT_COMMAND_HANDLER, UpdateClientCommand } from '../../..';
import { Client, CommandHandler, getDefaultClient, ValidationMessage } from '../../../../shared';
import { ClientInfoComponent } from './client-info.component';

describe('ClientInfoComponent', () => {
    let component: ClientInfoComponent;
    let fixture: ComponentFixture<ClientInfoComponent>;
    let mockValidationMessage: jasmine.SpyObj<ValidationMessage>;
    let mockUpdateClientHandler: jasmine.SpyObj<CommandHandler<UpdateClientCommand>>;

    const mockClient: Client = getDefaultClient();

    beforeEach(async () => {
        mockValidationMessage = jasmine.createSpyObj<ValidationMessage>(['getValidationMessage']);
        mockUpdateClientHandler = jasmine.createSpyObj('CommandHandler', ['dispatch']);

        mockValidationMessage.getValidationMessage.and.returnValue({ hasError: false, message: "" });

        await TestBed.configureTestingModule({
            declarations: [ClientInfoComponent],
            imports: [
                ReactiveFormsModule,
                MatExpansionModule,
                MatFormFieldModule,
                MatInputModule,
                BrowserAnimationsModule,
                MatDatepickerModule,
            ],
            providers: [
                provideNativeDateAdapter(),
                { provide: ValidationMessage, useValue: mockValidationMessage },
                { provide: UPDATE_CLIENT_COMMAND_HANDLER, useValue: mockUpdateClientHandler }
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(ClientInfoComponent);
        component = fixture.componentInstance;
        component.client = mockClient;
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize form group with client data on ngOnInit', () => {
        component.ngOnInit();
        expect(component.formGroup.value.name).toBe(mockClient.name);
        expect(component.formGroup.value.middleName).toBe(mockClient.middleName);
        expect(component.formGroup.value.lastName).toBe(mockClient.lastName);
        expect(component.formGroup.value.email).toBe(mockClient.email);
        expect(component.formGroup.value.address).toBe(mockClient.address);
        expect(component.formGroup.value.phone).toBe(mockClient.phone);
    });

    it('should call updateClient when form changes and is valid', fakeAsync(() => {
        component.ngOnInit();

        component.nameInput.setValue('Jane');
        component.middleNameInput.setValue('Michael');
        component.lastNameInput.setValue('Doe');
        component.dateOfBirthInput.setValue(new Date('1990-01-01'));
        component.addressInput.setValue('123 Main St');
        component.phoneInput.setValue('+3801234567');
        component.emailInput.setValue('jane.doe@example.com');

        component.formGroup.updateValueAndValidity();

        fixture.detectChanges();
        tick(400);

        expect(mockUpdateClientHandler.dispatch).toHaveBeenCalled();
    }));

    it('should not call updateClient if the form is invalid', () => {
        component.ngOnInit();
        component.nameInput.setValue('');
        component.formGroup.updateValueAndValidity();

        expect(mockUpdateClientHandler.dispatch).not.toHaveBeenCalled();
    });

    it('should validate input field and return validation message', () => {
        const mockControl = new FormControl('', Validators.required);
        mockValidationMessage.getValidationMessage.and.returnValue({ hasError: true, message: 'Required field' });

        const result = component.validateInputField(mockControl);
        expect(result.hasError).toBeTrue();
        expect(result.message).toBe('Required field');
    });

    it('should unsubscribe from observables on ngOnDestroy', () => {
        const spy = spyOn(component['destroy$'], 'next');
        const completeSpy = spyOn(component['destroy$'], 'complete');

        component.ngOnDestroy();
        expect(spy).toHaveBeenCalled();
        expect(completeSpy).toHaveBeenCalled();
    });

    it('should return the correct form controls via getters', () => {
        component.ngOnInit();

        expect(component.nameInput.value).toBe(mockClient.name);
        expect(component.middleNameInput.value).toBe(mockClient.middleName);
        expect(component.lastNameInput.value).toBe(mockClient.lastName);
        expect(component.emailInput.value).toBe(mockClient.email);
        expect(component.addressInput.value).toBe(mockClient.address);
        expect(component.phoneInput.value).toBe(mockClient.phone);
    });
});