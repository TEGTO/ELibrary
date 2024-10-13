import { Author, Command } from "../../../../shared";

export interface UpdateAuthorCommand extends Command {
    author: Author;
}