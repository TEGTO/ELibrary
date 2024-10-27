/* eslint-disable @typescript-eslint/no-explicit-any */
import { provideHttpClientTesting } from "@angular/common/http/testing";
import { TestBed } from "@angular/core/testing";
import { provideMockActions } from "@ngrx/effects/testing";
import { Observable, of, throwError } from "rxjs";
import { AdminEffects, getPaginatedUserAmount, getPaginatedUserAmountFailure, getPaginatedUserAmountSuccess, getPaginatedUsers, getPaginatedUsersFailure, getPaginatedUsersSuccess, getUser, getUserFailure, getUserSuccess, registerUser, registerUserFailure, registerUserSuccess } from "..";
import { AdminUser, AuthenticationApiService, ClientApiService, UserApiService } from "../../shared";

describe('AdminEffects', () => {
    let actions$: Observable<any>;
    let effects: AdminEffects;
    let mockAuthApiService: jasmine.SpyObj<AuthenticationApiService>;
    let mockUserApiService: jasmine.SpyObj<UserApiService>;
    let mockClientApiService: jasmine.SpyObj<ClientApiService>;

    const mockUser: AdminUser = {
        id: '1',
        userName: 'johndoe',
        email: 'john.doe@example.com',
        registredAt: new Date(),
        updatedAt: new Date(),
        roles: ['admin'],
        authenticationMethods: []
    };

    beforeEach(() => {
        mockAuthApiService = jasmine.createSpyObj('AuthenticationApiService', ['adminRegisterUser']);
        mockUserApiService = jasmine.createSpyObj('UserApiService', [
            'adminGetUser', 'adminGetPaginatedUsers', 'adminGetPaginatedUserAmount', 'adminUpdateUser', 'adminDeleteUser'
        ]);
        mockClientApiService = jasmine.createSpyObj('ClientApiService', ['adminGet', 'adminCreate', 'adminUpdate']);

        TestBed.configureTestingModule({
            providers: [
                AdminEffects,
                provideMockActions(() => actions$),
                { provide: AuthenticationApiService, useValue: mockAuthApiService },
                { provide: UserApiService, useValue: mockUserApiService },
                { provide: ClientApiService, useValue: mockClientApiService },
                provideHttpClientTesting(),
            ],
        });

        effects = TestBed.inject(AdminEffects);
    });

    describe('getUser$', () => {
        it('should return getUserSuccess on successful getUser', () => {
            const action = getUser({ info: '1' });
            const outcome = getUserSuccess({ user: mockUser });

            actions$ = of(action);
            mockUserApiService.adminGetUser.and.returnValue(of(mockUser));

            effects.getUser$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockUserApiService.adminGetUser).toHaveBeenCalledWith(action.info);
            });
        });

        it('should return getUserFailure on failed getUser', () => {
            const action = getUser({ info: '1' });
            const error = 'Failed to fetch user';
            const outcome = getUserFailure({ error });

            actions$ = of(action);
            mockUserApiService.adminGetUser.and.returnValue(throwError(() => Error(error)));

            effects.getUser$.subscribe(result => {
                expect(result).toEqual(outcome);
            });
        });
    });

    describe('registerUser$', () => {
        it('should return registerUserSuccess on successful registration', () => {
            const action = registerUser({ req: { email: 'new@example.com', password: 'password', confirmPassword: 'password', roles: [] } });
            const outcome = registerUserSuccess({ user: mockUser });

            actions$ = of(action);
            mockAuthApiService.adminRegisterUser.and.returnValue(of(mockUser));

            effects.registerUser$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockAuthApiService.adminRegisterUser).toHaveBeenCalledWith(action.req);
            });
        });

        it('should return registerUserFailure on failed registration', () => {
            const action = registerUser({ req: { email: 'new@example.com', password: 'password', confirmPassword: 'password', roles: [] } });
            const error = 'Registration failed';
            const outcome = registerUserFailure({ error });

            actions$ = of(action);
            mockAuthApiService.adminRegisterUser.and.returnValue(throwError(() => Error(error)));

            effects.registerUser$.subscribe(result => {
                expect(result).toEqual(outcome);
            });
        });
    });

    describe('getPaginatedUsers$', () => {
        it('should return getPaginatedUsersSuccess on success', () => {
            const action = getPaginatedUsers({ req: { pageNumber: 1, pageSize: 10, containsInfo: "" } });
            const outcome = getPaginatedUsersSuccess({ users: [mockUser] });

            actions$ = of(action);
            mockUserApiService.adminGetPaginatedUsers.and.returnValue(of([mockUser]));

            effects.getPaginatedUsers$.subscribe(result => {
                expect(result).toEqual(outcome);
                expect(mockUserApiService.adminGetPaginatedUsers).toHaveBeenCalledWith(action.req);
            });
        });

        it('should return getPaginatedUsersFailure on error', () => {
            const action = getPaginatedUsers({ req: { pageNumber: 1, pageSize: 10, containsInfo: "" } });
            const error = 'Failed to get users';
            const outcome = getPaginatedUsersFailure({ error });

            actions$ = of(action);
            mockUserApiService.adminGetPaginatedUsers.and.returnValue(throwError(() => Error(error)));

            effects.getPaginatedUsers$.subscribe(result => {
                expect(result).toEqual(outcome);
            });
        });
    });

    describe('getPaginatedUserAmount$', () => {
        it('should return getPaginatedUserAmountSuccess on success', () => {
            const action = getPaginatedUserAmount({ req: { pageNumber: 1, pageSize: 10, containsInfo: "" } });
            const amount = 100;
            const outcome = getPaginatedUserAmountSuccess({ amount });

            actions$ = of(action);
            mockUserApiService.adminGetPaginatedUserAmount.and.returnValue(of(amount));

            effects.getPaginatedUserAmount$.subscribe(result => {
                expect(result).toEqual(outcome);
            });
        });

        it('should return getPaginatedUserAmountFailure on error', () => {
            const action = getPaginatedUserAmount({ req: { pageNumber: 1, pageSize: 10, containsInfo: "" } });
            const error = 'Failed to get user amount';
            const outcome = getPaginatedUserAmountFailure({ error });

            actions$ = of(action);
            mockUserApiService.adminGetPaginatedUserAmount.and.returnValue(throwError(() => Error(error)));

            effects.getPaginatedUserAmount$.subscribe(result => {
                expect(result).toEqual(outcome);
            });
        });
    });
});