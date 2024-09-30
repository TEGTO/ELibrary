// import { TestBed } from '@angular/core/testing';
// import { MatDialog } from '@angular/material/dialog';
// import { of } from 'rxjs';
// import { AuthenticatedComponent, AuthenticationService, LoginComponent, RegisterComponent } from '../..';
// import { getDefaultUserAuth, UserAuth } from '../../../shared';
// import { AuthenticationDialogManagerService } from './authentication-dialog-manager.service';

// describe('AuthenticationDialogManagerService', () => {
//   const mockUserAuth: UserAuth =
//   {
//     ...getDefaultUserAuth(),
//     isAuthenticated: true,
//   }

//   let service: AuthenticationDialogManagerService;
//   let mockMatDialog: jasmine.SpyObj<MatDialog>
//   let mockAuthService: jasmine.SpyObj<AuthenticationService>

//   beforeEach(() => {
//     mockMatDialog = jasmine.createSpyObj<MatDialog>('MatDialog', ['open']);
//     mockAuthService = jasmine.createSpyObj<AuthenticationService>('AuthenticationService', ['getUserAuth']);
//     mockAuthService.getUserAuth.and.returnValue(of(mockUserAuth));

//     TestBed.configureTestingModule({
//       providers: [
//         { provide: MatDialog, useValue: mockMatDialog },
//         { provide: AuthenticationService, useValue: mockAuthService }
//       ],
//     });
//     service = TestBed.inject(AuthenticationDialogManagerService);
//   });

//   it('should be created', () => {
//     expect(service).toBeTruthy();
//   });

//   it('should open authenticated dialog', () => {
//     service.openLoginMenu();

//     expect(mockMatDialog.open).toHaveBeenCalledWith(AuthenticatedComponent, {
//       height: '455px',
//       width: '450px',
//     });
//   });

//   it('should open login dialog', () => {
//     const mockUserAuth: UserAuth =
//     {
//       ...getDefaultUserAuth(),
//       isAuthenticated: false,
//     }
//     mockAuthService.getUserAuth.and.returnValue(of(mockUserAuth));
//     service = new AuthenticationDialogManagerService(mockAuthService, mockMatDialog);

//     service.openLoginMenu();

//     expect(mockMatDialog.open).toHaveBeenCalledWith(LoginComponent, {
//       height: '345px',
//       width: '450px',
//     });
//   });

//   it('should open register dialog', () => {
//     service.openRegisterMenu();

//     expect(mockMatDialog.open).toHaveBeenCalledWith(RegisterComponent, {
//       height: '630px',
//       width: '450px',
//     });
//   });

// });