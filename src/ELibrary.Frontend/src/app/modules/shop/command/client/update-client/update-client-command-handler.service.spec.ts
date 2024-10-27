import { TestBed } from '@angular/core/testing';
import { ClientService, UpdateClientCommand } from '../../..';
import { Client, getDefaultClient, mapClientToUpdateClientRequest } from '../../../../shared';
import { UpdateClientCommandHandlerService } from './update-client-command-handler.service';

describe('UpdateClientCommandHandlerService', () => {
    let service: UpdateClientCommandHandlerService;
    let clientServiceSpy: jasmine.SpyObj<ClientService>;

    beforeEach(() => {
        clientServiceSpy = jasmine.createSpyObj('ClientService', ['updateClient']);
        TestBed.configureTestingModule({
            providers: [
                UpdateClientCommandHandlerService,
                { provide: ClientService, useValue: clientServiceSpy }
            ]
        });

        service = TestBed.inject(UpdateClientCommandHandlerService);
    });

    it('should create the service', () => {
        expect(service).toBeTruthy();
    });

    it('should dispatch update client command and call updateClient with mapped request', () => {
        const client: Client = getDefaultClient();
        const command: UpdateClientCommand = { client };
        const mappedRequest = mapClientToUpdateClientRequest(client);

        service.dispatch(command);

        expect(clientServiceSpy.updateClient).toHaveBeenCalledWith(mappedRequest);
    });
});