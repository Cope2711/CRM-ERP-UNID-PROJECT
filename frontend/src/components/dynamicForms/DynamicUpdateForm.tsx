import { useEffect, useState } from "react";
import { Button, Form, message } from "antd";
import genericService from "@/services/genericService";
import { normalizeKeysToLower } from "@/utils/objectUtils";
import { Schema } from "@/types/Schema";
import DynamicForm from "./DynamicForm";
import SuccessMessage from "../message/SuccessMessage";

type DynamicUpdateFormProps = {
    modelName: string; // "users", "roles", etc.
    id: string;        // El id del objeto a actualizar, como userId o roleId
};

export default function DynamicUpdateForm({ modelName, id }: DynamicUpdateFormProps) {
    const [schema, setSchema] = useState<Schema | null>(null);
    const [form] = Form.useForm();
    const [showSuccessMessage, setShowSuccessMessage] = useState(false); // Estado para manejar el mensaje de éxito

    useEffect(() => {
        if (!schema) {
            // Obtener el esquema para el modelo específico
            genericService.getSchemas(modelName, "update")
                .then(setSchema)
                .catch(() => message.error("Error al cargar el esquema"));

            // Cargar los datos del modelo específico usando el id
            genericService.getById(modelName, id)
                .then((data) => {
                    form.setFieldsValue(normalizeKeysToLower(data)); // Establecer valores en el formulario
                })
                .catch(() => message.error("Error al cargar los datos"));
        }
    }, [schema, id, modelName]);

    const handleSubmit = (values: Record<string, any>) => {
        genericService.update(modelName, values, id) // Pasar el modelName y el id para actualizar
            .then(() => {
                setShowSuccessMessage(true); // Muestra el mensaje de éxito
                setTimeout(() => setShowSuccessMessage(false), 3000); // Ocultar el mensaje después de 3 segundos
            })
            .catch((error: any) => {
                console.log("Error: ", error);
                
                // Verificar si el error tiene un campo específico y un mensaje de detalle
                if (error?.field && error?.detail) {
                    form.setFields([
                        {
                            name: error.field.toLowerCase(),
                            errors: [error.detail],
                        },
                    ]);
                } else {
                    message.error("Error al actualizar el objeto");
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
                    Actualizar
                </Button>
            </Form.Item>

            {/* Mostrar mensaje de éxito si el estado es verdadero */}
            {showSuccessMessage && <SuccessMessage message={`${modelName.charAt(0).toUpperCase() + modelName.slice(1)} actualizado con éxito`} />}
        </Form>
    );
}
