/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export abstract class CurrencyPipeApplier {
    abstract applyCurrencyPipe(value: any): any;

}
