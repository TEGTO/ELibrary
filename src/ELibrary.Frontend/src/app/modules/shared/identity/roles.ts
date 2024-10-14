export abstract class Roles {
    static readonly CLIENT = "Client";
    static readonly MANAGER = "Manager";
    static readonly ADMINISTRATOR = "Administrator";
}

export function getRoleArray(): string[] {
    return [Roles.CLIENT, Roles.MANAGER, Roles.ADMINISTRATOR];
}
