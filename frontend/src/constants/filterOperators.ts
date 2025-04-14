export const FilterOperators = {
    Equal: '==',
    NotEqual: '!=',
    GreaterThan: '>',
    LessThan: '<',
    GreaterThanOrEqual: '>=',
    LessThanOrEqual: '<=',
    Like: 'Like',
    Contains: 'Contains',
    StartsWith: 'StartsWith',
    EndsWith: 'EndsWith',
    In: 'In',
} as const;

export type FilterOperator = typeof FilterOperators[keyof typeof FilterOperators];