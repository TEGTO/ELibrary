/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { createReducer, on } from "@ngrx/store";
import { deleteUserFailure, deleteUserSuccess, getAuthDataFailure, getAuthDataSuccess, logOutUserSuccess, refreshAccessToken, refreshAccessTokenFailure, refreshAccessTokenSuccess, registerFailure, registerSuccess, registerUser, signInUser, signInUserFailure, signInUserSuccess, updateUserData, updateUserDataFailure, updateUserDataSuccess } from "../..";
import { copyAuthTokenToAuthData as copyAuthTokenToUserAuth, copyUserUpdateRequestToAuthData, getDefaultUserAuth, UserAuth } from "../../../shared";

export interface AuthState {
    isRegistrationSuccess: boolean,
    isUpdateSuccess: boolean,
    isRefreshSuccessful: boolean,
    userAuth: UserAuth,
    error: any
}
const initialAuthState: AuthState = {
    isRegistrationSuccess: false,
    isUpdateSuccess: false,
    isRefreshSuccessful: false,
    userAuth: getDefaultUserAuth(),
    error: null
};

export const authReducer = createReducer(
    initialAuthState,

    on(registerUser, (state) => ({
        ...initialAuthState,
    })),
    on(registerSuccess, (state, { userAuth: userAuth }) => ({
        ...state,
        userAuth: userAuth,
        isRegistrationSuccess: true,
        error: null
    })),
    on(registerFailure, (state, { error: error }) => ({
        ...initialAuthState,
        error: error
    })),

    on(signInUser, (state) => ({
        ...initialAuthState
    })),
    on(signInUserSuccess, (state, { userAuth: userAuth }) => ({
        ...state,
        userAuth: userAuth,
        error: null
    })),
    on(signInUserFailure, (state, { error: error }) => ({
        ...initialAuthState,
        error: error
    })),

    on(getAuthDataSuccess, (state, { userAuth: userAuth }) => ({
        ...state,
        userAuth: userAuth,
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
        isRefreshSuccessful: true,
        userAuth: copyAuthTokenToUserAuth(state.userAuth, authToken),
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

    on(updateUserData, (state, { req: updateRequest }) => ({
        ...state,
        isUpdateSuccess: false,
        error: null
    })),
    on(updateUserDataSuccess, (state, { req: updateRequest }) => ({
        ...state,
        isUpdateSuccess: true,
        userAuth: copyUserUpdateRequestToAuthData(state.userAuth, updateRequest),
        error: null
    })),
    on(updateUserDataFailure, (state, { error: error }) => ({
        ...state,
        isUpdateSuccess: false,
        error: error
    })),
);