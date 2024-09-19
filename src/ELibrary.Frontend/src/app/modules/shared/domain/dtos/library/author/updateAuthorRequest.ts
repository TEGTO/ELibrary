import { Author } from "../../../models/library/author";

export interface UpdateAuthorRequest {
    id: number;
    name: string;
    lastName: string;
    dateOfBirth: Date;
}
export function mapAuthorToUpdateAuthorRequest(author: Author): UpdateAuthorRequest {
    return {
        id: author.id,
        name: author.name,
        lastName: author.lastName,
        dateOfBirth: author.dateOfBirth
    }
}