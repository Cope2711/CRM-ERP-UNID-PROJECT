import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Table, Input, Typography, message, Popconfirm, Modal, Form, Select, InputNumber, Space, Card } from 'antd';
import { PlusOutlined, DeleteOutlined, EyeOutlined, SearchOutlined, MinusCircleOutlined } from '@ant-design/icons';
import { LoadingSpinner } from '@/components/Loading/loadingSpinner';
import genericService from '@/services/genericService';
import { GetAllDto } from '@/dtos/GenericDtos';
import { FilterOperators } from '@/constants/filterOperators';
import { useSelector } from 'react-redux';
import { RootState } from '@/redux/store';
import { axiosInstance } from '@/services/axiosConfig';
import { 
  SaleDto, 
  ProductDto, 
  CreateFullSaleDto, 
  CreateSaleDetailDto, 
  SalesPageState 
} from '@/dtos/SalesDtos';

const { Title } = Typography;

const SalesPage = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState<boolean>(true);
  const [createModalVisible, setCreateModalVisible] = useState<boolean>(false);
  const [detailModalVisible, setDetailModalVisible] = useState<boolean>(false);
  const [selectedSale, setSelectedSale] = useState<SaleDto | null>(null);
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [sales, setSales] = useState<SaleDto[]>([]);
  const [search, setSearch] = useState<string>('');
  const [form] = Form.useForm();
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 10,
    total: 0
  });

  // Se trae la sucursal desde Redux
  const currentBranch = useSelector((state: RootState) => state.session.actualBranch);

  const loadSales = async (pageNumber = 1, pageSize = 10, searchTerm = '') => {
    setLoading(true);
    try {
      const getAllDto: GetAllDto = {
        pageNumber,
        pageSize,
        orderBy: 'saleDate',
        descending: true,
        filters: searchTerm ? [
          {
            column: 'saleDate',
            operator: FilterOperators.Contains,
            value: searchTerm
          }
        ] : [],
        selects: ['saleId', 'saleDate', 'totalAmount']
      };
      
      const response = await genericService.getAll('sales', getAllDto);
      setSales(response.data);
      setPagination({
        ...pagination,
        current: pageNumber,
        total: response.total
      });
    } catch (error) {
      message.error('Error loading sales');
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const loadProducts = async () => {
    try {
      const getAllDto: GetAllDto = {
        pageNumber: 1,
        pageSize: 999,
        orderBy: 'ProductName',
        descending: false,
        filters: [],
        selects: ['ProductId', 'ProductName', 'ProductPrice', 'ProductDescription', 'ProductBarcode']
      };
      
      const response = await genericService.getAll('products', getAllDto);
      setProducts(response.data);
      console.log('Productos cargados:', response.data);
    } catch (error) {
      message.error('Error loading products');
      console.error(error);
    }
  };

  useEffect(() => {
    loadSales(pagination.current, pagination.pageSize, search);
  }, []);

  const handleTableChange = (pagination: any) => {
    loadSales(pagination.current, pagination.pageSize, search);
  };

  const handleSearch = () => {
    loadSales(1, pagination.pageSize, search);
  };

  const handleDelete = async (id: string) => {
    try {
      await axiosInstance.delete(`sales/delete`, {
        params: { id }
      });
      message.success('Sale deleted successfully');
      loadSales(pagination.current, pagination.pageSize, search);
    } catch (error) {
      message.error('Error deleting sale');
      console.error(error);
    }
  };

  const showCreateModal = () => {
    loadProducts();
    form.resetFields();
    
    // Se pre-establece el campo de sucursal con la ya seleccionada
    if (currentBranch) {
      form.setFieldsValue({
        branchId: currentBranch.branchId
      });
    }
    
    setCreateModalVisible(true);
  };

  const showDetailModal = async (id: string) => {
    try {
      const sale = await genericService.getById('sales', id);
      
      // Se cargan los productos para mostrar los nombres
      await loadProducts();
      
      setSelectedSale(sale);
      setDetailModalVisible(true);
    } catch (error) {
      message.error('Error loading sale details');
      console.error(error);
    }
  };

  const handleCreateSale = async (values: { saleDetails: CreateSaleDetailDto[] }) => {
    try {
      if (!currentBranch) {
        message.error('You must be assigned to a branch to create a sale');
        return;
      }
      
      // Se calcula el total de la venta
      let totalAmount = 0;
      const saleDetails: CreateSaleDetailDto[] = values.saleDetails.map((detail) => {
        const product = products.find((p) => p.ProductId === detail.productId);
        const subtotal = detail.quantity * detail.unitPrice;
        totalAmount += subtotal;
        
        return {
          productId: detail.productId,
          quantity: detail.quantity,
          unitPrice: detail.unitPrice
        };
      });

      const createSaleDto: CreateFullSaleDto = {
        branchId: currentBranch.branchId,
        totalAmount,
        saleDetails
      };

      await genericService.create('sales', createSaleDto);
      message.success('Sale created successfully');
      form.resetFields();
      setCreateModalVisible(false);
      loadSales(pagination.current, pagination.pageSize, search);
    } catch (error) {
      message.error('Error creating sale');
      console.error(error);
    }
  };

  const columns = [
    {
      title: 'ID',
      dataIndex: 'saleId',
      key: 'saleId',
      width: 250,
      render: (text: string) => <span className="text-xs">{text}</span>,
    },
    {
      title: 'Date',
      dataIndex: 'saleDate',
      key: 'saleDate',
      render: (date: string) => new Date(date).toLocaleString(),
    },
    {
      title: 'Total Amount',
      dataIndex: 'totalAmount',
      key: 'totalAmount',
      render: (amount: number) => `$${amount.toFixed(2)}`,
    },
    {
      title: 'Actions',
      key: 'actions',
      render: (record: SaleDto) => (
        <div className="flex gap-2">
          <Button 
            icon={<EyeOutlined />} 
            type="primary"
            onClick={() => showDetailModal(record.saleId)}
          />
          <Popconfirm
            title="Are you sure you want to delete this sale?"
            onConfirm={() => handleDelete(record.saleId)}
            okText="Yes"
            cancelText="No"
          >
            <Button icon={<DeleteOutlined />} danger />
          </Popconfirm>
        </div>
      )
    }
  ];

  if (loading && sales.length === 0) {
    return <LoadingSpinner />;
  }

  // Se verifica si hay una sucursal seleccionada
  if (!currentBranch) {
    return (
      <div className="p-6">
        <Title level={3}>Sales Management</Title>
        <div className="bg-red-100 p-4 rounded-md text-red-800 mt-4">
          You must be assigned to a branch to manage sales. Please select a branch in your profile settings.
        </div>
      </div>
    );
  }

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-6">
        <div>
          <Title level={3}>Sales Management</Title>
          <p className="text-gray-500">Current branch: {currentBranch.branchName}</p>
        </div>
        <Button 
          type="primary" 
          icon={<PlusOutlined />} 
          onClick={showCreateModal}
        >
          New Sale
        </Button>
      </div>

      <div className="mb-4 flex">
        <Input
          placeholder="Search sales"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          onPressEnter={handleSearch}
          suffix={<SearchOutlined onClick={handleSearch} />}
          className="w-64 mr-2"
        />
        <Button onClick={handleSearch}>Search</Button>
      </div>

      <Table
        columns={columns}
        dataSource={sales}
        rowKey="saleId"
        pagination={pagination}
        onChange={handleTableChange}
        className="shadow-md rounded-md"
      />

      {/* Create Sale Modal */}
      <Modal
        title="Create New Sale"
        open={createModalVisible}
        onCancel={() => setCreateModalVisible(false)}
        footer={null}
        width={800}
      >
        <Form
          form={form}
          layout="vertical"
          onFinish={handleCreateSale}
        >
          <div className="mb-4 p-3 bg-blue-50 rounded-md">
            <strong>Branch:</strong> {currentBranch.branchName}
          </div>

          <Form.List name="saleDetails">
            {(fields, { add, remove }) => (
              <>
                {fields.map(({ key, name, ...restField }) => (
                  <Card key={key} style={{ marginBottom: 16 }} size="small">
                    <Space direction="horizontal" style={{ display: 'flex', marginBottom: 8 }} align="baseline">
                      <Form.Item
                        {...restField}
                        name={[name, 'productId']}
                        rules={[{ required: true, message: 'Missing product' }]}
                        style={{ width: 300 }}
                      >
                        <Select 
                          placeholder="Select product" 
                          showSearch
                          optionFilterProp="children"
                          filterOption={(input, option) => 
                            (option?.children?.toString().toLowerCase() || '').includes(input.toLowerCase())
                          }
                          onChange={(value) => {
                            console.log("Producto seleccionado ID:", value);
                            const selectedProduct = products.find(p => p.ProductId === value);
                            console.log("Producto encontrado:", selectedProduct);
                            if (selectedProduct) {
                              form.setFieldsValue({
                                saleDetails: {
                                  ...form.getFieldValue('saleDetails'),
                                  [name]: {
                                    ...form.getFieldValue('saleDetails')[name],
                                    unitPrice: selectedProduct.ProductPrice
                                  }
                                }
                              });
                            }
                          }}
                        >
                          {products.map((product) => (
                            <Select.Option key={product.ProductId} value={product.ProductId}>
                              {product.ProductName} - ${product.ProductPrice?.toFixed(2)}
                            </Select.Option>
                          ))}
                        </Select>
                      </Form.Item>
                      <Form.Item
                        {...restField}
                        name={[name, 'quantity']}
                        rules={[{ required: true, message: 'Missing quantity' }]}
                      >
                        <InputNumber min={1} placeholder="Quantity" />
                      </Form.Item>
                      <Form.Item
                        {...restField}
                        name={[name, 'unitPrice']}
                        rules={[{ required: true, message: 'Missing price' }]}
                      >
                        <InputNumber 
                          min={0.01} 
                          step={0.01} 
                          placeholder="Unit Price"
                          addonBefore="$"
                          style={{ width: '100%' }}
                          className="custom-input-number"
                        />
                      </Form.Item>
                      <MinusCircleOutlined onClick={() => remove(name)} />
                    </Space>
                  </Card>
                ))}
                <Form.Item>
                  <Button type="dashed" onClick={() => add()} block icon={<PlusOutlined />}>
                    Add Product
                  </Button>
                </Form.Item>
              </>
            )}
          </Form.List>

          <Form.Item>
            <Button type="primary" htmlType="submit">
              Create Sale
            </Button>
          </Form.Item>
        </Form>
      </Modal>

        {/* Sale Detail Modal */}
      <Modal
        title="Sale Details"
        open={detailModalVisible}
        onCancel={() => setDetailModalVisible(false)}
        footer={null}
        width={800}
      >
        {selectedSale && (
          <div>
            <p><strong>Sale ID:</strong> {selectedSale.saleId}</p>
            <p><strong>Date:</strong> {new Date(selectedSale.saleDate).toLocaleString()}</p>
            <p><strong>Total Amount:</strong> ${selectedSale.totalAmount.toFixed(2)}</p>
            
            <h3 className="mt-4 mb-2 font-semibold">Sale Details</h3>
            <Table
              columns={[
                {
                  title: 'Product',
                  dataIndex: 'productId',
                  key: 'productId',
                  render: (productId: string) => {
                    const product = products.find(p => p.ProductId === productId);
                    return product ? product.ProductName : productId;
                  }
                },
                {
                  title: 'Quantity',
                  dataIndex: 'quantity',
                  key: 'quantity'
                },
                {
                  title: 'Unit Price',
                  dataIndex: 'unitPrice',
                  key: 'unitPrice',
                  render: (price: number) => `$${price.toFixed(2)}`,
                },
                {
                  title: 'Subtotal',
                  key: 'subtotal',
                  render: (record: any) => `$${(record.quantity * record.unitPrice).toFixed(2)}`,
                }
              ]}
              dataSource={selectedSale.saleDetails}
              rowKey="saleDetailId"
              pagination={false}
            />
          </div>
        )}
      </Modal>
    </div>
  );
};

export default SalesPage; 