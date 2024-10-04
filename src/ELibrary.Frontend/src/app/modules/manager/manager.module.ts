import { NgxMatDatetimePickerModule, NgxMatNativeDateModule, NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
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
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { RouterModule, Routes } from '@angular/router';
import { AuthorTableComponent, BookStockDetailsComponent, BookStockTableComponent, BookTableComponent, GenreTableComponent, ManagerTableComponent, OrderDetailsComponent, OrderTableComponent, PublisherTableComponent, StatisticsPageComponent } from '.';
import { LibraryModule } from '../library/library.module';
import { GenericTableComponent, LoadingComponent, pathes, PolicyType, RoleGuard } from '../shared';
import { ShopModule } from '../shop/shop.module';

const routes: Routes = [
  {
    path: "", component: ManagerTableComponent,
    canActivate: [RoleGuard],
    data: { policy: [PolicyType.ManagerPolicy] },
    children: [
      { path: pathes.manager_bookstock, component: BookStockTableComponent },
      { path: pathes.manager_bookstock_details, component: BookStockDetailsComponent },
      { path: pathes.manager_books, component: BookTableComponent },
      { path: pathes.manager_genres, component: GenreTableComponent },
      { path: pathes.manager_authors, component: AuthorTableComponent },
      { path: pathes.manager_publishers, component: PublisherTableComponent },
      { path: pathes.manager_orders, component: OrderTableComponent },
      { path: pathes.manager_orders_details, component: OrderDetailsComponent },
      { path: pathes.manager_statistics, component: StatisticsPageComponent },
      { path: "", redirectTo: pathes.manager_books, pathMatch: "full" },
    ]
  },
  { path: '**', redirectTo: '' }
];
@NgModule({
  declarations: [
    GenreTableComponent,
    BookTableComponent,
    AuthorTableComponent,
    ManagerTableComponent,
    PublisherTableComponent,
    BookStockTableComponent,
    BookStockDetailsComponent,
    OrderTableComponent,
    OrderDetailsComponent,
    StatisticsPageComponent,
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    LibraryModule,
    GenericTableComponent,
    MatDialogModule,
    MatInputModule,
    FormsModule,
    ShopModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatButtonModule,
    LoadingComponent,
    NgxMatTimepickerModule,
    NgxMatNativeDateModule,
    NgxMatDatetimePickerModule,
    MatNativeDateModule,
    ScrollingModule,
    MatSelectModule,
    MatPaginatorModule,
    MatDatepickerModule,
  ]
})
export class ManagerModule { }
