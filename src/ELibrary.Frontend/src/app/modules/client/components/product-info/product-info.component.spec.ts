/* eslint-disable @typescript-eslint/no-explicit-any */
import { NgOptimizedImage } from "@angular/common";
import { ComponentFixture, fakeAsync, TestBed } from "@angular/core/testing";
import { By } from "@angular/platform-browser";
import { ActivatedRoute, convertToParamMap, provideRouter } from "@angular/router";
import { BehaviorSubject, of } from "rxjs";
import { BookFallbackCoverPipe, BookService } from "../../../library";
import { Book, CommandHandler, CurrencyPipeApplier, getDefaultBook, PlaceholderPipe, RouteReader } from "../../../shared";
import { CART_ADD_BOOK_COMMAND_HANDLER, CartAddBookCommand } from "../../../shop";
import { ProductInfoComponent } from "./product-info.component";

describe('ProductInfoComponent', () => {
    let component: ProductInfoComponent;
    let fixture: ComponentFixture<ProductInfoComponent>;
    let mockCurrencyPipeApplier: jasmine.SpyObj<CurrencyPipeApplier>;
    let mockAddBookToCartHandler: jasmine.SpyObj<CommandHandler<CartAddBookCommand>>;
    let mockRouteReader: jasmine.SpyObj<RouteReader>;
    let activatedRouteStub: BehaviorSubject<any>;
    let mockBookService: jasmine.SpyObj<BookService>;

    const mockBook: Book = {
        ...getDefaultBook(),
        id: 1,
        name: "name",
        stockAmount: 10,
    };

    beforeEach(async () => {
        mockBookService = jasmine.createSpyObj<BookService>(['getById']);
        const currencyPipeApplierSpy = jasmine.createSpyObj('CurrencyPipeApplier', ['applyCurrencyPipe']);
        const addBookToCartHandlerSpy = jasmine.createSpyObj('CommandHandler', ['dispatch']);
        const routeReaderSpy = jasmine.createSpyObj<RouteReader>('RouteReader', ['readIdInt']);
        const fallbackImagePipeSpy = jasmine.createSpyObj<BookFallbackCoverPipe>(['transform']);

        fallbackImagePipeSpy.transform.and.returnValue("someurl");

        activatedRouteStub = new BehaviorSubject(convertToParamMap({ id: '1' }));
        routeReaderSpy.readIdInt.and.returnValue(of(mockBook));

        await TestBed.configureTestingModule({
            imports: [PlaceholderPipe, NgOptimizedImage],
            declarations: [ProductInfoComponent],
            providers: [
                { provide: BookService, useValue: mockBookService },
                { provide: CurrencyPipeApplier, useValue: currencyPipeApplierSpy },
                { provide: RouteReader, useValue: routeReaderSpy },
                { provide: CART_ADD_BOOK_COMMAND_HANDLER, useValue: addBookToCartHandlerSpy },
                { provide: BookFallbackCoverPipe, useValue: fallbackImagePipeSpy },
                provideRouter([]),
                {
                    provide: ActivatedRoute,
                    useValue: { paramMap: activatedRouteStub.asObservable() }
                }
            ],
        }).compileComponents();

        fixture = TestBed.createComponent(ProductInfoComponent);
        component = fixture.componentInstance;
        mockRouteReader = TestBed.inject(RouteReader) as jasmine.SpyObj<RouteReader>;
        mockCurrencyPipeApplier = TestBed.inject(CurrencyPipeApplier) as jasmine.SpyObj<CurrencyPipeApplier>;
        mockAddBookToCartHandler = TestBed.inject(CART_ADD_BOOK_COMMAND_HANDLER) as jasmine.SpyObj<CommandHandler<CartAddBookCommand>>;
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should fetch a book by ID from the route parameters', () => {
        fixture.detectChanges();

        component.book$.subscribe(book => {
            expect(book).toEqual(mockBook);
            expect(mockRouteReader.readIdInt).toHaveBeenCalled();
        });
    });

    it('should redirect to home if the book ID is not valid', fakeAsync(() => {
        fixture.detectChanges();
        component.ngOnInit();
        activatedRouteStub.next(convertToParamMap({ id: 'invalid' }));
        fixture.detectChanges();

        expect(mockRouteReader.readIdInt).toHaveBeenCalled();
    }));

    it('should check if the book is in stock', () => {
        const mockBook: Book = { ...getDefaultBook(), stockAmount: 10 };

        expect(component.checkIfInStock(mockBook)).toBeTrue();

        mockBook.stockAmount = 0;
        expect(component.checkIfInStock(mockBook)).toBeFalse();
    });

    it('should add a book to the cart', () => {
        component.addBookToCart(mockBook);

        expect(component.bookAdded).toBeTrue();
        expect(mockAddBookToCartHandler.dispatch).toHaveBeenCalledWith({
            book: mockBook
        });
    });

    it('should apply the currency pipe to the book price', () => {
        mockCurrencyPipeApplier.applyCurrencyPipe.and.returnValue('$100.00');

        const formattedPrice = component.applyCurrencyPipe(mockBook.price);
        expect(formattedPrice).toBe('$100.00');
        expect(mockCurrencyPipeApplier.applyCurrencyPipe).toHaveBeenCalledWith(mockBook.price);
    });

    it('should render the book details', () => {
        fixture.detectChanges();

        const title = fixture.debugElement.query(By.css('.book__detail-value span')).nativeElement;
        expect(title.textContent).toContain(mockBook.name);
    });

    it('should display "Out of stock" when the book is not available', () => {
        mockBook.stockAmount = 0;
        fixture.detectChanges();

        const stockStatus = fixture.debugElement.query(By.css('.text-red-600')).nativeElement;
        expect(stockStatus.textContent).toContain('Out of stock');
    });

    it('should display "In stock" when the book is available', () => {
        mockBook.stockAmount = 5;
        fixture.detectChanges();

        const stockStatus = fixture.debugElement.query(By.css('.text-green-600')).nativeElement;
        expect(stockStatus.textContent).toContain('In stock');
    });
});
