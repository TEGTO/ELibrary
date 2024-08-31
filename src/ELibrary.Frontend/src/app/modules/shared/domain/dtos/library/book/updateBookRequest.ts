import { BookResponse } from "../../../..";

export interface UpdateBookRequest {
    id: number;
    title: string;
    publicationDate: Date;
    authorId: number;
    genreId: number;
}
export function bookToUpdateRequest(book: BookResponse): UpdateBookRequest {
    return {
        id: book.id,
        title: book.title,
        publicationDate: book.publicationDate,
        authorId: book.author.id,
        genreId: book.genre.id,
    }
}