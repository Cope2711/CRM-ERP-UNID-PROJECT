import { useEffect, useState } from "react";
import { Button, Form, message } from "antd";
import genericService from "@/services/genericService";
import { Schema } from "@/types/Schema";
import DynamicForm from "./DynamicForm";
import SuccessMessage from "../message/SuccessMessage";

type DynamicCreateFormProps = {
    modelName: string; // "users", "roles", etc.
};

export default function DynamicCreateForm({ modelName }: DynamicCreateFormProps) {
    const [schema, setSchema] = useState<Schema | null>(null);
    const [form] = Form.useForm();
    const [showSuccessMessage, setShowSuccessMessage] = useState(false); // Estado para el mensaje de éxito

    useEffect(() => {
        if (!schema) {
            // Obtener el esquema para el modelo específico
            console.log("Cargando esquema...");
            genericService.getSchemas(modelName, "create")
                .then(setSchema)
                .catch(() => message.error("Error al cargar el esquema"));
        }
    }, [schema, modelName]);

    const handleSubmit = (values: Record<string, any>) => {
        // Crear el objeto para el modelo específico
        genericService.create(modelName, values)
            .then(() => {
                setShowSuccessMessage(true); // Muestra el mensaje de éxito
                setTimeout(() => setShowSuccessMessage(false), 3000); // Oculta el mensaje después de 3 segundos
            })
            .catch((error: any) => {
                console.log("Error: ", error);
    
                if (error?.field && error?.detail) {
                    form.setFields([
                        {
                            name: error.field.toLowerCase(),
                            errors: [error.detail],
                        },
                    ]);
                } else {
                    message.error("Error al crear el objeto");
                }
            });
    };

    if (!schema) return <p style={{ padding: 24 }}>Cargando formulario...</p>;

    return (
        <Form
            form={form}
            layout="vertical"
            onFinish={handleSubmit}
            style={{ maxWidth: 600, margin: "0 auto", padding: 24 }}
        >
            <DynamicForm schema={schema} />
            <Form.Item>
                <Button type="primary" htmlType="submit" block>
                    Guardar
                </Button>
            </Form.Item>

            {/* Mostrar el mensaje de éxito si el estado es verdadero */}
            {showSuccessMessage && <SuccessMessage message={`${modelName.charAt(0).toUpperCase() + modelName.slice(1)} creado con éxito`} />}
        </Form>
    );
}
