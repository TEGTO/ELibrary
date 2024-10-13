/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAction, props } from "@ngrx/store";
import { AuthToken, UserAuth, UserAuthenticationRequest, UserRegistrationRequest, UserUpdateRequest } from "../../../shared";

export const registerUser = createAction(
    '[Auth] Register New User',
    props<{ req: UserRegistrationRequest }>()
);
export const registerSuccess = createAction(
    '[Auth] Register New User Success',
    props<{ userAuth: UserAuth }>()
);
export const registerFailure = createAction(
    '[Auth] Register New User Failure',
    props<{ error: any }>()
);

export const signInUser = createAction(
    '[Auth] Sing In By User Data',
    props<{ req: UserAuthenticationRequest }>()
);
export const signInUserSuccess = createAction(
    '[Auth] Sing In By User Data Success',
    props<{ userAuth: UserAuth }>()
);
export const signInUserFailure = createAction(
    '[Auth] Sing In By User Data Failure',
    props<{ error: any }>()
);

export const getAuthData = createAction(
    '[Auth] Get Authenticated Data'
);
export const getAuthDataSuccess = createAction(
    '[Auth] Get Authenticated Data Success',
    props<{ userAuth: UserAuth }>()
);
export const getAuthDataFailure = createAction(
    '[Auth] Get Authenticated Data Failure'
);

export const logOutUser = createAction(
    '[Auth] Log out Authenticated User'
);
export const logOutUserSuccess = createAction(
    '[Auth] Log out Authenticated User Success'
);

export const refreshAccessToken = createAction(
    '[Auth] Refresh Access Token',
    props<{ authToken: AuthToken }>()
);
export const refreshAccessTokenSuccess = createAction(
    '[Auth] Refresh Access Token Success',
    props<{ authToken: AuthToken }>()
);
export const refreshAccessTokenFailure = createAction(
    '[Auth] Refresh Access Token Failure',
    props<{ error: any }>()
);

export const updateUserData = createAction(
    '[Auth] Update User Data',
    props<{ req: UserUpdateRequest }>()
);
export const updateUserDataSuccess = createAction(
    '[Auth] Update User Data Success',
    props<{ req: UserUpdateRequest }>()
);
export const updateUserDataFailure = createAction(
    '[Auth] Update User Data Failure',
    props<{ error: any }>()
);

export const deleteUser = createAction(
    '[Auth] Delete User',
);
export const deleteUserSuccess = createAction(
    '[Auth] Delete User Success'
);
export const deleteUserFailure = createAction(
    '[Auth] Delete User Failure',
    props<{ error: any }>()
);

