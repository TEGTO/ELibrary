import { CoverType, LibraryFilterRequest } from "../../..";

export interface BookFilterRequest extends LibraryFilterRequest {
    publicationFromUTC: Date;
    publicationToUTC: Date;
    minPrice: number;
    maxPrice: number;
    coverType: CoverType;
    onlyInStock: boolean;
    minPageAmount: number;
    maxPageAmount: number;
    authorId: number | null;
    genreId: number | null;
    publisherId: number | null;
}

export function defaultBookFilterRequest(): BookFilterRequest {
    return {
        containsName: "",
        pageNumber: 0,
        pageSize: 0,
        publicationFromUTC: new Date(-8640000000000000),
        publicationToUTC: new Date(8640000000000000),
        minPrice: 0,
        maxPrice: Number.MAX_VALUE,
        coverType: 1,
        onlyInStock: false,
        minPageAmount: 0,
        maxPageAmount: Number.MAX_VALUE,
        authorId: null,
        genreId: null,
        publisherId: null,
    };
}