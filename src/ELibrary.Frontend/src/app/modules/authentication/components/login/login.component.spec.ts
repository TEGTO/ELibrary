import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { AuthenticationDialogManager, SIGN_IN_COMMAND_HANDLER, SignInCommand } from '../..';
import { CommandHandler, ValidationMessage } from '../../../shared';
import { LoginComponent } from './login.component';

describe('LoginComponent', () => {
    let component: LoginComponent;
    let fixture: ComponentFixture<LoginComponent>;
    let authDialogManager: jasmine.SpyObj<AuthenticationDialogManager>;
    let signInCommandHandlerSpy: jasmine.SpyObj<CommandHandler<SignInCommand>>;

    beforeEach(waitForAsync(() => {
        const authDialogManagerSpy = jasmine.createSpyObj('AuthenticationDialogManager', ['openRegisterMenu']);
        const dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);
        const getValidationMessageSpy = jasmine.createSpyObj<ValidationMessage>(['getValidationMessage']);
        signInCommandHandlerSpy = jasmine.createSpyObj<CommandHandler<SignInCommand>>(['dispatch']);

        getValidationMessageSpy.getValidationMessage.and.returnValue({ hasError: false, message: "" });

        TestBed.configureTestingModule({
            declarations: [LoginComponent],
            imports: [
                ReactiveFormsModule,
                FormsModule,
                MatFormFieldModule,
                MatInputModule,
                MatButtonModule,
                NoopAnimationsModule,
            ],
            providers: [
                { provide: AuthenticationDialogManager, useValue: authDialogManagerSpy },
                { provide: MatDialogRef, useValue: dialogRefSpy },
                { provide: SIGN_IN_COMMAND_HANDLER, useValue: signInCommandHandlerSpy },
                { provide: ValidationMessage, useValue: getValidationMessageSpy }
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA]
        }).compileComponents();

        fixture = TestBed.createComponent(LoginComponent);
        component = fixture.componentInstance;
        authDialogManager = TestBed.inject(AuthenticationDialogManager) as jasmine.SpyObj<AuthenticationDialogManager>;

        fixture.detectChanges();
    }));

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should display validation errors', () => {
        const inputs = fixture.debugElement.queryAll(By.css('input'));

        const loginInput = inputs[0].nativeElement;
        loginInput.value = '';
        loginInput.dispatchEvent(new Event('input'));
        loginInput.blur();
        fixture.detectChanges();

        const passwordInput = inputs[1].nativeElement;
        passwordInput.value = 'short';
        passwordInput.dispatchEvent(new Event('input'));
        passwordInput.blur();
        fixture.detectChanges();

        expect(component.formGroup.valid).toBeFalse();
        expect(component.loginInput.hasError('required')).toBeTrue();
        expect(component.passwordInput.hasError('minlength')).toBeTrue();
    });

    it('should call signInUser on valid form submission', () => {
        component.formGroup.setValue({
            login: 'john@example.com',
            password: 'Password123;'
        });

        fixture.debugElement.query(By.css('button[type="submit"]')).nativeElement.click();
        fixture.detectChanges();

        expect(signInCommandHandlerSpy.dispatch).toHaveBeenCalled();
    });

    it('should open register menu on link click', () => {
        fixture.debugElement.query(By.css('a#to-register-link')).nativeElement.click();
        expect(authDialogManager.openRegisterMenu).toHaveBeenCalled();
    });

    it('should toggle password visibility', () => {
        const passwordInput = fixture.debugElement.query(By.css('input[formControlName="password"]')).nativeElement;
        const visibilityToggle = fixture.debugElement.query(By.css('span[matSuffix]')).nativeElement;

        expect(passwordInput.type).toBe('password');

        visibilityToggle.click();
        fixture.detectChanges();

        expect(passwordInput.type).toBe('text');

        visibilityToggle.click();
        fixture.detectChanges();

        expect(passwordInput.type).toBe('password');
    });
});