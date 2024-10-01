import { authorActions, bookActions, genreActions, publisherActions } from "..";
import { Author, Book, Genre, getDefaultBook, Publisher } from "../../shared";
import { authorReducer, AuthorState, bookReducer, BookState, genreReducer, GenreState, publisherReducer, PublisherState } from "./library.reducer";

describe('Library Reducer', () => {
    //#region Author Tests
    const initialAuthorState: AuthorState = {
        authors: [],
        totalAuthorAmount: 0,
        error: null
    };

    it('should handle getPaginatedSuccess for authors', () => {
        const authors: Author[] = [{ id: 1, name: 'Author 1', lastName: 'Last 1', dateOfBirth: new Date() }];
        const action = authorActions.getPaginatedSuccess({ entities: authors });
        const newState = authorReducer(initialAuthorState, action);

        expect(newState.authors).toEqual(authors);
        expect(newState.error).toBeNull();
    });

    it('should handle createSuccess for authors', () => {
        const newAuthor: Author = { id: 2, name: 'Author 2', lastName: 'Last 2', dateOfBirth: new Date() };
        const action = authorActions.createSuccess({ entity: newAuthor });
        const newState = authorReducer(initialAuthorState, action);

        expect(newState.authors).toContain(newAuthor);
        expect(newState.totalAuthorAmount).toEqual(1);
        expect(newState.error).toBeNull();
    });

    it('should handle deleteByIdSuccess for authors', () => {
        const initialStateWithAuthors: AuthorState = {
            ...initialAuthorState,
            authors: [{ id: 1, name: 'Author 1', lastName: 'Last 1', dateOfBirth: new Date() }],
            totalAuthorAmount: 1
        };
        const action = authorActions.deleteByIdSuccess({ id: 1 });
        const newState = authorReducer(initialStateWithAuthors, action);

        expect(newState.authors.length).toEqual(0);
        expect(newState.totalAuthorAmount).toEqual(0);
        expect(newState.error).toBeNull();
    });

    it('should handle getTotalAmountSuccess for authors', () => {
        const amount = 5;
        const action = authorActions.getTotalAmountSuccess({ amount });
        const newState = authorReducer(initialAuthorState, action);

        expect(newState.totalAuthorAmount).toEqual(amount);
        expect(newState.error).toBeNull();
    });

    it('should handle getPaginatedFailure for authors', () => {
        const error = 'Failed to fetch authors';
        const action = authorActions.getPaginatedFailure({ error });
        const newState = authorReducer(initialAuthorState, action);

        expect(newState.error).toEqual(error);
    });
    //#endregion

    //#region Genre Tests
    const initialGenreState: GenreState = {
        genres: [],
        totalGenreAmount: 0,
        error: null
    };

    it('should handle getPaginatedSuccess for genres', () => {
        const genres: Genre[] = [{ id: 1, name: 'Genre 1' }];
        const action = genreActions.getPaginatedSuccess({ entities: genres });
        const newState = genreReducer(initialGenreState, action);

        expect(newState.genres).toEqual(genres);
        expect(newState.error).toBeNull();
    });

    it('should handle createSuccess for genres', () => {
        const newGenre: Genre = { id: 2, name: 'Genre 2' };
        const action = genreActions.createSuccess({ entity: newGenre });
        const newState = genreReducer(initialGenreState, action);

        expect(newState.genres).toContain(newGenre);
        expect(newState.totalGenreAmount).toEqual(1);
        expect(newState.error).toBeNull();
    });

    it('should handle deleteByIdSuccess for genres', () => {
        const initialStateWithGenres: GenreState = {
            ...initialGenreState,
            genres: [{ id: 1, name: 'Genre 1' }],
            totalGenreAmount: 1
        };
        const action = genreActions.deleteByIdSuccess({ id: 1 });
        const newState = genreReducer(initialStateWithGenres, action);

        expect(newState.genres.length).toEqual(0);
        expect(newState.totalGenreAmount).toEqual(0);
        expect(newState.error).toBeNull();
    });

    it('should handle getTotalAmountSuccess for genres', () => {
        const amount = 7;
        const action = genreActions.getTotalAmountSuccess({ amount });
        const newState = genreReducer(initialGenreState, action);

        expect(newState.totalGenreAmount).toEqual(amount);
        expect(newState.error).toBeNull();
    });

    it('should handle getPaginatedFailure for genres', () => {
        const error = 'Failed to fetch genres';
        const action = genreActions.getPaginatedFailure({ error });
        const newState = genreReducer(initialGenreState, action);

        expect(newState.error).toEqual(error);
    });
    //#endregion

    //#region Publisher Tests
    const initialPublisherState: PublisherState = {
        publishers: [],
        totalPublisherAmount: 0,
        error: null
    };

    it('should handle getPaginatedSuccess for publishers', () => {
        const publishers: Publisher[] = [{ id: 1, name: 'Publisher 1' }];
        const action = publisherActions.getPaginatedSuccess({ entities: publishers });
        const newState = publisherReducer(initialPublisherState, action);

        expect(newState.publishers).toEqual(publishers);
        expect(newState.error).toBeNull();
    });

    it('should handle createSuccess for publishers', () => {
        const newPublisher: Publisher = { id: 2, name: 'Publisher 2' };
        const action = publisherActions.createSuccess({ entity: newPublisher });
        const newState = publisherReducer(initialPublisherState, action);

        expect(newState.publishers).toContain(newPublisher);
        expect(newState.totalPublisherAmount).toEqual(1);
        expect(newState.error).toBeNull();
    });

    it('should handle deleteByIdSuccess for publishers', () => {
        const initialStateWithPublishers: PublisherState = {
            ...initialPublisherState,
            publishers: [{ id: 1, name: 'Publisher 1' }],
            totalPublisherAmount: 1
        };
        const action = publisherActions.deleteByIdSuccess({ id: 1 });
        const newState = publisherReducer(initialStateWithPublishers, action);

        expect(newState.publishers.length).toEqual(0);
        expect(newState.totalPublisherAmount).toEqual(0);
        expect(newState.error).toBeNull();
    });

    it('should handle getTotalAmountSuccess for publishers', () => {
        const amount = 3;
        const action = publisherActions.getTotalAmountSuccess({ amount });
        const newState = publisherReducer(initialPublisherState, action);

        expect(newState.totalPublisherAmount).toEqual(amount);
        expect(newState.error).toBeNull();
    });

    it('should handle getPaginatedFailure for publishers', () => {
        const error = 'Failed to fetch publishers';
        const action = publisherActions.getPaginatedFailure({ error });
        const newState = publisherReducer(initialPublisherState, action);

        expect(newState.error).toEqual(error);
    });
    //#endregion

    //#region Book Tests
    const initialBookState: BookState = {
        books: [],
        totalBookAmount: 0,
        error: null
    };

    it('should handle getPaginatedSuccess for books', () => {
        const books: Book[] = [getDefaultBook(), getDefaultBook()];
        const action = bookActions.getPaginatedSuccess({ entities: books });
        const newState = bookReducer(initialBookState, action);

        expect(newState.books).toEqual(books);
        expect(newState.error).toBeNull();
    });

    it('should handle createSuccess for books', () => {
        const newBook: Book = getDefaultBook();
        const action = bookActions.createSuccess({ entity: newBook });
        const newState = bookReducer(initialBookState, action);

        expect(newState.books).toContain(newBook);
        expect(newState.totalBookAmount).toEqual(1);
        expect(newState.error).toBeNull();
    });

    it('should handle deleteByIdSuccess for books', () => {
        const initialStateWithBooks: BookState = {
            ...initialBookState,
            books: [{ ...getDefaultBook(), id: 1 }],
            totalBookAmount: 1
        };
        const action = bookActions.deleteByIdSuccess({ id: 1 });
        const newState = bookReducer(initialStateWithBooks, action);

        expect(newState.books.length).toEqual(0);
        expect(newState.totalBookAmount).toEqual(0);
        expect(newState.error).toBeNull();
    });

    it('should handle getTotalAmountSuccess for books', () => {
        const amount = 8;
        const action = bookActions.getTotalAmountSuccess({ amount });
        const newState = bookReducer(initialBookState, action);

        expect(newState.totalBookAmount).toEqual(amount);
        expect(newState.error).toBeNull();
    });

    it('should handle getPaginatedFailure for books', () => {
        const error = 'Failed to fetch books';
        const action = bookActions.getPaginatedFailure({ error });
        const newState = bookReducer(initialBookState, action);

        expect(newState.error).toEqual(error);
    });
    //#endregion
});
