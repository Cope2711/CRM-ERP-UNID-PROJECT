import { FilterOperators } from "@/constants/filterOperators";
import genericService from "@/services/genericService";
import { RelationSchema } from "@/types/Schema";
import { IdResponseStatusSchema } from "@/types/Status";
import { LoadingOutlined } from "@ant-design/icons";
import { Button, Select, Table, Tabs } from "antd";
import { useEffect, useState } from "react";

type NewRelationViewerProps = {
    columnName: string;
    id: string;
    relationsSchemas: RelationSchema[];
    modelName: string;
};

export default function DynamicRelationViewer({ columnName, id, relationsSchemas }: NewRelationViewerProps) {
    const [activeRelation, setActiveRelation] = useState<RelationSchema>(relationsSchemas[0]);
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState<any[]>([]);
    const [selectedRowKeys, setSelectedRowKeys] = useState<React.Key[]>([]);
    const [assignMode, setAssignMode] = useState<boolean>(false);

    useEffect(() => {
        if (activeRelation) {
            fetchData();
        }
    }, [activeRelation]);

    useEffect(() => {
        fetchData();
    }, [assignMode]);

    const handleTabChange = (key: string) => {
        const selectedRelation = relationsSchemas.find(r => r.modelName === key);
        if (selectedRelation && selectedRelation.modelName !== activeRelation.modelName) {
            setActiveRelation(selectedRelation);
            setSelectedRowKeys([]); // Limpiar selección al cambiar de tab
        }
    };

    const fetchData = async () => {
        setLoading(true);

        try {
            if (assignMode) {
                const response = await genericService.getAll(activeRelation.modelName, {
                    pageNumber: 1,
                    pageSize: 10,
                    orderBy: activeRelation.selects[1].split('.')[1],
                    descending: false,
                    filters: [],
                    selects: activeRelation.selects
                        .filter(select => select.includes('.')).map(select => select.split('.')[1]),
                });
                setData(response.data);
            } else {
                const response = await genericService.getAll(activeRelation.controller, {
                    pageNumber: 1,
                    pageSize: 10,
                    orderBy: activeRelation.selects[0],
                    descending: false,
                    filters:
                        [
                            {
                                column: columnName,
                                operator: assignMode ? FilterOperators.NotEqual : FilterOperators.Equal,
                                value: id
                            },
                        ],
                    selects: activeRelation.selects,
                });
                setData(response.data);
            }

        } catch (error: any) {
            console.error(error);
        } finally {
            setLoading(false);
        }
    };

    const handleActionClick = async () => {
        console.log("Selected Row Keys:", selectedRowKeys);

        if (assignMode) {
            try {
                const response = await genericService.assign(activeRelation.controller, { modelId: id, assignIds: selectedRowKeys.map(key => key.toString()) }, activeRelation.modelName);
                console.log(response);
            } catch (error: any) {
                console.error(error);
            };
        }
        else {
            try {
                const response: IdResponseStatusSchema = await genericService.revoke(activeRelation.controller, selectedRowKeys.map(key => key.toString()));
                console.log(response);
            } catch (error: any) {
                console.error(error);
            };
        }
    };

    const rowSelection = {
        selectedRowKeys,
        onChange: (selectedKeys: React.Key[]) => {
            setSelectedRowKeys(selectedKeys);
        },
    };

    if (relationsSchemas.length === 0) {
        return <div className="text-center text-gray-500">No relations to display.</div>;
    }

    return (
        <div className="h-full flex flex-col gap-4">
            <Tabs
                activeKey={activeRelation.modelName}
                items={relationsSchemas.map(r => ({
                    label: r.modelName,
                    key: r.modelName,
                }))}
                onChange={handleTabChange}
            />

            {loading && (
                <div className="flex justify-center py-4">
                    <LoadingOutlined spin style={{ fontSize: 24 }} />
                </div>
            )}

            {!loading && (
                <>
                    <div className="flex justify-end gap-4">
                        <Select
                            style={{ width: 120 }}
                            options={[{ value: 'Revocar', label: 'Revocar' }, { value: 'Asignar', label: 'Asignar' }]}
                            defaultValue={assignMode ? 'Asignar' : 'Revocar'}
                            onChange={(value) => setAssignMode(value === 'Asignar')}
                        />
                        <Button type="primary" onClick={handleActionClick} disabled={selectedRowKeys.length === 0}
                            className={assignMode ? "bg-green-500" : "bg-red-500"}>
                            {assignMode ? "Asignar" : "Revocar"}
                        </Button>

                    </div>

                    <div className="flex-1 flex flex-col gap-4">
                        <div className="border rounded-lg overflow-hidden overflow-x-auto">
                            <Table
                                dataSource={data}
                                rowKey={(record) => record[Object.keys(record)[0]]}
                                rowSelection={rowSelection}
                                columns={data.length > 0
                                    ? Object.keys(data[0]).map((key, index) => ({
                                        title: key,
                                        dataIndex: key,
                                        key: index.toString(),
                                    }))
                                    : []}
                            />
                        </div>
                    </div>
                </>
            )}
        </div>
    )
}
