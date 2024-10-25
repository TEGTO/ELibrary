/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from "@angular/core/testing";
import { of } from "rxjs";
import { ManagerUpdateOrderCommand, ManagerUpdateOrderCommandHandlerService, OrderService, ShopDialogManager } from "../../..";
import { getDefaultOrder, mapOrderToManagerUpdateOrderRequest } from "../../../../shared";

describe('ManagerUpdateOrderCommandHandlerService', () => {
    let service: ManagerUpdateOrderCommandHandlerService;
    let orderServiceMock: jasmine.SpyObj<OrderService>;
    let dialogManagerMock: jasmine.SpyObj<ShopDialogManager>;

    beforeEach(() => {
        orderServiceMock = jasmine.createSpyObj('OrderService', ['managerUpdateOrder']);
        dialogManagerMock = jasmine.createSpyObj('ShopDialogManager', ['openConfirmMenu']);

        TestBed.configureTestingModule({
            providers: [
                ManagerUpdateOrderCommandHandlerService,
                { provide: OrderService, useValue: orderServiceMock },
                { provide: ShopDialogManager, useValue: dialogManagerMock }
            ]
        });

        service = TestBed.inject(ManagerUpdateOrderCommandHandlerService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should call managerUpdateOrder when user confirms', () => {
        const mockOrder = getDefaultOrder();
        const command: ManagerUpdateOrderCommand = { order: mockOrder };
        const expectedRequest = mapOrderToManagerUpdateOrderRequest(mockOrder);

        dialogManagerMock.openConfirmMenu.and.returnValue({
            afterClosed: () => of(true)
        } as any);

        service.dispatch(command);

        expect(dialogManagerMock.openConfirmMenu).toHaveBeenCalled();
        expect(orderServiceMock.managerUpdateOrder).toHaveBeenCalledWith(expectedRequest);
    });

    it('should not call managerUpdateOrder if user cancels', () => {
        const command: ManagerUpdateOrderCommand = { order: getDefaultOrder() };

        dialogManagerMock.openConfirmMenu.and.returnValue({
            afterClosed: () => of(false)
        } as any);

        service.dispatch(command);

        expect(dialogManagerMock.openConfirmMenu).toHaveBeenCalled();
        expect(orderServiceMock.managerUpdateOrder).not.toHaveBeenCalled();
    });
});