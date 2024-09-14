import { Roles } from "./roles";

export function checkManagerPolicy(roles: string[]): boolean {
    return roles.includes(Roles.administrator) || roles.includes(Roles.mnager);
}
export function checkAdminPolicy(roles: string[]): boolean {
    return roles.includes(Roles.administrator);
}