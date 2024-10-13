import { Command } from "../../../../shared";

export interface AdminCreateClientCommand extends Command {
    userId: string;
}