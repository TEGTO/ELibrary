import { AuthorResponse, Book, CoverType, GenreResponse, mapAuthorResponseToAuthor, mapGenreResponseToGenre, mapPublisherResponseToPublisher, PublisherResponse } from "../../../..";

export interface BookResponse {
    id: number;
    name: string;
    publicationDate: Date;
    price: number;
    coverType: CoverType;
    pageAmount: number;
    stockAmount: number;
    author: AuthorResponse;
    genre: GenreResponse;
    publisher: PublisherResponse;
}
export function mapBookResponseToBook(response: BookResponse): Book {
    return {
        id: response.id,
        name: response.name,
        publicationDate: new Date(response.publicationDate),
        price: response.price,
        coverType: response.coverType,
        pageAmount: response.pageAmount,
        stockAmount: response.stockAmount,
        author: mapAuthorResponseToAuthor(response.author),
        genre: mapGenreResponseToGenre(response.genre),
        publisher: mapPublisherResponseToPublisher(response.publisher),
    }
}