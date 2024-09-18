import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginator } from '@angular/material/paginator';
import { RouterModule, Routes } from '@angular/router';
import { ProductPageComponent } from '.';
import { LibraryModule } from '../library/library.module';

const routes: Routes = [
  {
    path: "", component: ProductPageComponent,
  },
  { path: '**', redirectTo: '' }
];
@NgModule({
  declarations: [
    ProductPageComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    LibraryModule,
    MatButtonModule,
    MatPaginator
  ]
})
export class ClientModule { }
