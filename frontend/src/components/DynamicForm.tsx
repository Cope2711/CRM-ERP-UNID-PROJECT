import { Form, Input } from 'antd';
import type { Schema } from '@/types/Schema';

type DynamicFormProps = {
    schema: Schema;
};

export default function DynamicForm({ schema }: DynamicFormProps) {
    return (
        <>
            {Object.entries(schema).map(([key, config]) => {
                const rules = [];

                if (config.required) {
                    rules.push({ required: true, message: `${key} es requerido` });
                }

                if (config.type === 'number' || config.type === 'double') {
                    rules.push({
                        validator: (_: never, value: string | undefined) => {
                            if (value === undefined || value === '') return Promise.resolve();

                            const numberValue = Number(value);
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
                    name: key,
                    label: key,
                    rules,
                };

                const inputElement =
                    config.type === 'number' || config.type === 'double' ? (
                        <Input />
                    ) : (
                        <Input maxLength={config.maxLength} />
                    );

                return (
                    <Form.Item key={key} {...commonProps}>
                        {inputElement}
                    </Form.Item>
                );
            })}
        </>
    );
}
