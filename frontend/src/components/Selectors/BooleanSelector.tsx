import { Select } from "antd";

type BooleanSelectorProps = {
    value?: boolean;
    onChange?: (value: boolean) => void;
};

export default function BooleanSelector({ value, onChange }: BooleanSelectorProps) {
    return (
        <Select
            value={value}
            onChange={onChange}
            options={[
                { label: "Sí", value: true },
                { label: "No", value: false },
            ]}
            placeholder="Seleccione"
        />
    );
}