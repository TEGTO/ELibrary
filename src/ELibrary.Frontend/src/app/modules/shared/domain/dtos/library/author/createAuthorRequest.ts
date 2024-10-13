import { Author } from "../../../..";

export interface CreateAuthorRequest {
    name: string;
    lastName: string;
    dateOfBirth: Date;
}
export function mapAuthorToCreateAuthorRequest(author: Author): CreateAuthorRequest {
    return {
        name: author.name,
        lastName: author.lastName,
        dateOfBirth: author.dateOfBirth
    }
}