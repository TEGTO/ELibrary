import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, RouterModule } from '@angular/router';
import { pathes } from '../../../shared';
import { ManagerTableComponent } from './manager-table.component';

describe('ManagerTableComponent', () => {
    let component: ManagerTableComponent;
    let fixture: ComponentFixture<ManagerTableComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [RouterModule],
            declarations: [ManagerTableComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [provideRouter([])]
        }).compileComponents();

        fixture = TestBed.createComponent(ManagerTableComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create the component', () => {
        expect(component).toBeTruthy();
    });

    it('should have correct path for book stock', () => {
        expect(component.bookStockPath).toBe(`${pathes.manager_bookstock}`);
    });

    it('should have correct path for books', () => {
        expect(component.bookPath).toBe(`${pathes.manager_books}`);
    });

    it('should have correct path for authors', () => {
        expect(component.authorPath).toBe(`${pathes.manager_authors}`);
    });

    it('should have correct path for genres', () => {
        expect(component.genrePath).toBe(`${pathes.manager_genres}`);
    });

    it('should have correct path for publishers', () => {
        expect(component.publisherPath).toBe(`${pathes.manager_publishers}`);
    });

    it('should render all links with correct routerLinks', () => {
        const compiled = fixture.nativeElement as HTMLElement;

        const bookStockLink = compiled.querySelector(`a[href="/${pathes.manager_bookstock}"]`);
        expect(bookStockLink?.textContent?.trim()).toBe('Book Stock');

        const bookLink = compiled.querySelector(`a[href="/${pathes.manager_books}"]`);
        expect(bookLink?.textContent?.trim()).toBe('Books');

        const authorLink = compiled.querySelector(`a[href="/${pathes.manager_authors}"]`);
        expect(authorLink?.textContent?.trim()).toBe('Authors');

        const genreLink = compiled.querySelector(`a[href="/${pathes.manager_genres}"]`);
        expect(genreLink?.textContent?.trim()).toBe('Genres');

        const publisherLink = compiled.querySelector(`a[href="/${pathes.manager_publishers}"]`);
        expect(publisherLink?.textContent?.trim()).toBe('Publishers');
    });
});
