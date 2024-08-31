import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibraryTablesComponent } from './library-tables.component';

describe('LibraryTablesComponent', () => {
  let component: LibraryTablesComponent;
  let fixture: ComponentFixture<LibraryTablesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LibraryTablesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LibraryTablesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
