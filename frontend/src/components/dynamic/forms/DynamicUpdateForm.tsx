import genericService from "@/services/genericService";
import { PropertiesSchema } from "@/types/Schema";
import { setFormErrorField } from "@/utils/setterHelper";
import { Button, Form } from "antd";
import { useEffect, useState } from "react";
import DynamicForm from "./DynamicForm";
import PopupMessage from "@/components/message/SuccessMessage";
import { getChangedValues } from "@/utils/objectUtils";

type DynamicUpdateFormProps = {
    modelName: string; // Name of the model to update (eg. "users", "products")
    id: string; // ID of the record to update
    defaultData?: Record<string, any> | null; // Optional default data for the form
    defaultPropertiesSchema?: PropertiesSchema; // Optional default schema for the form
};

/**
 * Component for a dynamic form to update a record
 * 
 * Displays a form with fields based on the model schema
 * Handles the creation or update of records
 * Provides visual feedback for success or failure
 */
export default function DynamicUpdateForm({ modelName, id, defaultData, defaultPropertiesSchema: defaultSchema }: DynamicUpdateFormProps) {
    const [form] = Form.useForm();
    const [propertiesSchema, setPropertiesSchema] = useState<PropertiesSchema | undefined>(defaultSchema);
    const [data, setData] = useState<Record<string, any> | null>(null);
    const [showErrorMessage, setShowErrorMessage] = useState(false);
    const [showSuccessMessage, setShowSuccessMessage] = useState(false);

    // Load the schema when the model name changes
    useEffect(() => {
        getSchema();
        getData();
    }, [propertiesSchema, modelName]);

    // Set the data in the form when it changes
    useEffect(() => {
        if (data && propertiesSchema) {
            form.setFieldsValue(data);
        }
    }, [data]);

    // Load the schema from the API
    const getSchema = async () => {
        if (propertiesSchema) return;

        try {
            const schema = await genericService.getSchemas(modelName, true);
            setPropertiesSchema(schema);
            await getData(); // Load the data from the API
        } catch (error: any) {
            setShowErrorMessage(true);
        }
    };

    // Load the data from the API
    const getData = async () => {
        // If there is a default data, set it in the form
        if (defaultData) {
            setData(defaultData);
            return;
        }

        try {
            const data = await genericService.getById(modelName, id);
            setData(data);
        } catch (error: any) {
            setShowErrorMessage(true); // Show error message
        }
    };

    const handleSubmit = async (values: Record<string, any>) => {
        try {
            if (!data) return;

            const changedValues = getChangedValues(data, values); // Get the changed values

            // If there are no changes, do nothing
            if (Object.keys(changedValues).length === 0) {
                return;
            }

            await genericService.update(modelName, changedValues, id);
            setShowSuccessMessage(true);
        } catch (error: any) {
            setFormErrorField(form, error); // Show error message in the form
            // If there is no field or detail in the error, show a generic error message
            if (!error?.field || !error?.detail) {
                setShowErrorMessage(true);
            }
        }
    };

    // Show a loading spinner if the schema is not loaded
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

            {showSuccessMessage && <PopupMessage message={`Objeto actualizado con éxito`} />}
            {showErrorMessage && <PopupMessage message={`Error al actualizar objeto`} isSuccess={false} />}
        </Form>
    );
};