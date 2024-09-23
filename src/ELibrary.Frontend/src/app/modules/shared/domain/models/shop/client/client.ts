export interface Client {
    id: string;
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
        name: "",
        middleName: "",
        lastName: "",
        dateOfBirth: new Date(),
        address: "",
        phone: "",
        email: ""
    }
}