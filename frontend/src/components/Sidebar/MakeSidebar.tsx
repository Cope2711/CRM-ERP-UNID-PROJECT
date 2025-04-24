import {
    HomeIcon,
    UserIcon,
    InformationCircleIcon,
    ShoppingBagIcon,
    UsersIcon,
    ChartPieIcon,
    BuildingStorefrontIcon,
    ShoppingCartIcon,
    ComputerDesktopIcon,
    BuildingOffice2Icon,
} from '@heroicons/react/24/outline';
import { useState } from 'react';
import Sidebar from './Sidebar';
import SidebarItem from './SidebarItem';
import { BriefcaseIcon, TagIcon } from 'lucide-react';

export default function MakeSidebar() {
    const [expanded, setExpanded] = useState(true);

    const navBarItems = [
        {
            icon: <HomeIcon className="h-5 w-5" />,
            text: 'Home',
            path: '/Home',
        },
        // {
        //     icon: <ChartPieIcon className="h-5 w-5" />,
        //     text: 'Dashboard',
        //     path: '/dashboard',
        // },
        {
            icon: <ShoppingCartIcon className="h-5 w-5" />,
            text: 'Sales',
            path: '/sales',
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
            path: '/users',
        },
        {
            icon: <ComputerDesktopIcon className="h-5 w-5" />,
            text: 'Roles',
            path: '/roles',
        },
        {
            icon: <TagIcon className="h-5 w-5" />,
            text: 'Categories',
            path: '/categories',
        },
        {
            icon: <BriefcaseIcon className="h-5 w-5" />,
            text: 'Brands',
            path: '/brands',
        },
        {
            icon: <BuildingOffice2Icon className="h-5 w-5" />,
            text: 'Branches',
            path: '/branches',
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

    return (
        <Sidebar expanded={expanded} setExpanded={setExpanded}>
            <div className="flex flex-col h-full">
                <div className="flex-1">
                    {navBarItems.map((item, index) => (
                        <SidebarItem key={index} expanded={expanded} {...item} />
                    ))}
                </div>
            </div>
        </Sidebar>
    );
}
