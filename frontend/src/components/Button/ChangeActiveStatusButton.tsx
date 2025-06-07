import genericService from "@/services/genericService";
import { Button } from "antd";
import { useState } from "react";
import PopupMessage from "../message/SuccessMessage";

interface ChangeActiveStatusButtonProps {
    modelName: string;
    id: string;
    isActive: boolean;
}

const ChangeActiveStatusButton: React.FC<ChangeActiveStatusButtonProps> = ({ modelName, id, isActive }) => {
    const [loading, setLoading] = useState<boolean>(false);
    const [showSuccess, setShowSuccess] = useState<boolean>(false);
    const [active, setActive] = useState<boolean>(isActive);

    const onClick = async () => {
        setLoading(true);

        try {
            if (active) {
                await genericService.deactivate([id], modelName);
            } else {
                await genericService.activate([id], modelName);
            }

            setActive(!active);
            setShowSuccess(true);
            setLoading(false);
        } catch (error: any) {
            console.error("Error changing active status:", error);
            setShowSuccess(false);
        } finally {
            setLoading(false);
        }
    }

    return (
        <div>
        <Button
            children={active ? "Deactivate" : "Activate"}
            loading={loading}
            onClick={onClick}
        />
        {showSuccess && (
            <PopupMessage
                message={`${modelName.charAt(0).toUpperCase() + modelName.slice(1)} ${active ? "deactivated" : "activated"} successfully`}
            />
        )}
        </div>
    )
}

export default ChangeActiveStatusButton;