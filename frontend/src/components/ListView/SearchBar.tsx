import { Input, Button, Space, Select } from 'antd';
import { FilterOutlined, SearchOutlined } from '@ant-design/icons';

type SearchBarProps = {
  searchText: string;
  searchField: string;
  validFields: string[];
  onSearch: (text: string, field: string) => void;
  onToggleFilters: () => void;
  showFilters: boolean;
};

export default function SearchBar({
  searchText,
  searchField,
  validFields,
  onSearch,
  onToggleFilters,
  showFilters,
}: SearchBarProps) {
  return (
    <div className="flex justify-between mb-4">
      <Space>
        <Select
          allowClear
          value={searchField || undefined}
          onChange={(value) => onSearch(searchText, value)}
          style={{ width: 200 }}
          placeholder="Columna a buscar..."
          options={validFields.map(field => ({
            value: field,
            label: field.charAt(0).toUpperCase() + field.slice(1) // capitaliza
          }))}
        />
        <Input
          placeholder="Buscar..."
          value={searchText}
          onChange={(e) => onSearch(e.target.value, searchField)}
          onPressEnter={() => onSearch(searchText, searchField)}
          style={{ width: 300 }}
          suffix={<SearchOutlined onClick={() => onSearch(searchText, searchField)} />}
        />
        <Button
          icon={<FilterOutlined />}
          onClick={onToggleFilters}
          type={showFilters ? 'primary' : 'default'}
        >
          Filtros Avanzados
        </Button>
      </Space>
    </div>
  );
}
