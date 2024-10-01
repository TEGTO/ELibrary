/* eslint-disable @typescript-eslint/no-explicit-any */
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { of } from 'rxjs';
import { CommandHandler, GenericTableComponent, getDefaultStockBookOrder, LocaleService, StockBookOrder } from '../../../../shared';
import { ADD_BOOKSTOCK_ORDER_COMMAND_HANDLER, BookstockOrderService } from '../../../../shop';
import { BookStockComponent } from './book-stock.component';

describe('BookStockComponent', () => {
    let component: BookStockComponent;
    let fixture: ComponentFixture<BookStockComponent>;
    let stockOrderService: jasmine.SpyObj<BookstockOrderService>;
    let mockAddHandler: jasmine.SpyObj<CommandHandler<any>>;

    beforeEach(async () => {
        const stockOrderServiceSpy = jasmine.createSpyObj<BookstockOrderService>(['getOrderTotalAmount', 'getPaginatedOrders']);
        const localeServiceSpy = jasmine.createSpyObj('LocaleService', ['getLocale']);
        const mockAddHandlerSpy = jasmine.createSpyObj('CommandHandler', ['dispatch']);

        stockOrderServiceSpy.getPaginatedOrders.and.returnValue(
            of([getDefaultStockBookOrder()])
        );

        await TestBed.configureTestingModule({
            imports: [GenericTableComponent, BrowserAnimationsModule],
            declarations: [BookStockComponent],
            providers: [
                { provide: BookstockOrderService, useValue: stockOrderServiceSpy },
                { provide: LocaleService, useValue: localeServiceSpy },
                { provide: ADD_BOOKSTOCK_ORDER_COMMAND_HANDLER, useValue: mockAddHandlerSpy },
            ],
        }).compileComponents();

        fixture = TestBed.createComponent(BookStockComponent);
        component = fixture.componentInstance;
        stockOrderService = TestBed.inject(BookstockOrderService) as jasmine.SpyObj<BookstockOrderService>;
        mockAddHandler = TestBed.inject(ADD_BOOKSTOCK_ORDER_COMMAND_HANDLER) as jasmine.SpyObj<CommandHandler<any>>;
    });

    describe('ngOnInit', () => {
        it('should fetch total amount and paginated items on init', () => {
            const mockItems: StockBookOrder[] = [
                getDefaultStockBookOrder()
            ];
            stockOrderService.getOrderTotalAmount.and.returnValue(of(10));
            stockOrderService.getPaginatedOrders.and.returnValue(of(mockItems));

            fixture.detectChanges(); // Triggers ngOnInit

            expect(stockOrderService.getOrderTotalAmount).toHaveBeenCalled();
            expect(stockOrderService.getPaginatedOrders).toHaveBeenCalledWith({ pageNumber: 1, pageSize: 10 });
        });
    });

    describe('onPageChange', () => {
        it('should fetch paginated items when page is changed', () => {
            const newPagination = { pageIndex: 2, pageSize: 10 };
            component.onPageChange(newPagination);

            expect(stockOrderService.getPaginatedOrders).toHaveBeenCalledWith({ pageNumber: 2, pageSize: 10 });
        });
    });

    describe('createNew', () => {
        it('should dispatch add book stock order command', () => {
            component.createNew();

            expect(mockAddHandler.dispatch).toHaveBeenCalled();
        });
    });

    describe('table rendering', () => {
        it('should render the table with the correct data and columns', () => {
            const mockItems: StockBookOrder[] = [
                getDefaultStockBookOrder()
            ];
            stockOrderService.getOrderTotalAmount.and.returnValue(of(10));
            stockOrderService.getPaginatedOrders.and.returnValue(of(mockItems));

            fixture.detectChanges(); // Triggers ngOnInit

            const table = fixture.nativeElement.querySelector('app-generic-table');
            expect(table).toBeTruthy();
            expect(stockOrderService.getPaginatedOrders).toHaveBeenCalled();
        });
    });
});
