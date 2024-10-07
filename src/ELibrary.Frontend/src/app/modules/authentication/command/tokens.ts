import { InjectionToken } from "@angular/core";
import { LogOutCommand, SignInCommand, SignUpCommand, StartLoginCommand, StartRegistrationCommand, UpdateUserCommand } from "..";
import { CommandHandler } from "../../shared";

export const START_REGISTRATION_COMMAND_HANDLER = new InjectionToken<CommandHandler<StartRegistrationCommand>>('StartRegistrationCommandHandler');
export const START_LOGIN_COMMAND_HANDLER = new InjectionToken<CommandHandler<StartLoginCommand>>('StartLoginCommandHandler');
export const SIGN_UP_COMMAND_HANDLER = new InjectionToken<CommandHandler<SignUpCommand>>('SignUpCommandHandler');
export const SIGN_IN_COMMAND_HANDLER = new InjectionToken<CommandHandler<SignInCommand>>('SignInCommandHandler');
export const LOG_OUT_COMMAND_HANDLER = new InjectionToken<CommandHandler<LogOutCommand>>('LogOutCommandHandler');
export const UPDATE_USER_COMMAND_HANDLER = new InjectionToken<CommandHandler<UpdateUserCommand>>('UpdateUserCommandHandler');