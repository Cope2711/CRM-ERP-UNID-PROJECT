import {FilterOperator} from "@/constants/filterOperators.ts";

export interface GetAllDto {
    pageNumber: number;
    pageSize: number;
    orderBy: string;
    descending: boolean;
    filters: FilterDto[];
    selects: string[];
}

export interface FilterDto {
    column: string;
    operator: FilterOperator;
    value: string;
}