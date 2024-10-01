/* eslint-disable @typescript-eslint/no-explicit-any */
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { BookService } from '../../../library';
import { Book, CommandHandler, CurrencyPipeApplier, getDefaultBook } from '../../../shared';
import { CART_ADD_BOOK_COMMAND_HANDLER } from '../../../shop';
import { ProductPageComponent } from './product-page.component';

describe('ProductPageComponent', () => {
    let component: ProductPageComponent;
    let fixture: ComponentFixture<ProductPageComponent>;
    let bookServiceSpy: jasmine.SpyObj<BookService>;
    let addBookToCartHandlerSpy: jasmine.SpyObj<CommandHandler<any>>;

    const mockBooks: Book[] = [

        { ...getDefaultBook(), id: 1 },
        { ...getDefaultBook(), id: 2 },
    ];

    beforeEach(async () => {
        const bookServiceSpyObj = jasmine.createSpyObj('BookService', ['getPaginated', 'getItemTotalAmount']);
        const addBookToCartHandlerSpyObj = jasmine.createSpyObj('CommandHandler', ['dispatch']);
        const currencyPipeApplierSpy = jasmine.createSpyObj('CurrencyPipeApplier', ['applyCurrencyPipe']);

        await TestBed.configureTestingModule({
            declarations: [ProductPageComponent],
            imports: [
                MatPaginatorModule,
                MatDialogModule,
                BrowserAnimationsModule
            ],
            providers: [
                { provide: BookService, useValue: bookServiceSpyObj },
                { provide: CurrencyPipeApplier, useValue: currencyPipeApplierSpy },
                { provide: CART_ADD_BOOK_COMMAND_HANDLER, useValue: addBookToCartHandlerSpyObj },
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
        }).compileComponents();

        fixture = TestBed.createComponent(ProductPageComponent);
        component = fixture.componentInstance;
        bookServiceSpy = TestBed.inject(BookService) as jasmine.SpyObj<BookService>;
        addBookToCartHandlerSpy = TestBed.inject(CART_ADD_BOOK_COMMAND_HANDLER) as jasmine.SpyObj<CommandHandler<any>>;
    });

    beforeEach(() => {
        bookServiceSpy.getPaginated.and.returnValue(of(mockBooks));
        bookServiceSpy.getItemTotalAmount.and.returnValue(of(2));
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should fetch paginated items on initialization', () => {
        expect(bookServiceSpy.getPaginated).toHaveBeenCalled();
    });

    it('should fetch total amount on initialization', () => {
        expect(bookServiceSpy.getItemTotalAmount).toHaveBeenCalled();
    });

    it('should add a book to the cart', () => {
        const book = mockBooks[0];
        component.addBookToCart(book);
        expect(addBookToCartHandlerSpy.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({ book }));
        expect(component.bookAddedMap[book.id]).toBeTrue();
    });

    it('should call fetchPaginatedItems when page changes', () => {
        spyOn<any>(component, 'fetchPaginatedItems');
        const pageEvent: PageEvent = { pageIndex: 1, pageSize: 8, length: 16 };
        component.onPageChange(pageEvent);

        expect(component["fetchPaginatedItems"]).toHaveBeenCalledWith({ pageIndex: 2, pageSize: 8 });
    });

    it('should reset pagination when filter changes', () => {
        spyOn<any>(component, 'resetPagination');
        spyOn<any>(component, 'fetchTotalAmount');
        spyOn<any>(component, 'fetchPaginatedItems');

        const filterReq = { ...component['filterReq'], pageNumber: 1 };
        component.filterChange(filterReq);

        expect(component.resetPagination).toHaveBeenCalled();
        expect(component["fetchTotalAmount"]).toHaveBeenCalled();
        expect(component["fetchPaginatedItems"]).toHaveBeenCalledWith(component['defaultPagination']);
    });
});
