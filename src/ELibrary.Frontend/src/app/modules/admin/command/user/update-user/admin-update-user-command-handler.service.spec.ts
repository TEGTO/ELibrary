/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { AdminDialogManager, AdminService, AdminUpdateUserCommand, getDefaultAdminUpdateUserCommand, mapAdminUpdateUserCommandToAdminUserUpdateDataRequest } from '../../..';
import { AdminUpdateUserCommandHandlerService } from './admin-update-user-command-handler.service';

describe('AdminUpdateUserCommandHandlerService', () => {
  let service: AdminUpdateUserCommandHandlerService;
  let adminServiceMock: jasmine.SpyObj<AdminService>;
  let dialogManagerMock: jasmine.SpyObj<AdminDialogManager>;

  beforeEach(() => {
    adminServiceMock = jasmine.createSpyObj('AdminService', ['updateUser']);
    dialogManagerMock = jasmine.createSpyObj('AdminDialogManager', ['openConfirmMenu']);

    TestBed.configureTestingModule({
      providers: [
        AdminUpdateUserCommandHandlerService,
        { provide: AdminService, useValue: adminServiceMock },
        { provide: AdminDialogManager, useValue: dialogManagerMock }
      ]
    });

    service = TestBed.inject(AdminUpdateUserCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call updateUser when user confirms', () => {
    const command: AdminUpdateUserCommand = getDefaultAdminUpdateUserCommand();
    const expectedRequest = mapAdminUpdateUserCommandToAdminUserUpdateDataRequest(command);

    dialogManagerMock.openConfirmMenu.and.returnValue({
      afterClosed: () => of(true)
    } as any);

    service.dispatch(command);

    expect(dialogManagerMock.openConfirmMenu).toHaveBeenCalled();
    expect(adminServiceMock.updateUser).toHaveBeenCalledWith(expectedRequest);
  });

  it('should not call updateUser if user cancels', () => {
    const command: AdminUpdateUserCommand = getDefaultAdminUpdateUserCommand();

    dialogManagerMock.openConfirmMenu.and.returnValue({
      afterClosed: () => of(false)
    } as any);

    service.dispatch(command);

    expect(dialogManagerMock.openConfirmMenu).toHaveBeenCalled();
    expect(adminServiceMock.updateUser).not.toHaveBeenCalled();
  });
})
