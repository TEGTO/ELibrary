import { Command, Publisher } from "../../../../shared";

export interface DeletePublisherCommand extends Command {
    publisher: Publisher;
}