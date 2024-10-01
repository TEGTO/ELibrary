import { AuthToken, getDefaultUserAuth, UserAuthenticationRequest, UserRegistrationRequest, UserUpdateRequest } from "../../../shared";
import { getAuthData, getAuthDataFailure, getAuthDataSuccess, logOutUser, logOutUserSuccess, refreshAccessToken, refreshAccessTokenFailure, refreshAccessTokenSuccess, registerFailure, registerSuccess, registerUser, signInUser, signInUserFailure, signInUserSuccess, updateUserData, updateUserDataFailure, updateUserDataSuccess } from "./auth.actions";

describe('Authentication Actions', () => {
    const error = { message: 'An error occurred' };

    describe('Register User Actions', () => {
        it('should create registerUser action', () => {
            const req: UserRegistrationRequest = {
                email: 'email',
                password: 'password',
                confirmPassword: 'password',
            };
            const action = registerUser({ req });
            expect(action.type).toBe('[Auth] Register New User');
            expect(action.req).toBe(req);
        });

        it('should create registerSuccess action', () => {
            const userAuth = getDefaultUserAuth();
            const action = registerSuccess({ userAuth });
            expect(action.type).toBe('[Auth] Register New User Success');
            expect(action.userAuth).toBe(userAuth);
        });

        it('should create registerFailure action', () => {
            const action = registerFailure({ error });
            expect(action.type).toBe('[Auth] Register New User Failure');
            expect(action.error).toEqual(error);
        });
    });

    describe('Sign In Actions', () => {
        it('should create signInUser action', () => {
            const req: UserAuthenticationRequest = {
                login: 'login',
                password: 'password',
            };
            const action = signInUser({ req });
            expect(action.type).toBe('[Auth] Sing In By User Data');
            expect(action.req).toBe(req);
        });

        it('should create signInUserSuccess action', () => {
            const userAuth = getDefaultUserAuth();
            const action = signInUserSuccess({ userAuth });
            expect(action.type).toBe('[Auth] Sing In By User Data Success');
            expect(action.userAuth).toBe(userAuth);
        });

        it('should create signInUserFailure action', () => {
            const action = signInUserFailure({ error });
            expect(action.type).toBe('[Auth] Sing In By User Data Failure');
            expect(action.error).toEqual(error);
        });
    });

    describe('Get Authentication Data Actions', () => {
        it('should create getAuthData action', () => {
            const action = getAuthData();
            expect(action.type).toBe('[Auth] Get Authenticated Data');
        });

        it('should create getAuthDataSuccess action', () => {
            const userAuth = getDefaultUserAuth();
            const action = getAuthDataSuccess({ userAuth });
            expect(action.type).toBe('[Auth] Get Authenticated Data Success');
            expect(action.userAuth).toBe(userAuth);
        });

        it('should create getAuthDataFailure action', () => {
            const action = getAuthDataFailure();
            expect(action.type).toBe('[Auth] Get Authenticated Data Failure');
        });
    });

    describe('Log Out User Actions', () => {
        it('should create logOutUser action', () => {
            const action = logOutUser();
            expect(action.type).toBe('[Auth] Log out Authenticated User');
        });

        it('should create logOutUserSuccess action', () => {
            const action = logOutUserSuccess();
            expect(action.type).toBe('[Auth] Log out Authenticated User Success');
        });
    });

    describe('Refresh Access Token Actions', () => {
        it('should create refreshAccessToken action', () => {
            const authToken: AuthToken = {
                accessToken: 'accessToken',
                refreshToken: 'refreshToken',
                refreshTokenExpiryDate: new Date(),
            };
            const action = refreshAccessToken({ authToken });
            expect(action.type).toBe('[Auth] Refresh Access Token');
            expect(action.authToken).toBe(authToken);
        });

        it('should create refreshAccessTokenSuccess action', () => {
            const authToken: AuthToken = {
                accessToken: 'accessToken',
                refreshToken: 'refreshToken',
                refreshTokenExpiryDate: new Date(),
            };
            const action = refreshAccessTokenSuccess({ authToken });
            expect(action.type).toBe('[Auth] Refresh Access Token Success');
            expect(action.authToken).toBe(authToken);
        });

        it('should create refreshAccessTokenFailure action', () => {
            const action = refreshAccessTokenFailure({ error });
            expect(action.type).toBe('[Auth] Refresh Access Token Failure');
            expect(action.error).toEqual(error);
        });
    });

    describe('Update User Actions', () => {
        it('should create updateUserData action', () => {
            const req: UserUpdateRequest = {
                oldPassword: 'oldPass',
                password: 'newPass',
                email: 'test@example.com',
            };
            const action = updateUserData({ req });
            expect(action.type).toBe('[Auth] Update User Data');
            expect(action.req).toBe(req);
        });

        it('should create updateUserDataSuccess action', () => {
            const req: UserUpdateRequest = {
                oldPassword: 'oldPass',
                password: 'newPass',
                email: 'test@example.com',
            };
            const action = updateUserDataSuccess({ req });
            expect(action.type).toBe('[Auth] Update User Data Success');
            expect(action.req).toBe(req);
        });

        it('should create updateUserDataFailure action', () => {
            const action = updateUserDataFailure({ error });
            expect(action.type).toBe('[Auth] Update User Data Failure');
            expect(action.error).toEqual(error);
        });
    });
});
