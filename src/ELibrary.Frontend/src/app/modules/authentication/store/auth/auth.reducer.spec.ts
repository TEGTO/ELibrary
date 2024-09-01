import { getAuthDataFailure, getAuthDataSuccess, logOutUserSuccess, refreshAccessTokenFailure, refreshAccessTokenSuccess, registerFailure, registerSuccess, registerUser, signInUser, signInUserFailure, signInUserSuccess } from "../..";
import { AuthData, AuthToken, UserAuthenticationRequest, UserData } from "../../../shared";
import { AuthState, RegistrationState, UserDataState, authReducer, registrationReducer, userDataReducer } from "./auth.reducer";

describe('RegistrationReducer', () => {
    const initialState: RegistrationState = {
        isSuccess: false,
        error: null
    };

    it('should return the initial state', () => {
        const action = { type: 'Unknown' } as any;
        const state = registrationReducer(initialState, action);
        expect(state).toBe(initialState);
    });

    it('should handle registerUser', () => {
        const registrationRequest = {
            userName: 'user',
            password: 'password',
            confirmPassword: 'password',
            userInfo: {
                name: "",
                lastName: "",
                dateOfBirth: new Date(),
                address: ""
            }
        };
        const action = registerUser({ registrationRequest: registrationRequest });
        const state = registrationReducer(initialState, action);
        expect(state).toEqual({
            isSuccess: false,
            error: null
        });
    });

    it('should handle registerSuccess', () => {
        const action = registerSuccess();
        const state = registrationReducer(initialState, action);
        expect(state).toEqual({
            isSuccess: true,
            error: null
        });
    });

    it('should handle registerFailure', () => {
        const error = 'Error!';
        const action = registerFailure({ error });
        const state = registrationReducer(initialState, action);
        expect(state).toEqual({
            isSuccess: false,
            error
        });
    });
});

describe('AuthReducer', () => {
    const initialState: AuthState = {
        isAuthenticated: false,
        accessToken: "",
        refreshToken: "",
        refreshTokenExpiryDate: new Date(),
        isRefreshSuccessful: false,
        error: null
    };

    it('should return the initial state', () => {
        const action = { type: 'Unknown' } as any;
        const state = authReducer(initialState, action);
        expect(state).toBe(initialState);
    });

    it('should handle signInUser', () => {
        const authRequest: UserAuthenticationRequest = { login: 'user@example.com', password: 'password' };
        const action = signInUser({ authRequest: authRequest });
        const state = authReducer(initialState, action);
        expect(state).toEqual({
            ...initialState,
            refreshTokenExpiryDate: state.refreshTokenExpiryDate
        });
    });

    it('should handle signInUserSuccess', () => {
        const authData: AuthData = {
            isAuthenticated: true,
            accessToken: 'authToken',
            refreshToken: 'refreshToken',
            refreshTokenExpiryDate: new Date()
        };
        const userData: UserData = { userName: "userName" };
        const action = signInUserSuccess({ authData: authData, userData: userData });
        const state = authReducer(initialState, action);
        expect(state).toEqual({
            ...initialState,
            isAuthenticated: true,
            accessToken: authData.accessToken,
            refreshToken: authData.refreshToken,
            refreshTokenExpiryDate: authData.refreshTokenExpiryDate,
            error: null
        });
    });

    it('should handle signInUserFailure', () => {
        const error = 'Error!';
        const action = signInUserFailure({ error });
        const state = authReducer(initialState, action);
        expect(state).toEqual({
            ...initialState,
            refreshTokenExpiryDate: state.refreshTokenExpiryDate,
            error
        });
    });

    it('should handle getAuthDataSuccess', () => {
        const authData: AuthData = {
            isAuthenticated: true,
            accessToken: 'authToken',
            refreshToken: 'refreshToken',
            refreshTokenExpiryDate: new Date()
        };
        const userData: UserData = { userName: "userName" };
        const action = getAuthDataSuccess({ authData: authData, userData: userData });
        const state = authReducer(initialState, action);
        expect(state).toEqual({
            ...initialState,
            isAuthenticated: authData.isAuthenticated,
            accessToken: authData.accessToken,
            refreshToken: authData.refreshToken,
            refreshTokenExpiryDate: authData.refreshTokenExpiryDate,
            error: null
        });
    });

    it('should handle getAuthDataFailure', () => {
        const action = getAuthDataFailure();
        const state = authReducer(initialState, action);
        expect(state).toEqual({
            ...initialState,
            refreshTokenExpiryDate: state.refreshTokenExpiryDate
        });
    });

    it('should handle logOutUserSuccess', () => {
        const action = logOutUserSuccess();
        const state = authReducer(initialState, action);
        expect(state).toEqual({
            ...initialState,
            refreshTokenExpiryDate: state.refreshTokenExpiryDate
        });
    });

    it('should handle refreshAccessTokenSuccess', () => {
        const accessToken: AuthToken = {
            accessToken: 'newAccessToken',
            refreshToken: 'newRefreshToken',
            refreshTokenExpiryDate: new Date()
        };
        const action = refreshAccessTokenSuccess({ authData: accessToken });
        const state = authReducer(initialState, action);
        expect(state).toEqual({
            ...initialState,
            isAuthenticated: true,
            accessToken: accessToken.accessToken,
            refreshToken: accessToken.refreshToken,
            refreshTokenExpiryDate: accessToken.refreshTokenExpiryDate,
            isRefreshSuccessful: true,
            error: null
        });
    });

    it('should handle refreshAccessTokenFailure', () => {
        const error = 'Error!';
        const action = refreshAccessTokenFailure({ error });
        const state = authReducer(initialState, action);
        expect(state).toEqual({
            ...initialState,
            refreshTokenExpiryDate: state.refreshTokenExpiryDate,
            error
        });
    });
});

describe('UserDataReducer', () => {
    const initialState: UserDataState = {
        userName: "",
        error: null
    };

    it('should return the initial state', () => {
        const action = { type: 'Unknown' } as any;
        const state = userDataReducer(initialState, action);
        expect(state).toBe(initialState);
    });

    it('should handle signInUser', () => {
        const authRequest: UserAuthenticationRequest = { login: 'user@example.com', password: 'password' };
        const action = signInUser({ authRequest: authRequest });
        const state = userDataReducer(initialState, action);
        expect(state).toEqual({
            ...initialState
        });
    });

    it('should handle signInUserSuccess', () => {
        const userData: UserData = {
            userName: 'user',
        };
        const action = signInUserSuccess({ userData, authData: { isAuthenticated: true, accessToken: 'authToken', refreshToken: 'refreshToken', refreshTokenExpiryDate: new Date() } });
        const state = userDataReducer(initialState, action);
        expect(state).toEqual({
            ...initialState,
            userName: userData.userName,
            error: null
        });
    });

    it('should handle signInUserFailure', () => {
        const action = signInUserFailure({ error: 'Error!' });
        const state = userDataReducer(initialState, action);
        expect(state).toEqual({
            ...initialState
        });
    });

    it('should handle getAuthDataSuccess', () => {
        const userData: UserData = {
            userName: 'user',
        };
        const action = getAuthDataSuccess({ userData, authData: { isAuthenticated: true, accessToken: 'authToken', refreshToken: 'refreshToken', refreshTokenExpiryDate: new Date() } });
        const state = userDataReducer(initialState, action);
        expect(state).toEqual({
            ...initialState,
            userName: userData.userName,
            error: null
        });
    });

    it('should handle getAuthDataFailure', () => {
        const action = getAuthDataFailure();
        const state = userDataReducer(initialState, action);
        expect(state).toEqual({
            ...initialState
        });
    });

    it('should handle logOutUserSuccess', () => {
        const action = logOutUserSuccess();
        const state = userDataReducer(initialState, action);
        expect(state).toEqual({
            ...initialState
        });
    });

    it('should handle refreshAccessTokenFailure', () => {
        const action = refreshAccessTokenFailure({ error: 'Error!' });
        const state = userDataReducer(initialState, action);
        expect(state).toEqual({
            ...initialState
        });
    });
});