import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CartService } from '../..';

@Component({
  selector: 'app-shopping-card-button',
  templateUrl: './shopping-card-button.component.html',
  styleUrl: './shopping-card-button.component.scss'
})
export class ShoppingCardButtonComponent implements OnInit {
  cartAmount$!: Observable<number>;

  constructor(private readonly cartService: CartService) { }

  ngOnInit(): void {
    this.cartAmount$ = this.cartService.getInCartAmount();
  }
}
