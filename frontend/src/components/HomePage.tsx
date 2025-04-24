import {
    LineChart,
    Line,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    ResponsiveContainer
} from 'recharts';
import {
    UserCircle,
    Users,
    ShoppingCart,
    Package,
    DollarSign,
    Bell
} from 'lucide-react';
import { Link } from 'react-router-dom';
import { motion } from 'framer-motion';

const data = [
    { name: 'Lun', ventas: 120 },
    { name: 'Mar', ventas: 230 },
    { name: 'Mié', ventas: 180 },
    { name: 'Jue', ventas: 250 },
    { name: 'Vie', ventas: 300 },
];

const usuariosPreview = [
    { nombre: 'Ana López', estado: 'Activo', imagen: 'https://i.pravatar.cc/150?img=5' },
    { nombre: 'Carlos Ruiz', estado: 'Inactivo', imagen: 'https://i.pravatar.cc/150?img=3' },
    { nombre: 'María Pérez', estado: 'Activo', imagen: 'https://i.pravatar.cc/150?img=7' },
];

// Variantes de animación reutilizables
const fadeUp = {
    hidden: { opacity: 0, y: 30 },
    visible: (i = 0) => ({
        opacity: 1,
        y: 0,
        transition: {
            delay: i * 0.1,
            duration: 0.5,
            ease: 'easeOut'
        }
    })
};

export default function HomePage() {
    return (
        <div className="min-h-screen bg-gradient-to-b from-indigo-50 via-white to-indigo-100 p-6">
            <header className="flex justify-between items-center mb-8">
                <h1 className="text-3xl font-bold text-indigo-800">Dashboard ERP-CRM</h1>
                <div className="flex items-center gap-2 text-gray-700">
                    <UserCircle className="w-6 h-6 text-indigo-600" />
                    <span>Hola, Usuario</span>
                </div>
            </header>

            {/* Cards con animación */}
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 mb-10">
                {[
                    {
                        title: 'Usuarios',
                        value: '128',
                        icon: <Users className="w-5 h-5 text-blue-600" />,
                        color: 'bg-blue-100'
                    },
                    {
                        title: 'Ventas del mes',
                        value: '$12,340',
                        icon: <DollarSign className="w-5 h-5 text-green-600" />,
                        color: 'bg-green-100'
                    },
                    {
                        title: 'Productos',
                        value: '156',
                        icon: <Package className="w-5 h-5 text-purple-600" />,
                        color: 'bg-purple-100'
                    },
                    {
                        title: 'Pedidos',
                        value: '67',
                        icon: <ShoppingCart className="w-5 h-5 text-orange-600" />,
                        color: 'bg-orange-100'
                    }
                ].map((card, idx) => (
                    <motion.div
                        key={idx}
                        custom={idx}
                        initial="hidden"
                        animate="visible"
                        variants={fadeUp}
                        className="bg-white rounded-2xl shadow-lg p-5 flex items-center gap-4 hover:shadow-xl"
                    >
                        <div className={`p-3 rounded-full ${card.color}`}>
                            {card.icon}
                        </div>
                        <div>
                            <h2 className="text-sm text-gray-500">{card.title}</h2>
                            <p className="text-xl font-bold text-gray-800">{card.value}</p>
                        </div>
                    </motion.div>
                ))}
            </div>

            {/* Gráfica animada */}
            <motion.div
                initial="hidden"
                animate="visible"
                variants={fadeUp}
                className="bg-white p-6 rounded-2xl shadow-lg mb-10"
            >
                <h2 className="text-lg font-semibold text-gray-700 mb-4">Ventas semanales</h2>
                <ResponsiveContainer width="100%" height={300}>
                    <LineChart data={data}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="name" />
                        <YAxis />
                        <Tooltip />
                        <Line type="monotone" dataKey="ventas" stroke="#6366F1" strokeWidth={3} />
                    </LineChart>
                </ResponsiveContainer>
            </motion.div>

            {/* Secciones inferiores */}
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-4">
                {[
                    {
                        title: 'Actividad reciente',
                        icon: <Bell className="w-5 h-5" />,
                        content: (
                            <ul className="space-y-2 text-sm text-gray-600">
                                <li>📦 Nuevo producto agregado: "Laptop ASUS Zenbook"</li>
                                <li>👤 Usuario Ana López se ha registrado</li>
                                <li>🛒 Nueva venta realizada: $1,240</li>
                                <li>✅ Se completó el pedido #5482</li>
                            </ul>
                        )
                    },
                    {
                        title: 'Módulos',
                        content: (
                            <div className="grid grid-cols-2 gap-4 text-center">
                                <Link to="/users" className="bg-indigo-100 rounded-xl p-4 hover:bg-indigo-200 text-indigo-800 font-medium shadow-sm transition">Usuarios</Link>
                                <Link to="/products" className="bg-indigo-100 rounded-xl p-4 hover:bg-indigo-200 text-indigo-800 font-medium shadow-sm transition">Productos</Link>
                                <Link to="/sales" className="bg-indigo-100 rounded-xl p-4 hover:bg-indigo-200 text-indigo-800 font-medium shadow-sm transition">Ventas</Link>
                                <Link to="/suppliers" className="bg-indigo-100 rounded-xl p-4 hover:bg-indigo-200 text-indigo-800 font-medium shadow-sm transition">Proveedores</Link>
                            </div>
                        )
                    },
                    {
                        title: 'Usuarios recientes',
                        content: (
                            <ul className="space-y-3">
                                {usuariosPreview.map((user, i) => (
                                    <li key={i} className="flex items-center gap-4">
                                        <img src={user.imagen} alt={user.nombre} className="w-10 h-10 rounded-full" />
                                        <div>
                                            <p className="text-sm font-medium text-gray-800">{user.nombre}</p>
                                            <span className={`text-xs ${user.estado === 'Activo' ? 'text-green-600' : 'text-red-500'}`}>{user.estado}</span>
                                        </div>
                                    </li>
                                ))}
                            </ul>
                        )
                    }
                ].map((section, idx) => (
                    <motion.div
                        key={idx}
                        custom={idx}
                        initial="hidden"
                        animate="visible"
                        variants={fadeUp}
                        className="bg-white p-6 rounded-2xl shadow"
                    >
                        <h2 className="text-lg font-semibold text-gray-700 mb-4 flex items-center gap-2">
                            {section.icon || null} {section.title}
                        </h2>
                        {section.content}
                    </motion.div>
                ))}
            </div>
        </div>
    );
}
