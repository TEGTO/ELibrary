import { DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';
import { CartService, ShopDialogManager } from '../../..';
import { ShoppingCartButtonComponent } from './shopping-cart-button.component';

describe('ShoppingCartButtonComponent', () => {
    let component: ShoppingCartButtonComponent;
    let fixture: ComponentFixture<ShoppingCartButtonComponent>;
    let cartServiceSpy: jasmine.SpyObj<CartService>;
    let dialogManagerSpy: jasmine.SpyObj<ShopDialogManager>;

    beforeEach(async () => {
        cartServiceSpy = jasmine.createSpyObj('CartService', ['getInCartAmount']);
        dialogManagerSpy = jasmine.createSpyObj('ShopDialogManager', ['openCartMenu']);

        await TestBed.configureTestingModule({
            declarations: [ShoppingCartButtonComponent],
            providers: [
                { provide: CartService, useValue: cartServiceSpy },
                { provide: ShopDialogManager, useValue: dialogManagerSpy }
            ]
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(ShoppingCartButtonComponent);
        component = fixture.componentInstance;
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should initialize cartAmount$ with the value from cartService', () => {
        const cartAmount = 5;
        cartServiceSpy.getInCartAmount.and.returnValue(of(cartAmount));

        fixture.detectChanges();

        component.cartAmount$.subscribe((amount) => {
            expect(amount).toBe(cartAmount);
        });
    });

    it('should display the correct cart amount when cartAmount$ emits a value', () => {
        const cartAmount = 5;
        cartServiceSpy.getInCartAmount.and.returnValue(of(cartAmount));

        fixture.detectChanges();

        const cartCountDebugElement: DebugElement = fixture.debugElement.query(By.css('.cart-count'));
        expect(cartCountDebugElement.nativeElement.textContent.trim()).toBe(`${cartAmount}`);
    });

    it('should display "99+" when cart amount is greater than 99', () => {
        const cartAmount = 150;
        cartServiceSpy.getInCartAmount.and.returnValue(of(cartAmount));

        fixture.detectChanges();

        const cartCountDebugElement: DebugElement = fixture.debugElement.query(By.css('.cart-count'));
        expect(cartCountDebugElement.nativeElement.textContent.trim()).toBe('99+');
    });

    it('should call openCartMenu when the cart button is clicked', () => {
        const button = fixture.debugElement.query(By.css('button'));
        button.triggerEventHandler('click', null);

        expect(dialogManagerSpy.openCartMenu).toHaveBeenCalled();
    });
});