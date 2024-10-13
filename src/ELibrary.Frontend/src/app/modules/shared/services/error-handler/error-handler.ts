/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from "@angular/core";

@Injectable({
    providedIn: 'root'
})
export abstract class ErrorHandler {
    abstract handleApiError(error: any): string;
    abstract handleError(error: any): string;
}
