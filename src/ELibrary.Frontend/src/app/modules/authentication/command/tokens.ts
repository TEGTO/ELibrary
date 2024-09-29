import { InjectionToken } from "@angular/core";
import { LogOutCommand, SignInCommand, SignUpCommand, UpdateUserCommand } from "..";
import { CommandHandler } from "../../shared";

export const SIGN_UP_COMMAND_HANDLER = new InjectionToken<CommandHandler<SignUpCommand>>('SignUpCommandHandler');
export const SIGN_IN_COMMAND_HANDLER = new InjectionToken<CommandHandler<SignInCommand>>('SignInCommandHandler');
export const LOG_OUT_COMMAND_HANDLER = new InjectionToken<CommandHandler<LogOutCommand>>('LogOutCommandHandler');
export const UPDATE_USER_COMMAND_HANDLER = new InjectionToken<CommandHandler<UpdateUserCommand>>('UpdateUserCommandHandler');