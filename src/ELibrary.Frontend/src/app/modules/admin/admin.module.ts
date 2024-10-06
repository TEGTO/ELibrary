import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { ADMIN_CREATE_CLIENT_COMMAND_HANDLER, ADMIN_DELETE_USER_COMMAND_HANDLER, ADMIN_REGISTER_USER_COMMAND_HANDLER, ADMIN_UPDATE_CLIENT_COMMAND_HANDLER, ADMIN_UPDATE_USER_COMMAND_HANDLER, AdminCreateClientCommandHandlerService, AdminDeleteUserCommandHandlerService, AdminEffects, adminReducer, AdminRegisterUserCommandHandlerService, AdminTableComponent, AdminUpdateClientCommandHandlerService, AdminUpdateUserCommandHandlerService } from '.';
import { PolicyType, RoleGuard } from '../shared';

const routes: Routes = [
  {
    path: "", component: AdminTableComponent,
    canActivate: [RoleGuard],
    data: { policy: [PolicyType.AdminPolicy] },
    children: [
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    AdminTableComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    StoreModule.forFeature('admin', adminReducer),
    EffectsModule.forFeature([AdminEffects]),
  ],
  providers: [
    { provide: ADMIN_REGISTER_USER_COMMAND_HANDLER, useClass: AdminRegisterUserCommandHandlerService },
    { provide: ADMIN_UPDATE_USER_COMMAND_HANDLER, useClass: AdminUpdateUserCommandHandlerService },
    { provide: ADMIN_DELETE_USER_COMMAND_HANDLER, useClass: AdminDeleteUserCommandHandlerService },

    { provide: ADMIN_CREATE_CLIENT_COMMAND_HANDLER, useClass: AdminCreateClientCommandHandlerService },
    { provide: ADMIN_UPDATE_CLIENT_COMMAND_HANDLER, useClass: AdminUpdateClientCommandHandlerService },
  ]
})
export class AdminModule { }
