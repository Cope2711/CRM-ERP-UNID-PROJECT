import React, { useEffect, useState } from 'react';

interface PopupMessageProps {
    message: string;
    duration?: number;
    isSuccess?: boolean;
}

const PopupMessage: React.FC<PopupMessageProps> = ({ message, duration = 3000, isSuccess = true }) => {
    const [visible, setVisible] = useState(true);

    useEffect(() => {
        const timer = setTimeout(() => setVisible(false), duration);
        return () => clearTimeout(timer);
    }, [duration]);

    if (!visible) return null;

    const color = isSuccess ? 'bg-green-100 border border-green-400 text-green-700' : 'bg-red-100 border border-red-400 text-red-700';

    return (
        <div className={`fixed top-5 right-5 z-50 p-4 rounded-lg shadow-lg ${color}`}>
            {
                isSuccess
                ? <strong className="font-bold">¡Éxito! </strong>
                : <strong className="font-bold">¡Error! </strong>
            }
            <span className="block sm:inline">{message}</span>
        </div>
    );
};

export default PopupMessage;
