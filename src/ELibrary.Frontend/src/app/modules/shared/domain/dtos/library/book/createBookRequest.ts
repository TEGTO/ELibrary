import { BookResponse } from "../../../..";

export interface CreateBookRequest {
    title: string;
    publicationDate: Date;
    authorId: number;
    genreId: number;
}
export function bookToCreateRequest(book: BookResponse): CreateBookRequest {
    return {
        title: book.title,
        publicationDate: book.publicationDate,
        authorId: book.author.id,
        genreId: book.genre.id
    }
}