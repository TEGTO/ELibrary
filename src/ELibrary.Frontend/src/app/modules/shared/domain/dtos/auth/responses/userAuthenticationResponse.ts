import { AuthToken, UserData } from "../../../..";

export interface UserAuthenticationResponse {
    authToken: AuthToken;
    email: string;
    roles: string[];
}
export function mapAuthResponseToUserData(response: UserAuthenticationResponse): UserData {
    return {
        email: response.email,
    }
}