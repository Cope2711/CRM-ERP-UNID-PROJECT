import React, { useEffect, useState } from 'react';

interface AlertMessageProps {
    message: string;
    duration?: number;
}

const AlertMessage: React.FC<AlertMessageProps> = ({ message, duration = 3000 }) => {
    const [visible, setVisible] = useState(true);

    useEffect(() => {
        const timer = setTimeout(() => setVisible(false), duration);
        return () => clearTimeout(timer);
    }, [duration]);

    if (!visible) return null;

    return (
        <div className="fixed top-5 right-5 z-50 bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded shadow-lg w-fit max-w-sm animate-fade-in">
            <strong className="font-bold">¡Error! </strong>
            <span className="block sm:inline">{message}</span>
        </div>
    );
};

export default AlertMessage;
