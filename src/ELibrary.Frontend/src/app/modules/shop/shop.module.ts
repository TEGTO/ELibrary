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
import { ADD_BOOKSTOCK_ORDER_COMMAND_HANDLER, ADD_CLIENT_COMMAND_HANDLER, AddBookStockCommandHandlerService, AddClientCommandHandlerService, boockstockOrderReducer, BookstockEffects, BookstockOrderControllerService, BookstockOrderService, BookStockReplenishmentComponent, CART_ADD_BOOK_COMMAND_HANDLER, CartAddBookCommandHandlerService, CartControllerService, CartDialogComponent, CartEffects, cartReducer, CartService, CLIENT_ADD_ORDER_COMMAND_HANDLER, CLIENT_CANCEL_ORDER_COMMAND_HANDLER, CLIENT_UPDATE_ORDER_COMMAND_HANDLER, ClientAddOrderCommandHandlerService, ClientCancelOrderCommandHandlerService, ClientChangeDialogComponent, ClientControllerService, ClientEffects, ClientInfoComponent, clientReducer, ClientService, ClientUpdateOrderCommandHandlerService, DELETE_CART_BOOK_COMMAND_HANDLER, DELETE_CLIENT_COMMAND_HANDLER, DeleteCartBookCommandHandlerService, DeleteClientCommandHandlerService, InputMaxStockDirective, MANAGER_CANCEL_ORDER_COMMAND_HANDLER, MANAGER_UPDATE_ORDER_COMMAND_HANDLER, ManagerCancelOrderCommandHandlerService, managerOrderReducer, ManagerUpdateOrderCommandHandlerService, OrderControllerService, OrderEffects, orderReducer, OrderService, ShopDialogManager, ShopDialogManagerService, ShoppingCartButtonComponent, UPDATE_CART_BOOK_COMMAND_HANDLER, UPDATE_CLIENT_COMMAND_HANDLER, UpdateCartBookCommandHandlerService, UpdateClientCommandHandlerService } from '.';
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
    { provide: ShopDialogManager, useClass: ShopDialogManagerService },
    { provide: OrderService, useClass: OrderControllerService },
    { provide: BookstockOrderService, useClass: BookstockOrderControllerService },

    { provide: CART_ADD_BOOK_COMMAND_HANDLER, useClass: CartAddBookCommandHandlerService },
    { provide: UPDATE_CART_BOOK_COMMAND_HANDLER, useClass: UpdateCartBookCommandHandlerService },
    { provide: DELETE_CART_BOOK_COMMAND_HANDLER, useClass: DeleteCartBookCommandHandlerService },

    { provide: ADD_CLIENT_COMMAND_HANDLER, useClass: AddClientCommandHandlerService },
    { provide: UPDATE_CLIENT_COMMAND_HANDLER, useClass: UpdateClientCommandHandlerService },
    { provide: DELETE_CLIENT_COMMAND_HANDLER, useClass: DeleteClientCommandHandlerService },

    { provide: CLIENT_ADD_ORDER_COMMAND_HANDLER, useClass: ClientAddOrderCommandHandlerService },
    { provide: CLIENT_UPDATE_ORDER_COMMAND_HANDLER, useClass: ClientUpdateOrderCommandHandlerService },
    { provide: CLIENT_CANCEL_ORDER_COMMAND_HANDLER, useClass: ClientCancelOrderCommandHandlerService },
    { provide: MANAGER_UPDATE_ORDER_COMMAND_HANDLER, useClass: ManagerUpdateOrderCommandHandlerService },
    { provide: MANAGER_CANCEL_ORDER_COMMAND_HANDLER, useClass: ManagerCancelOrderCommandHandlerService },

    { provide: ADD_BOOKSTOCK_ORDER_COMMAND_HANDLER, useClass: AddBookStockCommandHandlerService },
  ],
  exports: [ShoppingCartButtonComponent, ClientInfoComponent],
})
export class ShopModule { }
