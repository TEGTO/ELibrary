import { Injectable } from "@angular/core";

export enum LibraryCommandObject {
    Author, Genre, Publisher, Book
}
export enum LibraryCommandType {
    Create, Update, Delete
}
@Injectable({
    providedIn: 'root'
})
export abstract class LibraryCommand {
    abstract dispatchCommand(commandObject: LibraryCommandObject, commandType: LibraryCommandType, dispatchedFrom: any, ...params: any): void;
}
