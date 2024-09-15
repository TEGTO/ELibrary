import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManagerTableComponent } from './components/manager-table/manager-table.component';

const routes: Routes = [
  {
    path: "", component: ManagerTableComponent,
    children: [
      {
        path: "", redirectTo: "books", pathMatch: "full"
      },
      // { path: "books", component: BookTableComponent },
      // { path: "genres", component: GenreTableComponent },
      // { path: "authors", component: AuthorTableComponent }
    ]
  },
  { path: '**', redirectTo: '' }
];
@NgModule({
  declarations: [
    ManagerTableComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    CommonModule
  ]
})
export class ManagerModule { }
