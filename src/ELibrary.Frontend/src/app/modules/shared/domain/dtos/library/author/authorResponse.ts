import { Author } from "../../../..";

export interface AuthorResponse {
    id: number;
    name: string;
    lastName: string;
    dateOfBirth: Date;
}
export function mapAuthorResponseToAuthor(response: AuthorResponse): Author {
    return {
        id: response.id,
        name: response.name,
        lastName: response.lastName,
        dateOfBirth: new Date(response.dateOfBirth)
    }
}