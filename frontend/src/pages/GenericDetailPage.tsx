import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import genericService from "@/services/genericService";
import { LoadingSpinner } from "@/components/Loading/loadingSpinner";
import NotFoundPage from "./NotFoundPage";
import ChangeActiveStatusButton from "@/components/Button/ChangeActiveStatusButton";
import DynamicUpdateForm from "@/components/dynamic/forms/DynamicUpdateForm";
import DynamicRelationViewer from "@/components/dynamic/dynamicRelationViewer/DynamicRelationViewer";
import { schemaToRelationsSchemas } from "@/utils/objectUtils";

/**
 * Props del componente GenericDetailPage
 * @property {string} modelName - Nombre del modelo que se está visualizando (ej. "users", "products")
 */
type DetailPageProps = {
    modelName: string;
};

/**
 * Hook personalizado para manejar la carga de datos del detalle
 * @param {string} modelName - Nombre del modelo
 * @param {string} [id] - ID del registro a mostrar (opcional)
 * @returns {Object} Objeto con los datos, esquema y funciones para manejar relaciones
 */
const useDetailData = (modelName: string, id?: string) => {
    const [data, setData] = useState<Record<string, any> | null>(null);
    const [propertiesSchema, setPropertiesSchema] = useState<any | null>(null);
    const [relationData, setRelationData] = useState<Record<string, any>>({});
    const [loading, setLoading] = useState(true);
    const [notFound, setNotFound] = useState(false);

    useEffect(() => {
        if (id) {
            setLoading(true);
            setNotFound(false);

            Promise.all([
                genericService.getById(modelName, id),
                genericService.getSchemas(modelName)
            ])
                .then(([entityData, entitySchema]) => {
                    if (!entityData) {
                        setNotFound(true);
                    } else {
                        setData(entityData);
                        setPropertiesSchema(entitySchema);
                    }
                })
                .catch(() => {
                    setNotFound(true);
                })
                .finally(() => setLoading(false));
        }
    }, [id, modelName]);

    return { data, propertiesSchema, relationData, setRelationData, loading, notFound }
};

/**
 * Componente principal para mostrar el detalle de un registro específico
 * Muestra dos paneles:
 * 1. Información principal del registro con formulario de edición
 * 2. Relaciones asociadas al registro
 */
const GenericDetailPage = ({ modelName }: DetailPageProps) => {
    const { id } = useParams<{ id: string }>();
    const { data, propertiesSchema, loading, notFound } = useDetailData(modelName, id);

    if (loading) return <LoadingSpinner />;
    if (notFound || !id) return <NotFoundPage />;

    return (
        <div className="flex w-full">
            {/* Panel izquierdo */}
            <div className="w-1/2 min-h-screen bg-white shadow rounded-xl p-6 border border-gray-200">
                {/* Header con título y botón alineados */}
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-lg font-semibold text-blue-700">
                        {modelName} Information
                    </h2>
                    <ChangeActiveStatusButton
                        modelName={modelName}
                        id={id}
                        isActive={data?.isActive}
                    />
                </div>

                <div className="bg-blue-100 p-4 rounded-xl mb-4">
                    <DynamicUpdateForm
                        modelName={modelName}
                        id={id}
                        defaultData={data}
                        defaultPropertiesSchema={propertiesSchema}
                    />
                </div>
            </div>

            {/* Panel derecho */}
            <div className="w-1/2 h-screen sticky top-0 overflow-hidden">
                <div className="bg-white shadow rounded-xl p-6 border border-gray-200 h-full flex flex-col">
                    <h2 className="text-lg font-semibold mb-4 text-purple-700">Relations</h2>
                    <DynamicRelationViewer
                        id={id}
                        relationsSchemas={schemaToRelationsSchemas(propertiesSchema)}
                    />
                </div>
            </div>
        </div>
    );
};

export default GenericDetailPage;
