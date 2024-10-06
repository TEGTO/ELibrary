/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAction, props } from "@ngrx/store";
import { AdminUser, AdminUserRegistrationRequest, AdminUserUpdateDataRequest, Client, CreateClientRequest, GetUserFilterRequest, UpdateClientRequest } from "../../shared";

//#region User

export const getUser = createAction(
    '[Admin] Get User',
    props<{ info: string }>()
);
export const getUserSuccess = createAction(
    '[Admin] Get User Success',
    props<{ user: AdminUser }>()
);
export const getUserFailure = createAction(
    '[Admin] Get User Failure',
    props<{ error: any }>()
);

export const registerUser = createAction(
    '[Admin] Register User',
    props<{ req: AdminUserRegistrationRequest }>()
);
export const registerUserSuccess = createAction(
    '[Admin] Register User Success',
    props<{ user: AdminUser }>()
);
export const registerUserFailure = createAction(
    '[Admin] Register User Failure',
    props<{ error: any }>()
);

export const getPaginatedUsers = createAction(
    '[Admin] Get Paginated Users',
    props<{ req: GetUserFilterRequest }>()
);
export const getPaginatedUsersSuccess = createAction(
    '[Admin] Get Paginated Users Success',
    props<{ users: AdminUser[] }>()
);
export const getPaginatedUsersFailure = createAction(
    '[Admin] Get Paginated Users Failure',
    props<{ error: any }>()
);

export const getPaginatedUserAmount = createAction(
    '[Admin] Get Paginated User Amount',
    props<{ req: GetUserFilterRequest }>()
);
export const getPaginatedUserAmountSuccess = createAction(
    '[Admin] Get Paginated User Amount Success',
    props<{ amount: number }>()
);
export const getPaginatedUserAmountFailure = createAction(
    '[Admin] Get Paginated User Amount Failure',
    props<{ error: any }>()
);

export const updateUser = createAction(
    '[Admin] Update User',
    props<{ req: AdminUserUpdateDataRequest }>()
);
export const updateUserSuccess = createAction(
    '[Admin] Update User Success',
    props<{ user: AdminUser }>()
);
export const updateUserFailure = createAction(
    '[Admin] Update User Failure',
    props<{ error: any }>()
);

export const deleteUser = createAction(
    '[Admin] Delete User',
    props<{ id: string }>()
);
export const deleteUserSuccess = createAction(
    '[Admin] Delete User Success',
    props<{ id: string }>()
);
export const deleteUserFailure = createAction(
    '[Admin] Delete User Failure',
    props<{ error: any }>()
);

//#endregion 

//#region  Client

export const getClient = createAction(
    '[Admin] Get Client',
    props<{ userId: string }>()
);
export const getClientSuccess = createAction(
    '[Admin] Get Client Success',
    props<{ client: Client }>()
);
export const getClientFailure = createAction(
    '[Admin] Get Client Failure',
    props<{ error: any }>()
);

export const createClient = createAction(
    '[Admin] Create Client',
    props<{ userId: string, req: CreateClientRequest }>()
);
export const createClientSuccess = createAction(
    '[Admin] Create Client Success',
    props<{ client: Client }>()
);
export const createClientFailure = createAction(
    '[Admin] Create Client Failure',
    props<{ error: any }>()
);

export const updateClient = createAction(
    '[Admin] Update Client',
    props<{ userId: string, req: UpdateClientRequest }>()
);
export const updateClientSuccess = createAction(
    '[Admin] Update Client Success',
    props<{ client: Client }>()
);
export const updateClientFailure = createAction(
    '[Admin] Update Client Failure',
    props<{ error: any }>()
);

//#endregion