import { Injectable } from "@angular/core";

export enum ShopCommandObject {
    Client, Order
}
export enum ShopCommandType {
    Create, Update, Delete
}
@Injectable({
    providedIn: 'root'
})
export abstract class ShopCommand {
    abstract dispatchCommand(commandObject: ShopCommandObject, commandType: ShopCommandType, dispatchedFrom: any, ...params: any): void;
}
