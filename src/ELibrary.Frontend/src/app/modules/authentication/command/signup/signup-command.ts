import { MatDialogRef } from "@angular/material/dialog";
import { Command, UserRegistrationRequest } from "../../../shared";

export interface SignUpCommand extends Command {
    email: string;
    password: string;
    confirmPassword: string;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    matDialogRef: MatDialogRef<any, any>;
}

export function mapSignUpCommandToUserRegistrationRequest(command: SignUpCommand): UserRegistrationRequest {
    return {
        email: command.email,
        password: command.password,
        confirmPassword: command.confirmPassword,
    };
}