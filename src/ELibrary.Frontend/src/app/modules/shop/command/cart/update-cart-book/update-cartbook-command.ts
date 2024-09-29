import { CartBook, Command } from "../../../../shared";

export interface UpdateCartBookCommand extends Command {
    cartBook: CartBook
}