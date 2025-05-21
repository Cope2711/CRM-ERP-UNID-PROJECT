import { Button, Tooltip } from 'antd';
import { useNavigate } from 'react-router-dom';

type Props = {
  model: string;  
  id: string;     
  text: string;   
  tooltip?: string;
};

export default function LinkedRef({ model, id, text, tooltip }: Props) {
  const navigate = useNavigate();

  return (
    <Tooltip title={tooltip}>
      <Button type="link" onClick={() => navigate(`/${model}/${id}`)}>
        {text}
      </Button>
    </Tooltip>
  );
}
