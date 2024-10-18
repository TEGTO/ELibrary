import { InjectionToken } from "@angular/core";
import { ChangeChatVisibilityCommand } from "..";
import { CommandHandler } from "../../shared";

export const CHANGE_CHAT_VISIBILITY_COMMAND_HANDLER = new InjectionToken<CommandHandler<ChangeChatVisibilityCommand>>('ChangeChatVisibilityCommandHandler');