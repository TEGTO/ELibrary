import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { AuthenticationService, LOG_OUT_COMMAND_HANDLER, LogOutCommand, UPDATE_USER_COMMAND_HANDLER, UpdateUserCommand } from '../..';
import { CommandHandler, getDefaultUserAuth, ValidationMessage } from '../../../shared';
import { AuthenticatedComponent } from './authenticated.component';

describe('AuthenticatedComponent', () => {
    let component: AuthenticatedComponent;
    let fixture: ComponentFixture<AuthenticatedComponent>;
    let mockAuthService: jasmine.SpyObj<AuthenticationService>;
    let mockUpdateHandler: jasmine.SpyObj<CommandHandler<UpdateUserCommand>>;
    let mockLogOutHandler: jasmine.SpyObj<CommandHandler<LogOutCommand>>;
    let mockMatDialogRef: jasmine.SpyObj<MatDialogRef<AuthenticatedComponent>>;
    let mockValidationMessage: jasmine.SpyObj<ValidationMessage>;

    beforeEach(waitForAsync(() => {
        mockAuthService = jasmine.createSpyObj('AuthenticationService', ['getUserAuth', 'logOutUser']);
        mockUpdateHandler = jasmine.createSpyObj<CommandHandler<UpdateUserCommand>>(['dispatch']);
        mockLogOutHandler = jasmine.createSpyObj<CommandHandler<LogOutCommand>>(['dispatch']);
        mockMatDialogRef = jasmine.createSpyObj<MatDialogRef<AuthenticatedComponent>>(['close']);
        mockValidationMessage = jasmine.createSpyObj<ValidationMessage>(['getValidationMessage']);
        mockValidationMessage.getValidationMessage.and.returnValue({ hasError: false, message: '' });

        TestBed.configureTestingModule({
            imports: [ReactiveFormsModule],
            declarations: [AuthenticatedComponent],
            providers: [
                { provide: AuthenticationService, useValue: mockAuthService },
                { provide: UPDATE_USER_COMMAND_HANDLER, useValue: mockUpdateHandler },
                { provide: LOG_OUT_COMMAND_HANDLER, useValue: mockLogOutHandler },
                { provide: MatDialogRef, useValue: mockMatDialogRef },
                { provide: ValidationMessage, useValue: mockValidationMessage },
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
        }).compileComponents();

        fixture = TestBed.createComponent(AuthenticatedComponent);
        component = fixture.componentInstance;

        mockAuthService.getUserAuth.and.returnValue(of(
            {
                ...getDefaultUserAuth(),
                email: 'test@example.com',
                isAuthenticated: true
            }
        ));
    }));

    beforeEach(() => {
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should load user data on initialization', () => {
        component.ngOnInit();
        fixture.detectChanges();

        expect(mockAuthService.getUserAuth).toHaveBeenCalled();
        expect(component.formGroup.get('email')?.value).toBe('test@example.com');
    });

    it('should call logOutUser on logout button click', () => {
        const logoutButton = fixture.debugElement.query(By.css('button#logout-button')).nativeElement;
        logoutButton.click();

        expect(mockLogOutHandler.dispatch).toHaveBeenCalled();
    });

    it('should call updateUser when form is valid and submit button is clicked', () => {
        component.formGroup.setValue({
            email: 'updated@example.com',
            oldPassword: 'oldPassword123',
            password: 'newPassword123;',
        });

        const saveButton = fixture.debugElement.query(By.css('button#send-button')).nativeElement;
        saveButton.click();

        expect(mockUpdateHandler.dispatch).toHaveBeenCalledWith({
            email: 'updated@example.com',
            oldPassword: 'oldPassword123',
            password: 'newPassword123;',
            matDialogRef: mockMatDialogRef,
        });
    });

    it('should toggle password visibility on icon click', () => {
        const toggleButton = fixture.debugElement.query(By.css('.material-icons[matSuffix]')).nativeElement;
        toggleButton.click();
        fixture.detectChanges();

        expect(component.hideNewPassword).toBe(false);
    });

    it('should display validation error message if input is invalid', () => {
        mockValidationMessage.getValidationMessage.and.returnValue({ hasError: true, message: 'Invalid email' });

        component.formGroup.get('email')?.setErrors({ invalidEmail: true });
        fixture.detectChanges();

        const errorMessage = fixture.debugElement.query(By.css('mat-error')).nativeElement.textContent;
        expect(errorMessage).toContain('Invalid email');
    });

    it('should not dispatch updateUser if form is invalid', () => {
        component.formGroup.setErrors({ invalid: true });

        const saveButton = fixture.debugElement.query(By.css('button#send-button')).nativeElement;
        saveButton.click();

        expect(mockUpdateHandler.dispatch).not.toHaveBeenCalled();
    });

    it('should unsubscribe from destroy$ on component destruction', () => {
        const destroySpy = spyOn(component['destroy$'], 'next');
        const completeSpy = spyOn(component['destroy$'], 'complete');

        component.ngOnDestroy();

        expect(destroySpy).toHaveBeenCalled();
        expect(completeSpy).toHaveBeenCalled();
    });
});