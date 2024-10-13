import { BookListingResponse, Cart, mapBookListingResponseToCartBook } from "../../../..";

export interface CartResponse {
    books: BookListingResponse[]
}

export function mapCartResponseToCart(response: CartResponse): Cart {
    return {
        books: response.books.map(x => mapBookListingResponseToCartBook(x))
    }
}