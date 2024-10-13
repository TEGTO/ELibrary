import { createFeatureSelector, createSelector } from "@ngrx/store";
import { AuthorState, BookState, GenreState, PublisherState } from "..";

//#region Author

export const selectAuthorState = createFeatureSelector<AuthorState>('author');
export const selectAuthors = createSelector(
    selectAuthorState,
    (state: AuthorState) => state.authors
);
export const selectAuthorAmount = createSelector(
    selectAuthorState,
    (state: AuthorState) => state.totalAuthorAmount
);
export const selectAuthorError = createSelector(
    selectAuthorState,
    (state: AuthorState) => state.error
);

//#endregion

//#region Genre

export const selectGenreState = createFeatureSelector<GenreState>('genre');
export const selectGenres = createSelector(
    selectGenreState,
    (state: GenreState) => state.genres
);
export const selectGenreAmount = createSelector(
    selectGenreState,
    (state: GenreState) => state.totalGenreAmount
);
export const selectGenreError = createSelector(
    selectGenreState,
    (state: GenreState) => state.error
);

//#endregion

//#region Publisher

export const selectPublisherState = createFeatureSelector<PublisherState>('publisher');
export const selectPublishers = createSelector(
    selectPublisherState,
    (state: PublisherState) => state.publishers
);
export const selectPublisherAmount = createSelector(
    selectPublisherState,
    (state: PublisherState) => state.totalPublisherAmount
);
export const selectPublisherError = createSelector(
    selectPublisherState,
    (state: PublisherState) => state.error
);

//#endregion

//#region Book

export const selectBoookState = createFeatureSelector<BookState>('book');
export const selectBooks = createSelector(
    selectBoookState,
    (state: BookState) => state.books
);
export const selectBookAmount = createSelector(
    selectBoookState,
    (state: BookState) => state.totalBookAmount
);
export const selectBookError = createSelector(
    selectBoookState,
    (state: BookState) => state.error
);

//#endregion