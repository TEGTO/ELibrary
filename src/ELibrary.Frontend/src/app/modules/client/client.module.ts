import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatPaginator } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterModule, Routes } from '@angular/router';
import { ClientViewComponent, CreateClientComponent, MakeOrderComponent, ProductInfoComponent, ProductPageComponent } from '.';
import { LibraryModule } from '../library/library.module';
import { ClientGuard, pathes, PolicyType, RoleGuard } from '../shared';
import { ShopModule } from '../shop/shop.module';

const routes: Routes = [
  {
    path: '', component: ClientViewComponent,
    children: [
      {
        path: pathes.client_order,
        children: [
          { path: pathes.client_order_addInformation, component: CreateClientComponent },
          {
            path: pathes.client_order_makeOrder, component: MakeOrderComponent,
            canActivate: [RoleGuard, ClientGuard],
            data: { policy: [PolicyType.ClientPolicy] }
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
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    LibraryModule,
    ShopModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    ScrollingModule,
    MatPaginator,
    MatExpansionModule,
    ReactiveFormsModule
  ]
})
export class ClientModule { }
