/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from "@angular/core/testing";
import { provideMockActions } from "@ngrx/effects/testing";
import { Observable, of, throwError } from "rxjs";
import { ClientEffects, createClient, createClientFailure, createClientSuccess, getClient, getClientFailure, getClientSuccess, updateClient, updateClientFailure, updateClientSuccess } from "../..";
import { Client, ClientApiService } from "../../../shared";

describe('ClientEffects', () => {
    let effects: ClientEffects;
    let actions$: Observable<any>;
    let apiServiceMock: jasmine.SpyObj<ClientApiService>;

    beforeEach(() => {
        apiServiceMock = jasmine.createSpyObj('ClientApiService', ['get', 'create', 'update']);

        TestBed.configureTestingModule({
            providers: [
                ClientEffects,
                provideMockActions(() => actions$),
                { provide: ClientApiService, useValue: apiServiceMock },
            ]
        });

        effects = TestBed.inject(ClientEffects);
    });

    it('should dispatch getClientSuccess on successful getClient API call', (done) => {
        const mockClient: Client = { id: '1', name: 'John Doe' } as Client;
        const response = { client: mockClient };
        apiServiceMock.get.and.returnValue(of(response));

        actions$ = of(getClient());

        effects.getClient$.subscribe(action => {
            expect(action).toEqual(getClientSuccess({ client: mockClient }));
            done();
        });
    });

    it('should dispatch getClientFailure if client is not found', (done) => {
        const response = { client: null };
        apiServiceMock.get.and.returnValue(of(response));

        actions$ = of(getClient());

        effects.getClient$.subscribe(action => {
            expect(action).toEqual(getClientFailure({ error: new Error('Client was not found!') }));
            done();
        });
    });

    it('should dispatch getClientFailure on getClient API error', (done) => {
        const mockError = new Error('API Error');
        apiServiceMock.get.and.returnValue(throwError(() => mockError));

        actions$ = of(getClient());

        effects.getClient$.subscribe(action => {
            expect(action).toEqual(getClientFailure({ error: mockError.message }));
            done();
        });
    });

    it('should dispatch createClientSuccess on successful createClient API call', (done) => {
        const mockClient: Client = { id: '1', name: 'Jane Doe' } as Client;
        apiServiceMock.create.and.returnValue(of(mockClient));

        actions$ = of(createClient({ req: mockClient }));

        effects.createClient$.subscribe(action => {
            expect(action).toEqual(createClientSuccess({ client: mockClient }));
            done();
        });
    });

    it('should dispatch createClientFailure on createClient API error', (done) => {
        const mockError = new Error('Create API Error');
        apiServiceMock.create.and.returnValue(throwError(() => mockError));

        actions$ = of(createClient({ req: {} as Client }));

        effects.createClient$.subscribe(action => {
            expect(action).toEqual(createClientFailure({ error: mockError.message }));
            done();
        });
    });

    it('should dispatch updateClientSuccess on successful updateClient API call', (done) => {
        const mockClient: Client = { id: '2', name: 'Alice' } as Client;
        apiServiceMock.update.and.returnValue(of(mockClient));

        actions$ = of(updateClient({ req: mockClient }));

        effects.updateClient$.subscribe(action => {
            expect(action).toEqual(updateClientSuccess({ client: mockClient }));
            done();
        });
    });

    it('should dispatch updateClientFailure on updateClient API error', (done) => {
        const mockError = new Error('Update API Error');
        apiServiceMock.update.and.returnValue(throwError(() => mockError));

        actions$ = of(updateClient({ req: {} as Client }));

        effects.updateClient$.subscribe(action => {
            expect(action).toEqual(updateClientFailure({ error: mockError.message }));
            done();
        });
    });
});