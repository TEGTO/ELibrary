import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { AuthenticationDialogManager, AuthenticationService, StartLoginCommand, StartLoginCommandHandlerService } from '../..';
import { getDefaultUserAuth, UserAuth } from '../../../shared';

describe('StartLoginCommandHandlerService', () => {
  let service: StartLoginCommandHandlerService;
  let mockDialogManager: jasmine.SpyObj<AuthenticationDialogManager>;
  let mockAuthService: jasmine.SpyObj<AuthenticationService>;

  beforeEach(() => {
    const dialogManagerSpy = jasmine.createSpyObj('AuthenticationDialogManager', ['openLoginMenu', 'openAuthenticatedMenu']);
    const authServiceSpy = jasmine.createSpyObj('AuthenticationService', ['getUserAuth']);

    const mockAuthData: UserAuth = { ...getDefaultUserAuth(), isAuthenticated: true };
    authServiceSpy.getUserAuth.and.returnValue(of(mockAuthData));

    TestBed.configureTestingModule({
      providers: [
        StartLoginCommandHandlerService,
        { provide: AuthenticationDialogManager, useValue: dialogManagerSpy },
        { provide: AuthenticationService, useValue: authServiceSpy }
      ]
    });

    service = TestBed.inject(StartLoginCommandHandlerService);
    mockDialogManager = TestBed.inject(AuthenticationDialogManager) as jasmine.SpyObj<AuthenticationDialogManager>;
    mockAuthService = TestBed.inject(AuthenticationService) as jasmine.SpyObj<AuthenticationService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize with user authentication state', () => {
    service = TestBed.inject(StartLoginCommandHandlerService);

    expect(service.isAuthenticated).toBeTrue();
    expect(mockAuthService.getUserAuth).toHaveBeenCalled();
  });

  it('should open authenticated menu if the user is authenticated', () => {
    service.isAuthenticated = true;
    const mockCommand: StartLoginCommand = {};

    service.dispatch(mockCommand);

    expect(mockDialogManager.openAuthenticatedMenu).toHaveBeenCalled();
    expect(mockDialogManager.openLoginMenu).not.toHaveBeenCalled();
  });

  it('should open login menu if the user is not authenticated', () => {
    service.isAuthenticated = false;
    const mockCommand: StartLoginCommand = {};

    service.dispatch(mockCommand);

    expect(mockDialogManager.openLoginMenu).toHaveBeenCalled();
    expect(mockDialogManager.openAuthenticatedMenu).not.toHaveBeenCalled();
  });

  it('should not open authenticated menu when no authentication info is available', () => {
    service.isAuthenticated = false;
    const mockCommand: StartLoginCommand = {};

    service.dispatch(mockCommand);

    expect(mockDialogManager.openLoginMenu).toHaveBeenCalled();
    expect(mockDialogManager.openAuthenticatedMenu).not.toHaveBeenCalled();
  });
});

