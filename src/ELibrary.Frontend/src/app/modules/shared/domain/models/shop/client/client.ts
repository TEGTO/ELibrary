export interface Client {
    id: string;
    userId: string;
    name: string;
    middleName: string;
    lastName: string;
    dateOfBirth: Date;
    address: string;
    phone: string;
    email: string;
}

export function getDefaultClient(): Client {
    return {
        id: "",
        userId: "",
        name: "",
        middleName: "",
        lastName: "",
        dateOfBirth: new Date(),
        address: "",
        phone: "",
        email: ""
    }
}
export function getClientName(client: Client): string {
    return `${client.name} ${client.middleName} ${client.lastName}`;
}