import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { GetShopStatistics, ShopStatistics } from "../../../shared";

@Injectable({
    providedIn: 'root'
})
export abstract class StatisticsService {
    abstract getShopStatistics(req: GetShopStatistics): Observable<ShopStatistics>;
}