export interface SchemaField {
    type: string;
    required: boolean;
    maxLength?: number;
    min?: number;
    max?: number;
}

export type Schema = {
    [key: string]: SchemaField;
};
