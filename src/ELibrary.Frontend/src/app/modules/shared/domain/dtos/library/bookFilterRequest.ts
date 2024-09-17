import { CoverType, LibraryFilterRequest } from "../../..";

export interface BookFilterRequest extends LibraryFilterRequest {
    publicationFromUTC: Date | null;
    publicationToUTC: Date | null;
    minPrice: number | null;
    maxPrice: number | null;
    coverType: CoverType | null;
    onlyInStock: boolean | null;
    minPageAmount: number | null;
    maxPageAmount: number | null;
    authorId: number | null;
    genreId: number | null;
    publisherId: number | null;
}

export function defaultBookFilterRequest(): BookFilterRequest {
    return {
        containsName: "",
        pageNumber: 0,
        pageSize: 0,
        publicationFromUTC: null,
        publicationToUTC: null,
        minPrice: null,
        maxPrice: null,
        coverType: null,
        onlyInStock: false,
        minPageAmount: null,
        maxPageAmount: null,
        authorId: null,
        genreId: null,
        publisherId: null,
    };
}