import { Link } from "react-router-dom";

const NotFoundPage = () => {
    return (
        <div className="flex flex-col items-center justify-center h-screen text-center">
            <h1 className="text-6xl font-bold text-red-500 mb-4">404</h1>
            <p className="text-xl mb-4">Oops! El recurso no fue encontrado.</p>
            <Link to="/" className="text-blue-600 underline hover:text-blue-800">
                Volver al inicio
            </Link>
        </div>
    );
};

export default NotFoundPage;