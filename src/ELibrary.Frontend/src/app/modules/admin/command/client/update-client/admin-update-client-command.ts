import { Command, UpdateClientRequest } from "../../../../shared";

export interface AdminUpdateClientCommand extends Command {
    userId: string;
    updateRequest: UpdateClientRequest;
}