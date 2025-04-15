export interface SchemaField {
    specialData?: any;
    minLength?: number | undefined;
    type: string;
    required: boolean;
    maxLength?: number;
    min?: number;
    max?: number;
}

export type Schema = {
    [key: string]: SchemaField;
};
