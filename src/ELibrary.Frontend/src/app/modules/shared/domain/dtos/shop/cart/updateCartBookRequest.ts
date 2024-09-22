import { CartBook } from "../../../models/shop/cart/cartBook";

export interface UpdateCartBookRequest {
    id: string;
    bookAmount: number;
}
export function mapCartBookToUpdateCartBookRequest(cartBook: CartBook): UpdateCartBookRequest {
    return {
        id: cartBook.id,
        bookAmount: cartBook.bookAmount
    }
}