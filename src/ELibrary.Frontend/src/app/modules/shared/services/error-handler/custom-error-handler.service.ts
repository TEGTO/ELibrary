/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { ErrorHandler } from './error-handler';

@Injectable({
  providedIn: 'root'
})
export class CustomErrorHandler implements ErrorHandler {
  handleError(error: any): string {
    let errorMessage;
    if (error.message) {
      errorMessage = error.message;
    }
    console.error(errorMessage);
    return errorMessage;
  }

  handleApiError(error: any): string {
    let errorMessage;
    if (error.error) {
      if (error.error.messages) {
        errorMessage = error.error.messages.join('\n');
      }
    } else if (error.message) {
      errorMessage = error.message;
    }
    if (!errorMessage) {
      const statusCode = getStatusCodeDescription(error.status);
      errorMessage = `An unknown error occurred! (${statusCode})`
    }
    console.error(errorMessage);
    return errorMessage;
  }

}

export function getStatusCodeDescription(statusCode: number): string {
  return HttpStatusCodes[statusCode] || 'Unknown Status Code';
}
export const HttpStatusCodes: Record<number, string> = {
  400: 'Bad Request',
  401: 'Unauthorized',
  403: 'Forbidden',
  404: 'Not Found',
  500: 'Internal Server Error',
};