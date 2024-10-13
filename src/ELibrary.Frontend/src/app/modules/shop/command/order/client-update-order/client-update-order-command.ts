import { Command, Order } from "../../../../shared";

export interface ClientUpdateOrderCommand extends Command {
    order: Order;
}