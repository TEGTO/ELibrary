/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";

export enum ShopCommandObject {
    Client, Order, Cart
}
export enum ShopCommandType {
    Add, Update, Delete
}
export enum ShopCommandRole {
    Client, Manager, Administrator
}
@Injectable({
    providedIn: 'root'
})
export abstract class ShopCommand {
    abstract dispatchCommand(commandObject: ShopCommandObject, commandType: ShopCommandType, commandRole: ShopCommandRole, dispatchedFrom: any, ...params: any): void;
}
