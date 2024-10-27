import { Client } from "../../../..";

export interface UpdateClientRequest {
    name: string;
    middleName: string;
    lastName: string;
    dateOfBirth: Date;
    address: string;
    phone: string;
    email: string;
}

export function mapClientToUpdateClientRequest(client: Client): UpdateClientRequest {
    return {
        name: client.name,
        middleName: client.middleName,
        lastName: client.lastName,
        dateOfBirth: client.dateOfBirth,
        address: client.address,
        phone: client.phone,
        email: client.email
    }
}

export function getDefaultUpdateClientRequest(): UpdateClientRequest {
    return {
        name: 'Jane',
        middleName: 'M',
        lastName: 'Doe',
        address: '456 Main St',
        phone: '0987654321',
        email: 'jane.doe@example.com',
        dateOfBirth: new Date('1992-02-02')
    }
}