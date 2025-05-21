import { useNavigate } from "react-router-dom";

type InventoryCardProps = {
    id: string;
    name: string;
    brandName?: string;
    description: string;
    price: number;
    quantity: number;
};

export default function InventoryCard({ id, name, brandName, description, price, quantity }: InventoryCardProps) {
    const navigate = useNavigate();

    const getColorClass = () => {
        if (quantity === 0) return "bg-red-500";
        if (quantity > 0 && quantity < 10) return "bg-yellow-500";
        return "bg-green-500";
    };

    const onClick = () => {
        navigate(`/inventory/${id}`);
    };

    return (
        <div key={id} className="bg-white rounded-lg shadow-md hover:shadow-xl transition-shadow duration-300" onClick={onClick}>
            <div className={"h-2 rounded-t-lg " + getColorClass()} />
            <div className="p-4">
                <h3 className="font-bold text-lg">{name}</h3>
                <p className="text-sm text-gray-500 mb-1">Marca: {brandName}</p>
                <p className="text-gray-600">{description}</p>
                <p className="text-blue-600 font-semibold mt-2">${price}</p>
                <p className="text-sm text-gray-500">Cantidad: {quantity}</p>
            </div>
        </div>
    );
}