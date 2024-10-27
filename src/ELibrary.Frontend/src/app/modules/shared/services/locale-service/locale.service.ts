import { Inject, Injectable, LOCALE_ID } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocaleService {
  private readonly currentCurrency = 'UAH';

  constructor(@Inject(LOCALE_ID) private readonly locale: string) { }

  getLocale(): string {
    return this.locale;
  }

  getCurrency(): string {
    return this.currentCurrency;
  }
}
