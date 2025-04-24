import { Routes, Route } from 'react-router-dom';
import MakeSidebar from './components/Sidebar/MakeSidebar';
import Home from './pages/Home';
import Dashboard from './pages/Dashboard';
import GenericDetailPage from './pages/GenericDetailPage';
import AboutPage from './pages/AboutPage';
import ListViewPage from './pages/ListViewPage';

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
      <Route path="/inventory" element={<Home />} />
      <Route path="/about" element={<AboutPage />} />
      <Route path="/settings" element={<div>Settings Page</div>} />      

      <Route path="/products" element={<ListViewPage key="products" modelName="products" />} />
      <Route path="/products/:id" element={<GenericDetailPage modelName="products" />} />

      <Route path="/suppliers" element={<ListViewPage key="suppliers" modelName="suppliers" />} />
      <Route path="/suppliers/:id" element={<GenericDetailPage modelName="suppliers" />} />

      <Route path="/users" element={<ListViewPage key="users" modelName="users" />} />
      <Route path="/users/:id" element={<GenericDetailPage modelName="users" />} />

      <Route path="/roles" element={<ListViewPage key="roles" modelName="roles" />} />
      <Route path="/roles/:id" element={<GenericDetailPage modelName="roles" />} />

      <Route path="/categories" element={<ListViewPage key="categories" modelName="categories" />} />
      <Route path="/categories/:id" element={<GenericDetailPage modelName="categories" />} />

      <Route path="/brands" element={<ListViewPage key="brands" modelName="brands" />} />
      <Route path="/brands/:id" element={<GenericDetailPage modelName="brands" />} />

      <Route path="/branches" element={<ListViewPage key="branches" modelName="branches" />} />
      <Route path="/branches/:id" element={<GenericDetailPage modelName="branches" />} />
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
