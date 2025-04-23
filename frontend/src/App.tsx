import { Routes, Route } from 'react-router-dom';
import MakeSidebar from './components/Sidebar/MakeSidebar';
import Home from './pages/Home';
import Dashboard from './pages/Dashboard';
import GenericDetailPage from './pages/GenericDetailPage';
import Inventory from './pages/Inventory';

const MainLayout = ({ children }: { children: React.ReactNode }) => {
  return (
    <div className="flex">
      <MakeSidebar />
      <div className="flex-1 p-4">
        {children}
      </div>
    </div>
  );
};

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/dashboard" element={<Dashboard />} />
      <Route path="/inventory" element={<Inventory/>} />
      <Route path="/products" element={<div>Products Page</div>} />
      <Route path="/suppliers" element={<div>Suppliers Page</div>} />
      <Route path="/about" element={<div>About Page</div>} />
      <Route path="/settings" element={<div>Settings Page</div>} />
      <Route path="/users/list" element={<div>User List Page</div>} />
      <Route path="/users/roles" element={<div>User Roles Page</div>} />
      <Route path="/users/:id" element={<GenericDetailPage modelName="users" />} />

    </Routes>
  );
};

function App() {
  return (
    <MainLayout>
      <AppRoutes />
    </MainLayout>
  );
}

export default App;



// import MakeSidebar from './components/Sidebar/MakeSidebar';

// function App() {
//   return (
//     <div className="flex">
//       <MakeSidebar />
//       <main className="flex-1 p-4">Texto de Prueba</main>
//     </div>
//   );
// }

// export default App;
