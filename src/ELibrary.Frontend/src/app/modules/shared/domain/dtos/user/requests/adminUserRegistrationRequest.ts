export interface AdminUserRegistrationRequest {
    email: string;
    password: string;
    confirmPassword: string;
    roles: string[];
}