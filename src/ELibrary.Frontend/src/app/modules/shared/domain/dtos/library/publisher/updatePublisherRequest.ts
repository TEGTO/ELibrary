import { Publisher } from "../../../..";

export interface UpdatePublisherRequest {
    id: number;
    name: string;
}
export function mapPublisherToUpdatePublisherRequest(publisher: Publisher): UpdatePublisherRequest {
    return {
        id: publisher.id,
        name: publisher.name
    }
}