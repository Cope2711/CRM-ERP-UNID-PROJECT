import { useEffect, useState } from "react";
import genericService from "@/services/genericService";
import { GetAllDto } from "@/dtos/GenericDtos";
import { message } from "antd";

// Define types for data models
type Product = {
  productId: string;
  productName: string;
  productPrice: number;
  productDescription: string;
  brandId: string;
};

type InventoryItem = {
  inventoryId: string;
  productId: string;
  branchId: string;
  quantity: number;
};

type Brand = {
  brandId: string;
  brandName: string;
};

// Combined data type for display
type CombinedInventoryItem = {
  inventoryId: string;
  productName: string;
  productDescription: string;
  productPrice: number;
  quantity: number;
  brandName: string;
};

const Inventory = () => {
  const [combinedData, setCombinedData] = useState<CombinedInventoryItem[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchInventory = async () => {
      setLoading(true);
      const datosInventario: GetAllDto = {
        pageNumber: 1,
        pageSize: 50,
        orderBy: "CreatedDate",
        descending: true,
        filters: [],
        selects: ["InventoryId", "ProductId", "BranchId", "Quantity"],
      };

      const datosProductos: GetAllDto = {
        pageNumber: 1,
        pageSize: 50,
        orderBy: "CreatedDate",
        descending: true,
        filters: [],
        selects: ["ProductId", "ProductName", "ProductPrice", "ProductDescription", "BrandId"],
      };

      const datosBrands: GetAllDto = {
        pageNumber: 1,
        pageSize: 50,
        orderBy: "CreatedDate",
        descending: true,
        filters: [],
        selects: ["BrandId", "BrandName"],
      };

      try {
        const [invRes, prodRes, brandRes] = await Promise.all([
          genericService.getAll("inventory", datosInventario),
          genericService.getAll("products", datosProductos),
          genericService.getAll("brands", datosBrands),
        ]);

        const combined: CombinedInventoryItem[] = [];

        invRes.data.forEach((inv: any) => {
          const alreadyExists = combined.some(item => item.inventoryId === inv.InventoryId);

          if (alreadyExists) return;

          const product = prodRes.data.find((p: any) => p.ProductId === inv.ProductId);
          if (!product) return;
          
          const brand = brandRes.data.find((b: any) => b.BrandId === product.BrandId);

          combined.push({
            inventoryId: inv.InventoryId,
            productName: product.ProductName,
            productDescription: product.ProductDescription,
            productPrice: product.ProductPrice,
            quantity: inv.Quantity,
            brandName: brand?.BrandName ?? "Sin marca",
          });
        });

        setCombinedData(combined);
      } catch (err) {
        console.error("Error loading inventory data", err);
        message.error("Failed to load inventory data. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchInventory();
  }, []);

  return (
    <div className="p-6">
      <h1 className="text-3xl font-bold text-gray-800 mb-2">Inventario</h1>
      <hr className="mb-6 border-gray-300" />

      {loading ? (
        <div className="flex justify-center items-center h-64">
          <span className="text-gray-500">Cargando información de inventario...</span>
        </div>
      ) : combinedData.length === 0 ? (
        <div className="text-center py-8">
          <p className="text-gray-500">No hay elementos de inventario disponibles.</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {combinedData.map((item) => (
            <div key={item.inventoryId} className="bg-white rounded-lg shadow-md hover:shadow-lg transition-shadow duration-300">
              <div className="h-2 rounded-t-lg bg-gray-200" />
              <div className="p-4">
                <h3 className="font-bold text-lg">{item.productName}</h3>
                <p className="text-sm text-gray-500 mb-1">Marca: {item.brandName}</p>
                <p className="text-gray-600">{item.productDescription}</p>
                <p className="text-blue-600 font-semibold mt-2">${item.productPrice}</p>
                <p className="text-sm text-gray-500">Cantidad: {item.quantity}</p>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Inventory;
