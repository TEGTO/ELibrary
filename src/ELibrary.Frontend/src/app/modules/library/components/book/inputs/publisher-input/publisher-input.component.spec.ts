import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PublisherInputComponent } from './PublisherInputComponent';

describe('PublisherInputComponent', () => {
  let component: PublisherInputComponent;
  let fixture: ComponentFixture<PublisherInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PublisherInputComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(PublisherInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
