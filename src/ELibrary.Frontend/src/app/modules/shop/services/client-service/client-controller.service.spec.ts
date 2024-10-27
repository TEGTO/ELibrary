import { TestBed } from "@angular/core/testing";
import { Store } from "@ngrx/store";
import { provideMockStore } from "@ngrx/store/testing";
import { of } from "rxjs";
import { ClientControllerService, createClient, getClient, updateClient } from "../..";
import { Client, CreateClientRequest, getDefaultClient, getDefaultCreateClientRequest, getDefaultUpdateClientRequest, UpdateClientRequest } from "../../../shared";

describe('ClientControllerService', () => {
    let service: ClientControllerService;
    let store: Store;

    const mockClient: Client = getDefaultClient();

    const initialState = {
        client: mockClient
    };

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                ClientControllerService,
                provideMockStore({ initialState }),
            ],
        });

        service = TestBed.inject(ClientControllerService);
        store = TestBed.inject(Store);
        spyOn(store, 'dispatch').and.callThrough();
    });

    describe('getClient', () => {
        it('should dispatch getClient action and return client from the store', (done) => {
            const mockSelectClient = of(mockClient);
            spyOn(store, 'select').and.returnValue(mockSelectClient);

            service.getClient().subscribe(client => {
                expect(client).toEqual(mockClient);
                expect(store.dispatch).toHaveBeenCalledWith(getClient());
                done();
            });
        });
    });

    describe('createClient', () => {
        it('should dispatch createClient action with correct request', () => {
            const req: CreateClientRequest = getDefaultCreateClientRequest();

            service.createClient(req);
            expect(store.dispatch).toHaveBeenCalledWith(createClient({ req }));
        });
    });

    describe('updateClient', () => {
        it('should dispatch updateClient action with correct request', () => {
            const req: UpdateClientRequest = getDefaultUpdateClientRequest();

            service.updateClient(req);
            expect(store.dispatch).toHaveBeenCalledWith(updateClient({ req }));
        });
    });
});