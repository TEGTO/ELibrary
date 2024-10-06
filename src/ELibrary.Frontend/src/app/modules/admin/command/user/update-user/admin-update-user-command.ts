import { AdminUserUpdateDataRequest, Command } from "../../../../shared";

export interface AdminUpdateUserCommand extends Command {
    updaterRequest: AdminUserUpdateDataRequest
}