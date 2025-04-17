import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import genericService from "@/services/genericService";
import DynamicUpdateForm from "@/components/dynamic/forms/DynamicUpdateForm";
import DynamicRelationViewer from "@/components/dynamic/DynamicRelationViewer";
import { LoadingSpinner } from "@/components/Loading/loadingSpinner";

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
    // Estado para los datos principales del registro
    const [data, setData] = useState<Record<string, any> | null>(null);
    
    // Estado para el esquema del modelo
    const [schema, setSchema] = useState<any | null>(null);
    
    // Estado para los datos de relaciones
    const [relationData, setRelationData] = useState<Record<string, any>>({});

    useEffect(() => {
        if (id) {
            // Carga en paralelo los datos del registro y su esquema
            Promise.all([
                // Obtiene los datos del registro específico
                genericService.getById(modelName, id).catch(() => console.error(`Error loading ${modelName}`)),
                
                // Obtiene el esquema del modelo para saber su estructura
                genericService.getSchemas(modelName, "model").catch(() => console.error(`Error loading ${modelName} schema`))
            ]).then(([entityData, entitySchema]) => {
                // Actualiza los estados con los datos obtenidos
                setData(entityData);
                setSchema(entitySchema);
            });
        }
    }, [id, modelName]); // Se ejecuta cuando cambia el id o el modelName

    return { data, schema, relationData, setRelationData };
};

/**
 * Componente principal para mostrar el detalle de un registro específico
 * Muestra dos paneles:
 * 1. Información principal del registro con formulario de edición
 * 2. Relaciones asociadas al registro
 */
const GenericDetailPage = ({ modelName }: DetailPageProps) => {
    // Obtiene el ID de los parámetros de la URL
    const { id } = useParams<{ id: string }>();
    
    // Usa el hook personalizado para cargar los datos
    const { data, schema, relationData, setRelationData } = useDetailData(modelName, id);

    // Muestra un spinner de carga mientras se obtienen los datos
    if (!data || !id || !schema) return <LoadingSpinner />;

    return (
        <div className="flex w-full">
            {/* Panel izquierdo: Información principal del registro */}
            <div className="w-1/2 min-h-screen bg-white shadow rounded-xl p-6 border border-gray-200">
                <h2 className="text-lg font-semibold mb-4 text-blue-700">
                    {modelName} Information
                </h2>
                
                {/* Contenedor del formulario de actualización */}
                <div className="bg-blue-100 p-4 rounded-xl mb-4">
                    {/* Componente dinámico para editar el registro */}
                    <DynamicUpdateForm 
                        modelName={modelName} 
                        id={id} 
                        defaultData={data} 
                    />
                </div>
            </div>

            {/* Panel derecho: Relaciones del registro */}
            <div className="w-1/2 h-screen sticky top-0 overflow-hidden">
                <div className="bg-white shadow rounded-xl p-6 border border-gray-200 h-full flex flex-col">
                    <h2 className="text-lg font-semibold mb-4 text-purple-700">Relations</h2>
                    
                    {/* Componente para visualizar relaciones dinámicas */}
                    <DynamicRelationViewer
                        id={id}
                        schema={schema}
                        setRelationData={setRelationData}
                    />
                </div>
            </div>
        </div>
    );
};

export default GenericDetailPage;