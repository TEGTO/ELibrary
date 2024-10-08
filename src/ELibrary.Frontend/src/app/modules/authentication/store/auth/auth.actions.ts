import { createAction, props } from "@ngrx/store";
import { AuthData, AuthToken, UserAuthenticationRequest, UserData, UserRegistrationRequest } from "../../../shared";

//Registration
export const registerUser = createAction(
    '[Registration] Register New User',
    props<{ registrationRequest: UserRegistrationRequest }>()
);
export const registerSuccess = createAction(
    '[Registration] Register New User Success'
);
export const registerFailure = createAction(
    '[Registration] Register New User Failure',
    props<{ error: any }>()
);
//Auth
export const signInUser = createAction(
    '[Auth] Sing In By User Data',
    props<{ authRequest: UserAuthenticationRequest }>()
);
export const signInUserSuccess = createAction(
    '[Auth] Sing In By User Data Success',
    props<{ authData: AuthData, userData: UserData }>()
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
    props<{ authData: AuthData, userData: UserData }>()
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
    props<{ authData: AuthToken }>()
);
export const refreshAccessTokenFailure = createAction(
    '[Auth] Refresh Access Token Failure',
    props<{ error: any }>()
);