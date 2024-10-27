import { TestBed } from "@angular/core/testing";
import { environment } from "../../../../../environment/environment";
import { URLDefiner } from "./url-definer.service";

describe('URLDefiner', () => {
    let service: URLDefiner;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [URLDefiner],
        });

        service = TestBed.inject(URLDefiner);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    describe('combineWithUserApiUrl', () => {
        it('should return correct URL with userApi base and subpath', () => {
            const subpath = '/auth/login';
            const expectedUrl = environment.userApi + subpath;

            const result = service.combineWithUserApiUrl(subpath);

            expect(result).toBe(expectedUrl);
        });
    });

    describe('combineWithLibraryApiUrl', () => {
        it('should return correct URL with libraryApi base and subpath', () => {
            const subpath = '/books/search';
            const expectedUrl = environment.libraryApi + subpath;

            const result = service.combineWithLibraryApiUrl(subpath);

            expect(result).toBe(expectedUrl);
        });
    });

    describe('combineWithShopApiUrl', () => {
        it('should return correct URL with shopApi base and subpath', () => {
            const subpath = '/cart/add';
            const expectedUrl = environment.shopApi + subpath;

            const result = service.combineWithShopApiUrl(subpath);

            expect(result).toBe(expectedUrl);
        });
    });
});