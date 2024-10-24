/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpResponse } from "@angular/common/http";
import { TestBed } from "@angular/core/testing";
import { provideMockActions } from "@ngrx/effects/testing";
import { Observable, of, throwError } from "rxjs";
import { deleteUser, deleteUserFailure, deleteUserSuccess, getAuthData, getAuthDataFailure, getAuthDataSuccess, logOutUser, logOutUserSuccess, refreshAccessToken, refreshAccessTokenFailure, refreshAccessTokenSuccess, registerFailure, registerSuccess, registerUser, signInUser, signInUserFailure, signInUserSuccess, updateUserData, updateUserDataFailure, updateUserDataSuccess } from "../..";
import { AuthenticationApiService, AuthToken, LocalStorageService, UserApiService, UserAuth } from "../../../shared";
import { AuthEffects } from "./auth.effects";

describe('AuthEffects', () => {
    let actions$: Observable<any>;
    let effects: AuthEffects;
    let mockAuthApiService: jasmine.SpyObj<AuthenticationApiService>;
    let mockUserApiService: jasmine.SpyObj<UserApiService>;
    let mockLocalStorage: jasmine.SpyObj<LocalStorageService>;

    const mockUserAuth: UserAuth = {
        isAuthenticated: true,
        authToken: {
            accessToken: 'mockAccessToken',
            refreshToken: 'mockRefreshToken',
            refreshTokenExpiryDate: new Date(),
        },
        email: 'test@example.com',
        roles: ['CLIENT']
    };

    const mockAuthToken: AuthToken = {
        accessToken: 'newAccessToken',
        refreshToken: 'newRefreshToken',
        refreshTokenExpiryDate: new Date(),
    };

    beforeEach(() => {
        mockAuthApiService = jasmine.createSpyObj('AuthenticationApiService', ['registerUser', 'loginUser', 'refreshToken']);
        mockUserApiService = jasmine.createSpyObj('UserApiService', ['updateUser', 'deleteUser']);
        mockLocalStorage = jasmine.createSpyObj('LocalStorageService', ['setItem', 'getItem', 'removeItem']);

        TestBed.configureTestingModule({
            providers: [
                AuthEffects,
                provideMockActions(() => actions$),
                { provide: AuthenticationApiService, useValue: mockAuthApiService },
                { provide: LocalStorageService, useValue: mockLocalStorage },
                { provide: UserApiService, useValue: mockUserApiService },
            ]
        });

        effects = TestBed.inject(AuthEffects);
    });

    describe('registerUser$', () => {
        it('should dispatch registerSuccess on successful registerUser', (() => {
            const action = registerUser({ req: { email: 'test@example.com', password: 'password', confirmPassword: 'password' } });
            const outcome = registerSuccess({ userAuth: mockUserAuth });

            actions$ = of(action);
            mockAuthApiService.registerUser.and.returnValue(of(mockUserAuth));

            effects.registerUser$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockAuthApiService.registerUser).toHaveBeenCalledWith(action.req);
                expect(mockLocalStorage.setItem).toHaveBeenCalledWith('userAuth', JSON.stringify(mockUserAuth));
            });
        }));

        it('should dispatch registerFailure on failed registerUser', (() => {
            const action = registerUser({ req: { email: 'test@example.com', password: 'password', confirmPassword: 'password' } });
            const error = new Error('Registration failed');
            const outcome = registerFailure({ error: error.message });

            actions$ = of(action);
            mockAuthApiService.registerUser.and.returnValue(throwError(error));

            effects.registerUser$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockAuthApiService.registerUser).toHaveBeenCalledWith(action.req);
            });
        }));
    });

    describe('signInUser$', () => {
        it('should dispatch signInUserSuccess on successful login', (() => {
            const action = signInUser({ req: { login: 'test@example.com', password: 'password' } });
            const outcome = signInUserSuccess({ userAuth: mockUserAuth });

            actions$ = of(action);
            mockAuthApiService.loginUser.and.returnValue(of(mockUserAuth));

            effects.singInUser$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockAuthApiService.loginUser).toHaveBeenCalledWith(action.req);
                expect(mockLocalStorage.setItem).toHaveBeenCalledWith('userAuth', JSON.stringify(mockUserAuth));
            });
        }));

        it('should dispatch signInUserFailure on failed login', (() => {
            const action = signInUser({ req: { login: 'test@example.com', password: 'password' } });
            const error = new Error('Login failed');
            const outcome = signInUserFailure({ error: error.message });

            actions$ = of(action);
            mockAuthApiService.loginUser.and.returnValue(throwError(error));

            effects.singInUser$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockAuthApiService.loginUser).toHaveBeenCalledWith(action.req);
            });
        }));
    });

    describe('getAuthData$', () => {
        it('should dispatch getAuthDataSuccess if auth data exists in local storage', (() => {
            const action = getAuthData();
            const outcome = getAuthDataSuccess({ userAuth: mockUserAuth });

            mockLocalStorage.getItem.and.returnValue(JSON.stringify(mockUserAuth));

            actions$ = of(action);

            effects.getAuthData$.subscribe(result => {
                expect(result.type).toEqual(outcome.type);
                expect(mockLocalStorage.getItem).toHaveBeenCalledWith(effects.storageUserAuthKey);
            });
        }));

        it('should dispatch getAuthDataFailure if auth data does not exist', (() => {
            const action = getAuthData();
            const outcome = getAuthDataFailure();

            mockLocalStorage.getItem.and.returnValue(null);

            actions$ = of(action);

            effects.getAuthData$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockLocalStorage.getItem).toHaveBeenCalledWith(effects.storageUserAuthKey);
            });
        }));
    });

    describe('logOutUser$', () => {
        it('should dispatch logOutUserSuccess and clear auth data from local storage', (() => {
            const action = logOutUser();
            const outcome = logOutUserSuccess();

            actions$ = of(action);

            effects.logOutUser$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockLocalStorage.removeItem).toHaveBeenCalledWith(effects.storageUserAuthKey);
            });
        }));
    });

    describe('refreshToken$', () => {
        it('should dispatch refreshAccessTokenSuccess on successful token refresh', (() => {
            const action = refreshAccessToken({ authToken: mockAuthToken });
            const outcome = refreshAccessTokenSuccess({ authToken: mockAuthToken });

            mockLocalStorage.getItem.and.returnValue(JSON.stringify(mockUserAuth));

            actions$ = of(action);
            mockAuthApiService.refreshToken.and.returnValue(of(mockAuthToken));

            effects.refreshToken$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockAuthApiService.refreshToken).toHaveBeenCalledWith(action.authToken);
                expect(mockLocalStorage.setItem).toHaveBeenCalledWith(effects.storageUserAuthKey, JSON.stringify({
                    ...mockUserAuth,
                    authToken: mockAuthToken
                }));
            });
        }));

        it('should dispatch refreshAccessTokenFailure on failed token refresh', (() => {
            const action = refreshAccessToken({ authToken: mockAuthToken });
            const error = new Error('Token refresh failed');
            const outcome = refreshAccessTokenFailure({ error: error.message });

            actions$ = of(action);
            mockAuthApiService.refreshToken.and.returnValue(throwError(error));

            effects.refreshToken$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockAuthApiService.refreshToken).toHaveBeenCalledWith(action.authToken);
                expect(mockLocalStorage.removeItem).toHaveBeenCalledWith(effects.storageUserAuthKey);
            });
        }));
    });

    describe('updateUserData$', () => {
        it('should dispatch updateUserDataSuccess on successful user update', (() => {
            const action = updateUserData({ req: { email: 'test@example.com', oldPassword: 'password', password: 'newPassword' } });
            const outcome = updateUserDataSuccess({ req: action.req });

            mockLocalStorage.getItem.and.returnValue(JSON.stringify(mockUserAuth));
            mockUserApiService.updateUser.and.returnValue(of(new HttpResponse<void>()));

            actions$ = of(action);

            effects.updateUserData$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockUserApiService.updateUser).toHaveBeenCalledWith(action.req);
                expect(mockLocalStorage.setItem).toHaveBeenCalledWith(effects.storageUserAuthKey, JSON.stringify({
                    ...mockUserAuth,
                    email: action.req.email
                }));
            });
        }));

        it('should dispatch updateUserDataFailure on failed user update', (() => {
            const action = updateUserData({ req: { email: 'test@example.com', oldPassword: 'password', password: 'newPassword' } });
            const error = new Error('Update failed');
            const outcome = updateUserDataFailure({ error: error.message });

            mockUserApiService.updateUser.and.returnValue(throwError(error));

            actions$ = of(action);

            effects.updateUserData$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockUserApiService.updateUser).toHaveBeenCalledWith(action.req);
            });
        }));
    });

    describe('deleteUser$', () => {
        it('should dispatch deleteUserSuccess on successful user deletion', (() => {
            const action = deleteUser();
            const outcome = deleteUserSuccess();

            mockUserApiService.deleteUser.and.returnValue(of(new HttpResponse<void>()));

            actions$ = of(action);
            effects.deleteUser$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockUserApiService.deleteUser).toHaveBeenCalled();
                expect(mockLocalStorage.removeItem).toHaveBeenCalledWith(effects.storageUserAuthKey);
            });
        }));

        it('should dispatch deleteUserFailure on failed user deletion', (() => {
            const action = deleteUser();
            const error = new Error('Delete failed');
            const outcome = deleteUserFailure({ error: error.message });

            mockUserApiService.deleteUser.and.returnValue(throwError(error));

            actions$ = of(action);

            effects.deleteUser$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockUserApiService.deleteUser).toHaveBeenCalled();
            });
        }));
    });
});