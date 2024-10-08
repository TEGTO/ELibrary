import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, mergeMap, of } from "rxjs";
import { createClient, createClientFailure, createClientSuccess, deleteUser, deleteUserFailure, deleteUserSuccess, getClient, getClientFailure, getClientSuccess, getPaginatedUserAmount, getPaginatedUserAmountFailure, getPaginatedUserAmountSuccess, getPaginatedUsers, getPaginatedUsersFailure, getPaginatedUsersSuccess, getUser, getUserFailure, getUserSuccess, registerUser, registerUserFailure, registerUserSuccess, updateClient, updateClientFailure, updateClientSuccess, updateUser, updateUserFailure, updateUserSuccess } from "..";
import { AuthenticationApiService, ClientApiService } from "../../shared";

@Injectable()
export class AdminEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly userApiService: AuthenticationApiService,
        private readonly clientApiService: ClientApiService,
    ) { }

    //#region User

    getUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getUser),
            mergeMap((action) =>
                this.userApiService.adminGetUser(action.info).pipe(
                    map(response => getUserSuccess({ user: response })),
                    catchError(error => of(getUserFailure({ error: error.message })))
                )
            )
        )
    );

    registerUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(registerUser),
            mergeMap((action) =>
                this.userApiService.adminRegisterUser(action.req).pipe(
                    map(response => registerUserSuccess({ user: response })),
                    catchError(error => of(registerUserFailure({ error: error.message })))
                )
            )
        )
    );

    getPaginatedUsers$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getPaginatedUsers),
            mergeMap((action) =>
                this.userApiService.adminGetPaginatedUsers(action.req).pipe(
                    map(response => getPaginatedUsersSuccess({ users: response })),
                    catchError(error => of(getPaginatedUsersFailure({ error: error.message })))
                )
            )
        )
    );

    getPaginatedUserAmount$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getPaginatedUserAmount),
            mergeMap((action) =>
                this.userApiService.adminGetPaginatedUserAmount(action.req).pipe(
                    map(response => getPaginatedUserAmountSuccess({ amount: response })),
                    catchError(error => of(getPaginatedUserAmountFailure({ error: error.message })))
                )
            )
        )
    );

    updateUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(updateUser),
            mergeMap((action) =>
                this.userApiService.adminUpdateUser(action.req).pipe(
                    map(response => updateUserSuccess({ user: response })),
                    catchError(error => of(updateUserFailure({ error: error.message })))
                )
            )
        )
    );

    deleteUser$ = createEffect(() =>
        this.actions$.pipe(
            ofType(deleteUser),
            mergeMap((action) =>
                this.userApiService.adminDeleteUser(action.id).pipe(
                    map(() => deleteUserSuccess({ id: action.id })),
                    catchError(error => of(deleteUserFailure({ error: error.message })))
                )
            )
        )
    );
    //#endregion

    //#region  Client

    getClient$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getClient),
            mergeMap((action) =>
                this.clientApiService.adminGet(action.userId).pipe(
                    map((response) => getClientSuccess({ client: response })),
                    catchError(error => of(getClientFailure({ error: error.message })))
                )
            )
        )
    );

    createClient$ = createEffect(() =>
        this.actions$.pipe(
            ofType(createClient),
            mergeMap((action) =>
                this.clientApiService.adminCreate(action.userId, action.req).pipe(
                    map((response) => createClientSuccess({ client: response })),
                    catchError(error => of(createClientFailure({ error: error.message })))
                )
            )
        )
    );

    updateClient$ = createEffect(() =>
        this.actions$.pipe(
            ofType(updateClient),
            mergeMap((action) =>
                this.clientApiService.adminUpdate(action.userId, action.req).pipe(
                    map((response) => updateClientSuccess({ client: response })),
                    catchError(error => of(updateClientFailure({ error: error.message })))
                )
            )
        )
    );

    //#endregion
}