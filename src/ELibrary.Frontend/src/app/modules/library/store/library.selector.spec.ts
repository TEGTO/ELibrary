// import { AuthorState, BookState, GenreState, PublisherState, selectAuthorAmount, selectAuthorError, selectAuthors, selectAuthorState, selectBookAmount, selectBookError, selectBooks, selectBoookState, selectGenreAmount, selectGenreError, selectGenres, selectGenreState, selectPublisherAmount, selectPublisherError, selectPublishers, selectPublisherState } from "..";

// describe('Library Selectors', () => {
//     const initialAuthorState: AuthorState = {
//         authors: [],
//         totalAuthorAmount: 2,
//         error: null
//     };

//     const initialGenreState: GenreState = {
//         genres: [],
//         totalGenreAmount: 3,
//         error: null
//     };

//     const initialPublisherState: PublisherState = {
//         publishers: [],
//         totalPublisherAmount: 4,
//         error: null
//     };

//     const initialBookState: BookState = {
//         books: [],
//         totalBookAmount: 5,
//         error: null
//     };

//     //#region Author Tests
//     it('should select author state', () => {
//         const result = selectAuthorState.projector(initialAuthorState);
//         expect(result).toEqual(initialAuthorState);
//     });

//     it('should select authors', () => {
//         const result = selectAuthors.projector(initialAuthorState);
//         expect(result).toEqual(initialAuthorState.authors);
//     });

//     it('should select total author amount', () => {
//         const result = selectAuthorAmount.projector(initialAuthorState);
//         expect(result).toEqual(initialAuthorState.totalAuthorAmount);
//     });

//     it('should select author error', () => {
//         const result = selectAuthorError.projector(initialAuthorState);
//         expect(result).toEqual(initialAuthorState.error);
//     });
//     //#endregion

//     //#region Genre Tests
//     it('should select genre state', () => {
//         const result = selectGenreState.projector(initialGenreState);
//         expect(result).toEqual(initialGenreState);
//     });

//     it('should select genres', () => {
//         const result = selectGenres.projector(initialGenreState);
//         expect(result).toEqual(initialGenreState.genres);
//     });

//     it('should select total genre amount', () => {
//         const result = selectGenreAmount.projector(initialGenreState);
//         expect(result).toEqual(initialGenreState.totalGenreAmount);
//     });

//     it('should select genre error', () => {
//         const result = selectGenreError.projector(initialGenreState);
//         expect(result).toEqual(initialGenreState.error);
//     });
//     //#endregion

//     //#region Publisher Tests
//     it('should select publisher state', () => {
//         const result = selectPublisherState.projector(initialPublisherState);
//         expect(result).toEqual(initialPublisherState);
//     });

//     it('should select publishers', () => {
//         const result = selectPublishers.projector(initialPublisherState);
//         expect(result).toEqual(initialPublisherState.publishers);
//     });

//     it('should select total publisher amount', () => {
//         const result = selectPublisherAmount.projector(initialPublisherState);
//         expect(result).toEqual(initialPublisherState.totalPublisherAmount);
//     });

//     it('should select publisher error', () => {
//         const result = selectPublisherError.projector(initialPublisherState);
//         expect(result).toEqual(initialPublisherState.error);
//     });
//     //#endregion

//     //#region Book Tests
//     it('should select book state', () => {
//         const result = selectBoookState.projector(initialBookState);
//         expect(result).toEqual(initialBookState);
//     });

//     it('should select books', () => {
//         const result = selectBooks.projector(initialBookState);
//         expect(result).toEqual(initialBookState.books);
//     });

//     it('should select total book amount', () => {
//         const result = selectBookAmount.projector(initialBookState);
//         expect(result).toEqual(initialBookState.totalBookAmount);
//     });

//     it('should select book error', () => {
//         const result = selectBookError.projector(initialBookState);
//         expect(result).toEqual(initialBookState.error);
//     });
//     //#endregion
// });
