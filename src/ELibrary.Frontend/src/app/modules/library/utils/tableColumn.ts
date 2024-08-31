import { PipeTransform } from "@angular/core";

export interface TableColumn {
    header: string;
    field: string;
    pipe?: PipeTransform;
    pipeArgs?: any[];
}
