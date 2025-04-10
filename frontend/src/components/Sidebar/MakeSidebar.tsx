import {
    HomeIcon,
    UserIcon,
    CogIcon,
    InformationCircleIcon,
    ShoppingBagIcon,
    UsersIcon,
    ChartPieIcon,
    BuildingStorefrontIcon,

  } from '@heroicons/react/24/outline';
  import { useState } from 'react';
  import Sidebar from './Sidebar';
  import SidebarItem from './SidebarItem';

  
  export default function MakeSidebar() {
    const [expanded, setExpanded] = useState(true);
  
    const navBarItems = [
        {
          icon: <HomeIcon className="h-5 w-5" />,
          text: 'Home',
          path: '/',
          active: true,
        },
        {
          icon: <ChartPieIcon className="h-5 w-5" />,
          text: 'Dashboard',
          path: '/dashboard',
        },
        {
          icon: <BuildingStorefrontIcon className="h-5 w-5" />,
          text: 'Inventory',
          path: '/inventory',
        },
        {
          icon: <ShoppingBagIcon className="h-5 w-5" />,
          text: 'Products',
          path: '/products',
        },
        {
          icon: <UsersIcon className="h-5 w-5" />,
          text: 'Suppliers',
          path: '/suppliers',
        },
        {
          icon: <UserIcon className="h-5 w-5" />,
          text: 'Users',
          subMenu: [
            {
              icon: <UserIcon className="h-5 w-5" />,
              text: 'User List',
              path: '/users/list',
            },
            {
              icon: <CogIcon className="h-5 w-5" />,
              text: 'User Roles',
              path: '/users/roles',
            },
          ],
        },
        {
          icon: <InformationCircleIcon className="h-5 w-5" />,
          text: 'About',
          path: '/about',
        },
        {
          icon: <ChartPieIcon className="h-5 w-5" />,
          text: 'Settings',
          path: '/settings',
        },
      ];
      
          
    //   {
    //     icon: <HomeIcon className="h-5 w-5" />,
    //     text: 'Home',
    //     active: true,
    //     onClick: () => console.log('Go to Home'),
    //   },
    //   {
    //     icon: <ChartPieIcon className="h-5 w-5" />,
    //     text: 'Dashboard',
    //     onClick: () => console.log('Go to Dashboard'),
    //   },
    //   {
    //     icon: <BuildingStorefrontIcon className="h-5 w-5" />,
    //     text: 'Inventory',
    //     onClick: () => console.log('Go to Inventory'),
    //   },
    //   {
    //     icon: <ShoppingBagIcon className="h-5 w-5" />,
    //     text: 'Products',
    //     onClick: () => console.log('Go to Products'),
    //   },
    //   {
    //     icon: <UsersIcon className="h-5 w-5" />,
    //     text: 'Suppliers',
    //     onClick: () => console.log('Go to Suppliers'),
    //   },
    //   {
    //     icon: <UserIcon className="h-5 w-5" />,
    //     text: 'Users',
    //     subMenu: [
    //       {
    //         icon: <UserIcon className="h-5 w-5" />,
    //         text: 'User List',
    //         onClick: () => console.log('Go to User List'),
    //       },
    //       {
    //         icon: <CogIcon className="h-5 w-5" />,
    //         text: 'User Roles',
    //         onClick: () => console.log('Go to User Roles'),
    //       },
    //     ],
    //   },
    //   {
    //     icon: <InformationCircleIcon className="h-5 w-5" />,
    //     text: 'About',
    //     onClick: () => console.log('Go to About'),
    //   },
    //   {
    //     icon: <CogIcon className="h-5 w-5" />,
    //     text: 'Settings',
    //     onClick: () => console.log('Go to Settings'),
    //   },
    
  
    return (
        <Sidebar expanded={expanded} setExpanded={setExpanded}>
          {navBarItems.map((item, index) => (
            <SidebarItem key={index} expanded={expanded} {...item} />
          ))}
        </Sidebar>
      );
    }
    