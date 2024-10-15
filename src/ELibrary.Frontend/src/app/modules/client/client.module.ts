import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator } from '@angular/material/paginator';
import { MatRadioModule } from '@angular/material/radio';
import { RouterModule, Routes } from '@angular/router';
import { MatTimepickerModule } from '@dhutaryan/ngx-mat-timepicker';
import { ClientViewComponent, CreateClientComponent, MakeOrderComponent, OrderHistoryComponent, ProductInfoComponent, ProductPageComponent } from '.';
import { LibraryModule } from '../library/library.module';
import { ClientGuard, LoadingComponent, pathes, PolicyType, RoleGuard } from '../shared';
import { ShopModule } from '../shop/shop.module';

const routes: Routes = [
  {
    path: '', component: ClientViewComponent,
    children: [
      {
        path: pathes.client_order,
        canActivate: [RoleGuard],
        data: { policy: [PolicyType.ClientPolicy] },
        children: [
          {
            path: pathes.client_order_history,
            component: OrderHistoryComponent,
            canActivate: [ClientGuard],
          },
          {
            path: pathes.client_order_addInformation,
            component: CreateClientComponent,
          },
          {
            path: pathes.client_order_makeOrder, component: MakeOrderComponent,
            canActivate: [ClientGuard],
          },
          { path: "", redirectTo: '', pathMatch: "full" },
        ]
      },
      { path: pathes.client_productInfo, component: ProductInfoComponent },
      { path: pathes.client_products, component: ProductPageComponent },
      { path: "", redirectTo: '', pathMatch: "full" },
    ]
  },
  { path: '**', redirectTo: '' }
];
@NgModule({
  declarations: [
    ProductPageComponent,
    ProductInfoComponent,
    ClientViewComponent,
    MakeOrderComponent,
    CreateClientComponent,
    OrderHistoryComponent,
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    LibraryModule,
    ShopModule,
    MatFormFieldModule,
    LoadingComponent,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatRadioModule,
    MatButtonModule,
    MatTimepickerModule,
    ScrollingModule,
    MatPaginator,
    MatExpansionModule,
    ReactiveFormsModule,
  ]
})
export class ClientModule { }
