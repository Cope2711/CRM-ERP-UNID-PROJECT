import React from 'react';

const TopBar = () => (
  <div className="bg-white shadow-md flex justify-between items-center p-4">
    <h1 className="text-xl font-semibold">Welcome back, User!</h1>
    <div>
      <button className="text-gray-700 bg-gray-100 px-4 py-2 rounded-lg hover:bg-gray-200">Profile</button>
    </div>
  </div>
);

const MainContent = () => (
  <div className="flex-1 p-6">
    <h2 className="text-2xl font-bold mb-4">Overview</h2>
    <div className="grid grid-cols-3 gap-6">
      <div className="bg-white p-6 shadow-lg rounded-lg">
        <h3 className="font-semibold">Sales</h3>
        <p className="text-gray-700">$45,000</p>
      </div>
      <div className="bg-white p-6 shadow-lg rounded-lg">
        <h3 className="font-semibold">Users</h3>
        <p className="text-gray-700">1,200</p>
      </div>
      <div className="bg-white p-6 shadow-lg rounded-lg">
        <h3 className="font-semibold">Revenue</h3>
        <p className="text-gray-700">$75,000</p>
      </div>
    </div>
  </div>
);

const Dashboard = () => (
  <div className="flex flex-col min-h-screen">
    <TopBar />
    <MainContent />
  </div>
);

export default Dashboard;
