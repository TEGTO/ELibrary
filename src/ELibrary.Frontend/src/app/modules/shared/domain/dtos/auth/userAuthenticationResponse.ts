import { AuthToken } from "../../..";

export interface UserAuthenticationResponse {
    authToken: AuthToken;
    email: string;
    roles: string[];
}