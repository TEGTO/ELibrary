/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { AdminDialogManager, AdminService, AdminUpdateClientCommand, getDefaultAdminUpdateClientCommand, mapAdminUpdateClientCommandToUpdateClientRequest } from '../../..';
import { AdminUpdateClientCommandHandlerService } from './admin-update-client-command-handler.service';

describe('AdminUpdateClientCommandHandlerService', () => {
  let service: AdminUpdateClientCommandHandlerService;
  let adminServiceMock: jasmine.SpyObj<AdminService>;
  let dialogManagerMock: jasmine.SpyObj<AdminDialogManager>;

  beforeEach(() => {
    adminServiceMock = jasmine.createSpyObj('AdminService', ['updateClientForUser']);
    dialogManagerMock = jasmine.createSpyObj('AdminDialogManager', ['openConfirmMenu']);

    TestBed.configureTestingModule({
      providers: [
        AdminUpdateClientCommandHandlerService,
        { provide: AdminService, useValue: adminServiceMock },
        { provide: AdminDialogManager, useValue: dialogManagerMock }
      ]
    });

    service = TestBed.inject(AdminUpdateClientCommandHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call updateClientForUser when user confirms', () => {
    const command: AdminUpdateClientCommand = getDefaultAdminUpdateClientCommand();
    const expectedRequest = mapAdminUpdateClientCommandToUpdateClientRequest(command);

    dialogManagerMock.openConfirmMenu.and.returnValue({
      afterClosed: () => of(true)
    } as any);

    service.dispatch(command);

    expect(dialogManagerMock.openConfirmMenu).toHaveBeenCalled();
    expect(adminServiceMock.updateClientForUser).toHaveBeenCalledWith(command.userId, expectedRequest);
  });

  it('should not call updateClientForUser if user cancels', () => {
    const command: AdminUpdateClientCommand = getDefaultAdminUpdateClientCommand();;

    dialogManagerMock.openConfirmMenu.and.returnValue({
      afterClosed: () => of(false)
    } as any);

    service.dispatch(command);

    expect(dialogManagerMock.openConfirmMenu).toHaveBeenCalled();
    expect(adminServiceMock.updateClientForUser).not.toHaveBeenCalled();
  });
});