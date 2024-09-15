import { createReducer, on } from "@ngrx/store";
import { AuthorResponse, BookResponse, GenreResponse, PublisherResponse } from "../../shared";
import { authorActions, bookActions, genreActions, publisherActions } from "./library.actions";

//#region Author

export interface AuthorState {
    authors: AuthorResponse[],
    totalAuthorAmount: number,
    error: any
}
const initialAuthorState: AuthorState = {
    authors: [],
    totalAuthorAmount: 0,
    error: null
};
export const authorReducer = createReducer(
    initialAuthorState,

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
);

//#endregion

//#region Genre

export interface GenreState {
    genres: GenreResponse[],
    totalGenreAmount: number,
    error: any
}
const initialLGenreState: GenreState = {
    genres: [],
    totalGenreAmount: 0,
    error: null
};
export const genreReducer = createReducer(
    initialLGenreState,

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

//#endregion

//#region Publisher

export interface PublisherState {
    publishers: PublisherResponse[],
    totalPublisherAmount: number,
    error: any
}
const initialPublisherState: PublisherState = {
    publishers: [],
    totalPublisherAmount: 0,
    error: null
};
export const publisherReducer = createReducer(
    initialPublisherState,

    on(publisherActions.getPaginatedSuccess, (state, { entities }) =>
        handlePaginatedSuccess(state, entities, 'publishers')
    ),
    on(publisherActions.getPaginatedFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(publisherActions.getTotalAmountSuccess, (state, { amount }) =>
    ({
        ...state,
        totalPublisherAmount: amount,
        error: null
    })),
    on(publisherActions.createSuccess, (state, { entity }) =>
        handleCreateSuccess(state, entity, 'publishers', 'totalPublisherAmount')
    ),
    on(publisherActions.createFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(publisherActions.updateSuccess, (state, { entity }) =>
        handleUpdateSuccess(state, entity, 'publishers')
    ),
    on(publisherActions.updateFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(publisherActions.deleteByIdSuccess, (state, { id }) =>
        handleDeleteSuccess(state, id, 'publishers', 'totalPublisherAmount')
    ),
    on(publisherActions.deleteByIdFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
);

//#endregion

//#region Book

export interface BookState {
    books: BookResponse[],
    totalBookAmount: number,
    error: any
}
const initialBookState: BookState = {
    books: [],
    totalBookAmount: 0,
    error: null
};
export const bookReducer = createReducer(
    initialBookState,

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
);

//#endregion


const handlePaginatedSuccess = <T>(state: any, entities: T[], key: keyof any): any => ({
    ...state,
    [key]: entities,
    error: null
});

const handleCreateSuccess = <T>(state: any, entity: T, key: keyof any, amountKey: keyof any): any => ({
    ...state,
    [key]: [entity, ...(state[key] as T[])],
    [amountKey]: (state[amountKey] as number) + 1,
    error: null
});

const handleUpdateSuccess = <T extends { id: number }>(state: any, entity: T, key: keyof any): any => ({
    ...state,
    [key]: (state[key] as T[]).map(item => item.id === entity.id ? entity : item),
    error: null
});

const handleFailure = (state: any, error: any): any => ({
    ...state,
    error
});

const handleDeleteSuccess = <T extends { id: number }>(state: any, id: number, key: keyof any, amountKey: keyof any): any => ({
    ...state,
    [key]: (state[key] as T[]).filter(entity => entity.id !== id),
    [amountKey]: (state[amountKey] as number) - 1,
    error: null
});