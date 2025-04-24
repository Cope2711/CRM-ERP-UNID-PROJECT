import { Card, Form, Row, Col, Button, Space, DatePicker, Input, Select } from 'antd';
import { FilterOperators, FilterOperator } from '@/constants/filterOperators';
import { FilterDto } from '@/dtos/GenericDtos';
import React from 'react';

const { RangePicker } = DatePicker;

type AdvancedFiltersProps = {
    schema: Record<string, any>;
    validFields: string[];
    onSubmit: (filter: FilterDto) => void; 
    onClear: () => void;
  };

export default function AdvancedFilters({ schema, validFields, onSubmit, onClear }: AdvancedFiltersProps) {
    const [form] = Form.useForm();
    const [selectedField, setSelectedField] = React.useState<string | null>(null);
    const [selectedOperator, setSelectedOperator] = React.useState<FilterOperator | null>(null);

    // Obtener los operadores según el tipo de campo
    const getOperatorsForType = (type: string): FilterOperator[] => {
        const stringOperators = [
            FilterOperators.Equal,
            FilterOperators.NotEqual,
            FilterOperators.Like,
            FilterOperators.Contains,
            FilterOperators.StartsWith,
            FilterOperators.EndsWith,
        ];

        const numberDateOperators = [
            FilterOperators.Equal,
            FilterOperators.NotEqual,
            FilterOperators.GreaterThan,
            FilterOperators.LessThan,
            FilterOperators.GreaterThanOrEqual,
            FilterOperators.LessThanOrEqual,
        ];

        switch (type) {
            case 'string':
                return stringOperators;
            case 'int':
            case 'decimal':
            case 'double':
            case 'float':
            case 'DateTime':
                return numberDateOperators;
            default:
                return [FilterOperators.Equal, FilterOperators.NotEqual];
        }
    };

    const renderFilterInput = (field: string, type: string, operator: FilterOperator) => {
        const meta = schema[field];

        switch (type) {
            case 'DateTime':
                return operator === FilterOperators.Equal ? (
                    <DatePicker showTime style={{ width: '100%' }} />
                ) : (
                    <RangePicker showTime style={{ width: '100%' }} />
                );
            case 'boolean':
                return (
                    <Select placeholder="Seleccione">
                        <Select.Option value="true">Verdadero</Select.Option>
                        <Select.Option value="false">Falso</Select.Option>
                    </Select>
                );
            default:
                return <Input placeholder={`Filtrar por ${field}`} />;
        }
    };

    const handleFieldChange = (field: string) => {
        setSelectedField(field);
        setSelectedOperator(null); // Reset operador al cambiar campo
    };

    const handleOperatorChange = (operator: FilterOperator) => {
        setSelectedOperator(operator); // Actualiza el operador seleccionado
    };

    const handleSubmit = (values: any) => {
        const { column, operator, value } = values;
      
        if (!column || !operator || value === undefined || value === null || value === '') {
          return;
        }
      
        const newFilter: FilterDto = {
          column,
          operator,
          value: value instanceof Date
            ? value.toISOString()
            : typeof value === 'object' && value.length === 2 && value[0] instanceof Date
              ? `${value[0].toISOString()}|${value[1].toISOString()}`
              : String(value),
        };
      
        onSubmit(newFilter);
        form.resetFields(); // Limpiar después de aplicar
      };

    return (
        <Card title="Filtros Avanzados" style={{ marginBottom: 20 }}>
            <Form form={form} onFinish={handleSubmit}>
                <Row gutter={16}>
                    <Col span={8}>
                        {/* Selección de columna */}
                        <Form.Item
                            name="column"
                            label="Columna"
                            rules={[{ required: true, message: 'Por favor seleccione una columna' }]}
                        >
                            <Select onChange={handleFieldChange} placeholder="Seleccionar columna">
                                {validFields.map(field => (
                                    <Select.Option key={field} value={field}>
                                        {field}
                                    </Select.Option>
                                ))}
                            </Select>
                        </Form.Item>
                    </Col>

                    <Col span={8}>
                        {/* Selección de operador */}
                        {selectedField && (
                            <Form.Item
                                name="operator"
                                label="Operador"
                                rules={[{ required: true, message: 'Por favor seleccione un operador' }]}
                            >
                                <Select onChange={handleOperatorChange} placeholder="Seleccionar operador">
                                    {getOperatorsForType(schema[selectedField].type).map(operator => (
                                        <Select.Option key={operator} value={operator}>
                                            {operator}
                                        </Select.Option>
                                    ))}
                                </Select>
                            </Form.Item>
                        )}
                    </Col>

                    <Col span={8}>
                        {/* Valor del filtro */}
                        {selectedField && selectedOperator && (
                            <Form.Item
                                name="value"
                                label="Valor"
                                rules={[{ required: true, message: 'Por favor ingrese un valor' }]}
                            >
                                {renderFilterInput(selectedField, schema[selectedField].type, selectedOperator)}
                            </Form.Item>
                        )}
                    </Col>
                </Row>

                <Space>
                    <Button type="primary" htmlType="submit">
                        Aplicar Filtros
                    </Button>
                    <Button onClick={() => {
                        form.resetFields();
                        onClear();
                    }}>
                        Limpiar
                    </Button>
                </Space>
            </Form>
        </Card>
    );
}
