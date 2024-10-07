
import { Command, Order } from "../../../../shared";

export interface ClientAddOrderCommand extends Command {
    order: Order;
}