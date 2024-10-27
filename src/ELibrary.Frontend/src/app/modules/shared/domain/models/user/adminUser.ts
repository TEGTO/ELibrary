export enum AuthenticationMethod {
    BaseAuthentication = 0, GoogleOAuth
}
export interface AdminUser {
    id: string;
    userName: string;
    email: string;
    registredAt: Date;
    updatedAt: Date;
    roles: string[];
    authenticationMethods: AuthenticationMethod[]
}