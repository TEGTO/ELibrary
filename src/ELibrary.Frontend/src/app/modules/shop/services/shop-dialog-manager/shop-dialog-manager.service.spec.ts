/* eslint-disable @typescript-eslint/no-explicit-any */
import { TestBed } from '@angular/core/testing';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { of } from 'rxjs';
import { BookStockReplenishmentComponent, CartDialogComponent, ClientChangeDialogComponent } from '../..';
import { Client, DialogManagerService, getDefaultClient } from '../../../shared';
import { ShopDialogManagerService } from './shop-dialog-manager.service';

describe('ShopDialogManagerService', () => {
    let service: ShopDialogManagerService;
    let dialogSpy: jasmine.SpyObj<MatDialog>;
    let dialogManagerSpy: jasmine.SpyObj<DialogManagerService>;

    beforeEach(() => {
        const matDialogSpy = jasmine.createSpyObj('MatDialog', ['open']);
        const dialogManagerServiceSpy = jasmine.createSpyObj('DialogManagerService', ['openConfirmMenu']);

        TestBed.configureTestingModule({
            providers: [
                ShopDialogManagerService,
                { provide: MatDialog, useValue: matDialogSpy },
                { provide: DialogManagerService, useValue: dialogManagerServiceSpy },
            ],
        });

        service = TestBed.inject(ShopDialogManagerService);
        dialogSpy = TestBed.inject(MatDialog) as jasmine.SpyObj<MatDialog>;
        dialogManagerSpy = TestBed.inject(DialogManagerService) as jasmine.SpyObj<DialogManagerService>;

        const matDialogRefMock = { afterClosed: () => of(true) } as MatDialogRef<any>;
        dialogSpy.open.and.returnValue(matDialogRefMock);
        dialogManagerSpy.openConfirmMenu.and.returnValue(matDialogRefMock);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    it('openConfirmMenu should call dialogManager.openConfirmMenu', () => {
        service.openConfirmMenu();
        expect(dialogManagerSpy.openConfirmMenu).toHaveBeenCalled();
    });

    it('openCartMenu should open CartDialogComponent with correct dimensions', () => {
        service.openCartMenu();

        expect(dialogSpy.open).toHaveBeenCalledWith(CartDialogComponent, {
            width: '630px',
            maxHeight: '640px',
            maxWidth: '630px',
        });
    });

    it('openClientChangeMenu should open ClientChangeDialogComponent with correct dimensions and client data', () => {
        const mockClient: Client = getDefaultClient();

        service.openClientChangeMenu(mockClient);

        expect(dialogSpy.open).toHaveBeenCalledWith(ClientChangeDialogComponent, {
            width: '530px',
            maxHeight: '710px',
            maxWidth: '530px',
            data: mockClient,
        });
    });

    it('openReplenishmentMenu should open BookStockReplenishmentComponent with correct dimensions', () => {
        service.openReplenishmentMenu();

        expect(dialogSpy.open).toHaveBeenCalledWith(BookStockReplenishmentComponent, {
            width: '550px',
            maxHeight: '430px',
            maxWidth: '550px',
        });
    });
});