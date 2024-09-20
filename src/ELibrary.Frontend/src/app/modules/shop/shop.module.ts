import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { CartControllerService, CartEffects, cartReducer, CartService, ClientChangeDialogComponent, ClientControllerService, ClientService, ShoppingCardButtonComponent } from '.';

@NgModule({
  declarations: [
    ShoppingCardButtonComponent,
    ClientChangeDialogComponent
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    MatInputModule,
    FormsModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    StoreModule.forFeature('cart', cartReducer),
    EffectsModule.forFeature([CartEffects]),
  ],
  providers: [
    { provide: CartService, useClass: CartControllerService },
    { provide: ClientService, useClass: ClientControllerService },
  ],
  exports: [ShoppingCardButtonComponent],
})
export class ShopModule { }
