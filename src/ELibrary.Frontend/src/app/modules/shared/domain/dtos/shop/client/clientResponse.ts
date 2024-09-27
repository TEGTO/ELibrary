import { Client } from "../../../..";

export interface ClientResponse {
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
export function mapClientResponseToClient(response: ClientResponse): Client {
    return {
        id: response.id,
        userId: response.userId,
        name: response.name,
        middleName: response.middleName,
        lastName: response.lastName,
        dateOfBirth: new Date(response.dateOfBirth),
        address: response.address,
        phone: response.phone,
        email: response.email,
    };
}