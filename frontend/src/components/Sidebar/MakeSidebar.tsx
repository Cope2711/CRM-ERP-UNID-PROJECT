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
import {useState} from 'react';
import Sidebar from './Sidebar';
import SidebarItem from './SidebarItem';
import SidebarLogoutItem from "@/components/Sidebar/SidebarLogoutItem.tsx";


export default function MakeSidebar() {
    const [expanded, setExpanded] = useState(true);

    const navBarItems = [
        {
            icon: <HomeIcon className="h-5 w-5"/>,
            text: 'Home',
            path: '/',
            active: true,
        },
        {
            icon: <ChartPieIcon className="h-5 w-5"/>,
            text: 'Dashboard',
            path: '/dashboard',
        },
        {
            icon: <BuildingStorefrontIcon className="h-5 w-5"/>,
            text: 'Inventory',
            path: '/inventory',
        },
        {
            icon: <ShoppingBagIcon className="h-5 w-5"/>,
            text: 'Products',
            path: '/products',
        },
        {
            icon: <UsersIcon className="h-5 w-5"/>,
            text: 'Suppliers',
            path: '/suppliers',
        },
        {
            icon: <UserIcon className="h-5 w-5"/>,
            text: 'Users',
            subMenu: [
                {
                    icon: <UserIcon className="h-5 w-5"/>,
                    text: 'User List',
                    path: '/users/list',
                },
                {
                    icon: <CogIcon className="h-5 w-5"/>,
                    text: 'User Roles',
                    path: '/users/roles',
                },
            ],
        },
        {
            icon: <InformationCircleIcon className="h-5 w-5"/>,
            text: 'About',
            path: '/about',
        },
        {
            icon: <ChartPieIcon className="h-5 w-5"/>,
            text: 'Settings',
            path: '/settings',
        },
    ];

    return (
        <Sidebar expanded={expanded} setExpanded={setExpanded}>
            <div className="flex flex-col h-full">
                <div className="flex-1">
                    {navBarItems.map((item, index) => (
                        <SidebarItem key={index} expanded={expanded} {...item} />
                    ))}
                </div>
                <div>
                    <SidebarLogoutItem />
                </div>
            </div>
        </Sidebar>
    );
}
    