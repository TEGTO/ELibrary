import { Book, Command } from "../../../../shared";

export interface DeleteBookCommand extends Command {
    book: Book;
}