import { PublisherResponse } from "../../../..";

export interface CreatePublisherRequest {
    name: string;
}
export function publisherToCreateRequest(genre: PublisherResponse): CreatePublisherRequest {
    return {
        name: genre.name
    }
}