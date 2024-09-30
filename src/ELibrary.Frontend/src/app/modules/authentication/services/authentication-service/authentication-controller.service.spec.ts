// import { TestBed } from '@angular/core/testing';
// import { Store } from '@ngrx/store';
// import { of } from 'rxjs';
// import { getAuthData, logOutUser, refreshAccessToken, registerUser, signInUser, updateUserData } from '../..';
// import { AuthToken, getDefaultAuthToken, UserAuth, UserAuthenticationRequest, UserRegistrationRequest, UserUpdateRequest } from '../../../shared';
// import { AuthenticationControllerService } from './authentication-controller.service';

// describe('AuthenticationControllerService', () => {
//   let service: AuthenticationControllerService;
//   let store: jasmine.SpyObj<Store>;

//   const mockUserAuth: UserAuth = {
//     isAuthenticated: true,
//     authToken: getDefaultAuthToken(),
//     email: 'user@example.com',
//     roles: ['user']
//   };

//   beforeEach(() => {
//     const storeSpy = jasmine.createSpyObj('Store', ['dispatch', 'select']);

//     TestBed.configureTestingModule({
//       providers: [
//         AuthenticationControllerService,
//         { provide: Store, useValue: storeSpy }
//       ]
//     });

//     service = TestBed.inject(AuthenticationControllerService);
//     store = TestBed.inject(Store) as jasmine.SpyObj<Store>;
//     store.select.and.returnValue(of(mockUserAuth));
//   });

//   it('should dispatch registerUser action and return isSuccess observable', (done) => {
//     const userRegistrationData: UserRegistrationRequest = {
//       email: 'user@example.com',
//       password: 'password',
//       confirmPassword: 'password'
//     };
//     store.select.and.returnValue(of(true));

//     service.registerUser(userRegistrationData).subscribe(result => {
//       expect(store.dispatch).toHaveBeenCalledWith(registerUser({ req: userRegistrationData }));
//       expect(result).toBe(true);
//       done();
//     });
//   });

//   it('should dispatch signInUser action and not return an observable', () => {
//     const userAuthData: UserAuthenticationRequest = { login: 'user@example.com', password: 'password' };

//     service.signInUser(userAuthData);
//     expect(store.dispatch).toHaveBeenCalledWith(signInUser({ req: userAuthData }));
//   });

//   it('should dispatch logOutUser action', () => {
//     service.logOutUser();
//     expect(store.dispatch).toHaveBeenCalledWith(logOutUser());
//   });

//   it('should dispatch refreshAccessToken action and return isRefreshSuccessful observable', (done) => {
//     const accessToken: AuthToken = { accessToken: 'newToken', refreshToken: 'newRefresh', refreshTokenExpiryDate: new Date() };
//     store.select.and.returnValue(of(true));

//     service.refreshToken(accessToken).subscribe(result => {
//       expect(result).toBe(true);
//       expect(store.dispatch).toHaveBeenCalledWith(refreshAccessToken({ authToken: accessToken }));
//       done();
//     });
//   });

//   it('should dispatch updateUserData action and return isUpdateSuccess observable', (done) => {
//     const userUpdateRequest: UserUpdateRequest = {
//       email: 'updated@example.com',
//       oldPassword: 'oldPassword',
//       password: 'newPassword'
//     };
//     store.select.and.returnValue(of(true));

//     service.updateUserAuth(userUpdateRequest).subscribe(result => {
//       expect(result).toBe(true);
//       expect(store.dispatch).toHaveBeenCalledWith(updateUserData({ req: userUpdateRequest }));
//       done();
//     });
//   });

//   it('should dispatch getUserAuth action and return userAuth observable', (done) => {
//     store.select.and.returnValue(of(mockUserAuth));

//     service.getUserAuth().subscribe(result => {
//       expect(result).toEqual(mockUserAuth);
//       expect(store.dispatch).toHaveBeenCalledWith(getAuthData());
//       done();
//     });
//   });

//   it('should return auth errors observable', (done) => {
//     store.select.and.returnValue(of('Error'));

//     service.getAuthErrors().subscribe(result => {
//       expect(result).toBe('Error');
//       done();
//     });
//   });
// });
