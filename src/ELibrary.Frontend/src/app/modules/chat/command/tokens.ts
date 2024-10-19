import { InjectionToken } from "@angular/core";
import { ChangeChatVisibilityCommand, SendAdvisorMessageCommand } from "..";
import { CommandHandler } from "../../shared";

export const CHANGE_CHAT_VISIBILITY_COMMAND_HANDLER = new InjectionToken<CommandHandler<ChangeChatVisibilityCommand>>('ChangeChatVisibilityCommandHandler');
export const SEND_ADVISOR_MESSAGE_COMMAND_HANDLER = new InjectionToken<CommandHandler<SendAdvisorMessageCommand>>('SendAdvisorMessageCommandHandler');