import { UserAuth } from "../../../..";

export interface UserUpdateRequest {
    email: string;
    oldPassword: string;
    password: string;
}

export function copyUserUpdateRequestToAuthData(authData: UserAuth, req: UserUpdateRequest): UserAuth {
    return {
        ...authData,
        email: req.email,
    }
}