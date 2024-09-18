import { BookResponse, CoverType } from "../../../..";

export interface UpdateBookRequest {
    id: number;
    name: string;
    publicationDate: Date;
    price: number;
    coverType: CoverType;
    pageAmount: number;
    stockAmount: number;
    authorId: number;
    genreId: number;
    publisherId: number;
}
export function bookToUpdateRequest(book: BookResponse): UpdateBookRequest {
    return {
        id: book.id,
        name: book.name,
        publicationDate: book.publicationDate,
        price: book.price,
        coverType: book.coverType,
        pageAmount: book.pageAmount,
        stockAmount: book.stockAmount,
        authorId: book.author.id,
        genreId: book.genre.id,
        publisherId: book.publisher.id
    }
}