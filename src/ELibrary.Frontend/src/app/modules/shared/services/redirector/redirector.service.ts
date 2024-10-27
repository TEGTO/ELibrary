/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RedirectorService {

  constructor(
    private readonly router: Router
  ) { }

  redirectToHome(): void {
    this.router.navigate(['']);
  }
  redirectTo(path: string, queryParams?: Record<string, any>): void {
    this.router.navigate([path], { queryParams });
  }
  redirectToExternalUrl(url: string): void {
    window.location.href = url;
  }
}
