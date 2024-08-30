import { UserInfo } from "../user-info/userInfo";

export interface UserRegistrationRequest {
    userName: string;
    password: string;
    confirmPassword: string;
    userInfo: UserInfo;
}