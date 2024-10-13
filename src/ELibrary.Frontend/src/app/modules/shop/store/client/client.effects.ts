import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, mergeMap, of } from "rxjs";
import { createClient, createClientFailure, createClientSuccess, getClient, getClientFailure, getClientSuccess, updateClient, updateClientFailure, updateClientSuccess } from "../..";
import { ClientApiService } from "../../../shared";

@Injectable()
export class ClientEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly apiService: ClientApiService,
    ) { }

    getClient$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getClient),
            mergeMap(() =>
                this.apiService.get().pipe(
                    map(response => getClientSuccess({ client: response })),
                    catchError(error => of(getClientFailure({ error: error.message })))
                )
            )
        )
    );

    createClient$ = createEffect(() =>
        this.actions$.pipe(
            ofType(createClient),
            mergeMap((action) =>
                this.apiService.create(action.req).pipe(
                    map(response => createClientSuccess({ client: response })),
                    catchError(error => of(createClientFailure({ error: error.message })))
                )
            )
        )
    );

    updateClient$ = createEffect(() =>
        this.actions$.pipe(
            ofType(updateClient),
            mergeMap((action) =>
                this.apiService.update(action.req).pipe(
                    map(response => updateClientSuccess({ client: response })),
                    catchError(error => of(updateClientFailure({ error: error.message })))
                )
            )
        )
    );
}