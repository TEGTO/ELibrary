/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { MatDialogRef } from '@angular/material/dialog';
import { of } from 'rxjs';
import { ADD_CLIENT_COMMAND_HANDLER, AddBookStockOrderCommand, AddClientCommand, BookstockOrderService, ClientService, ShopDialogManager } from '../../..';
import { Client, CommandHandler, CreateStockBookOrderRequest, StockBookChange, StockBookOrderType, getDefaultClient, getDefaultStockBookChange, mapStockBookChangeToStockBookChangeRequest } from '../../../../shared';
import { AddBookStockCommandHandlerService } from './add-bookstock-command-handler.service';

describe('AddBookStockCommandHandlerService', () => {
    let service: AddBookStockCommandHandlerService;
    let mockClientService: jasmine.SpyObj<ClientService>;
    let mockStockbookService: jasmine.SpyObj<BookstockOrderService>;
    let mockDialogManager: jasmine.SpyObj<ShopDialogManager>;
    let mockAddClientHandler: jasmine.SpyObj<CommandHandler<AddClientCommand>>;
    let mockDialogRef: jasmine.SpyObj<MatDialogRef<any>>;

    beforeEach(() => {
        mockClientService = jasmine.createSpyObj('ClientService', ['getClient']);
        mockStockbookService = jasmine.createSpyObj('BookstockOrderService', ['createStockOrder']);
        mockDialogManager = jasmine.createSpyObj('ShopDialogManager', ['openReplenishmentMenu']);
        mockAddClientHandler = jasmine.createSpyObj('CommandHandler', ['dispatch']);
        mockDialogRef = jasmine.createSpyObj('MatDialogRef', ['afterClosed']);

        TestBed.configureTestingModule({
            providers: [
                AddBookStockCommandHandlerService,
                { provide: ClientService, useValue: mockClientService },
                { provide: BookstockOrderService, useValue: mockStockbookService },
                { provide: ShopDialogManager, useValue: mockDialogManager },
                { provide: ADD_CLIENT_COMMAND_HANDLER, useValue: mockAddClientHandler }
            ]
        });

        service = TestBed.inject(AddBookStockCommandHandlerService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('should open replenishment menu when client is available', () => {
        const client: Client = getDefaultClient();
        const changes: StockBookChange[] = [getDefaultStockBookChange()];
        const expectedRequest: CreateStockBookOrderRequest = {
            type: StockBookOrderType.StockReplenishment,
            clientId: client.id,
            stockBookChanges: changes.map(x => mapStockBookChangeToStockBookChangeRequest(x))
        };

        mockClientService.getClient.and.returnValue(of(client));
        mockDialogRef.afterClosed.and.returnValue(of(changes));
        mockDialogManager.openReplenishmentMenu.and.returnValue(mockDialogRef);

        const command: AddBookStockOrderCommand = {};
        service.dispatch(command);

        expect(mockDialogManager.openReplenishmentMenu).toHaveBeenCalled();
        expect(mockStockbookService.createStockOrder).toHaveBeenCalledWith(expectedRequest);
    });

    it('should dispatch add client command when client is not available', () => {
        mockClientService.getClient.and.returnValue(of(null));

        const command: AddBookStockOrderCommand = {};
        service.dispatch(command);

        expect(mockAddClientHandler.dispatch).toHaveBeenCalledWith(jasmine.objectContaining({ redirectAfter: undefined }));
    });

    it('should not create stock order when no changes are returned from dialog', () => {
        const client: Client = getDefaultClient();

        mockClientService.getClient.and.returnValue(of(client));
        mockDialogRef.afterClosed.and.returnValue(of(null));
        mockDialogManager.openReplenishmentMenu.and.returnValue(mockDialogRef);

        const command: AddBookStockOrderCommand = {};
        service.dispatch(command);

        expect(mockStockbookService.createStockOrder).not.toHaveBeenCalled();
    });

    it('should handle error cases gracefully', () => {
        const client: Client = getDefaultClient();
        mockClientService.getClient.and.returnValue(of(client));
        mockDialogManager.openReplenishmentMenu.and.returnValue(mockDialogRef);
        mockDialogRef.afterClosed.and.returnValue(of(undefined));

        const command: AddBookStockOrderCommand = {};
        service.dispatch(command);

        expect(mockStockbookService.createStockOrder).not.toHaveBeenCalled();
    });
});
