import { pageSizeOptions } from '@/constants/table';
import { Typography, Select } from 'antd';

const { Title } = Typography;

type PageHeaderProps = {
  modelName: string;
  pageSize: number;
  onPageSizeChange: (value: number) => void;
  extraButton?: React.ReactNode;
};

export default function PageHeader({ modelName, pageSize, onPageSizeChange, extraButton }: PageHeaderProps) {
  return (
    <div className="flex justify-between items-center flex-wrap gap-4">
      <Title level={3} className="m-0">Lista de {modelName}</Title>
      <div className="flex items-center gap-4">
        {extraButton}
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
    </div>
  );
}