import { useEffect, useState } from "react";
import DynamicForm from "@/components/DynamicForm";
import { Schema } from "@/types/Schema";
import { Button, Form, message } from "antd";
import genericService from "@/services/genericService";

export default function DynamicCreateRolePage() {
    const [schema, setSchema] = useState<Schema | null>(null);
    const [form] = Form.useForm();

    useEffect(() => {
        if (!schema) {
            console.log("Cargando esquema...");
            genericService.getCreateSchema("roles")
                .then(setSchema)
                .catch(() => message.error("Error al cargar el esquema"));
        }
    }, [schema]);

    const handleSubmit = (values: Record<string, any>) => {
        console.log("Datos enviados:", values);
        genericService.create("roles", values)
            .then(() => message.success("Registro creado con éxito"))
            .catch((errorDetail) => message.error(errorDetail.detail));
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
        </Form>
    );
}
