import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BookStatistics, GetBookStatistics } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class StatisticsService {
    abstract getBookStatistics(req: GetBookStatistics): Observable<BookStatistics>;
}