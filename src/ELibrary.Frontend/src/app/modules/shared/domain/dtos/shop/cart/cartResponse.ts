import { Cart, CartBookResponse, mapCartBookResponseToCartBook } from "../../../..";

export interface CartResponse {
    books: CartBookResponse[]
}

export function mapCartResponseToCart(response: CartResponse): Cart {
    return {
        books: response.books.map(x => mapCartBookResponseToCartBook(x))
    }
}