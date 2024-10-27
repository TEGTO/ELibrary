import { Command } from "../../../shared";

export interface ChangeChatVisibilityCommand extends Command {
    state: boolean;
}