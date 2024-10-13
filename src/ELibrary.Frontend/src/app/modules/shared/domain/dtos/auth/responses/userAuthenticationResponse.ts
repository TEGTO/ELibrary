import { AuthTokenResponse, mapAuthTokenResponseToAuthToken, UserAuth } from "../../../..";

export interface UserAuthenticationResponse {
    authToken: AuthTokenResponse;
    email: string;
    roles: string[];
}

export function mapUserAuthenticationResponseToUserAuthentication(response: UserAuthenticationResponse): UserAuth {
    return {
        isAuthenticated: true,
        authToken: mapAuthTokenResponseToAuthToken(response.authToken),
        email: response.email,
        roles: response.roles
    }
}