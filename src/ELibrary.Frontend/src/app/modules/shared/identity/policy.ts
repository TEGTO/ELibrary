import { Roles } from "./roles";

export enum PolicyType {
    ClientPolicy, ManagerPolicy, AdminPolicy
}
export abstract class Policy {

    static checkPolicy(type: PolicyType, roles: string[]): boolean {
        switch (type) {
            case PolicyType.AdminPolicy:
                return roles.includes(Roles.ADMINISTRATOR);
            case PolicyType.ClientPolicy:
                return roles.includes(Roles.CLIENT) || roles.includes(Roles.MANAGER) || roles.includes(Roles.ADMINISTRATOR);
            case PolicyType.ManagerPolicy:
                return roles.includes(Roles.ADMINISTRATOR) || roles.includes(Roles.MANAGER);
            default:
                throw new Error("Invalid Policy Type!");
        }
    }
}