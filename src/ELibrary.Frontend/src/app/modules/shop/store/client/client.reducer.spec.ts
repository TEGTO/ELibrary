/* eslint-disable @typescript-eslint/no-explicit-any */
import { clientReducer, ClientState, createClientFailure, createClientSuccess, getClientFailure, getClientSuccess, updateClientFailure, updateClientSuccess } from "../..";
import { Client, getDefaultClient } from "../../../shared";

describe('ClientReducer', () => {
    const initialState: ClientState = {
        client: null,
        error: null,
    };

    it('should return the initial state for unknown action', () => {
        const action = { type: 'UNKNOWN' } as any;
        const state = clientReducer(undefined, action);

        expect(state).toEqual(initialState);
    });

    it('should update client on getClientSuccess', () => {
        const mockClient: Client = { ...getDefaultClient(), id: '1', name: 'John' };
        const action = getClientSuccess({ client: mockClient });
        const state = clientReducer(initialState, action);

        expect(state.client).toEqual(mockClient);
        expect(state.error).toBeNull();
    });

    it('should set error on getClientFailure', () => {
        const mockError = { message: 'Failed to get client' };
        const action = getClientFailure({ error: mockError });
        const state = clientReducer(initialState, action);

        expect(state.client).toBeNull();
        expect(state.error).toEqual(mockError);
    });

    it('should update client on createClientSuccess', () => {
        const mockClient: Client = { ...getDefaultClient(), id: '2', name: 'Jane' };
        const action = createClientSuccess({ client: mockClient });
        const state = clientReducer(initialState, action);

        expect(state.client).toEqual(mockClient);
        expect(state.error).toBeNull();
    });

    it('should set error on createClientFailure', () => {
        const mockError = { message: 'Failed to create client' };
        const action = createClientFailure({ error: mockError });
        const state = clientReducer(initialState, action);

        expect(state.client).toBeNull();
        expect(state.error).toEqual(mockError);
    });

    it('should update client on updateClientSuccess', () => {
        const mockClient: Client = { ...getDefaultClient(), id: '3', name: 'Alice' };
        const action = updateClientSuccess({ client: mockClient });
        const state = clientReducer(initialState, action);

        expect(state.client).toEqual(mockClient);
        expect(state.error).toBeNull();
    });

    it('should set error on updateClientFailure', () => {
        const mockError = { message: 'Failed to update client' };
        const action = updateClientFailure({ error: mockError });
        const state = clientReducer(initialState, action);

        expect(state.client).toBeNull();
        expect(state.error).toEqual(mockError);
    });
});