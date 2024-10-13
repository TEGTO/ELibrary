import { Book, Command } from "../../../../shared";

export interface CartAddBookCommand extends Command {
    book: Book
}