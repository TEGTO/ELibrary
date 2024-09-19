import { Publisher } from "../../../..";

export interface PublisherResponse {
    id: number;
    name: string;
}
export function mapPublisherResponseToPublisher(response: PublisherResponse): Publisher {
    return {
        id: response.id,
        name: response.name,
    }
}