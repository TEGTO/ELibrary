import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { RouterModule, Routes } from '@angular/router';
import { AuthorTableComponent, BookStockComponent, BookTableComponent, GenreTableComponent, ManagerTableComponent, PublisherTableComponent } from '.';
import { LibraryModule } from '../library/library.module';
import { GenericTableComponent, pathes } from '../shared';

const routes: Routes = [
  {
    path: "", component: ManagerTableComponent,
    children: [
      { path: pathes.manager_bookstock, component: BookStockComponent },
      { path: pathes.manager_books, component: BookTableComponent },
      { path: pathes.manager_genres, component: GenreTableComponent },
      { path: pathes.manager_authors, component: AuthorTableComponent },
      { path: pathes.manager_publishers, component: PublisherTableComponent },
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
    BookStockComponent,
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule,
    LibraryModule,
    GenericTableComponent,
    MatDialogModule,
    MatInputModule,
    FormsModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatButtonModule,
    ScrollingModule,
    MatSelectModule,
    CommonModule,
    MatButtonModule,
    MatPaginatorModule,
    MatDatepickerModule,
  ]
})
export class ManagerModule { }
