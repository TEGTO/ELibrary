import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AddBookToCartRequest, CartBook, UpdateCartBookRequest } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class CartService {
    abstract getCartBooks(): Observable<CartBook[]>;
    abstract getInCartAmount(): Observable<number>;
    abstract addBookToCart(req: AddBookToCartRequest): void;
    abstract updateCartBook(req: UpdateCartBookRequest): void;
    abstract deleteCartBook(id: string): void;
    abstract clearCart(): void;
}