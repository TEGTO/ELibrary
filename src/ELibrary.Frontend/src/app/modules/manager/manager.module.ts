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
import { AuthorTableComponent, BookTableComponent, GenreTableComponent, ManagerTableComponent, PublisherTableComponent } from '.';
import { LibraryModule } from '../library/library.module';
import { GenericTableComponent } from '../shared';

const routes: Routes = [
  {
    path: "", component: ManagerTableComponent,
    children: [
      {
        path: "", redirectTo: "books", pathMatch: "full"
      },
      { path: "books", component: BookTableComponent },
      { path: "genres", component: GenreTableComponent },
      { path: "authors", component: AuthorTableComponent },
      { path: "publishers", component: PublisherTableComponent }
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
