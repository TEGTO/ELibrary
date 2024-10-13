import { Book, Command } from "../../../../shared";

export interface UpdateBookCommand extends Command {
    book: Book;
}