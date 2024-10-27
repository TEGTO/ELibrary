import { Command } from "../../../shared";

export interface SendAdvisorMessageCommand extends Command {
    message: string;
}