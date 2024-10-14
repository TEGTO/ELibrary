/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';

import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ConfirmMenuComponent } from '../../components/confirm-menu/confirm-menu.component';
import { DialogManagerService } from './dialog-manager.service';

describe('DialogManagerService', () => {
  let service: DialogManagerService;
  let mockMatDialog: jasmine.SpyObj<MatDialog>;

  beforeEach(() => {
    mockMatDialog = jasmine.createSpyObj<MatDialog>('MatDialog', ['open']);

    TestBed.configureTestingModule({
      providers: [
        DialogManagerService,
        { provide: MatDialog, useValue: mockMatDialog }
      ]
    });

    service = TestBed.inject(DialogManagerService);
  });

  it('should open confirm menu dialog', () => {
    const dialogRef = {} as MatDialogRef<any>;
    mockMatDialog.open.and.returnValue(dialogRef);

    const result = service.openConfirmMenu();

    expect(mockMatDialog.open).toHaveBeenCalledWith(ConfirmMenuComponent, {
      maxHeight: '200px',
      width: '450px'
    });
    expect(result).toBe(dialogRef);
  });

});
