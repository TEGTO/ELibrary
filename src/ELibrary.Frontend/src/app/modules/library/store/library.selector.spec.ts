import { LibraryState, selectAuthorAmount, selectAuthors, selectBookAmount, selectBooks, selectGenreAmount, selectGenres, selectLibraryError, selectLibraryState } from "..";

describe('Library Selectors', () => {
    const initialState: LibraryState = {
        books: [],
        authors: [],
        genres: [],
        totalBookAmount: 1,
        totalAuthorAmount: 2,
        totalGenreAmount: 3,
        error: null
    };
    const errorState: LibraryState = {
        books: [],
        authors: [],
        genres: [],
        totalBookAmount: 0,
        totalAuthorAmount: 0,
        totalGenreAmount: 0,
        error: 'An error occurred'
    };

    it('should select a library state', () => {
        const result = selectLibraryState.projector(initialState);
        expect(result).toEqual(initialState);
    });
    it('should select authors', () => {
        const result = selectAuthors.projector(initialState);
        expect(result).toEqual(initialState.authors);
    });
    it('should select genres', () => {
        const result = selectGenres.projector(initialState);
        expect(result).toEqual(initialState.genres);
    });
    it('should select books', () => {
        const result = selectBooks.projector(initialState);
        expect(result).toEqual(initialState.books);
    });
    it('should select author amount', () => {
        const result = selectAuthorAmount.projector(initialState);
        expect(result).toEqual(initialState.totalAuthorAmount);
    });
    it('should select genre amount', () => {
        const result = selectGenreAmount.projector(initialState);
        expect(result).toEqual(initialState.totalGenreAmount);
    });
    it('should select book amount', () => {
        const result = selectBookAmount.projector(initialState);
        expect(result).toEqual(initialState.totalBookAmount);
    });
    it('should select library error', () => {
        const result = selectLibraryError.projector(errorState);
        expect(result).toEqual(errorState.error);
    });
});
