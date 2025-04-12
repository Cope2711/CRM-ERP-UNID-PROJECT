import { ArrowLeftOnRectangleIcon } from '@heroicons/react/24/outline';
import { useLogout } from '@/hooks/useLogout.ts';

const LogoutButton = () => {
    const handleLogout = useLogout();

    return (
        <button
            onClick={handleLogout}
            className="flex items-center gap-2 px-4 py-2 text-sm text-red-600 hover:bg-red-100 w-full"
        >
            <ArrowLeftOnRectangleIcon className="h-5 w-5" />
            Cerrar sesión
        </button>
    );
};

export default LogoutButton;