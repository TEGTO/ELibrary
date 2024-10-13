import { Author, Command } from "../../../../shared";

export interface DeleteAuthorCommand extends Command {
    author: Author;
}