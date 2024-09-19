import { UserAuthenticationResponse } from "../../dtos/auth/responses/userAuthenticationResponse";

export interface UserData {
    email: string;
}
export function mapAuthResponseToUserData(response: UserAuthenticationResponse): UserData {
    return {
        email: response.email,
    }
}