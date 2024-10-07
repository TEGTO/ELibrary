import { Command } from "../../../../shared";

export interface StartAddingClientCommand extends Command {
    redirectAfter: string;
}