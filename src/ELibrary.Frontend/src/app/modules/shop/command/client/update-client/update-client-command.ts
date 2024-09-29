import { Client, Command } from "../../../../shared";

export interface UpdateClientCommand extends Command {
    client: Client
}