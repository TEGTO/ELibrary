import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShoppingCardButtonComponent } from './shopping-card-button.component';

describe('ShoppingCardButtonComponent', () => {
  let component: ShoppingCardButtonComponent;
  let fixture: ComponentFixture<ShoppingCardButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ShoppingCardButtonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShoppingCardButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
