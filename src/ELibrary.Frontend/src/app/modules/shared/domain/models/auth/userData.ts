import { UserAuthenticationResponse } from "../../dtos/auth/userAuthenticationResponse";

export interface UserData {
    email: string;
}
export function getUserFromAuthResponse(response: UserAuthenticationResponse): UserData {
    return {
        email: response.email,
    }
}