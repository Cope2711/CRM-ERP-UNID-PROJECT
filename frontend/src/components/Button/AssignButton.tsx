import { GetAllDto } from "@/dtos/GenericDtos";
import genericService from "@/services/genericService";
import { LoadingOutlined, CloseOutlined, CheckOutlined } from "@ant-design/icons";
import { Button, Divider, notification, Space, Tooltip } from "antd";
import { useState, useRef, useEffect, Key } from "react";
import classNames from "classnames";

interface AssignButtonProps {
    modelId: string; // ID del modelo principal
    modelController: string; // Endpoint para obtener los datos
    relationController: string; // Endpoint para asignar relaciones
    orderBy: string; // Campo para ordenar los resultados
    selects: string[]; // Campos a seleccionar en la consulta
    senderResource: string; // Recurso del modelo que envía la relación
    onSuccess?: () => void; // Callback al completar asignaciones exitosas
}

/**
 * Componente para asignar relaciones entre modelos mediante un popup con selección múltiple
 */
const AssignButton = ({
    modelId,
    modelController,
    relationController,
    orderBy,
    selects,
    senderResource,
    onSuccess,
}: AssignButtonProps) => {
    // Estado del componente
    const [visible, setVisible] = useState(false); // Visibilidad del popup
    const [selectedIds, setSelectedIds] = useState<string[]>([]); // IDs seleccionados
    const [loading, setLoading] = useState(false); // Estado de carga
    const [options, setOptions] = useState<any[]>([]); // Opciones disponibles
    const [failedMap, setFailedMap] = useState<Record<string, { status: string; message: string }>>({}); // Errores por ID
    const [successIds, setSuccessIds] = useState<Set<string>>(new Set()); // IDs con asignación exitosa

    const popupRef = useRef<HTMLDivElement>(null); // Referencia al popup para cerrar al hacer clic fuera

    /**
     * Obtiene los datos para mostrar en el popup
     */
    const fetchData = async () => {
        setLoading(true);
        try {
            const getAllDto: GetAllDto = {
                pageNumber: 1,
                pageSize: 10,
                orderBy,
                descending: false,
                filters: [],
                selects,
            };

            const response = await genericService.getAll(modelController, getAllDto);
            setOptions(response.data);
        } catch (error) {
            console.error("Error al obtener los datos:", error);
            notification.error({
                message: "Error",
                description: "No se pudieron cargar las opciones disponibles",
            });
        } finally {
            setLoading(false);
        }
    };

    /**
     * Maneja el cambio en las selecciones
     * @param id ID del elemento seleccionado/deseleccionado
     */
    const handleChange = (id: string) => {
        setSelectedIds(prev =>
            prev.includes(id) 
                ? prev.filter(item => item !== id) // Remueve si ya está seleccionado
                : [...prev, id] // Agrega si no está seleccionado
        );
    };

    // Efecto para manejar el cierre del popup al hacer clic fuera
    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (popupRef.current && !popupRef.current.contains(event.target as Node)) {
                setVisible(false);
            }
        };

        if (visible) {
            // Resetear estado al abrir el popup
            fetchData();
            setSelectedIds([]);
            setFailedMap({});
            setSuccessIds(new Set());
            document.addEventListener("mousedown", handleClickOutside);
        }

        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [visible]);

    // Efecto para manejar el callback onSuccess cuando se cierra el popup
    useEffect(() => {
        if (!visible && successIds.size > 0) {
            onSuccess?.();
        }
    }, [visible, successIds, onSuccess]);

    /**
     * Confirma y procesa las asignaciones seleccionadas
     */
    const handleConfirm = async () => {
        if (selectedIds.length === 0) {
            notification.warning({
                message: "Nada seleccionado",
                description: "Por favor selecciona al menos un elemento",
            });
            return;
        }

        setLoading(true);
        
        try {
            // Enviar asignaciones al servidor
            const response: any = await genericService.assign(relationController, {
                modelId,
                assignIds: selectedIds,
            }, senderResource);

            const { success, failed } = response;

            // Procesar resultados fallidos
            if (failed?.length) {
                const newFailedMap: Record<string, { status: string; message: string }> = {};
                failed.forEach((item: any) => {
                    const id = item.modelAssignIds.assignId;
                    newFailedMap[id] = {
                        status: item.status,
                        message: item.message,
                    };
                });
                setFailedMap(newFailedMap);

                notification.warning({
                    message: "Algunas asignaciones fallaron",
                    description: `${failed.length} elemento(s) no pudieron ser asignados.`,
                    duration: 6,
                });
            }

            // Procesar resultados exitosos
            if (success?.length) {
                const successSet = new Set<string>(success.map((item: any) => item.modelAssignIds.assignId));
                setSuccessIds(successSet);

                notification.success({
                    message: "Asignaciones exitosas",
                    description: `${success.length} elemento(s) asignados correctamente.`,
                });
            }

            // Cerrar popup solo si no hubo errores
            if (!failed || failed.length === 0) {
                setVisible(false);
            }
        } catch (error) {
            console.error("Error al asignar:", error);
            notification.error({
                message: "Error inesperado",
                description: "Hubo un problema al procesar las asignaciones",
            });
        } finally {
            setLoading(false);
        }
    };

    // Campos a mostrar (excluyendo el primer select que se usa como ID)
    const visibleSelects = selects.slice(1);

    return (
        <div className="relative inline-block">
            {/* Botón principal que abre el popup */}
            <Button
                type="primary"
                onClick={() => setVisible(prev => !prev)}
                style={{ backgroundColor: "#52c41a" }}
            >
                Asignar
            </Button>

            {/* Popup de selección */}
            {visible && (
                <div
                    ref={popupRef}
                    className="absolute top-full mt-2 right-0 bg-white border rounded-xl shadow-lg p-4 w-[500px] z-[9999]"
                >
                    {loading ? (
                        // Estado de carga
                        <div className="flex justify-center py-4">
                            <LoadingOutlined spin style={{ fontSize: 24 }} />
                        </div>
                    ) : (
                        <>
                            {/* Tabla de opciones */}
                            <div className="max-h-80 overflow-y-auto">
                                <table className="min-w-full table-auto border-collapse">
                                    <thead>
                                        <tr className="bg-gray-100 text-left">
                                            <th></th>
                                            {visibleSelects.map((col: string, index: Key) => (
                                                <th key={index} className="px-2 py-1 border-b">
                                                    {col.split('.').pop()}
                                                </th>
                                            ))}
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {options.map((item, index) => {
                                            const rowId = item.id || item[selects[0]];
                                            const failed = failedMap[rowId];
                                            const isSuccess = successIds.has(rowId);

                                            return (
                                                <tr
                                                    key={index}
                                                    className={classNames({
                                                        "bg-red-100": failed, // Rojo para errores
                                                        "bg-green-100": isSuccess, // Verde para éxitos
                                                        "hover:bg-gray-50": !failed && !isSuccess, // Hover normal
                                                    })}
                                                >
                                                    <td className="px-2 py-1 border-b">
                                                        <input
                                                            type="checkbox"
                                                            checked={selectedIds.includes(rowId)}
                                                            onChange={() => handleChange(rowId)}
                                                            disabled={isSuccess} // Deshabilitar si ya fue exitoso
                                                        />
                                                    </td>
                                                    {visibleSelects.map((col: string, i: number) => (
                                                        <td key={i} className="px-2 py-1 border-b">
                                                            <Tooltip
                                                                title={failed ? `${failed.status}: ${failed.message}` : undefined}
                                                            >
                                                                {item[col]}
                                                            </Tooltip>
                                                        </td>
                                                    ))}
                                                </tr>
                                            );
                                        })}
                                    </tbody>
                                </table>
                            </div>

                            <Divider className="my-2" />

                            {/* Botones de acción */}
                            <Space style={{ display: "flex", justifyContent: "flex-end" }}>
                                <Button
                                    onClick={() => setVisible(false)}
                                    icon={<CloseOutlined />}
                                    danger
                                    title="Cancelar"
                                />
                                <Button
                                    onClick={handleConfirm}
                                    icon={<CheckOutlined />}
                                    type="primary"
                                    title="Confirmar"
                                    disabled={selectedIds.length === 0 || loading}
                                    loading={loading}
                                />
                            </Space>
                        </>
                    )}
                </div>
            )}
        </div>
    );
};

export default AssignButton;