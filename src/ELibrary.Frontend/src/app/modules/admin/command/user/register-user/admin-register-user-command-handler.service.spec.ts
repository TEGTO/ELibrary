/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { MatDialogRef } from '@angular/material/dialog';
import { AdminRegisterUserCommand, AdminService, mapAdminRegisterUserCommandToAdminUserRegistrationRequest } from '../../..';
import { AdminRegisterUserCommandHandlerService } from './admin-register-user-command-handler.service';

describe('AdminRegisterUserCommandHandlerService', () => {
  let service: AdminRegisterUserCommandHandlerService;
  let adminServiceMock: jasmine.SpyObj<AdminService>;
  let dialogRefMock: jasmine.SpyObj<MatDialogRef<any>>;

  beforeEach(() => {
    adminServiceMock = jasmine.createSpyObj('AdminService', ['registerUser']);
    dialogRefMock = jasmine.createSpyObj('MatDialogRef', ['close']);

    TestBed.configureTestingModule({
      providers: [
        AdminRegisterUserCommandHandlerService,
        { provide: AdminService, useValue: adminServiceMock },
        { provide: MatDialogRef, useValue: dialogRefMock }
      ]
    });

    service = TestBed.inject(AdminRegisterUserCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call registerUser and close dialog on dispatch', () => {
    const command: AdminRegisterUserCommand = {
      email: 'test@example.com',
      password: 'password123',
      confirmPassword: 'password123',
      roles: [],
      matDialogRef: dialogRefMock
    };
    const expectedRequest = mapAdminRegisterUserCommandToAdminUserRegistrationRequest(command);

    service.dispatch(command);

    expect(adminServiceMock.registerUser).toHaveBeenCalledWith(expectedRequest);
    expect(dialogRefMock.close).toHaveBeenCalled();
  });
});