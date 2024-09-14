import { createReducer, on } from "@ngrx/store";
import { deleteUserFailure, deleteUserSuccess, getAuthDataFailure, getAuthDataSuccess, logOutUserSuccess, refreshAccessToken, refreshAccessTokenFailure, refreshAccessTokenSuccess, registerFailure, registerSuccess, registerUser, signInUser, signInUserFailure, signInUserSuccess, updateUserData, updateUserDataFailure, updateUserDataSuccess } from "../..";

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
        ...initialRegistrationState,
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
    roles: string[],
    error: any
}
const initialAuthState: AuthState = {
    isAuthenticated: false,
    accessToken: "",
    refreshToken: "",
    refreshTokenExpiryDate: new Date(),
    isRefreshSuccessful: false,
    roles: [],
    error: null
};

export const authReducer = createReducer(
    initialAuthState,

    on(registerSuccess, (state, { authData: authData }) => ({
        ...state,
        isAuthenticated: true,
        accessToken: authData.accessToken,
        refreshToken: authData.refreshToken,
        refreshTokenExpiryDate: authData.refreshTokenExpiryDate,
        roles: authData.roles,
        error: null
    })),
    on(registerFailure, (state, { error: error }) => ({
        ...initialAuthState,
        error: error
    })),

    on(signInUser, (state) => ({
        ...initialAuthState
    })),
    on(signInUserSuccess, (state, { authData: authData }) => ({
        ...state,
        isAuthenticated: true,
        accessToken: authData.accessToken,
        refreshToken: authData.refreshToken,
        refreshTokenExpiryDate: authData.refreshTokenExpiryDate,
        roles: authData.roles,
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
        roles: authData.roles,
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
    on(refreshAccessTokenSuccess, (state, { authToken: authToken }) => ({
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

    on(deleteUserSuccess, (state) => ({
        ...initialAuthState,
    })),
    on(deleteUserFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),
);
//UserData
export interface UserDataState {
    email: string,
    isUpdateSuccess: boolean,
    error: any
}
const initialUserState: UserDataState = {
    email: "",
    isUpdateSuccess: false,
    error: null
};

export const userDataReducer = createReducer(
    initialUserState,

    on(registerSuccess, (state, { userData: userData }) => ({
        ...state,
        email: userData.email,
        error: null
    })),
    on(registerFailure, (state, { error: error }) => ({
        ...initialUserState,
        error: error
    })),

    on(signInUser, (state) => ({
        ...initialUserState
    })),
    on(signInUserSuccess, (state, { userData: userData }) => ({
        ...state,
        email: userData.email,
        error: null
    })),
    on(signInUserFailure, (state) => ({
        ...initialUserState
    })),

    on(getAuthDataSuccess, (state, { userData: userData }) => ({
        ...state,
        email: userData.email,
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

    on(updateUserData, (state, { updateRequest: updateRequest }) => ({
        ...state,
        isUpdateSuccess: false,
        error: null
    })),
    on(updateUserDataSuccess, (state, { updateRequest: updateRequest }) => ({
        ...state,
        email: updateRequest.email,
        isUpdateSuccess: true,
        error: null
    })),
    on(updateUserDataFailure, (state, { error: error }) => ({
        ...state,
        isUpdateSuccess: false,
        error: error
    })),

    on(deleteUserSuccess, (state) => ({
        ...initialUserState,
    })),
    on(deleteUserFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),
);