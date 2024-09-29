import { UserData } from "../../../..";

export interface UserUpdateRequest {
    email: string;
    oldPassword: string;
    password: string;
}

export function mapUserUpdateRequestToUserData(response: UserUpdateRequest): UserData {
    return {
        email: response.email,
    }
}