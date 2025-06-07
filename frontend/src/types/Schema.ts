export type PropertiesSchema = {
  [key: string]: {
    type: string;
    required: boolean;
    maxLength?: number;
    minLength?: number;
    min?: number;
    max?: number;
    nonmodificable?: boolean;
    isPassword?: boolean;
    key?: boolean;
    controller?: string;
    select?: string;
  };
}

export type RelationSchema = {
  controller: string;
  selects: string[];
  modelName: string;
};