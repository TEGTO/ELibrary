import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, map, mergeMap, of } from "rxjs";
import { createAuthor, createAuthorFailure, createAuthorSuccess, deleteAuthorById, deleteAuthorByIdFailure, deleteAuthorByIdSuccess, getPaginatedAuthors, getPaginatedAuthorsFailure, getPaginatedAuthorsSuccess, updateAuthor, updateAuthorFailure, updateAuthorSuccess } from "../..";
import { AuthorApiService } from "../../../shared";


@Injectable()
export class RegistrationEffects {
    constructor(
        private readonly actions$: Actions,
        private readonly apiService: AuthorApiService
    ) { }

    getPaginatedAuthors$ = createEffect(() =>
        this.actions$.pipe(
            ofType(getPaginatedAuthors),
            mergeMap((action) =>
                this.apiService.getPaginated(action.request).pipe(
                    map((authors) => getPaginatedAuthorsSuccess({ authors: authors })),
                    catchError(error => of(getPaginatedAuthorsFailure({ error: error.message })))
                )
            )
        )
    );

    createAuthor$ = createEffect(() =>
        this.actions$.pipe(
            ofType(createAuthor),
            mergeMap((action) =>
                this.apiService.create(action.request).pipe(
                    map((author) => createAuthorSuccess({ author: author })),
                    catchError(error => of(createAuthorFailure({ error: error.message })))
                )
            )
        )
    );

    updateAuthor$ = createEffect(() =>
        this.actions$.pipe(
            ofType(updateAuthor),
            mergeMap((action) =>
                this.apiService.update(action.request).pipe(
                    map(() => updateAuthorSuccess({ author: action.request })),
                    catchError(error => of(updateAuthorFailure({ error: error.message })))
                )
            )
        )
    );

    deleteAuthorById$ = createEffect(() =>
        this.actions$.pipe(
            ofType(deleteAuthorById),
            mergeMap((action) =>
                this.apiService.deleteById(action.id).pipe(
                    map(() => deleteAuthorByIdSuccess({ id: action.id })),
                    catchError(error => of(deleteAuthorByIdFailure({ error: error.message })))
                )
            )
        )
    );
}