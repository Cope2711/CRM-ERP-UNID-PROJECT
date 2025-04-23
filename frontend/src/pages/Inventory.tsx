import { useEffect, useState } from "react";
import genericService from "@/services/genericService";
import { GetAllDto } from "@/dtos/GenericDtos";

type Product = {
  productId: string;
  productName: string;
  productPrice: number;
  productDescription: string;
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

const Inventory = () => {
  const [combinedData, setCombinedData] = useState<
    {
      inventoryId: string;
      productName: string;
      productDescription: string;
      productPrice: number;
      quantity: number;
      brandName: string;
    }[]
  >([]);

  useEffect(() => {
    const fetchInventory = async () => {
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

        const combined: {
          inventoryId: string;
          productName: string;
          productDescription: string;
          productPrice: number;
          quantity: number;
          brandName: string;
        }[] = [];

        invRes.data.forEach((inv: any) => {
          const alreadyExists = combined.some(item => item.inventoryId === inv.InventoryId);

          if (alreadyExists) return;

          const product = prodRes.data.find((p: any) => p.ProductId === inv.ProductId);
          if (!product) return;
          console.log(product.BrandId)
          const brand = brandRes.data.find((b: any) => b.BrandId === product.BrandId);

          console.log("SSS", brand)
          combined.push({
            inventoryId: inv.InventoryId,
            productName: product.ProductName,
            productDescription: product.ProductDescription,
            productPrice: product.ProductPrice,
            quantity: inv.Quantity,
            brandName: brand?.BrandName ?? "Sin marca",
          });

          console.log("XXXXX", combined)
        });

        setCombinedData(combined);
      } catch (err) {
        console.error("Error no cargo", err);
      }
    };

    fetchInventory();
  }, []);

  return (
    <div className="p-6">
      <h1 className="text-3xl font-bold text-gray-800 mb-2">Inventario</h1>
      <hr className="mb-6 border-gray-300" />

      <div className="grid grid-cols-3 gap-6">
        {combinedData.map((item) => (
          <div key={item.inventoryId} className="bg-white rounded-lg shadow-md">
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
    </div>
  );
};

export default Inventory;
