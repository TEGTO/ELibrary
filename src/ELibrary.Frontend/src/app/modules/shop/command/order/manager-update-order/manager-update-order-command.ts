import { Command, Order } from "../../../../shared";

export interface ManagerUpdateOrderCommand extends Command {
    order: Order;
}