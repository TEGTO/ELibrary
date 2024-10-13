import { MatDialogRef } from "@angular/material/dialog";
import { Command, UserAuthenticationRequest } from "../../../shared";

export interface SignInCommand extends Command {
    login: string;
    password: string;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    matDialogRef: MatDialogRef<any, any>;
}

export function mapSignInCommandToUserAuthenticationRequest(command: SignInCommand): UserAuthenticationRequest {
    return {
        login: command.login,
        password: command.password,
    };
}