import { MatDialogRef } from "@angular/material/dialog";
import { Command, UserUpdateRequest } from "../../../shared";

export interface UpdateUserCommand extends Command {
    email: string;
    oldPassword: string;
    password: string;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    matDialogRef: MatDialogRef<any, any>;
}

export function mapUpdateUserCommandToUserUpdateRequest(command: UpdateUserCommand): UserUpdateRequest {
    return {
        email: command.email,
        oldPassword: command.oldPassword,
        password: command.password,
    };
}