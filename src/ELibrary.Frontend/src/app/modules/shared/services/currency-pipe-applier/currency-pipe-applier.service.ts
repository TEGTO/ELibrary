/* eslint-disable @typescript-eslint/no-explicit-any */
import { CurrencyPipe } from '@angular/common';
import { Injectable } from '@angular/core';
import { LocaleService } from '../..';
import { CurrencyPipeApplier } from './currency-pipe-applier';

@Injectable({
  providedIn: 'root'
})
export class CurrencyPipeApplierService implements CurrencyPipeApplier {
  currencyPipe!: CurrencyPipe;

  constructor
    (
      private readonly localeService: LocaleService
    ) {
    this.currencyPipe = new CurrencyPipe(this.localeService.getLocale(), this.localeService.getCurrency());
  }

  applyCurrencyPipe(value: any) {
    return this.currencyPipe ? this.currencyPipe.transform(value) : value;
  }
}
