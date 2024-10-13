export abstract class Roles {
    static CLIENT = "Client";
    static MANAGER = "Manager";
    static ADMINISTRATOR = "Administrator";
}

export function getRoleArray(): string[] {
    return [Roles.CLIENT, Roles.MANAGER, Roles.ADMINISTRATOR];
}
