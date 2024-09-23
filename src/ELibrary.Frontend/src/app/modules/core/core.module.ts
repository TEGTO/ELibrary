import { CommonModule, registerLocaleData } from '@angular/common';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import localeUa from '@angular/common/locales/uk';
import { LOCALE_ID, NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DATE_LOCALE } from '@angular/material/core';
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
import { CurrencyPipeApplier, CurrencyPipeApplierService, CustomErrorHandler, ErrorHandler, pathes, PolicyType, RoleGuard, ValidationMessage, ValidationMessageService } from '../shared';
import { ShopModule } from '../shop/shop.module';

registerLocaleData(localeUa, 'uk-UA');

const routes: Routes = [
  {
    path: "", component: MainViewComponent,
    children: [
      {
        path: pathes.manager,
        loadChildren: () => import('../manager/manager.module').then(m => m.ManagerModule),
        canActivate: [RoleGuard],
        data: { policy: [PolicyType.ManagerPolicy] }
      },
      {
        path: pathes.client,
        loadChildren: () => import('../client/client.module').then(m => m.ClientModule),
      },
    ],
  },
  { path: '**', redirectTo: '' }
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
    ShopModule,
    MatDialogModule,
    HttpClientModule,
    StoreModule.forRoot({}, {}),
    EffectsModule.forRoot([]),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: ErrorHandler, useClass: CustomErrorHandler },
    { provide: ValidationMessage, useClass: ValidationMessageService },
    { provide: CurrencyPipeApplier, useClass: CurrencyPipeApplierService },
    { provide: LOCALE_ID, useValue: "uk-UA" },
    { provide: MAT_DATE_LOCALE, useValue: 'uk-UA' },
  ],
  bootstrap: [AppComponent]
})
export class CoreModule { }