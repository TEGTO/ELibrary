import { TestBed } from "@angular/core/testing";
import { MatDialogRef } from "@angular/material/dialog";
import { of } from "rxjs";
import { AddClientCommand, ClientService, ShopDialogManager } from "../../..";
import { getDefaultClient, mapClientToCreateClientRequest } from "../../../../shared";
import { AddClientCommandHandlerService } from "./add-client-command-handler.service";

describe('AddClientCommandHandlerService', () => {
    let service: AddClientCommandHandlerService;
    let clientServiceMock: jasmine.SpyObj<ClientService>;
    let shopDialogMock: jasmine.SpyObj<ShopDialogManager>;

    beforeEach(() => {
        clientServiceMock = jasmine.createSpyObj('ClientService', ['createClient']);
        shopDialogMock = jasmine.createSpyObj('ShopDialogManager', ['openClientChangeMenu']);

        TestBed.configureTestingModule({
            providers: [
                AddClientCommandHandlerService,
                { provide: ClientService, useValue: clientServiceMock },
                { provide: ShopDialogManager, useValue: shopDialogMock }
            ]
        });

        service = TestBed.inject(AddClientCommandHandlerService);
    });

    it('should create the service', () => {
        expect(service).toBeTruthy();
    });

    it('should call createClient when a client is returned from the dialog', () => {
        const mockClient = getDefaultClient();
        const expectedRequest = mapClientToCreateClientRequest(mockClient);

        const mockDialogRef = {
            afterClosed: () => of(mockClient),
        } as MatDialogRef<never>;
        shopDialogMock.openClientChangeMenu.and.returnValue(mockDialogRef);

        const command: AddClientCommand = {};
        service.dispatch(command);

        expect(shopDialogMock.openClientChangeMenu).toHaveBeenCalled();
        expect(clientServiceMock.createClient).toHaveBeenCalledWith(expectedRequest);
    });

    it('should not call createClient if no client is returned from the dialog', () => {
        const mockDialogRef = {
            afterClosed: () => of(null),
        } as MatDialogRef<never>;
        shopDialogMock.openClientChangeMenu.and.returnValue(mockDialogRef);

        const command: AddClientCommand = {};
        service.dispatch(command);

        expect(shopDialogMock.openClientChangeMenu).toHaveBeenCalledWith(getDefaultClient());
        expect(clientServiceMock.createClient).not.toHaveBeenCalled();
    });
});