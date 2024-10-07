import { Command } from "../../../../shared";

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