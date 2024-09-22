import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { CartControllerService, CartDialogComponent, CartEffects, cartReducer, CartService, ClientChangeDialogComponent, ClientControllerService, ClientService, ShopCommand, ShopCommandService, ShopDialogManager, ShopDialogManagerService, ShoppingCartButtonComponent } from '.';
import { AuthenticationModule } from '../authentication/authentication.module';
import { InputRangeDirective } from '../shared';

@NgModule({
  declarations: [
    ShoppingCartButtonComponent,
    ClientChangeDialogComponent,
    CartDialogComponent
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    MatInputModule,
    FormsModule,
    AuthenticationModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    InputRangeDirective,
    ScrollingModule,
    MatDialogModule,
    MatMenuModule,
    StoreModule.forFeature('cart', cartReducer),
    EffectsModule.forFeature([CartEffects]),
  ],
  providers: [
    { provide: CartService, useClass: CartControllerService },
    { provide: ClientService, useClass: ClientControllerService },
    { provide: ShopCommand, useClass: ShopCommandService },
    { provide: ShopDialogManager, useClass: ShopDialogManagerService },
  ],
  exports: [ShoppingCartButtonComponent],
})
export class ShopModule { }
