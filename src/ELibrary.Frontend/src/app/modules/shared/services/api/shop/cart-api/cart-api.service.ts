import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { AddBookToCartRequest, BaseApiService, BookListingResponse, Cart, CartBook, CartResponse, mapBookListingResponseToCartBook, mapCartResponseToCart, UpdateCartBookRequest } from '../../../..';

@Injectable({
  providedIn: 'root'
})
export class CartApiService extends BaseApiService {

  getCart(): Observable<Cart> {
    return this.httpClient.get<CartResponse>(this.combinePathWithCartApiUrl(``)).pipe(
      map((response) => mapCartResponseToCart(response)),
      catchError((error) => this.handleError(error))
    );
  }
  getInCartCart(): Observable<number> {
    return this.httpClient.get<number>(this.combinePathWithCartApiUrl(`/amount`)).pipe(
      catchError((error) => this.handleError(error)),
    );
  }
  addBookToCart(request: AddBookToCartRequest): Observable<CartBook> {
    return this.httpClient.post<BookListingResponse>(this.combinePathWithCartApiUrl(`/cartbook`), request).pipe(
      map((response) => mapBookListingResponseToCartBook(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  updateCartBookInCart(request: UpdateCartBookRequest): Observable<CartBook> {
    return this.httpClient.put<BookListingResponse>(this.combinePathWithCartApiUrl(`/cartbook`), request).pipe(
      map((response) => mapBookListingResponseToCartBook(response)),
      catchError((error) => this.handleError(error)),
    );
  }
  deleteCartBookFromCart(id: string): Observable<HttpResponse<void>> {
    return this.httpClient.delete<void>(this.combinePathWithCartApiUrl(`/cartbook/${id}`), { observe: 'response' }).pipe(
      catchError((error) => this.handleError(error)),
    );
  }
  clearCart(): Observable<Cart> {
    return this.httpClient.put<CartResponse>(this.combinePathWithCartApiUrl(`/cartbook`), null).pipe(
      map((response) => mapCartResponseToCart(response)),
      catchError((error) => this.handleError(error)),
    );
  }

  private combinePathWithCartApiUrl(subpath: string): string {
    return this.urlDefiner.combineWithShopApiUrl("/cart" + subpath);
  }

}
