/* eslint-disable @typescript-eslint/no-explicit-any */
import { createReducer, on } from "@ngrx/store";
import { createClientFailure, createClientSuccess, getClientFailure, getClientSuccess, updateClientFailure, updateClientSuccess } from "../..";
import { Client } from "../../../shared";

export interface ClientState {
    client: Client | null
    error: any
}
const initialClientState: ClientState = {
    client: null,
    error: null
};
export const clientReducer = createReducer(
    initialClientState,

    on(getClientSuccess, (state, { client: client }) => ({
        ...state,
        client: client,
        error: null
    })),
    on(getClientFailure, (state, { error: error }) => ({
        ...initialClientState,
        error: error
    })),

    on(createClientSuccess, (state, { client: client }) => ({
        ...state,
        client: client,
        error: null
    })),
    on(createClientFailure, (state, { error: error }) => ({
        ...initialClientState,
        error: error
    })),

    on(updateClientSuccess, (state, { client: client }) => ({
        ...state,
        client: client,
        error: null
    })),
    on(updateClientFailure, (state, { error: error }) => ({
        ...initialClientState,
        error: error
    })),
);