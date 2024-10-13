import { Command, Order } from "../../../../shared";

export interface ClientCancelOrderCommand extends Command {
    order: Order
}