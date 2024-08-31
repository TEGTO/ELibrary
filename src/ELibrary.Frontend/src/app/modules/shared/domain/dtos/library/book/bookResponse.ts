import { AuthorResponse, GenreResponse, mapAuthorData, UpdateBookRequest } from "../../../..";

export interface BookResponse {
    id: number;
    title: string;
    publicationDate: Date;
    author: AuthorResponse;
    genre: GenreResponse;
}
export function mapBookData(resp: BookResponse): BookResponse {
    return {
        ...resp,
        publicationDate: new Date(resp.publicationDate),
        author: mapAuthorData(resp.author)
    }
}
export function getBookFromUpdateRequest(request: UpdateBookRequest, author: AuthorResponse, genre: GenreResponse): BookResponse {
    return {
        id: request.id,
        title: request.title,
        publicationDate: request.publicationDate,
        author: author,
        genre: genre
    }
}