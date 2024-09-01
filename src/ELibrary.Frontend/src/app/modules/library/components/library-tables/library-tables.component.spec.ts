import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { LibraryTablesComponent } from './library-tables.component';

describe('LibraryTablesComponent', () => {
  let component: LibraryTablesComponent;
  let fixture: ComponentFixture<LibraryTablesComponent>;
  let router: Router;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LibraryTablesComponent],
      imports: [
        RouterTestingModule.withRoutes([
          { path: 'books', component: LibraryTablesComponent },
          { path: 'authors', component: LibraryTablesComponent },
          { path: 'genres', component: LibraryTablesComponent }
        ])
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraryTablesComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have navigation links for books, authors, and genres', () => {
    const compiled = fixture.nativeElement as HTMLElement;

    const bookLink = compiled.querySelector('a[routerLink="books"]');
    const authorLink = compiled.querySelector('a[routerLink="authors"]');
    const genreLink = compiled.querySelector('a[routerLink="genres"]');

    expect(bookLink).toBeTruthy();
    expect(authorLink).toBeTruthy();
    expect(genreLink).toBeTruthy();

    expect(bookLink?.textContent?.trim()).toBe('Books');
    expect(authorLink?.textContent?.trim()).toBe('Authors');
    expect(genreLink?.textContent?.trim()).toBe('Genres');

    expect(bookLink?.classList).toContain('bg-white');
    expect(authorLink?.classList).toContain('bg-white');
    expect(genreLink?.classList).toContain('bg-white');
  });

  it('should apply "selected" class to active router link', fakeAsync(() => {
    const compiled = fixture.nativeElement as HTMLElement;

    router.navigate(['books']);

    tick();
    fixture.detectChanges();

    const bookLink = compiled.querySelector('a[routerLink="books"]');
    const authorLink = compiled.querySelector('a[routerLink="authors"]');
    const genreLink = compiled.querySelector('a[routerLink="genres"]');

    expect(bookLink?.classList).toContain('selected');
    expect(authorLink?.classList).not.toContain('selected');
    expect(genreLink?.classList).not.toContain('selected');
  }));
});