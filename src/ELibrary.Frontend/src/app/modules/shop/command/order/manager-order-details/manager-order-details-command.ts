import { Command, Order } from "../../../../shared";

export interface ManagerOrderDetailsCommand extends Command {
    order: Order;
}