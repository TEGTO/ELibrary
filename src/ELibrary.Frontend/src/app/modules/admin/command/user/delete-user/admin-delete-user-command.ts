import { Command } from "../../../../shared";

export interface AdminDeleteUserCommand extends Command {
    userId: string
}