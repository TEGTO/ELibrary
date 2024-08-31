import { createReducer, on } from "@ngrx/store";
import { AuthorResponse, BookResponse, GenreResponse, getAuthorFromUpdateRequest, getBookFromUpdateRequest, getGenreFromUpdateRequest } from "../../shared";
import { authorActions, bookActions, genreActions } from "./library.actions";

export interface LibraryState {
    books: BookResponse[],
    authors: AuthorResponse[],
    genres: GenreResponse[],
    error: any
}
const initialLibraryState: LibraryState = {
    books: [],
    authors: [],
    genres: [],
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
    on(bookActions.createSuccess, (state, { entity }) =>
        handleCreateSuccess(state, entity, 'books')
    ),
    on(bookActions.createFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(bookActions.updateSuccess, (state, { entity }) => {
        let original = state.books.find(b => b.id === entity.id);
        let author = state.authors.find(x => x.id === entity.authorId);
        let genre = state.genres.find(x => x.id === entity.genreId);
        if (original && author && genre) {
            return {
                ...state,
                books: state.books.map(b => b.id === entity.id ? getBookFromUpdateRequest(entity, author, genre) : b),
                error: null
            };
        }
        return state;
    }),
    on(bookActions.updateFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(bookActions.deleteByIdSuccess, (state, { id }) =>
        handleDeleteSuccess(state, id, 'books')
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
    on(authorActions.createSuccess, (state, { entity }) =>
        handleCreateSuccess(state, entity, 'authors')
    ),
    on(authorActions.createFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(authorActions.updateSuccess, (state, { entity }) => ({
        ...state,
        authors: state.authors.map(g => g.id === entity.id ? getAuthorFromUpdateRequest(entity) : g),
        error: null
    })),
    on(authorActions.updateFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(authorActions.deleteByIdSuccess, (state, { id }) =>
        handleDeleteSuccess(state, id, 'authors')
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
    on(genreActions.createSuccess, (state, { entity }) =>
        handleCreateSuccess(state, entity, 'genres')
    ),
    on(genreActions.createFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(genreActions.updateSuccess, (state, { entity }) => ({
        ...state,
        genres: state.genres.map(g => g.id === entity.id ? getGenreFromUpdateRequest(entity) : g),
        error: null
    })),
    on(genreActions.updateFailure, (state, { error }) =>
        handleFailure(state, error)
    ),
    on(genreActions.deleteByIdSuccess, (state, { id }) =>
        handleDeleteSuccess(state, id, 'genres')
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

const handleCreateSuccess = <T>(state: LibraryState, entity: T, key: keyof LibraryState): LibraryState => ({
    ...state,
    [key]: [entity, ...(state[key] as T[])],
    error: null
});

const handleFailure = (state: LibraryState, error: any): LibraryState => ({
    ...state,
    error
});

const handleDeleteSuccess = <T extends { id: number }>(state: LibraryState, id: number, key: keyof LibraryState): LibraryState => ({
    ...state,
    [key]: (state[key] as T[]).filter(entity => entity.id !== id),
    error: null
});