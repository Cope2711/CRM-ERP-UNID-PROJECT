import { DtoSchema } from '@/types/Schema';
import { Form, Input, Select } from 'antd';

type DynamicFormProps = {
    schema: DtoSchema;
};

export default function DynamicForm({ schema }: DynamicFormProps) {
    return (
        <>
            {Object.entries(schema).map(([key, config]) => {
                const lowerKey = key.toLowerCase();

                const rules = [];
                
                // Reglas de validación dinámicas
                if (config.required) {
                    rules.push({ required: true, message: `${key} es requerido` });
                }

                if (config.minLength) {
                    rules.push({ min: config.minLength, message: `${key} debe tener al menos ${config.minLength} caracteres` });
                }

                if (config.maxLength) {
                    rules.push({ max: config.maxLength, message: `${key} no debe superar los ${config.maxLength} caracteres` });
                }

                if (config.specialData?.includes("IsEmail")) {
                    rules.push({ type: "email", message: `${key} debe ser un correo válido` });
                }

                if (config.specialData?.includes("IsPhoneNumberWithLada")) {
                    // Validación de número telefónico con lada
                    rules.push({
                        validator: async (_: any, value: string) => {
                            if (!value) return Promise.resolve();

                            if (value.length === 13 && value.startsWith('+')) {
                                return Promise.resolve(); 
                            }

                            return Promise.reject(`${key} debe tener 13 caracteres y comenzar con '+'`);
                        },
                    });
                }
                
                if (config.type === 'number' || config.type === 'double' || config.type === 'decimal') {
                    rules.push({
                        validator: (_: never, value: string | undefined) => {
                            if (value === undefined || value === '') return Promise.resolve();
                
                            const numberValue = parseFloat(value); 
                            if (isNaN(numberValue)) {
                                return Promise.reject(`${key} debe ser un número válido`);
                            }
                
                            if (config.min !== undefined && numberValue < config.min) {
                                return Promise.reject(`${key} debe ser mayor o igual a ${config.min}`);
                            }
                
                            if (config.max !== undefined && numberValue > config.max) {
                                return Promise.reject(`${key} debe ser menor o igual a ${config.max}`);
                            }
                
                            return Promise.resolve();
                        },
                    });
                }

                const commonProps = {
                    name: lowerKey,
                    label: key,
                    rules,
                    initialValue: config.type === 'boolean' ? true : undefined,
                };

                let inputElement: React.ReactNode;

                switch (config.type) {
                    case 'boolean':
                        inputElement = (
                            <Select options={[
                                { label: 'Sí', value: true },
                                { label: 'No', value: false }
                            ]} />
                        );
                        break;

                    case 'number':
                    case 'double':
                        inputElement = <Input />;
                        break;

                    default:
                        if (config.specialData?.includes("IsPassword")) {
                            inputElement = (
                                <Input.Password
                                    maxLength={config.maxLength}
                                    minLength={config.minLength}
                                />
                            );
                        } else {
                            inputElement = (
                                <Input
                                    type={config.specialData?.includes("IsEmail") ? "email" : "text"}
                                    maxLength={config.maxLength}
                                    minLength={config.minLength}
                                />
                            );
                        }
                        break;
                }

                return (
                    <Form.Item key={lowerKey} {...commonProps}>
                        {inputElement}
                    </Form.Item>
                );
            })}
        </>
    );
}
