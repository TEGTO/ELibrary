import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CartService, ShopDialogManager } from '../../..';

@Component({
  selector: 'app-shopping-cart-button',
  templateUrl: './shopping-cart-button.component.html',
  styleUrl: './shopping-cart-button.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ShoppingCartButtonComponent implements OnInit {
  cartAmount$!: Observable<number>;

  constructor(
    private readonly cartService: CartService,
    private readonly dialogManager: ShopDialogManager
  ) { }

  ngOnInit(): void {
    this.cartAmount$ = this.cartService.getInCartAmount();
  }

  openCartMenu() {
    this.dialogManager.openCartMenu();
  }
}
