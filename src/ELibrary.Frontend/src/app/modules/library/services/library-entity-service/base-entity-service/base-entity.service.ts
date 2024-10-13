/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export abstract class BaseControllerService<TResponse, TFilterRequest, TCreateRequest, TUpdateRequest> {
  constructor(
    private readonly apiService: any,
    private readonly store: Store,
    private readonly actions: any,
    private readonly selectors: any
  ) { }

  getById(id: number): Observable<TResponse> {
    return this.apiService.getById(id);
  }

  getPaginated(request: TFilterRequest): Observable<TResponse[]> {
    this.store.dispatch(this.actions.getPaginated({ request }));
    return this.store.select(this.selectors.selectItems);
  }

  getItemTotalAmount(request: TFilterRequest): Observable<number> {
    this.store.dispatch(this.actions.getTotalAmount({ request }));
    return this.store.select(this.selectors.selectTotalAmount);
  }

  create(request: TCreateRequest): void {
    this.store.dispatch(this.actions.create({ request }));
  }

  update(request: TUpdateRequest): void {
    this.store.dispatch(this.actions.update({ request }));
  }

  deleteById(id: number): void {
    this.store.dispatch(this.actions.deleteById({ id }));
  }
}
