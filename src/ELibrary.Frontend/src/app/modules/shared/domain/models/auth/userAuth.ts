import { AuthToken, getDefaultAuthToken } from "../../..";

export interface UserAuth {
    isAuthenticated: boolean,
    authToken: AuthToken;
    email: string;
    roles: string[];
}

export function copyAuthTokenToAuthData(authData: UserAuth, authToken: AuthToken): UserAuth {
    return {
        ...authData,
        authToken: authToken
    };
}
export function getDefaultUserAuth(): UserAuth {
    return {
        isAuthenticated: false,
        authToken: getDefaultAuthToken(),
        email: "",
        roles: []
    }
}