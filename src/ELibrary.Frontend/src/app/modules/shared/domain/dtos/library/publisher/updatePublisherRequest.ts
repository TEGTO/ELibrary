import { PublisherResponse } from "../../../..";

export interface UpdatePublisherRequest {
    id: number;
    name: string;
}
export function publisherToUpdateRequest(genre: PublisherResponse): UpdatePublisherRequest {
    return {
        id: genre.id,
        name: genre.name
    }
}