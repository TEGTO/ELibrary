import { createReducer, on } from "@ngrx/store";
import { getAuthDataFailure, getAuthDataSuccess, logOutUserSuccess, refreshAccessToken, refreshAccessTokenFailure, refreshAccessTokenSuccess, registerFailure, registerSuccess, registerUser, signInUser, signInUserFailure, signInUserSuccess } from "../..";

//Registration
export interface RegistrationState {
    isSuccess: boolean,
    error: any
}
const initialRegistrationState: RegistrationState = {
    isSuccess: false,
    error: null
};
export const registrationReducer = createReducer(
    initialRegistrationState,
    on(registerUser, (state) => ({
        ...initialRegistrationState,
    })),
    on(registerSuccess, (state) => ({
        ...state,
        isSuccess: true,
        error: null
    })),
    on(registerFailure, (state, { error: error }) => ({
        ...state,
        isSuccess: false,
        error: error
    })),
);
//Auth
export interface AuthState {
    isAuthenticated: boolean,
    accessToken: string,
    refreshToken: string,
    refreshTokenExpiryDate: Date,
    isRefreshSuccessful: boolean,
    error: any
}
const initialAuthState: AuthState = {
    isAuthenticated: false,
    accessToken: "",
    refreshToken: "",
    refreshTokenExpiryDate: new Date(),
    isRefreshSuccessful: false,
    error: null
};

export const authReducer = createReducer(
    initialAuthState,
    on(signInUser, (state) => ({
        ...initialAuthState
    })),
    on(signInUserSuccess, (state, { authData: authData }) => ({
        ...state,
        isAuthenticated: true,
        accessToken: authData.accessToken,
        refreshToken: authData.refreshToken,
        refreshTokenExpiryDate: authData.refreshTokenExpiryDate,
        error: null
    })),
    on(signInUserFailure, (state, { error: error }) => ({
        ...initialAuthState,
        error: error
    })),

    on(getAuthDataSuccess, (state, { authData: authData }) => ({
        ...state,
        isAuthenticated: authData.isAuthenticated,
        accessToken: authData.accessToken,
        refreshToken: authData.refreshToken,
        refreshTokenExpiryDate: authData.refreshTokenExpiryDate,
        error: null
    })),
    on(getAuthDataFailure, (state) => ({
        ...initialAuthState
    })),

    on(logOutUserSuccess, (state) => ({
        ...initialAuthState,
    })),

    on(refreshAccessToken, (state) => ({
        ...state,
        isRefreshSuccessful: false,
        error: null
    })),
    on(refreshAccessTokenSuccess, (state, { authData: authToken }) => ({
        ...state,
        isAuthenticated: true,
        accessToken: authToken.accessToken,
        refreshToken: authToken.refreshToken,
        refreshTokenExpiryDate: authToken.refreshTokenExpiryDate,
        isRefreshSuccessful: true,
        error: null
    })),
    on(refreshAccessTokenFailure, (state, { error: error }) => ({
        ...initialAuthState,
        error: error
    })),
);
//UserData
export interface UserDataState {
    userName: string,
    error: any
}
const initialUserState: UserDataState = {
    userName: "",
    error: null
};

export const userDataReducer = createReducer(
    initialUserState,
    on(signInUser, (state) => ({
        ...initialUserState
    })),
    on(signInUserSuccess, (state, { userData: userData }) => ({
        ...state,
        userName: userData.userName,
        error: null
    })),
    on(signInUserFailure, (state) => ({
        ...initialUserState
    })),

    on(getAuthDataSuccess, (state, { userData: userData }) => ({
        ...state,
        userName: userData.userName,
        error: null
    })),
    on(getAuthDataFailure, (state) => ({
        ...initialUserState
    })),

    on(logOutUserSuccess, (state) => ({
        ...initialUserState,
    })),

    on(refreshAccessTokenFailure, (state) => ({
        ...initialUserState
    })),
);