import { Table, Tooltip, Space, Button } from 'antd';
import { ColumnsType } from 'antd/es/table';
import { EyeOutlined } from '@ant-design/icons';
import { useMemo } from 'react';

type ListTableProps = {
  modelName: string;
  items: any[];
  schema: Record<string, any> | null;
  validFields: string[];
  primaryKey: string;
  page: number;
  pageSize: number;
  totalItems: number;
  onPageChange: (page: number, pageSize: number) => void;
  navigate: (path: string) => void;
};

export default function ListTable({
  modelName,
  items,
  schema,
  validFields,
  primaryKey,
  page,
  pageSize,
  totalItems,
  onPageChange,
  navigate
}: ListTableProps) {
  const columns: ColumnsType<any> = useMemo(() => {
    if (!schema) return [];

    const generatedColumns = validFields.map((key) => {
      const meta = schema[key];
      return {
        title: key,
        dataIndex: key,
        key,
        render: (value: any, record: any) => {
          const tooltip = `${meta.type}${meta.required ? ' (requerido)' : ''}`;
          return <Tooltip title={tooltip}>{String(value)}</Tooltip>;
        },
      };
    });

    generatedColumns.push({
      title: 'Acciones',
      key: 'actions',
      dataIndex: '',  // Asignar un dataIndex vacío
      render: (_: any, record: any) => (
        <Space>
          <Button
            icon={<EyeOutlined />}
            onClick={() => navigate(`/${modelName}/${record[primaryKey]}`)}
          >
            Ver
          </Button>
        </Space>
      ),
    });

    return generatedColumns;
  }, [schema, validFields, modelName, primaryKey, navigate]);

  return (
    <Table
      columns={columns}
      dataSource={items}
      rowKey={(record) => record[primaryKey] || 'id'}
      pagination={{
        current: page,
        pageSize,
        total: totalItems,
        onChange: onPageChange,
        showSizeChanger: false,
      }}
    />
  );
}
