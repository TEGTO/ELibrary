import { AdminUser } from "../../../..";

export interface AdminUserResponse {
    id: string;
    userName: string;
    email: string;
    registredAtUtc: Date;
    roles: string[];
}

export function mapAdminUserResponseToAdminUser(response: AdminUserResponse): AdminUser {
    return {
        id: response.id,
        userName: response.userName,
        email: response.email,
        registredAt: new Date(response.registredAtUtc),
        roles: response.roles
    }
}