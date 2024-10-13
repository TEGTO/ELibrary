import { Command, Publisher } from "../../../../shared";

export interface UpdatePublisherCommand extends Command {
    publisher: Publisher;
}