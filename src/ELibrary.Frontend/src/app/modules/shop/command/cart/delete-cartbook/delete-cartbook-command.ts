import { CartBook, Command } from "../../../../shared";

export interface DeleteCartBookCommand extends Command {
    cartBook: CartBook
}