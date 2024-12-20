import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { AuthenticationService, SIGN_UP_COMMAND_HANDLER, SignUpCommand, START_LOGIN_COMMAND_HANDLER, StartLoginCommand } from '../..';
import { CommandHandler, SnackbarManager, ValidationMessage } from '../../../shared';
import { RegisterComponent } from './register.component';

describe('RegisterComponent', () => {
    let component: RegisterComponent;
    let fixture: ComponentFixture<RegisterComponent>;
    let signUpHandlerSpy: jasmine.SpyObj<CommandHandler<SignUpCommand>>;
    let loginHandlerSpy: jasmine.SpyObj<CommandHandler<StartLoginCommand>>;

    beforeEach(async () => {
        const authServiceSpy = jasmine.createSpyObj('AuthenticationService', ['registerUser', 'getRegistrationErrors']);
        const snackbarManagerSpy = jasmine.createSpyObj('SnackbarManager', ['openInfoSnackbar', 'openErrorSnackbar']);
        const dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);
        signUpHandlerSpy = jasmine.createSpyObj<CommandHandler<SignUpCommand>>(['dispatch']);
        loginHandlerSpy = jasmine.createSpyObj<CommandHandler<StartLoginCommand>>(['dispatch']);
        const validationMessageSpy = jasmine.createSpyObj<ValidationMessage>(['getValidationMessage']);

        validationMessageSpy.getValidationMessage.and.returnValue({ hasError: false, message: "" });

        await TestBed.configureTestingModule({
            declarations: [RegisterComponent],
            imports: [
                ReactiveFormsModule,
                FormsModule,
                MatFormFieldModule,
                MatInputModule,
                NoopAnimationsModule
            ],
            providers: [
                { provide: AuthenticationService, useValue: authServiceSpy },
                { provide: SnackbarManager, useValue: snackbarManagerSpy },
                { provide: MatDialogRef, useValue: dialogRefSpy },
                { provide: MAT_DIALOG_DATA, useValue: {} },
                { provide: SIGN_UP_COMMAND_HANDLER, useValue: signUpHandlerSpy },
                { provide: START_LOGIN_COMMAND_HANDLER, useValue: loginHandlerSpy },
                { provide: ValidationMessage, useValue: validationMessageSpy },
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();

        fixture = TestBed.createComponent(RegisterComponent);
        component = fixture.componentInstance;

        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should display validation errors', () => {
        const inputs = fixture.debugElement.queryAll(By.css('input'));

        const nameInput = inputs[0].nativeElement;
        nameInput.value = '';
        nameInput.dispatchEvent(new Event('input'));
        nameInput.blur();
        fixture.detectChanges();

        const passwordInput = inputs[1].nativeElement;
        passwordInput.value = 'short';
        passwordInput.dispatchEvent(new Event('input'));
        passwordInput.blur();
        fixture.detectChanges();

        const confirmPasswordInput = inputs[2].nativeElement;
        confirmPasswordInput.value = 'mismatch';
        confirmPasswordInput.dispatchEvent(new Event('input'));
        confirmPasswordInput.blur();
        fixture.detectChanges();

        expect(component.formGroup.valid).toBeFalse();
        expect(component.emailInput.hasError('required')).toBeTruthy();
        expect(component.passwordInput.hasError('minlength')).toBeTruthy();
        expect(component.passwordConfirmInput.hasError('passwordNoMatch')).toBeTruthy();
    });

    it('should call registerUser on valid form submission', () => {
        const formValues = {
            userName: 'John Doe',
            password: 'password123',
            passwordConfirm: 'password123',
            userInfo: {
                name: 'John',
                lastName: 'Doe',
                dateOfBirth: new Date(),
                address: '123 Main St'
            }
        };

        component.formGroup = new FormGroup({
            userName: new FormControl(formValues.userName, [Validators.required, Validators.maxLength(256)]),
            password: new FormControl(formValues.password, [Validators.required, Validators.minLength(8), Validators.maxLength(256)]),
            passwordConfirm: new FormControl(formValues.passwordConfirm, [Validators.required, Validators.maxLength(256)]),
            userInfo: new FormGroup({
                name: new FormControl(formValues.userInfo.name),
                lastName: new FormControl(formValues.userInfo.lastName),
                dateOfBirth: new FormControl(formValues.userInfo.dateOfBirth),
                address: new FormControl(formValues.userInfo.address)
            })
        });

        fixture.detectChanges();

        fixture.debugElement.query(By.css('button[type="submit"]')).nativeElement.click();
        fixture.detectChanges();

        expect(signUpHandlerSpy.dispatch).toHaveBeenCalled();
    });

});