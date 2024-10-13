import { CartBook, OrderBook } from "../../../..";

export interface DeleteCartBookFromCartRequest {
    id: number;
}

export function mapOrderBookToDeleteCartBookFromCartRequest(order: OrderBook): DeleteCartBookFromCartRequest {
    return {
        id: order.bookId
    }
}
export function mapCartBookToDeleteCartBookFromCartRequest(cartBook: CartBook): DeleteCartBookFromCartRequest {
    return {
        id: cartBook.bookId
    }
}