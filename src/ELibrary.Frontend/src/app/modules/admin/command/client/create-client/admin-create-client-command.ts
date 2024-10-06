import { Command, CreateClientRequest } from "../../../../shared";

export interface AdminCreateClientCommand extends Command {
    userId: string;
    createRequest: CreateClientRequest;
}