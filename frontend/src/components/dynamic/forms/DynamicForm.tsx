import BooleanSelector from "@/components/Selectors/BooleanSelector";
import { PropertiesSchema } from "@/types/Schema";
import { Form, Input } from "antd";
import { JSX } from "react";

type DynamicFormProps = {
    propertiesSchema: PropertiesSchema;
};

type ValidationRule = { [key: string]: any };

const validatorsDict: Record<
    string,
    (key: string, config: PropertiesSchema[string]) => ValidationRule | null
> = {
    required: (key, config) =>
        config.required ? { required: true, message: `${key} es requerido` } : null,

    minLength: (key, config) =>
        config.minLength !== undefined
            ? { min: config.minLength, message: `${key} debe tener al menos ${config.minLength} caracteres` }
            : null,

    maxLength: (key, config) =>
        config.maxLength !== undefined
            ? { max: config.maxLength, message: `${key} no debe superar los ${config.maxLength} caracteres` }
            : null,

    min: (key, config) =>
        config.min !== undefined
            ? {
                validator: async (_: any, value: any) => {
                    if (value !== undefined && value < config.min!) {
                        return Promise.reject(new Error(`${key} debe ser mayor o igual a ${config.min}`));
                    }
                },
            }
            : null,

    max: (key, config) =>
        config.max !== undefined
            ? {
                validator: async (_: any, value: any) => {
                    if (value !== undefined && value > config.max!) { 
                        return Promise.reject(new Error(`${key} debe ser menor o igual a ${config.max}`));
                    }
                },
            }
            : null,
};

const typeToComponent: Record<
    string,
    () => JSX.Element
> = {
    boolean: () => <BooleanSelector />,
    string: () => <Input />,
    number: () => <Input type="number" />,
    int32: () => <Input type="number" />,
    decimal: () => <Input type="number" />,
    double: () => <Input type="number" />,
    guid: () => <Input type="text" />,
    datetime: () => <Input type="text" />,
};

export default function DynamicForm({ propertiesSchema: propoertiesSchema }: DynamicFormProps) {
    return (
        <>
            {Object.entries(propoertiesSchema)
                .filter(([_, config]) => Object.keys(typeToComponent).includes(config.type) && !config.nonmodificable)
                .map(([key, config]) => { 
                    const rules = Object.entries(validatorsDict)
                        .map(([_, validator]) => validator(key, config))
                        .filter((rule): rule is ValidationRule => rule !== null);

                    const inputComponent = typeToComponent[config.type];

                    return (
                        <Form.Item key={key} name={key} label={key} rules={rules}>
                            {inputComponent()}
                        </Form.Item>
                    );
                })}
        </>
    );
}
