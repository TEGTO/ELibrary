export interface AdminUser {
    id: string;
    userName: string;
    email: string;
    registredAt: Date;
    updatedAt: Date;
    roles: string[];
}