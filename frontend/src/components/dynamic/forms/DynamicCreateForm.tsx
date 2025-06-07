import genericService from "@/services/genericService";
import { PropertiesSchema } from "@/types/Schema";
import { Button, Form, message } from "antd";
import { useEffect, useState } from "react";
import DynamicForm from "./DynamicForm";
import PopupMessage from "@/components/message/SuccessMessage";
import { setFormErrorField } from "@/utils/setterHelper";

type DynamicCreateFormProps = {
    modelName: string;
    defaultPropertiesSchema?: PropertiesSchema;
};

export default function DynamicCreateForm({ modelName, defaultPropertiesSchema }: DynamicCreateFormProps) {
    const [propertiesSchema, setPropertiesSchema] = useState<PropertiesSchema | undefined>(defaultPropertiesSchema); 
    const [form] = Form.useForm();
    const [showSuccessMessage, setShowSuccessMessage] = useState(false);
    const [showErrorMessage, setShowErrorMessage] = useState(false);

    useEffect(() => {
        getSchema();
    }, [propertiesSchema, modelName]);

    const getSchema = async () => {
        if (propertiesSchema) return;

        try {
            const schema = await genericService.getSchemas(modelName);
            setPropertiesSchema(schema);
        } catch (error) {
            message.error("Error al cargar el esquema");
        }
    };

    const handleSubmit = async (values: Record<string, any>) => {
        try {
            await genericService.create(modelName, values);
            setShowSuccessMessage(true);
        } catch (error: any) {
            setFormErrorField(form, error);
            if (!error?.field || !error?.detail){
                setShowErrorMessage(true);
            }
        }
    };

    if (!propertiesSchema) return <p style={{ padding: 24 }}>Cargando formulario...</p>;

    return (
        <Form
            form={form}
            layout="vertical"
            onFinish={handleSubmit}
            style={{ maxWidth: 600, margin: "0 auto", padding: 24 }}
        >
            <DynamicForm propertiesSchema={propertiesSchema} />

            <Form.Item>
                <Button type="primary" htmlType="submit" block> Guardar </Button>
            </Form.Item>

            {showSuccessMessage && <PopupMessage message={`Objeto creado con éxito`} />}
            {showErrorMessage && <PopupMessage message={`Error al crear objeto`} isSuccess={false} />}
        </Form>
    );
};