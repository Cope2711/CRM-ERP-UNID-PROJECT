import { useEffect, useState } from "react";
import { Button, Form, message } from "antd";
import genericService from "@/services/genericService";
import { normalizeKeysToLower } from "@/utils/objectUtils";
import { Schema } from "@/types/Schema";
import DynamicForm from "./DynamicForm";
import SuccessMessage from "../../message/SuccessMessage";
import { LoadingSpinner } from "@/components/Loading/loadingSpinner";

/**
 * Props del componente DynamicUpdateForm
 * @property {string} modelName - Nombre del modelo a actualizar (ej. "users", "products")
 * @property {string} id - ID del registro a actualizar
 * @property {Record<string, any>} [defaultData] - Datos iniciales opcionales para el formulario
 */
type UpdateFormProps = {
  modelName: string;
  id: string;
  defaultData?: Record<string, any> | null;
};

/**
 * Hook personalizado para manejar la lógica del formulario de actualización
 * @param {string} modelName - Nombre del modelo
 * @param {string} id - ID del registro
 * @param {Record<string, any>} [defaultData] - Datos iniciales opcionales
 * @returns {Object} Objeto con:
 * - schema: Esquema del formulario
 * - form: Instancia del formulario Antd
 * - showSuccess: Estado para mostrar mensaje de éxito
 * - handleSubmit: Función para manejar el envío del formulario
 */
const useUpdateForm = (modelName: string, id: string, defaultData?: Record<string, any> | null) => {
  // Estado para el esquema del formulario
  const [schema, setSchema] = useState<Schema | null>(null);

  // Instancia del formulario Antd
  const [form] = Form.useForm();

  // Estado para controlar el mensaje de éxito
  const [showSuccess, setShowSuccess] = useState(false);

  /**
   * Efecto para cargar:
   * 1. El esquema del formulario
   * 2. Los datos iniciales del registro
   */
  useEffect(() => {
    // Carga el esquema solo si no está cargado
    if (!schema) {
      genericService.getSchemas(modelName, "update")
        .then(setSchema)
        .catch(() => message.error("Error loading schema"));
    }

    // Si hay datos por defecto, los establece en el formulario
    if (defaultData) {
      form.setFieldsValue(normalizeKeysToLower(defaultData));
    } else {
      // Si no hay datos por defecto, los carga desde la API
      genericService.getById(modelName, id)
        .then(data => form.setFieldsValue(normalizeKeysToLower(data)))
        .catch(() => message.error("Error loading data"));
    }
  }, [schema, modelName, id, defaultData, form]);

  /**
   * Función para manejar el envío del formulario
   * @param {Record<string, any>} values - Valores del formulario
   */
  const handleSubmit = async (values: Record<string, any>) => {
    try {
      // Envía los datos a la API
      await genericService.update(modelName, values, id);

      // Muestra mensaje de éxito
      setShowSuccess(true);
    } catch (error: any) {
      console.error("Update error:", error);

      // Manejo de errores específicos por campo
      if (error?.field && error?.detail) {
        form.setFields([{
          name: error.field.toLowerCase(),
          errors: [error.detail]
        }]);
      } else {
        // Error genérico
        message.error("Update failed");
      }
    }
  };

  return { schema, form, showSuccess, handleSubmit };
};

/**
 * Componente principal del formulario dinámico de actualización
 * - Genera campos basados en el esquema del modelo
 * - Maneja la actualización de registros
 * - Proporciona feedback visual
 */
const DynamicUpdateForm = ({ modelName, id, defaultData }: UpdateFormProps) => {
  // Usa el hook personalizado para manejar la lógica del formulario
  const { schema, form, showSuccess, handleSubmit } = useUpdateForm(modelName, id, defaultData);

  // Muestra spinner mientras carga el esquema
  if (!schema) return <LoadingSpinner />;

  return (
    <Form
      form={form}
      layout="vertical"  // Diseño vertical para los campos
      onFinish={handleSubmit}  // Manejador de envío
      style={{ maxWidth: 600, margin: "0 auto", padding: 24 }}  // Estilos del contenedor
    >
      {/* Componente dinámico que genera los campos basados en el schema */}
      <DynamicForm schema={schema} />

      {/* Botón de envío del formulario */}
      <Form.Item>
        <Button type="primary" htmlType="submit" block>
          Update
        </Button>
      </Form.Item>

      {/* Mensaje de éxito al actualizar */}
      {showSuccess && (
        <SuccessMessage
          message={`${modelName.charAt(0).toUpperCase() + modelName.slice(1)} updated successfully`}
        />
      )}
    </Form>
  );
};

export default DynamicUpdateForm;