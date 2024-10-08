import { Command, UpdateClientRequest } from "../../../../shared";

export interface AdminUpdateClientCommand extends Command {
    userId: string;
    name: string;
    middleName: string;
    lastName: string;
    dateOfBirth: Date;
    address: string;
    phone: string;
    email: string;
}

export function mapAdminUpdateClientCommandToUpdateClientRequest(command: AdminUpdateClientCommand): UpdateClientRequest {
    return {
        name: command.name,
        middleName: command.middleName,
        lastName: command.lastName,
        dateOfBirth: command.dateOfBirth,
        address: command.address,
        phone: command.phone,
        email: command.email
    }
}