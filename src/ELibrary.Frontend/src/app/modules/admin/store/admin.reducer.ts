/* eslint-disable @typescript-eslint/no-explicit-any */
import { createReducer, on } from "@ngrx/store";
import { createClientFailure, createClientSuccess, deleteUserFailure, deleteUserSuccess, getClientFailure, getClientSuccess, getPaginatedUserAmountFailure, getPaginatedUserAmountSuccess, getPaginatedUsersFailure, getPaginatedUsersSuccess, getUserFailure, getUserSuccess, registerUserFailure, registerUserSuccess, updateClientFailure, updateClientSuccess, updateUserFailure, updateUserSuccess } from "..";
import { AdminUser, Client } from "../../shared";

export interface AdminState {
    users: AdminUser[],
    clients: Client[],
    userTotalAmount: number,
    error: any
}
const initialAdminState: AdminState = {
    users: [],
    userTotalAmount: 0,
    clients: [],
    error: null
};

export const adminReducer = createReducer(
    initialAdminState,

    on(getUserSuccess, (state, { user: user }) => ({
        ...state,
        users: [user, ...state.users],
        error: null
    })),
    on(getUserFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),

    on(registerUserSuccess, (state, { user: user }) => ({
        ...state,
        users: [user, ...state.users],
        error: null
    })),
    on(registerUserFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),

    on(getPaginatedUsersSuccess, (state, { users: users }) => ({
        ...state,
        users: users,
        error: null
    })),
    on(getPaginatedUsersFailure, (state, { error: error }) => ({
        ...initialAdminState,
        error: error
    })),

    on(getPaginatedUserAmountSuccess, (state, { amount: amount }) => ({
        ...state,
        userTotalAmount: amount,
        error: null
    })),
    on(getPaginatedUserAmountFailure, (state, { error: error }) => ({
        ...initialAdminState,
        error: error
    })),

    on(updateUserSuccess, (state, { user: user }) => (
        {
            ...state,
            users: state.users.map(u => u.id === user.id ? user : u),
            error: null
        }
    )),
    on(updateUserFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),

    on(deleteUserSuccess, (state, { id: id }) => (
        {
            ...state,
            users: state.users.filter(user =>
                user.id !== id
            ),
            error: null
        }
    )),
    on(deleteUserFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),

    on(getClientSuccess, (state, { client: client }) => ({
        ...state,
        clients: [client, ...state.clients],
        error: null
    })),
    on(getClientFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),

    on(createClientSuccess, (state, { client: client }) => ({
        ...state,
        clients: [client, ...state.clients],
        error: null
    })),
    on(createClientFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),

    on(updateClientSuccess, (state, { client: client }) => ({
        ...state,
        clients: state.clients.map(c => c.id === client.id ? client : c),
        error: null
    })),
    on(updateClientFailure, (state, { error: error }) => ({
        ...state,
        error: error
    })),
);