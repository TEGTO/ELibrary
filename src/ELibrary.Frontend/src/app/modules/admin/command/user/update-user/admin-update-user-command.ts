import { AdminUserUpdateDataRequest, Command } from "../../../../shared";

export interface AdminUpdateUserCommand extends Command {
    currentLogin: string;
    email: string;
    password: string;
    confirmPassword: string;
    roles: string[];
}

export function mapAdminUpdateUserCommandToAdminUserUpdateDataRequest(command: AdminUpdateUserCommand): AdminUserUpdateDataRequest {
    return {
        currentLogin: command.currentLogin,
        email: command.email,
        password: command.password,
        confirmPassword: command.confirmPassword,
        roles: command.roles
    }
}

export function getDefaultAdminUpdateUserCommand(): AdminUpdateUserCommand {
    return {
        currentLogin: "",
        email: "",
        password: "",
        confirmPassword: "",
        roles: [],
    }
}