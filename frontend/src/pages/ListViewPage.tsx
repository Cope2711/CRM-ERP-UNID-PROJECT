import { useEffect, useState, useMemo } from 'react';
import { Button, Space, Spin } from 'antd';
import { useNavigate } from 'react-router-dom';
import { ErrorMessage } from '@/components/message/ErrorMessage';
import genericService from '@/services/genericService';
import { GetAllDto, FilterDto } from '@/dtos/GenericDtos';
import { FilterOperators } from '@/constants/filterOperators';
import PageHeader from '@/components/ListView/PageHeader';
import SearchBar from '@/components/ListView/SearchBar';
import AdvancedFilters from '@/components/ListView/AdvancedFilters';
import ListTable from '@/components/ListView/ListTable';
import { Tag, Tooltip } from 'antd';
import { Drawer } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import DynamicCreateForm from "@/components/dynamic/forms/DynamicCreateForm";

type ListViewPageProps = {
    modelName: string;
};

export default function ListViewPage({ modelName }: ListViewPageProps) {
    const [items, setItems] = useState<any[]>([]);
    const [schema, setSchema] = useState<Record<string, any> | null>(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(5);
    const [totalItems, setTotalItems] = useState(0);
    const [filters, setFilters] = useState<FilterDto[]>([]);
    const [showFilters, setShowFilters] = useState(false);
    const [searchText, setSearchText] = useState('');
    const [searchField, setSearchField] = useState<string>('');
    const [showCreateForm, setShowCreateForm] = useState(false);

    const navigate = useNavigate();

    // Memoized primary key and valid fields based on schema
    const { primaryKey, validFields } = useMemo(() => {
        if (!schema) return { primaryKey: 'id', validFields: [] };

        let pk = 'id';
        const fields: string[] = [];

        for (const [key, meta] of Object.entries(schema) as [string, any][]) {
            if (meta.type === 'list`1') continue;

            if (Array.isArray(meta.specialData) && meta.specialData.includes('IsPassword')) {
                continue;
            }

            if (
                meta.type === 'guid' &&
                Array.isArray(meta.specialData) &&
                meta.specialData.includes('IsObjectKey')
            ) {
                pk = key;
            }

            if (meta.type !== 'guid') {
                fields.push(key);
            }
        }

        return { primaryKey: pk, validFields: fields };
    }, [schema]);

    // Fetch schema only when modelName changes
    useEffect(() => {
        const fetchSchema = async () => {
            setIsLoading(true);
            try {
                const schemaData = await genericService.getSchemas(modelName, 'model');
                setSchema(schemaData);
            } catch (err) {
                setError((err as Error).message || 'Failed to load schema');
            } finally {
                setIsLoading(false);
            }
        };

        fetchSchema();
    }, [modelName]);

    // Fetch data when filters, page, pageSize, or schema changes
    useEffect(() => {
        if (!schema || !primaryKey || validFields.length === 0) return;

        const fetchData = async () => {
            setIsLoading(true);
            setError(null);

            try {
                const getAllDto: GetAllDto = {
                    pageNumber: page,
                    pageSize,
                    orderBy: validFields[0] || primaryKey,
                    descending: false,
                    filters: filters,
                    selects: [primaryKey, ...validFields],
                };
                const response = await genericService.getAll(modelName, getAllDto);
                setItems(response.data);
                setTotalItems(response.totalItems);
            } catch (err) {
                setError((err as Error).message || 'Ocurrió un error inesperado.');
            } finally {
                setIsLoading(false);
            }
        };

        fetchData();
    }, [page, pageSize, schema, primaryKey, validFields, modelName, filters]);

    const handleSearch = (text: string, field: string) => {
        setSearchText(text);
        setSearchField(field);

        if (text.trim() && field) {
            const searchFilters: FilterDto[] = [{
                column: field,
                operator: FilterOperators.Contains,
                value: text
            }];

            setFilters(searchFilters);
            setPage(1);
        } else {
            setFilters([]);
        }
    };

    return (
        <Space direction="vertical" size="middle" className="w-full">
            <PageHeader
                modelName={modelName}
                pageSize={pageSize}
                onPageSizeChange={(value) => {
                    setPageSize(value);
                    setPage(1);
                }}
                extraButton={
                    <Button
                        type="primary"
                        icon={<PlusOutlined />}
                        onClick={() => setShowCreateForm(true)}
                    >
                        Crear
                    </Button>
                }
            />

            <Drawer
                title={`Crear ${modelName}`}
                width={600}
                onClose={() => setShowCreateForm(false)}
                open={showCreateForm}
                destroyOnClose
            >
                {schema && (
                    <DynamicCreateForm
                        modelName={modelName}
                    />
                )}
            </Drawer>

            <SearchBar
                searchText={searchText}
                searchField={searchField}
                validFields={validFields.filter(field => schema?.[field]?.type === 'string')}
                onSearch={handleSearch}
                onToggleFilters={() => setShowFilters(!showFilters)}
                showFilters={showFilters}
            />

            {/* Mostrar filtros activos */}
            {filters.length > 0 && (
                <div className="px-4">
                    <div className="mb-2 font-semibold">Filtros aplicados:</div>
                    <Space wrap>
                        {filters.map((filter, index) => (
                            <Tag
                                key={index}
                                closable
                                onClose={() => {
                                    const newFilters = [...filters];
                                    newFilters.splice(index, 1);
                                    setFilters(newFilters);
                                }}
                            >
                                <Tooltip title={`${filter.column} ${filter.operator} ${filter.value}`}>
                                    <span>{`${filter.column} ${filter.operator} ${filter.value}`}</span>
                                </Tooltip>
                            </Tag>
                        ))}
                    </Space>
                </div>
            )}

            {showFilters && schema && (
                <AdvancedFilters
                    schema={schema}
                    validFields={validFields}
                    onSubmit={(newFilter) => {
                        setFilters(prev => [...prev, newFilter]);
                        setPage(1);
                        setShowFilters(false);
                    }}
                    onClear={() => {
                        setFilters([]);
                        setSearchText('');
                    }}
                />
            )}

            {error && <ErrorMessage message={error} />}

            <Spin spinning={isLoading}>
                <ListTable
                    modelName={modelName}
                    items={items}
                    schema={schema}
                    validFields={validFields}
                    primaryKey={primaryKey}
                    page={page}
                    pageSize={pageSize}
                    totalItems={totalItems}
                    onPageChange={(newPage, newPageSize) => {
                        setPage(newPage);
                        if (newPageSize !== pageSize) {
                            setPageSize(newPageSize);
                        }
                    }}
                    navigate={navigate}
                />
            </Spin>
        </Space>
    );
}