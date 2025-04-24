// DTOs relacionados con el módulo de ventas

// DTO para los detalles de una venta (linea de venta)
export interface SaleDetailDto {
  saleDetailId?: string;
  productId: string;
  quantity: number;
  unitPrice: number;
}

// DTO para crear un detalle de venta
export interface CreateSaleDetailDto {
  productId: string;
  quantity: number;
  unitPrice: number;
}

// DTO para crear una venta completa
export interface CreateFullSaleDto {
  branchId: string;
  totalAmount: number;
  saleDetails: CreateSaleDetailDto[];
}

// DTO para la información de una venta
export interface SaleDto {
  saleId: string;
  branchId: string;
  userId: string;
  totalAmount: number;
  saleDate: string;
  createdDate?: string;
  updatedDate?: string;
  saleDetails: SaleDetailDto[];
}

// Interface para el estado de la página de ventas
export interface SalesPageState {
  loading: boolean;
  createModalVisible: boolean;
  detailModalVisible: boolean;
  selectedSale: SaleDto | null;
  products: ProductDto[];
  sales: SaleDto[];
  search: string;
  pagination: {
    current: number;
    pageSize: number;
    total: number;
  };
}

// DTO simplificado para productos (usado en el módulo de ventas)
export interface ProductDto {
  ProductId: string;
  ProductName: string;
  ProductPrice: number;
  ProductDescription?: string;
  ProductBarcode?: string;
} 