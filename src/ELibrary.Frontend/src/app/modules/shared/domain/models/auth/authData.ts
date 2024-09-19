import { AuthToken, UserAuthenticationResponse } from "../../..";

export interface AuthData {
    isAuthenticated: boolean;
    accessToken: string;
    refreshToken: string;
    refreshTokenExpiryDate: Date;
    roles: string[];
}

export function mapAuthResponseToAuthData(response: UserAuthenticationResponse): AuthData {
    return {
        isAuthenticated: true,
        accessToken: response.authToken.accessToken,
        refreshToken: response.authToken.refreshToken,
        refreshTokenExpiryDate: response.authToken.refreshTokenExpiryDate,
        roles: response.roles
    };
}
export function copyAuthTokenToAuthData(authData: AuthData, authToken: AuthToken): AuthData {
    return {
        isAuthenticated: authData.isAuthenticated,
        accessToken: authToken.accessToken,
        refreshToken: authToken.refreshToken,
        refreshTokenExpiryDate: authToken.refreshTokenExpiryDate,
        roles: authData.roles
    };
}