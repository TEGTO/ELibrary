/* eslint-disable @typescript-eslint/no-explicit-any */
import { CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { ComponentFixture, TestBed } from "@angular/core/testing";
import { By } from "@angular/platform-browser";
import { ActivatedRoute, convertToParamMap, provideRouter } from "@angular/router";
import { BehaviorSubject, of, throwError } from "rxjs";
import { CurrencyPipeApplier, getDefaultStockBookOrder, RedirectorService, StockBookOrder, StockBookOrderType, stockBookOrderTypeToString } from "../../../../shared";
import { BookstockOrderService } from "../../../../shop";
import { BookStockDetailsComponent } from "./book-stock-details.component";

describe('BookStockDetailsComponent', () => {
    let component: BookStockDetailsComponent;
    let fixture: ComponentFixture<BookStockDetailsComponent>;
    let stockOrderServiceSpy: jasmine.SpyObj<BookstockOrderService>;
    let redirectorServiceSpy: jasmine.SpyObj<RedirectorService>;
    let currencyPipeApplierSpy: jasmine.SpyObj<CurrencyPipeApplier>;
    let activatedRouteStub: BehaviorSubject<any>;

    const mockStockBookOrder: StockBookOrder = getDefaultStockBookOrder();

    beforeEach(async () => {
        const stockOrderServiceSpyObj = jasmine.createSpyObj('BookstockOrderService', ['getById']);
        const redirectorServiceSpyObj = jasmine.createSpyObj('RedirectorService', ['redirectToHome']);
        const currencyPipeApplierSpyObj = jasmine.createSpyObj('CurrencyPipeApplier', ['applyCurrencyPipe']);

        activatedRouteStub = new BehaviorSubject(convertToParamMap({ id: '1' }));

        await TestBed.configureTestingModule({
            declarations: [BookStockDetailsComponent],
            providers: [
                { provide: BookstockOrderService, useValue: stockOrderServiceSpyObj },
                { provide: RedirectorService, useValue: redirectorServiceSpyObj },
                { provide: CurrencyPipeApplier, useValue: currencyPipeApplierSpyObj },
                { provide: ActivatedRoute, useValue: activatedRouteStub },
                provideRouter([]),
                {
                    provide: ActivatedRoute,
                    useValue: { paramMap: activatedRouteStub.asObservable() }
                }
            ],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
        }).compileComponents();

        fixture = TestBed.createComponent(BookStockDetailsComponent);
        component = fixture.componentInstance;
        stockOrderServiceSpy = TestBed.inject(BookstockOrderService) as jasmine.SpyObj<BookstockOrderService>;
        redirectorServiceSpy = TestBed.inject(RedirectorService) as jasmine.SpyObj<RedirectorService>;
        currencyPipeApplierSpy = TestBed.inject(CurrencyPipeApplier) as jasmine.SpyObj<CurrencyPipeApplier>;
    });

    beforeEach(() => {
        stockOrderServiceSpy.getById.and.returnValue(of(mockStockBookOrder));
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should redirect to home if no id is provided in the route', () => {
        activatedRouteStub.next(convertToParamMap({}));
        component.ngOnInit();
        fixture.detectChanges();
        expect(redirectorServiceSpy.redirectToHome).toHaveBeenCalled();
    });

    it('should redirect to home if ID is invalid', () => {
        activatedRouteStub.next(convertToParamMap({ id: 'invalid' }));
        component.ngOnInit();
        fixture.detectChanges();
        expect(redirectorServiceSpy.redirectToHome).toHaveBeenCalled();
    });

    it('should call stockOrderService.getById when valid ID is provided', () => {
        expect(stockOrderServiceSpy.getById).toHaveBeenCalledWith(1);
    });

    it('should handle stockBookOrder correctly and display the data', () => {
        component.ngOnInit();
        fixture.detectChanges();

        const details = fixture.debugElement.query(By.css('.order__detail-value'));
        expect(details.nativeElement.textContent).toContain(`#${mockStockBookOrder.id}`);
    });

    it('should handle service error and redirect to home', () => {
        stockOrderServiceSpy.getById.and.returnValue(throwError(() => new Error('Service error')));
        component.ngOnInit();
        fixture.detectChanges();
        expect(redirectorServiceSpy.redirectToHome).toHaveBeenCalled();
    });

    it('should return the correct string for StockBookOrderType', () => {
        const typeString = component.getStringOrderType(StockBookOrderType.ClientOrder);
        expect(typeString).toBe(stockBookOrderTypeToString(StockBookOrderType.ClientOrder));
    });

    it('should apply the currency pipe correctly', () => {
        currencyPipeApplierSpy.applyCurrencyPipe.and.returnValue('$20.00');
        const result = component.applyCurrencyPipe(20);
        expect(result).toBe('$20.00');
        expect(currencyPipeApplierSpy.applyCurrencyPipe).toHaveBeenCalledWith(20);
    });

    it('should calculate the correct selection size for books', () => {
        const result = component.calculateSelectionSize(5);
        expect(result).toBe(420);
    });

    it('should track book changes by ID', () => {
        const mockStockBookChange = mockStockBookOrder.stockBookChanges[0];
        const trackedId = component.trackById(0, mockStockBookChange);
        expect(trackedId).toBe(mockStockBookChange.id);
    });
});
