import { Command } from "../../../../shared";

export interface AdminUpdateUserCommand extends Command {
    currentLogin: string;
    email: string;
    password: string;
    confirmPassword: string;
    roles: string[];
}