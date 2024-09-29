import { Command } from "../../../../shared";

export interface AddClientCommand extends Command {
    redirectAfter: string | undefined;
}