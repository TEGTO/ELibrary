import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { ADMIN_CREATE_CLIENT_COMMAND_HANDLER, ADMIN_DELETE_USER_COMMAND_HANDLER, ADMIN_REGISTER_USER_COMMAND_HANDLER, ADMIN_UPDATE_CLIENT_COMMAND_HANDLER, ADMIN_UPDATE_USER_COMMAND_HANDLER, AdminClientDetailsComponent, AdminControllerService, AdminCreateClientCommandHandlerService, AdminDeleteUserCommandHandlerService, AdminDialogManager, AdminDialogManagerService, AdminEffects, adminReducer, AdminRegisterUserCommandHandlerService, AdminRegisterUserDialogComponent, AdminService, AdminTableComponent, AdminUpdateClientCommandHandlerService, AdminUpdateUserCommandHandlerService, AdminUserDetailsComponent, AdminUserFilterComponent, AdminUserPageComponent, AdminUserTableComponent, START_ADMIN_REGISTER_USER_COMMAND_HANDLER, StartAdminRegisterUserCommandHandlerService } from '.';
import { OrderTableComponent } from '../manager';
import { GenericTableComponent, LoadingComponent, pathes, PlaceholderPipe, PolicyType, RoleGuard } from '../shared';

const routes: Routes = [
  {
    path: "", component: AdminTableComponent,
    canActivate: [RoleGuard],
    data: { policy: [PolicyType.AdminPolicy] },
    children: [
      { path: pathes.admin_userTable, component: AdminUserTableComponent },
      { path: pathes.admin_userPage, component: AdminUserPageComponent },
      { path: "", redirectTo: pathes.admin_userTable, pathMatch: "full" },
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    AdminTableComponent,
    AdminUserFilterComponent,
    AdminUserTableComponent,
    AdminRegisterUserDialogComponent,
    AdminUserDetailsComponent,
    AdminUserPageComponent,
    AdminClientDetailsComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    StoreModule.forFeature('admin', adminReducer),
    EffectsModule.forFeature([AdminEffects]),
    GenericTableComponent,
    ReactiveFormsModule,
    FormsModule,
    LoadingComponent,
    MatInputModule,
    MatFormFieldModule,
    MatDialogModule,
    MatButtonModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    OrderTableComponent,
    PlaceholderPipe
  ],
  providers: [
    { provide: AdminService, useClass: AdminControllerService },
    { provide: AdminDialogManager, useClass: AdminDialogManagerService },

    { provide: START_ADMIN_REGISTER_USER_COMMAND_HANDLER, useClass: StartAdminRegisterUserCommandHandlerService },
    { provide: ADMIN_REGISTER_USER_COMMAND_HANDLER, useClass: AdminRegisterUserCommandHandlerService },
    { provide: ADMIN_UPDATE_USER_COMMAND_HANDLER, useClass: AdminUpdateUserCommandHandlerService },
    { provide: ADMIN_DELETE_USER_COMMAND_HANDLER, useClass: AdminDeleteUserCommandHandlerService },

    { provide: ADMIN_CREATE_CLIENT_COMMAND_HANDLER, useClass: AdminCreateClientCommandHandlerService },
    { provide: ADMIN_UPDATE_CLIENT_COMMAND_HANDLER, useClass: AdminUpdateClientCommandHandlerService },
  ]
})
export class AdminModule { }
