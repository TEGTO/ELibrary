import { AdminUserRegistrationRequest, Command } from "../../../../shared";

export interface AdminRegisterUserCommand extends Command {
    registerRequest: AdminUserRegistrationRequest
}