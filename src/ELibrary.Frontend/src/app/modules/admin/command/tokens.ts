import { InjectionToken } from "@angular/core";
import { AdminCreateClientCommand, AdminDeleteUserCommand, AdminRegisterUserCommand, AdminUpdateClientCommand, AdminUpdateUserCommand, StartAdminRegisterUserCommand } from "..";
import { CommandHandler } from "../../shared";

export const START_ADMIN_REGISTER_USER_COMMAND_HANDLER = new InjectionToken<CommandHandler<StartAdminRegisterUserCommand>>('StartAdminRegisterUserCommandHandler');
export const ADMIN_REGISTER_USER_COMMAND_HANDLER = new InjectionToken<CommandHandler<AdminRegisterUserCommand>>('AdminRegisterUserCommandHandler');
export const ADMIN_UPDATE_USER_COMMAND_HANDLER = new InjectionToken<CommandHandler<AdminUpdateUserCommand>>('AdminUpdateUserCommandHandler');
export const ADMIN_DELETE_USER_COMMAND_HANDLER = new InjectionToken<CommandHandler<AdminDeleteUserCommand>>('AdminDeleteUserCommandHandler');

export const ADMIN_CREATE_CLIENT_COMMAND_HANDLER = new InjectionToken<CommandHandler<AdminCreateClientCommand>>('AdminCreateClientCommandHandler');
export const ADMIN_UPDATE_CLIENT_COMMAND_HANDLER = new InjectionToken<CommandHandler<AdminUpdateClientCommand>>('AdminUpdateClientCommandHandler');