/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, mergeMap, of } from "rxjs";
import { Author, AuthorApiService, Book, BookApiService, BookFilterRequest, CreateAuthorRequest, CreateBookRequest, CreateGenreRequest, CreatePublisherRequest, Genre, GenreApiService, LibraryEntityApi, LibraryFilterRequest, Publisher, PublisherApiService, UpdateAuthorRequest, UpdateBookRequest, UpdateGenreRequest, UpdatePublisherRequest } from "../../shared";
import { authorActions, bookActions, genreActions, publisherActions } from "./library.actions";

@Injectable()
export abstract class GenericLibraryEntityEffects<Response, Create, Update, Filter> {
    constructor(
        private readonly actions$: Actions,
        private readonly apiService: LibraryEntityApi<Response, Create, Update, Filter>,
        private readonly entityActions: any
    ) { }

    getPaginated$ = createEffect(() =>
        this.actions$.pipe(
            ofType(this.entityActions.getPaginated),
            mergeMap((action: { request: Filter }) =>
                this.apiService.getPaginated(action.request).pipe(
                    map((entities: Response[]) => { return this.entityActions.getPaginatedSuccess({ entities }) }),
                    catchError((error: any) => of(this.entityActions.getPaginatedFailure({ error: error.message })))
                )
            )
        )
    );

    getTotalAmount$ = createEffect(() =>
        this.actions$.pipe(
            ofType(this.entityActions.getTotalAmount),
            mergeMap((action: { request: Filter }) =>
                this.apiService.getItemTotalAmount(action.request).pipe(
                    map((amount: number) => this.entityActions.getTotalAmountSuccess({ amount: amount })),
                    catchError((error: any) => of(this.entityActions.getTotalAmountFailure({ error: error.message })))
                )
            )
        )
    );

    create$ = createEffect(() =>
        this.actions$.pipe(
            ofType(this.entityActions.create),
            mergeMap((action: { request: Create }) =>
                this.apiService.create(action.request).pipe(
                    map((entity: Response) => this.entityActions.createSuccess({ entity })),
                    catchError((error: any) => of(this.entityActions.createFailure({ error: error.message })))
                )
            )
        )
    );

    update$ = createEffect(() =>
        this.actions$.pipe(
            ofType(this.entityActions.update),
            mergeMap((action: { request: Update }) =>
                this.apiService.update(action.request).pipe(
                    map((entity: Response) => this.entityActions.updateSuccess({ entity: entity })),
                    catchError((error: any) => of(this.entityActions.updateFailure({ error: error.message })))
                )
            )
        )
    );

    deleteById$ = createEffect(() =>
        this.actions$.pipe(
            ofType(this.entityActions.deleteById),
            mergeMap((action: { id: number }) =>
                this.apiService.deleteById(action.id).pipe(
                    map(() => this.entityActions.deleteByIdSuccess({ id: action.id })),
                    catchError((error: any) => of(this.entityActions.deleteByIdFailure({ error: error.message })))
                )
            )
        )
    );
}

@Injectable()
export class AuthorEffects extends GenericLibraryEntityEffects<Author, CreateAuthorRequest, UpdateAuthorRequest, LibraryFilterRequest> {
    constructor(
        actions$: Actions,
        apiService: AuthorApiService
    ) {
        super(actions$, apiService, authorActions);
    }
}

@Injectable()
export class GenreEffects extends GenericLibraryEntityEffects<Genre, CreateGenreRequest, UpdateGenreRequest, LibraryFilterRequest> {
    constructor(
        actions$: Actions,
        apiService: GenreApiService
    ) {
        super(actions$, apiService, genreActions);
    }
}

@Injectable()
export class PublisherEffects extends GenericLibraryEntityEffects<Publisher, CreatePublisherRequest, UpdatePublisherRequest, LibraryFilterRequest> {
    constructor(
        actions$: Actions,
        apiService: PublisherApiService
    ) {
        super(actions$, apiService, publisherActions);
    }
}

@Injectable()
export class BookEffects extends GenericLibraryEntityEffects<Book, CreateBookRequest, UpdateBookRequest, BookFilterRequest> {
    constructor(
        actions$: Actions,
        apiService: BookApiService
    ) {
        super(actions$, apiService, bookActions);
    }
}