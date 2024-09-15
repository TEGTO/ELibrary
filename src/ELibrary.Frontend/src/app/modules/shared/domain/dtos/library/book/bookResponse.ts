import { AuthorResponse, GenreResponse, mapAuthorData, PublisherResponse } from "../../../..";

export enum CoverType {
    All = 0, Hard, Soft
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