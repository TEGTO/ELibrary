import { CartBook, getDefaultCartBook } from "../../../..";

export interface Cart {
    books: CartBook[]
}

export function getDefaultCart(): Cart {
    return {
        books: [getDefaultCartBook()]
    }
}