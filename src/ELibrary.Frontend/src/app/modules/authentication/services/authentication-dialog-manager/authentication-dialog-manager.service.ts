/* eslint-disable @typescript-eslint/no-explicit-any */
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AuthenticatedComponent, AuthenticationService, LoginComponent, RegisterComponent } from '../..';
import { DialogManagerService } from '../../../shared/services/dialog-manager/dialog-manager.service';
import { AuthenticationDialogManager } from './authentication-dialog-manager';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationDialogManagerService implements AuthenticationDialogManager {
  isAuthenticated = false;

  constructor(
    private readonly authService: AuthenticationService,
    private readonly dialog: MatDialog,
    private readonly dialogManager: DialogManagerService
  ) {
    this.authService.getUserAuth().subscribe(data => {
      this.isAuthenticated = data.isAuthenticated;
    })
  }

  openConfirmMenu(): MatDialogRef<any> {
    return this.dialogManager.openConfirmMenu();
  }
  openLoginMenu(): MatDialogRef<any, any> {
    let dialogRef: MatDialogRef<any, any>;
    if (this.isAuthenticated) {
      dialogRef = this.dialog.open(AuthenticatedComponent, {
        height: '440px',
        width: '450px',
      });
    }
    else {
      dialogRef = this.dialog.open(LoginComponent, {
        height: '345px',
        width: '450px',
      });
    }
    return dialogRef;
  }
  openRegisterMenu(): MatDialogRef<any, any> {
    const dialogRef = this.dialog.open(RegisterComponent, {
      height: '390px',
      width: '450px',
    });
    return dialogRef;
  }
}
