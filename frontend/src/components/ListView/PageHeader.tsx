import { pageSizeOptions } from '@/constants/table';
import { Typography, Select } from 'antd';

const { Title } = Typography;

type PageHeaderProps = {
  modelName: string;
  pageSize: number;
  onPageSizeChange: (value: number) => void;
};

export default function PageHeader({ modelName, pageSize, onPageSizeChange }: PageHeaderProps) {
  return (
    <div className="flex justify-between items-center">
      <Title level={3}>Lista de {modelName}</Title>
      <div>
        <span className="mr-2">Tamaño de página:</span>
        <Select
          value={pageSize}
          onChange={onPageSizeChange}
          options={pageSizeOptions.map(size => ({ label: size, value: size }))}
          style={{ width: 100 }}
        />
      </div>
    </div>
  );
}