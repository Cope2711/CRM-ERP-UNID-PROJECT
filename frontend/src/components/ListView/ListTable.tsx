import { Table, Tooltip, Space, Button } from 'antd';
import { ColumnsType } from 'antd/es/table';
import { EyeOutlined } from '@ant-design/icons';
import { useMemo } from 'react';
import LinkedRef from '../LinkedRef';

type SchemaMeta = {
  type?: string;
  required?: boolean;
  select?: string;
  controller?: string;
  [key: string]: any;
};

type ListTableProps = {
  modelName: string; // Current model name, used for routing and LinkedRef component
  items: any[]; // Data rows to display in the table
  schema: Record<string, SchemaMeta> | null; // Metadata describing each field of the model
  validFields: string[]; // Fields to show in the table (filtered externally)
  primaryKey: string; // Field name used as unique identifier for rows
  page: number; // Current pagination page
  pageSize: number; // Number of rows per page
  totalItems: number; // Total number of items in dataset
  onPageChange: (page: number, pageSize: number) => void; // Callback to update pagination
  navigate: (path: string) => void; // Navigation function (e.g. React Router) for action buttons
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
  navigate,
}: ListTableProps) {
  // Generate table columns based on schema and valid fields
  const columns: ColumnsType<any> = useMemo(() => {
    if (!schema) return [];

    // Build a map of "select" fields to their corresponding schema keys for quick lookups
    const selectMap = new Map<string, string>();
    Object.entries(schema).forEach(([key, meta]) => {
      if (meta.select) selectMap.set(meta.select, key);
    });

    // Filter out guid fields that are referenced via "select" to avoid duplicate columns
    const filteredFields = validFields.filter((key) => {
      const meta = schema[key];
      return !(meta?.type === 'guid' && meta.select);
    });

    // Map filtered fields to column definitions
    const generatedColumns = filteredFields.map((key) => {
      // Resolve field metadata, accounting for nested fields (dot notation)
      let meta: SchemaMeta = schema[key];
      if (key.includes('.')) {
        const relatedKey = selectMap.get(key);
        // Fallback to default meta if no related key found
        meta = relatedKey ? schema[relatedKey] : { type: 'unknown', required: false };
      }

      return {
        title: key, // Column header
        dataIndex: key, // Data index in each record
        key, // Unique key for React rendering
        render: (value: any, record: any) => {
          // Tooltip shows type and if the field is required
          const tooltip = `${meta.type ?? 'unknown'}${meta.required ? ' (required)' : ''}`;

          // Render normal value with tooltip if not a guid type
          if (meta.type !== 'guid') {
            return <Tooltip title={tooltip}>{String(value)}</Tooltip>;
          }

          // For guid fields, render a LinkedRef component linking to related record
          const relatedKey = selectMap.get(key);
          const relatedMeta = relatedKey ? schema[relatedKey] : undefined;
          const relatedId = relatedKey ? record[relatedKey] : undefined;
          const linkedModel = relatedMeta?.controller || modelName;

          return (
            <LinkedRef
              model={linkedModel}
              id={relatedId}
              text={value}
              tooltip={tooltip}
            />
          );
        },
      };
    });

    // Append a fixed "Actions" column with a button to navigate to detail view
    generatedColumns.push({
      title: 'Actions',
      key: 'actions',
      render: (_: any, record: any) => (
        <Space>
          <Button
            icon={<EyeOutlined />}
            onClick={() => navigate(`/${modelName}/${record[primaryKey]}`)}
          >
            View
          </Button>
        </Space>
      ),
      dataIndex: '', // no dataIndex needed here
    });

    return generatedColumns;
  }, [schema, validFields, modelName, primaryKey, navigate]);

  return (
    <Table
      columns={columns}
      dataSource={items}
      // Use primaryKey for row identification, fallback to 'id'
      rowKey={(record) => record[primaryKey] ?? 'id'}
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
