import { UserAuthenticationResponse } from "../../dtos/auth/userAuthenticationResponse";

export interface UserData {
    userName: string;
}
export function getUserFromAuthResponse(response: UserAuthenticationResponse): UserData {
    return {
        userName: response.userName,
    }
}