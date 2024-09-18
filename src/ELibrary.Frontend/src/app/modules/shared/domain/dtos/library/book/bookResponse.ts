import { AuthorResponse, GenreResponse, getDefaultAuthorResponse, getDefaultGenreResponse, getDefaultPublisherResponse, mapAuthorData, PublisherResponse } from "../../../..";

export enum CoverType {
    Any = 0, Hard = 1, Soft = 2
}
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
export function mapBookData(resp: BookResponse): BookResponse {
    return {
        ...resp,
        publicationDate: new Date(resp.publicationDate),
        author: mapAuthorData(resp.author)
    }
}
export function getDefaultBookResponse(): BookResponse {
    return {
        id: 0,
        name: "",
        publicationDate: new Date(),
        price: 0,
        coverType: CoverType.Hard,
        pageAmount: 0,
        stockAmount: 0,
        author: getDefaultAuthorResponse(),
        genre: getDefaultGenreResponse(),
        publisher: getDefaultPublisherResponse()
    }
}