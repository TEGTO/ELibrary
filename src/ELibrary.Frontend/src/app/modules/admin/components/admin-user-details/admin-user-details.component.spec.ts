import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { ADMIN_DELETE_USER_COMMAND_HANDLER, ADMIN_UPDATE_USER_COMMAND_HANDLER, AdminDeleteUserCommand, AdminService, AdminUpdateUserCommand, AdminUserDetailsComponent } from '../..';
import { AdminUser, CommandHandler, RouteReader, ValidationMessage } from '../../../shared';

describe('AdminUserDetailsComponent', () => {
  let component: AdminUserDetailsComponent;
  let fixture: ComponentFixture<AdminUserDetailsComponent>;
  let mockAdminService: jasmine.SpyObj<AdminService>;
  let mockUpdateHandler: jasmine.SpyObj<CommandHandler<AdminUpdateUserCommand>>;
  let mockDeleteHandler: jasmine.SpyObj<CommandHandler<AdminDeleteUserCommand>>;
  let mockRouteReader: jasmine.SpyObj<RouteReader>;
  let mockValidationMessage: jasmine.SpyObj<ValidationMessage>;

  const mockUser: AdminUser = {
    id: '1',
    userName: 'johndoe',
    email: 'johndoe@example.com',
    registredAt: new Date(),
    updatedAt: new Date(),
    roles: ['Admin'],
    authenticationMethods: []
  };

  const activatedRouteStub = {
    snapshot: {
      paramMap: {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        get: (key: string) => '1'  // Mocking user ID retrieval
      }
    }
  };

  beforeEach(async () => {
    mockAdminService = jasmine.createSpyObj('AdminService', ['getUserById']);
    mockUpdateHandler = jasmine.createSpyObj('CommandHandler', ['dispatch']);
    mockDeleteHandler = jasmine.createSpyObj('CommandHandler', ['dispatch']);
    mockRouteReader = jasmine.createSpyObj('RouteReader', ['readId']);
    mockValidationMessage = jasmine.createSpyObj('ValidationMessage', ['getValidationMessage']);

    mockRouteReader.readId.and.returnValue(of(mockUser));
    mockValidationMessage.getValidationMessage.and.returnValue({ hasError: false, message: "" });

    await TestBed.configureTestingModule({
      declarations: [AdminUserDetailsComponent],
      imports: [
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        NoopAnimationsModule
      ],
      providers: [
        { provide: AdminService, useValue: mockAdminService },
        { provide: ActivatedRoute, useValue: activatedRouteStub },
        { provide: RouteReader, useValue: mockRouteReader },
        { provide: ValidationMessage, useValue: mockValidationMessage },
        { provide: ADMIN_UPDATE_USER_COMMAND_HANDLER, useValue: mockUpdateHandler },
        { provide: ADMIN_DELETE_USER_COMMAND_HANDLER, useValue: mockDeleteHandler },
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUserDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the form with user data', () => {
    const formGroup = component.getFormGroup(mockUser);

    expect(formGroup).toBeTruthy();
    expect(formGroup.get('email')?.value).toEqual(mockUser.email);
    expect(formGroup.get('roles')?.value).toEqual(mockUser.roles);
  });

  it('should toggle password visibility on click', () => {
    component.hidePassword = true;

    const passwordIcon = fixture.debugElement.query(By.css('.material-icons'));
    passwordIcon.triggerEventHandler('click', null);
    fixture.detectChanges();

    expect(component.hidePassword).toBeFalse();
  });

  it('should call updateHandler when form is valid and submitted', () => {
    const formGroup = component.getFormGroup(mockUser);
    formGroup.get('password')?.setValue('12345;QWERTYy');
    formGroup.get('passwordConfirm')?.setValue('12345;QWERTYy');
    formGroup.get('roles')?.setValue(['Admin']);

    component.updateUser(mockUser);

    expect(mockUpdateHandler.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({
      currentLogin: mockUser.email,
      email: mockUser.email,
      password: '12345;QWERTYy',
      confirmPassword: '12345;QWERTYy',
      roles: ['Admin']
    }));
  });

  it('should not call updateHandler if form is invalid', () => {
    const formGroup = component.getFormGroup(mockUser);
    formGroup.get('email')?.setValue('');

    component.updateUser(mockUser);

    expect(mockUpdateHandler.dispatch).not.toHaveBeenCalled();
  });

  it('should call deleteHandler when deleteUser is invoked', () => {
    component.deleteUser(mockUser);

    expect(mockDeleteHandler.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({
      userId: mockUser.id
    }));
  });

  it('should show validation message for invalid email', () => {
    const formGroup = component.getFormGroup(mockUser);
    formGroup.get('email')?.setValue('invalidemail');
    mockValidationMessage.getValidationMessage.and.returnValue({ hasError: true, message: "ERROR" });

    const emailErrorMessage = component.validate(formGroup.get('email')!);
    expect(emailErrorMessage.hasError).toBeTrue();
  });
});