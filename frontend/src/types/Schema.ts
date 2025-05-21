interface PropertySchema {
  type: string;
  required?: boolean;
  nullable?: boolean;
  maxLength?: number;
  minLength?: number;
  min?: number;
  max?: number;
  specialData?: string[];
  select?: string;
  controller?: string;
  relationInfo?: {
    model: string;
    controller: string;
    selects: string[];
  };
}

export type DtoSchema = Record<string, PropertySchema>;
