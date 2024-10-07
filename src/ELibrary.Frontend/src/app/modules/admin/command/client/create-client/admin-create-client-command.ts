import { Command } from "../../../../shared";

export interface AdminCreateClientCommand extends Command {
    userId: string;
    name: string;
    middleName: string;
    lastName: string;
    dateOfBirth: Date;
    address: string;
    phone: string;
    email: string;
}