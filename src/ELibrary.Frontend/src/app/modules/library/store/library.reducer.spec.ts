import { authorActions, bookActions, genreActions } from "..";
import { AuthorResponse, BookResponse, GenreResponse } from "../../shared";
import { LibraryState, libraryReducer } from "./library.reducer";

describe('Library Reducer', () => {
    const initialState: LibraryState = {
        books: [],
        authors: [],
        genres: [],
        totalBookAmount: 0,
        totalAuthorAmount: 0,
        totalGenreAmount: 0,
        error: null
    };

    it('should handle getPaginatedSuccess for books', () => {
        const books: BookResponse[] = [{ id: 1, name: 'Book 1', publicationDate: new Date(), author: { id: 1, name: 'Author 1', lastName: 'Last 1', dateOfBirth: new Date() }, genre: { id: 1, name: 'Genre 1' } }];
        const action = bookActions.getPaginatedSuccess({ entities: books });
        const newState = libraryReducer(initialState, action);

        expect(newState.books).toEqual(books);
        expect(newState.error).toBeNull();
    });

    it('should handle getPaginatedFailure for books', () => {
        const error = 'Failed to fetch books';
        const action = bookActions.getPaginatedFailure({ error });
        const newState = libraryReducer(initialState, action);

        expect(newState.error).toEqual(error);
    });

    it('should handle getTotalAmountSuccess for books', () => {
        const amount = 10;
        const action = bookActions.getTotalAmountSuccess({ amount });
        const newState = libraryReducer(initialState, action);

        expect(newState.totalBookAmount).toEqual(amount);
        expect(newState.error).toBeNull();
    });

    it('should handle createSuccess for books', () => {
        const newBook: BookResponse = { id: 2, name: 'Book 2', publicationDate: new Date(), author: { id: 1, name: 'Author 1', lastName: 'Last 1', dateOfBirth: new Date() }, genre: { id: 1, name: 'Genre 1' } };
        const action = bookActions.createSuccess({ entity: newBook });
        const newState = libraryReducer(initialState, action);

        expect(newState.books).toContain(newBook);
        expect(newState.totalBookAmount).toEqual(1);
        expect(newState.error).toBeNull();
    });

    it('should handle updateSuccess for books', () => {
        const initialStateWithBooks: LibraryState = {
            ...initialState,
            books: [{ id: 1, name: 'Book 1', publicationDate: new Date(), author: { id: 1, name: 'Author 1', lastName: 'Last 1', dateOfBirth: new Date() }, genre: { id: 1, name: 'Genre 1' } }],
        };
        const updatedBook: BookResponse = {
            id: 1,
            name: 'Updated Book 1',
            publicationDate: new Date(),
            author: { id: 1, name: 'Author 1', lastName: 'Last 1', dateOfBirth: new Date() },
            genre: { id: 1, name: 'Genre 1' }
        };
        const action = bookActions.updateSuccess({ entity: updatedBook });
        const newState = libraryReducer(initialStateWithBooks, action);

        expect(newState.books[0].name).toEqual(updatedBook.name);
        expect(newState.error).toBeNull();
    });

    it('should handle deleteByIdSuccess for books', () => {
        const initialStateWithBooks: LibraryState = {
            ...initialState,
            books: [{ id: 1, name: 'Book 1', publicationDate: new Date(), author: { id: 1, name: 'Author 1', lastName: 'Last 1', dateOfBirth: new Date() }, genre: { id: 1, name: 'Genre 1' } }],
            totalBookAmount: 1
        };
        const action = bookActions.deleteByIdSuccess({ id: 1 });
        const newState = libraryReducer(initialStateWithBooks, action);

        expect(newState.books.length).toEqual(0);
        expect(newState.totalBookAmount).toEqual(0);
        expect(newState.error).toBeNull();
    });

    // For other entities tests are simmilar

    it('should handle getPaginatedSuccess for authors', () => {
        const authors: AuthorResponse[] = [{ id: 1, name: 'Author 1', lastName: 'Last 1', dateOfBirth: new Date() }];
        const action = authorActions.getPaginatedSuccess({ entities: authors });
        const newState = libraryReducer(initialState, action);

        expect(newState.authors).toEqual(authors);
        expect(newState.error).toBeNull();
    });

    it('should handle createSuccess for genres', () => {
        const newGenre: GenreResponse = { id: 2, name: 'Genre 2' };
        const action = genreActions.createSuccess({ entity: newGenre });
        const newState = libraryReducer(initialState, action);

        expect(newState.genres).toContain(newGenre);
        expect(newState.totalGenreAmount).toEqual(1);
        expect(newState.error).toBeNull();
    });

    //And so on...
});