import { InjectionToken } from "@angular/core";
import { AddBookStockOrderCommand, AddClientCommand, CartAddBookCommand, ClientAddOrderCommand, ClientCancelOrderCommand, ClientStartAddingOrderCommand, ClientUpdateOrderCommand, DeleteCartBookCommand, ManagerCancelOrderCommand, ManagerOrderDetailsCommand, ManagerUpdateOrderCommand, StartAddingClientCommand, UpdateCartBookCommand, UpdateClientCommand } from "..";
import { CommandHandler } from "../../shared";


export const CART_ADD_BOOK_COMMAND_HANDLER = new InjectionToken<CommandHandler<CartAddBookCommand>>('CartAddBookCommandHandler');
export const UPDATE_CART_BOOK_COMMAND_HANDLER = new InjectionToken<CommandHandler<UpdateCartBookCommand>>('UpdateCartBookCommandHandler');
export const DELETE_CART_BOOK_COMMAND_HANDLER = new InjectionToken<CommandHandler<DeleteCartBookCommand>>('DeleteCartBookCommandHandler');

export const START_ADDING_CLIENT_COMMAND_HANDLER = new InjectionToken<CommandHandler<StartAddingClientCommand>>('StartAddingClientCommandHandler');
export const ADD_CLIENT_COMMAND_HANDLER = new InjectionToken<CommandHandler<AddClientCommand>>('AddClientCommandHandler');
export const UPDATE_CLIENT_COMMAND_HANDLER = new InjectionToken<CommandHandler<UpdateClientCommand>>('UpdateClientCommandHandler');

export const CLIENT_START_ADDING_ORDER_COMMAND_HANDLER = new InjectionToken<CommandHandler<ClientStartAddingOrderCommand>>('ClientStartAddingOrderCommandHandler');
export const CLIENT_ADD_ORDER_COMMAND_HANDLER = new InjectionToken<CommandHandler<ClientAddOrderCommand>>('ClientAddOrderCommandHandler');
export const CLIENT_UPDATE_ORDER_COMMAND_HANDLER = new InjectionToken<CommandHandler<ClientUpdateOrderCommand>>('ClientUpdateOrderCommandHandler');
export const CLIENT_CANCEL_ORDER_COMMAND_HANDLER = new InjectionToken<CommandHandler<ClientCancelOrderCommand>>('ClientCancelOrderCommandHandler');
export const MANAGER_UPDATE_ORDER_COMMAND_HANDLER = new InjectionToken<CommandHandler<ManagerUpdateOrderCommand>>('ManagerUpdateOrderCommandHandler');
export const MANAGER_CANCEL_ORDER_COMMAND_HANDLER = new InjectionToken<CommandHandler<ManagerCancelOrderCommand>>('ManagerCancelOrderCommandHandler');
export const MANAGER_ORDER_DETAILS_COMMAND_HANDLER = new InjectionToken<CommandHandler<ManagerOrderDetailsCommand>>('ManagerOrderDetailsCommandHandler');

export const ADD_BOOKSTOCK_ORDER_COMMAND_HANDLER = new InjectionToken<CommandHandler<AddBookStockOrderCommand>>('AddBookStockOrderCommandHandler');