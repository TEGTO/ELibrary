import { createFeatureSelector, createSelector } from "@ngrx/store";
import { LibraryState } from "..";

export const selectLibraryState = createFeatureSelector<LibraryState>('library');
export const selectAuthors = createSelector(
    selectLibraryState,
    (state: LibraryState) => state.authors
);
export const selectGenres = createSelector(
    selectLibraryState,
    (state: LibraryState) => state.genres
);
export const selectBooks = createSelector(
    selectLibraryState,
    (state: LibraryState) => state.books
);
export const selectAuthorAmount = createSelector(
    selectLibraryState,
    (state: LibraryState) => state.totalAuthorAmount
);
export const selectGenreAmount = createSelector(
    selectLibraryState,
    (state: LibraryState) => state.totalGenreAmount
);
export const selectBookAmount = createSelector(
    selectLibraryState,
    (state: LibraryState) => state.totalBookAmount
);
export const selectLibraryError = createSelector(
    selectLibraryState,
    (state: LibraryState) => state.error
);
