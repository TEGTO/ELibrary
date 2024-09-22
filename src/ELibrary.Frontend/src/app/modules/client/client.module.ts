import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginator } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterModule, Routes } from '@angular/router';
import { ClientViewComponent, ProductInfoComponent, ProductPageComponent } from '.';
import { LibraryModule } from '../library/library.module';
import { ShopModule } from '../shop/shop.module';

const routes: Routes = [
  {
    path: "", component: ClientViewComponent,
    children: [
      {
        path: "", component: ProductPageComponent,
      },
      {
        path: ":id", component: ProductInfoComponent,
      },
    ]
  },
  { path: '**', redirectTo: '' }
];
@NgModule({
  declarations: [
    ProductPageComponent,
    ProductInfoComponent,
    ClientViewComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    LibraryModule,
    ShopModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatPaginator
  ]
})
export class ClientModule { }
