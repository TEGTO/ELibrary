import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { boockstockOrderReducer, BookstockEffects, BookstockOrderControllerService, BookstockOrderService, BookStockReplenishmentComponent, CartControllerService, CartDialogComponent, CartEffects, cartReducer, CartService, ClientChangeDialogComponent, ClientControllerService, ClientEffects, ClientInfoComponent, clientReducer, ClientService, InputMaxStockDirective, managerOrderReducer, OrderControllerService, OrderEffects, orderReducer, OrderService, ShopCommand, ShopCommandService, ShopDialogManager, ShopDialogManagerService, ShoppingCartButtonComponent } from '.';
import { AuthenticationModule } from '../authentication/authentication.module';
import { LibraryModule } from '../library/library.module';
import { InputRangeDirective } from '../shared';

@NgModule({
  declarations: [
    ShoppingCartButtonComponent,
    ClientChangeDialogComponent,
    CartDialogComponent,
    ClientInfoComponent,
    InputMaxStockDirective,
    BookStockReplenishmentComponent,
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
    MatDatepickerModule,
    MatNativeDateModule,
    InputRangeDirective,
    ScrollingModule,
    MatExpansionModule,
    MatDialogModule,
    LibraryModule,
    MatMenuModule,
    MatSelectModule,
    MatAutocompleteModule,
    StoreModule.forFeature('cart', cartReducer),
    StoreModule.forFeature('client', clientReducer),
    StoreModule.forFeature('order', orderReducer),
    StoreModule.forFeature('managerorder', managerOrderReducer),
    StoreModule.forFeature('bookstockorder', boockstockOrderReducer),
    EffectsModule.forFeature([CartEffects, ClientEffects, OrderEffects, BookstockEffects]),
  ],
  providers: [
    { provide: CartService, useClass: CartControllerService },
    { provide: ClientService, useClass: ClientControllerService },
    { provide: ShopCommand, useClass: ShopCommandService },
    { provide: ShopDialogManager, useClass: ShopDialogManagerService },
    { provide: OrderService, useClass: OrderControllerService },
    { provide: BookstockOrderService, useClass: BookstockOrderControllerService },
  ],
  exports: [ShoppingCartButtonComponent, ClientInfoComponent],
})
export class ShopModule { }
