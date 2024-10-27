export interface AdminUserUpdateDataRequest {
    currentLogin: string;
    email: string;
    password: string;
    confirmPassword: string;
    roles: string[];
}