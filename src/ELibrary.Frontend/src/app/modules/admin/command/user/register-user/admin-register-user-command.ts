import { MatDialogRef } from "@angular/material/dialog";
import { AdminUserRegistrationRequest, Command } from "../../../../shared";

export interface AdminRegisterUserCommand extends Command {
    email: string;
    password: string;
    confirmPassword: string;
    roles: string[];
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    matDialogRef: MatDialogRef<any>;
}

export function mapAdminRegisterUserCommandToAdminUserRegistrationRequest(command: AdminRegisterUserCommand): AdminUserRegistrationRequest {
    return {
        email: command.email,
        password: command.password,
        confirmPassword: command.confirmPassword,
        roles: command.roles,
    }
}