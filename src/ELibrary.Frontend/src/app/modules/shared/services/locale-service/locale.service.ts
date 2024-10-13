import { Inject, Injectable, LOCALE_ID } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocaleService {
  private currentCurrency = 'UAH';

  constructor(@Inject(LOCALE_ID) private locale: string) { }

  getLocale(): string {
    return this.locale;
  }

  getCurrency(): string {
    return this.currentCurrency;
  }
}
