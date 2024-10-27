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

    on(getUserSuccess, (state, { user }) => ({
        ...state,
        users: [user],
        error: null
    })),
    on(getUserFailure, (state, { error }) => ({
        ...state,
        error: error
    })),

    on(registerUserSuccess, (state, { user }) => ({
        ...state,
        users: [user, ...state.users],
        userTotalAmount: state.userTotalAmount + 1,
        error: null
    })),
    on(registerUserFailure, (state, { error }) => ({
        ...state,
        error: error
    })),

    on(getPaginatedUsersSuccess, (state, { users }) => ({
        ...state,
        users: users,
        error: null
    })),
    on(getPaginatedUsersFailure, (state, { error }) => ({
        ...initialAdminState,
        error: error
    })),

    on(getPaginatedUserAmountSuccess, (state, { amount }) => ({
        ...state,
        userTotalAmount: amount,
        error: null
    })),
    on(getPaginatedUserAmountFailure, (state, { error }) => ({
        ...initialAdminState,
        error: error
    })),

    on(updateUserSuccess, (state, { user }) => (
        {
            ...state,
            users: state.users.map(u => u.id === user.id ? user : u),
            error: null
        }
    )),
    on(updateUserFailure, (state, { error }) => ({
        ...state,
        error: error
    })),

    on(deleteUserSuccess, (state, { id }) => (
        {
            ...state,
            users: state.users.filter(user =>
                user.id !== id
            ),
            userTotalAmount: state.userTotalAmount - 1,
            error: null
        }
    )),
    on(deleteUserFailure, (state, { error }) => ({
        ...state,
        error: error
    })),

    on(getClientSuccess, (state, { client }) => ({
        ...state,
        clients: [client, ...state.clients],
        error: null
    })),
    on(getClientFailure, (state, { error }) => ({
        ...state,
        error: error
    })),

    on(createClientSuccess, (state, { client }) => ({
        ...state,
        clients: [client, ...state.clients],
        error: null
    })),
    on(createClientFailure, (state, { error }) => ({
        ...state,
        error: error
    })),

    on(updateClientSuccess, (state, { client }) => ({
        ...state,
        clients: state.clients.map(c => c.id === client.id ? client : c),
        error: null
    })),
    on(updateClientFailure, (state, { error }) => ({
        ...state,
        error: error
    })),
);