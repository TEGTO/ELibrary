import { Injectable } from "@angular/core";
import { Command } from "..";

@Injectable()
export abstract class CommandHandler<T extends Command> {
    abstract dispatch(command: T): void;
}