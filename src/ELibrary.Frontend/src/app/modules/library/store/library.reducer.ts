import { createReducer, on } from "@ngrx/store";
import { AuthorResponse, BookResponse, GenreResponse } from "../../shared";
import { authorActions, bookActions, genreActions } from "./library.actions";

export interface LibraryState {
    books: BookResponse[],
    authors: AuthorResponse[],
    genres: GenreResponse[],
    totalBookAmount: number,
    totalAuthorAmount: number,
    totalGenreAmount: number,
    error: any
}
const initialLibraryState: LibraryState = {
    books: [],
    authors: [],
    genres: [],
    totalBookAmount: 0,
    totalAuthorAmount: 0,
    totalGenreAmount: 0,
    error: null
};

export const libraryReducer = createReducer(
    initialLibraryState,

    on(bookActions.getPaginatedSuccess, (state, { entities }) =>
        handlePaginatedSuccess(state, entities, 'books')
    ),
    on(bookActions.getPaginatedFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(bookActions.getTotalAmountSuccess, (state, { amount }) =>
    ({
        ...state,
        totalBookAmount: amount,
        error: null
    })),
    on(bookActions.createSuccess, (state, { entity }) =>
        handleCreateSuccess(state, entity, 'books', 'totalBookAmount')
    ),
    on(bookActions.createFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(bookActions.updateSuccess, (state, { entity }) =>
        handleUpdateSuccess(state, entity, 'books')
    ),
    on(bookActions.updateFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(bookActions.deleteByIdSuccess, (state, { id }) =>
        handleDeleteSuccess(state, id, 'books', 'totalBookAmount')
    ),
    on(bookActions.deleteByIdFailure, (state, { error }) =>
        handleFailure(state, error)
    ),

    on(authorActions.getPaginatedSuccess, (state, { entities }) =>
        handlePaginatedSuccess(state, entities, 'authors')
    ),
    on(authorActions.getPaginatedFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(authorActions.getTotalAmountSuccess, (state, { amount }) =>
    ({
        ...state,
        totalAuthorAmount: amount,
        error: null
    })),
    on(authorActions.createSuccess, (state, { entity }) =>
        handleCreateSuccess(state, entity, 'authors', 'totalAuthorAmount')
    ),
    on(authorActions.createFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(authorActions.updateSuccess, (state, { entity }) =>
        handleUpdateSuccess(state, entity, 'authors')
    ),
    on(authorActions.updateFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(authorActions.deleteByIdSuccess, (state, { id }) =>
        handleDeleteSuccess(state, id, 'authors', 'totalAuthorAmount')
    ),
    on(authorActions.deleteByIdFailure, (state, { error }) =>
        handleFailure(state, error)
    ),

    on(genreActions.getPaginatedSuccess, (state, { entities }) =>
        handlePaginatedSuccess(state, entities, 'genres')
    ),
    on(genreActions.getPaginatedFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(genreActions.getTotalAmountSuccess, (state, { amount }) =>
    ({
        ...state,
        totalGenreAmount: amount,
        error: null
    })),
    on(genreActions.createSuccess, (state, { entity }) =>
        handleCreateSuccess(state, entity, 'genres', 'totalGenreAmount')
    ),
    on(genreActions.createFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(genreActions.updateSuccess, (state, { entity }) =>
        handleUpdateSuccess(state, entity, 'genres')
    ),
    on(genreActions.updateFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(genreActions.deleteByIdSuccess, (state, { id }) =>
        handleDeleteSuccess(state, id, 'genres', 'totalGenreAmount')
    ),
    on(genreActions.deleteByIdFailure, (state, { error }) =>
        handleFailure(state, error)
    )
);

const handlePaginatedSuccess = <T>(state: LibraryState, entities: T[], key: keyof LibraryState): LibraryState => ({
    ...state,
    [key]: entities,
    error: null
});

const handleCreateSuccess = <T>(state: LibraryState, entity: T, key: keyof LibraryState, amountKey: keyof LibraryState): LibraryState => ({
    ...state,
    [key]: [entity, ...(state[key] as T[])],
    [amountKey]: (state[amountKey] as number) + 1,
    error: null
});

const handleUpdateSuccess = <T extends { id: number }>(state: LibraryState, entity: T, key: keyof LibraryState): LibraryState => ({
    ...state,
    [key]: (state[key] as T[]).map(item => item.id === entity.id ? entity : item),
    error: null
});

const handleFailure = (state: LibraryState, error: any): LibraryState => ({
    ...state,
    error
});

const handleDeleteSuccess = <T extends { id: number }>(state: LibraryState, id: number, key: keyof LibraryState, amountKey: keyof LibraryState): LibraryState => ({
    ...state,
    [key]: (state[key] as T[]).filter(entity => entity.id !== id),
    [amountKey]: (state[amountKey] as number) - 1,
    error: null
});