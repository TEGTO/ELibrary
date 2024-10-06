export interface AdminUser {
    id: string;
    userName: string;
    email: string;
    registredAt: Date;
    roles: string[];
}