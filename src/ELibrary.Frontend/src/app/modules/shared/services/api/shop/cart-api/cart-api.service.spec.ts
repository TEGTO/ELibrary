/* eslint-disable @typescript-eslint/no-explicit-any */
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AddBookToCartRequest, Cart, CartBookResponse, DeleteCartBookFromCartRequest, UpdateCartBookRequest, URLDefiner } from '../../../..';
import { CartApiService } from './cart-api.service';

describe('CartApiService', () => {
  let service: CartApiService;
  let httpTestingController: HttpTestingController;
  let mockUrlDefiner: jasmine.SpyObj<URLDefiner>;

  beforeEach(() => {
    mockUrlDefiner = jasmine.createSpyObj('URLDefiner', ['combineWithShopApiUrl']);
    mockUrlDefiner.combineWithShopApiUrl.and.callFake((subpath: string) => `/api${subpath}`);

    TestBed.configureTestingModule({
      providers: [
        CartApiService,
        { provide: URLDefiner, useValue: mockUrlDefiner },
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(CartApiService);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getCart should return a cart', () => {
    const expectedUrl = `/api/cart`;
    const mockResponse: Cart = { books: [{ id: '1', bookId: 1, bookAmount: 2, book: {} as any }] };

    service.getCart().subscribe(response => {
      expect(response.books.length).toBe(1);
      expect(response.books[0].id).toBe('1');
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('getInCartCart should return the amount of items in cart', () => {
    const expectedUrl = `/api/cart/amount`;
    const mockResponse = 3;

    service.getInCartCart().subscribe(response => {
      expect(response).toBe(3);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('addBookToCart should post a new cart book', () => {
    const request: AddBookToCartRequest = { bookId: 1, bookAmount: 2 };
    const expectedUrl = `/api/cart/cartbook`;
    const mockResponse: CartBookResponse = { id: '1', bookId: 1, bookAmount: 2, book: {} as any };

    service.addBookToCart(request).subscribe(response => {
      expect(response.id).toBe('1');
      expect(response.bookId).toBe(1);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('updateCartBookInCart should update a cart book', () => {
    const request: UpdateCartBookRequest = { id: '1', bookAmount: 3 };
    const expectedUrl = `/api/cart/cartbook`;
    const mockResponse: CartBookResponse = { id: '1', bookId: 1, bookAmount: 3, book: {} as any };

    service.updateCartBookInCart(request).subscribe(response => {
      expect(response.bookAmount).toBe(3);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('PUT');
    req.flush(mockResponse);
  });

  it('deleteBooksFromCart should delete books and return an updated cart', () => {
    const requests: DeleteCartBookFromCartRequest[] = [{ id: 1 }];
    const expectedUrl = `/api/cart`;
    const mockResponse: Cart = { books: [] };

    service.deleteBooksFromCart(requests).subscribe(response => {
      expect(response.books.length).toBe(0);
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('PUT');
    req.flush(mockResponse);
  });

  it('should handle error response', () => {
    const expectedUrl = `/api/cart/amount`;

    service.getInCartCart().subscribe({
      next: () => fail('Expected an error, but got a response'),
      error: (error) => {
        expect(error).toBeTruthy();
      }
    });

    const req = httpTestingController.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush({ message: 'Internal Server Error' }, { status: 500, statusText: 'Server Error' });
  });
});