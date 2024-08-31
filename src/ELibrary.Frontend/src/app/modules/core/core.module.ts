import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AppComponent, MainViewComponent } from '.';
import { AuthInterceptor } from '../authentication';
import { AuthenticationModule } from '../authentication/authentication.module';
import { CustomErrorHandler, ErrorHandler } from '../shared';

const routes: Routes = [
  {
    path: "", component: MainViewComponent,
    children: [
      { path: "", loadChildren: () => import('../library/library.module').then(m => m.LibraryModule) }
    ],
  },
];
@NgModule({
  declarations: [
    AppComponent,
    MainViewComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    RouterModule.forRoot(routes),
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatDialogModule,
    AuthenticationModule,
    MatDialogModule,
    HttpClientModule,
    StoreModule.forRoot({}, {}),
    EffectsModule.forRoot([]),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: ErrorHandler, useClass: CustomErrorHandler },
  ],
  bootstrap: [AppComponent]
})
export class CoreModule { }