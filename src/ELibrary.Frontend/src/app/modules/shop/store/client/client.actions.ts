/* eslint-disable @typescript-eslint/no-explicit-any */
import { createAction, props } from "@ngrx/store";
import { Client, CreateClientRequest, UpdateClientRequest } from "../../../shared";

export const getClient = createAction(
    '[Client] Get Client'
);
export const getClientSuccess = createAction(
    '[Client] Get Client Success',
    props<{ client: Client }>()
);
export const getClientFailure = createAction(
    '[Client] Get Client Failure',
    props<{ error: any }>()
);

export const createClient = createAction(
    '[Client] Create Client',
    props<{ req: CreateClientRequest }>()
);
export const createClientSuccess = createAction(
    '[Client] Create Client Success',
    props<{ client: Client }>()
);
export const createClientFailure = createAction(
    '[Client] Create Client Failure',
    props<{ error: any }>()
);

export const updateClient = createAction(
    '[Client] Update Client',
    props<{ req: UpdateClientRequest }>()
);
export const updateClientSuccess = createAction(
    '[Client] Update Client Success',
    props<{ client: Client }>()
);
export const updateClientFailure = createAction(
    '[Client] Update Client Failure',
    props<{ error: any }>()
);

export const deleteClient = createAction(
    '[Client] Delete Client',
);
export const deleteClientSuccess = createAction(
    '[Client] Delete Client Success',
);
export const deleteClientFailure = createAction(
    '[Client] Delete Client Failure',
    props<{ error: any }>()
);