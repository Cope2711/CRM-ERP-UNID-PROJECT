import { useState, useEffect } from "react";
import genericService from "@/services/genericService";
import { FilterOperators } from "@/constants/filterOperators";
import { FilterDto, GetAllDto } from "@/dtos/GenericDtos";
import { extractIsObjectKeyName } from "@/utils/objectUtils";
import { LoadingSpinner } from "../Loading/loadingSpinner";
import { ErrorMessage } from "../message/ErrorMessage";

/**
 * Props del componente DynamicRelationViewer
 * @property {string} id - ID del registro principal
 * @property {Record<string, any>} schema - Esquema que define las relaciones
 * @property {(data: Record<string, any>) => void} setRelationData - Callback para actualizar datos de relaciones
 */
type RelationViewerProps = {
    id: string;
    schema: Record<string, any>;
    setRelationData: (data: Record<string, any>) => void;
};

/**
 * Hook para obtener las claves de relaciones válidas del esquema
 * @param {Record<string, any>} schema - Esquema del modelo
 * @param {string} id - ID del registro
 * @returns {string[]} Array con las claves de relaciones válidas
 */
const useRelationData = (schema: Record<string, any>, id: string) => {
    return Object.keys(schema).filter(
        (key) => schema[key]?.type?.startsWith('list') && schema[key]?.specialData?.includes("IsRelation")
    );
};

/**
 * Componente para mostrar una tabla de datos de relación
 * @param {Object} props
 * @param {string[]} props.columns - Columnas a mostrar
 * @param {any[]} props.data - Datos a mostrar en la tabla
 */
const RelationTable = ({ columns, data }: { columns: string[]; data: any[] }) => (
    <table className="min-w-full divide-y divide-gray-200">
        <thead className="bg-gray-50">
            <tr>
                {columns.map((column) => (
                    <th key={column} className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                        {column}
                    </th>
                ))}
            </tr>
        </thead>
        <tbody className="bg-white divide-y divide-gray-200">
            {data.map((item, index) => (
                <tr key={index}>
                    {columns.map((column) => (
                        <td key={`${index}-${column}`} className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                            {item[column]?.toString() || '-'}
                        </td>
                    ))}
                </tr>
            ))}
        </tbody>
    </table>
);

/**
 * Componente principal para visualizar relaciones dinámicas de un registro
 * Muestra:
 * - Botones para navegar entre diferentes relaciones
 * - Tabla con los datos de la relación seleccionada
 * - Estados de carga y error
 */
const DynamicRelationViewer = ({ id, schema, setRelationData }: RelationViewerProps) => {
    // Estado para la relación activa seleccionada
    const [activeKey, setActiveKey] = useState<string | null>(null);
    
    // Estado para controlar qué relaciones ya se han cargado
    const [loadedRelations, setLoadedRelations] = useState<Set<string>>(new Set());
    
    // Estado para los datos de las relaciones
    const [relationData, setLocalRelationData] = useState<Record<string, any>>({});
    
    // Estado de carga
    const [loading, setLoading] = useState(false);
    
    // Estado para mensajes de error
    const [error, setError] = useState<string | null>(null);

    // Obtiene las claves de relaciones válidas del esquema
    const relationKeys = useRelationData(schema, id);

    /**
     * Efecto para inicializar la primera relación al montar el componente
     * - Selecciona la primera relación disponible
     * - Carga sus datos automáticamente
     */
    useEffect(() => {
        if (relationKeys.length > 0 && !activeKey) {
            const firstKey = relationKeys[0];
            setActiveKey(firstKey);
            loadRelationData(firstKey); 
        }
    }, [relationKeys]); // Se ejecuta cuando cambian las relaciones disponibles

    /**
     * Función para cargar los datos de una relación específica
     * @param {string} key - Clave de la relación a cargar
     */
    const loadRelationData = async (key: string) => {
        // Evita recargar si ya está cargada
        if (loadedRelations.has(key)) return;

        const relationInfo = schema[key]?.relationInfo;
        if (!relationInfo) return;

        setLoading(true);
        setError(null);

        try {
            // Configuración del filtro para obtener solo los relacionados con el registro principal
            const filters: FilterDto[] = [
                { column: extractIsObjectKeyName(schema), operator: FilterOperators.Equal, value: id }
            ];

            // Configuración de la petición
            const getAllDto: GetAllDto = {
                pageNumber: 1,
                pageSize: 10,
                orderBy: relationInfo.selects[0],
                descending: false,
                filters,
                selects: relationInfo.selects
            };

            // Obtiene los datos de la API
            const response = await genericService.getAll(relationInfo.controller, getAllDto);

            // Actualiza los estados con los nuevos datos
            const newData = { [key]: response.data };
            setLocalRelationData(prev => ({ ...prev, ...newData }));
            setRelationData((prev: any) => ({ ...prev, ...newData }));
            setLoadedRelations(prev => new Set(prev).add(key));
        } catch (err) {
            console.error(`Error loading relation ${key}`, err);
            setError(`Error loading ${key} data`);
        } finally {
            setLoading(false);
        }
    };

    /**
     * Efecto para cargar datos cuando cambia la relación activa
     */
    useEffect(() => {
        if (activeKey && !loadedRelations.has(activeKey)) {
            loadRelationData(activeKey);
        }
    }, [activeKey]); // Se ejecuta cuando cambia la relación activa

    // Mensaje cuando no hay relaciones disponibles
    if (relationKeys.length === 0) {
        return <div className="text-center text-gray-500">No relations to display.</div>;
    }

    // Obtiene datos y columnas para la relación activa
    const currentData = activeKey ? relationData[activeKey] : [];
    const columns = activeKey && schema[activeKey]?.relationInfo?.selects || [];

    return (
        <div className="h-full flex flex-col">
            {/* Barra de navegación entre relaciones */}
            <div className="flex gap-2 mb-4">
                {relationKeys.map((key) => (
                    <button
                        key={key}
                        onClick={() => setActiveKey(key)}
                        className={`px-4 py-2 rounded transition text-white ${activeKey === key ? "bg-blue-600" : "bg-blue-500 hover:bg-blue-600"
                            }`}
                    >
                        {/* Formatea el nombre de la relación (primera letra mayúscula) */}
                        {key.charAt(0).toUpperCase() + key.slice(1)}
                    </button>
                ))}
            </div>

            {/* Indicador de carga */}
            {loading && <LoadingSpinner />}
            
            {/* Mensaje de error */}
            {error && <ErrorMessage message={error} />}

            {/* Contenido principal: Tabla de datos o mensaje vacío */}
            {activeKey && !loading && !error && (
                <div className="flex-1 overflow-auto">
                    {currentData?.length > 0 ? (
                        <RelationTable columns={columns} data={currentData} />
                    ) : (
                        <div className="text-center text-gray-500 py-4">
                            No data available for this relation.
                        </div>
                    )}
                </div>
            )}
        </div>
    );
};

export default DynamicRelationViewer;