import { Command, Order } from "../../../../shared";

export interface ManagerCancelOrderCommand extends Command {
    order: Order;
}