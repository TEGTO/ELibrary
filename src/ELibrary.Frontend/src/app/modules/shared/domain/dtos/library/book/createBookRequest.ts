import { BookResponse } from "../../../..";

export interface CreateBookRequest {
    name: string;
    publicationDate: Date;
    authorId: number;
    genreId: number;
}
export function bookToCreateRequest(book: BookResponse): CreateBookRequest {
    return {
        name: book.name,
        publicationDate: book.publicationDate,
        authorId: book.author.id,
        genreId: book.genre.id
    }
}