import { useState, useEffect, useMemo } from "react";
import genericService from "@/services/genericService";
import { extractIsObjectKeyName, toLowerRelationKey } from "@/utils/objectUtils";
import { FilterDto, GetAllDto } from "@/dtos/GenericDtos";
import { FilterOperators } from "@/constants/filterOperators";
import { Tabs, Button, Space, Alert } from "antd";
import AlertMessage from "@/components/message/AlertMessage";
import SuccessMessage from "@/components/message/SuccessMessage";
import { LoadingOutlined } from "@ant-design/icons";
import AssignButton from "@/components/Button/AssignButton";
import { useRelationData } from "./useRelationData";
import { RelationTable } from "./RelationTable";
import { extractLastKeyPart, extractLastKeyParts } from "@/utils/stringUtils";

/**
 * Props para el componente DynamicRelationViewer
 */
type RelationViewerProps = {
    id: string; // ID del modelo principal
    schema: Record<string, any>; // Esquema que define las relaciones
    setRelationData: (data: Record<string, any>) => void; // Callback para actualizar datos de relación
};

/**
 * Componente para visualizar y gestionar relaciones dinámicas entre modelos
 * 
 * Muestra las relaciones disponibles en pestañas y permite:
 * - Visualizar datos relacionados
 * - Asignar nuevas relaciones
 * - Revocar relaciones existentes
 */
const DynamicRelationViewer = ({ id, schema, setRelationData }: RelationViewerProps) => {
    // Estados del componente
    const [activeKey, setActiveKey] = useState<string>(""); // Pestaña activa
    const [loadedRelations, setLoadedRelations] = useState<Set<string>>(new Set()); // Relaciones ya cargadas
    const [relationData, setLocalRelationData] = useState<Record<string, any>>({}); // Datos de relaciones
    const [loading, setLoading] = useState(false); // Estado de carga
    const [error, setError] = useState<string | null>(null); // Mensaje de error
    const [selectedIds, setSelectedIds] = useState<string[]>([]); // IDs seleccionados
    const [showSuccess, setShowSuccess] = useState(false); // Mostrar mensaje de éxito

    // Obtener las claves de relación del esquema
    const relationKeys = useRelationData(schema);

    // Mapeo de información de relación para acceso rápido
    const relationInfoMap = useMemo(() => {
        const map: Record<string, any> = {};
        relationKeys.forEach(key => {
            map[key] = schema[key]?.relationInfo;
        });
        return map;
    }, [schema, relationKeys]);

    // Efecto para cargar la primera relación al montar el componente
    useEffect(() => {
        if (relationKeys.length > 0 && !activeKey) {
            const firstKey = relationKeys[0];
            setActiveKey(firstKey);
            loadRelationData();
        }
    }, [relationKeys]);

    // Efecto para cargar datos cuando cambia la pestaña activa
    useEffect(() => {
        if (activeKey && !loadedRelations.has(activeKey)) {
            loadRelationData();
        }
    }, [activeKey]);

    /**
     * Carga los datos de la relación activa
     */
    const loadRelationData = async () => {
        if (!activeKey) return;
        await fetchRelationData(activeKey, false);
    };

    /**
     * Fuerza la recarga de los datos de la relación activa
     */
    const forceReloadRelationData = async () => {
        if (!activeKey) return;
        await fetchRelationData(activeKey, true);
    };

    /**
     * Obtiene los datos de una relación específica
     * @param key - Clave de la relación a cargar
     * @param force - Forzar recarga incluso si ya estaba cargada
     */
    const fetchRelationData = async (key: string, force = false) => {
        const relationInfo = relationInfoMap[key];
        if (!relationInfo) return;

        // Evitar recarga innecesaria
        if (!force && loadedRelations.has(key)) return;

        setLoading(true);
        setError(null);
        setSelectedIds([]);

        try {
            // Configurar filtros para obtener solo las relaciones del modelo actual
            const filters: FilterDto[] = [
                { 
                    column: extractIsObjectKeyName(schema), 
                    operator: FilterOperators.Equal, 
                    value: id 
                },
            ];

            // Configurar DTO para la consulta
            const getAllDto: GetAllDto = {
                pageNumber: 1,
                pageSize: 10,
                orderBy: relationInfo.selects[0],
                descending: false,
                filters,
                selects: relationInfo.selects,
            };

            // Obtener datos del servicio
            const response = await genericService.getAll(relationInfo.controller, getAllDto);
            const newData = { [key]: response.data };

            // Actualizar estados con los nuevos datos
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
     * Maneja la selección/deselección de filas
     * @param id - ID del elemento
     * @param isSelected - Indica si está seleccionado o no
     */
    const handleRowSelect = (id: string, isSelected: boolean) => {
        setSelectedIds(prev =>
            isSelected 
                ? [...prev, id] // Agregar si está seleccionado
                : prev.filter(existingId => existingId !== id) // Remover si no está seleccionado
        );
    };

    /**
     * Revoca las relaciones seleccionadas
     */
    const handleRevoke = async () => {
        if (selectedIds.length === 0) return;
        setLoading(true);
        setError(null);

        const relationInfo = relationInfoMap[activeKey];
        if (!relationInfo) return;

        try {
            // Enviar solicitud de revocación
            await genericService.revoke(relationInfo.controller, selectedIds);
            
            // Recargar datos y resetear selección
            forceReloadRelationData();
            setSelectedIds([]);
            setShowSuccess(true);
        } catch (err) {
            console.error(`Error revoking relation ${activeKey}`, err);
            setError(`Error revoking ${activeKey} data`);
        } finally {
            setLoading(false);
        }
    };

    // Mostrar mensaje si no hay relaciones
    if (relationKeys.length === 0) {
        return <div className="text-center text-gray-500">No relations to display.</div>;
    }

    // Datos y columnas para la relación activa
    const currentData = activeKey ? relationData[activeKey] : [];
    const columns = activeKey && relationInfoMap[activeKey]?.selects || [];

    return (
        <div className="h-full flex flex-col gap-4">
            {/* Pestañas de relaciones */}
            <Tabs
                activeKey={activeKey}
                onChange={setActiveKey}
                items={relationKeys.map((key: string) => ({
                    label: key.charAt(0).toUpperCase() + key.slice(1), // Capitalizar primera letra
                    key,
                }))}
            />

            {/* Indicador de carga */}
            {loading && (
                <div className="flex justify-center py-4">
                    <LoadingOutlined spin style={{ fontSize: 24 }} />
                </div>
            )}

            {/* Mensajes de estado */}
            {error && <AlertMessage message={error} />}
            {showSuccess && <SuccessMessage message="Operación Completada!" />}

            {/* Contenido de la relación activa */}
            {activeKey && !loading && (
                <div className="flex-1 flex flex-col gap-4">
                    <div className="border rounded-lg overflow-hidden">
                        {currentData?.length > 0 ? (
                            <>
                                {/* Tabla de datos de relación */}
                                <RelationTable
                                    columns={columns.filter((_: number, i: number) => i !== 1)} // Excluir columna ID
                                    data={currentData}
                                    selectedIds={selectedIds}
                                    onRowSelect={handleRowSelect}
                                />
                                
                                {/* Info de elementos seleccionados */}
                                {selectedIds.length > 0 && (
                                    <Alert
                                        message={`${selectedIds.length} elemento(s) seleccionado(s).`}
                                        type="info"
                                        showIcon
                                    />
                                )}
                            </>
                        ) : (
                            <div className="text-center text-gray-500 py-4">
                                No hay datos disponibles para esta relación.
                            </div>
                        )}
                    </div>

                    {/* Botones de acción */}
                    <Space className="justify-end w-full">
                        {/* Botón para asignar nuevas relaciones */}
                        <AssignButton
                            modelId={id}
                            modelController={toLowerRelationKey(activeKey)}
                            relationController={relationInfoMap[activeKey]?.controller}
                            orderBy={extractLastKeyPart(relationInfoMap[activeKey]?.selects[2])}
                            selects={extractLastKeyParts(relationInfoMap[activeKey]?.selects.slice(1))}
                            onSuccess={() => {
                                forceReloadRelationData();
                                setShowSuccess(true);
                            }}
                        />
                        
                        {/* Botón para revocar relaciones seleccionadas */}
                        <Button
                            danger
                            disabled={selectedIds.length === 0}
                            onClick={handleRevoke}
                        >
                            Revocar
                        </Button>
                    </Space>
                </div>
            )}
        </div>
    );
};

export default DynamicRelationViewer;