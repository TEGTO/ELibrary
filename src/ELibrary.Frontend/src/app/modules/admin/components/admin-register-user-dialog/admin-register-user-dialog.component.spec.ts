import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ADMIN_REGISTER_USER_COMMAND_HANDLER, AdminRegisterUserCommand } from '../..';
import { CommandHandler, Roles, ValidationMessage } from '../../../shared';
import { AdminRegisterUserDialogComponent } from './admin-register-user-dialog.component';

describe('AdminRegisterUserDialogComponent', () => {
  let component: AdminRegisterUserDialogComponent;
  let fixture: ComponentFixture<AdminRegisterUserDialogComponent>;
  let registerHandlerSpy: jasmine.SpyObj<CommandHandler<AdminRegisterUserCommand>>;
  let dialogRefSpy: jasmine.SpyObj<MatDialogRef<AdminRegisterUserDialogComponent>>;
  let validationMessageSpy: jasmine.SpyObj<ValidationMessage>;

  beforeEach(waitForAsync(() => {
    const registerHandlerSpyObj = jasmine.createSpyObj('CommandHandler', ['dispatch']);
    const dialogRefSpyObj = jasmine.createSpyObj('MatDialogRef', ['close']);
    const validationMessageSpyObj = jasmine.createSpyObj<ValidationMessage>('ValidationMessage', ['getValidationMessage']);

    validationMessageSpyObj.getValidationMessage.and.returnValue({ hasError: false, message: "" });

    TestBed.configureTestingModule({
      declarations: [AdminRegisterUserDialogComponent],
      imports: [
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatSelectModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: ADMIN_REGISTER_USER_COMMAND_HANDLER, useValue: registerHandlerSpyObj },
        { provide: MatDialogRef, useValue: dialogRefSpyObj },
        { provide: ValidationMessage, useValue: validationMessageSpyObj }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(AdminRegisterUserDialogComponent);
    component = fixture.componentInstance;
    registerHandlerSpy = TestBed.inject(ADMIN_REGISTER_USER_COMMAND_HANDLER) as jasmine.SpyObj<CommandHandler<AdminRegisterUserCommand>>;
    dialogRefSpy = TestBed.inject(MatDialogRef) as jasmine.SpyObj<MatDialogRef<AdminRegisterUserDialogComponent>>;
    validationMessageSpy = TestBed.inject(ValidationMessage) as jasmine.SpyObj<ValidationMessage>;

    fixture.detectChanges();
  }));

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with empty controls', () => {
    expect(component.formGroup).toBeDefined();
    expect(component.emailInput.value).toBe('');
    expect(component.passwordInput.value).toBe('');
    expect(component.passwordConfirmInput.value).toBe('');
    expect(component.rolesInput.value).toEqual([Roles.CLIENT]);
  });

  it('should toggle password visibility when clicking the visibility icon', () => {
    const passwordInput = fixture.debugElement.query(By.css('input#password')).nativeElement;
    const visibilityToggle = fixture.debugElement.query(By.css('span[matSuffix]')).nativeElement;

    expect(passwordInput.type).toBe('password');

    visibilityToggle.click();
    fixture.detectChanges();

    expect(passwordInput.type).toBe('text');

    visibilityToggle.click();
    fixture.detectChanges();

    expect(passwordInput.type).toBe('password');
  });

  it('should toggle password visibility on Enter key press', () => {
    const event = new KeyboardEvent('keydown', { key: 'Enter' });
    component.hidePassword = true;

    component.hidePasswordOnKeydown(event);
    fixture.detectChanges();

    expect(component.hidePassword).toBeFalse();

    component.hidePasswordOnKeydown(event);
    fixture.detectChanges();

    expect(component.hidePassword).toBeTrue();
  });

  it('should display validation errors for invalid form inputs', () => {
    const inputs = fixture.debugElement.queryAll(By.css('input'));

    const emailInput = inputs[0].nativeElement;
    emailInput.value = '';
    emailInput.dispatchEvent(new Event('input'));
    emailInput.blur();
    fixture.detectChanges();

    expect(component.emailInput.hasError('required')).toBeTrue();
    expect(validationMessageSpy.getValidationMessage).toHaveBeenCalled();
  });

  it('should call registerUser when form is valid and submitted', () => {
    component.formGroup.setValue({
      email: 'admin@example.com',
      password: 'Password123!',
      passwordConfirm: 'Password123!',
      roles: [Roles.ADMINISTRATOR]
    });

    fixture.debugElement.query(By.css('button[type="submit"]')).nativeElement.click();
    fixture.detectChanges();

    expect(registerHandlerSpy.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({
      email: 'admin@example.com',
      password: 'Password123!',
      confirmPassword: 'Password123!',
      roles: [Roles.ADMINISTRATOR],
      matDialogRef: dialogRefSpy
    }));
  });

  it('should not call registerUser when form is invalid', () => {
    component.formGroup.setValue({
      email: 'admin@example.com',
      password: 'Password123!',
      passwordConfirm: 'DifferentPassword',
      roles: [Roles.ADMINISTRATOR]
    });

    fixture.debugElement.query(By.css('button[type="submit"]')).nativeElement.click();
    fixture.detectChanges();

    expect(registerHandlerSpy.dispatch).not.toHaveBeenCalled();
    expect(component.formGroup.valid).toBeFalse();
  });
});