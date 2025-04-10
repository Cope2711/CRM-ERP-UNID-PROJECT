import {
    ArrowRightIcon,
    ArrowLeftIcon,
    EllipsisVerticalIcon,
  } from '@heroicons/react/24/outline';
  import type { ReactNode } from 'react';
  
  interface SidebarProps {
    children: ReactNode;
    expanded: boolean;
    setExpanded: (value: boolean | ((prev: boolean) => boolean)) => void;
  }
  
  export default function Sidebar({ children, expanded, setExpanded }: SidebarProps) {
    return (
      <div className="relative">
        <div className={`fixed inset-0 -z-10 block bg-gray-400 ${expanded ? 'block sm:hidden' : 'hidden'}`} />
        <aside className={`box-border h-screen transition-all ${expanded ? 'w-5/6 sm:w-64' : 'w-0 sm:w-20'}`}>
          <nav className="flex h-full flex-col border-r bg-white shadow-sm">
            <div className="flex items-center justify-between p-4 pb-2">
              <img
                src="https://cdn.worldvectorlogo.com/logos/minecraft-1.svg"
                className={`overflow-hidden transition-all ${expanded ? 'w-32' : 'w-0'}`}
                alt="Logo"
              />
              <div className={`${expanded ? '' : 'hidden sm:block'}`}>
                <button
                  onClick={() => setExpanded((curr) => !curr)}
                  className="rounded-lg bg-gray-50 p-1.5 hover:bg-gray-100"
                >
                  {expanded ? (
                    <ArrowRightIcon className="h-6 w-6" />
                  ) : (
                    <ArrowLeftIcon className="h-6 w-6" />
                  )}
                </button>
              </div>
            </div>
            <ul className="flex-1 px-3">{children}</ul>
            <div className="flex border-t p-3">
              <img
                src="https://pic.onlinewebfonts.com/thumbnails/icons_401325.svg"
                alt="User Avatar"
                className="h-10 w-10 rounded-md"
              />
              <div className={`flex items-center justify-between overflow-hidden transition-all ${expanded ? 'ml-3 w-52' : 'w-0'}`}>
                <div className="leading-4">
                  <h4 className="font-semibold">Steve</h4>
                  <span className="text-xs text-gray-600">CRM-ERP@gmail.com</span>
                </div>
                <EllipsisVerticalIcon className="h-6 w-6" />
              </div>
            </div>
          </nav>
        </aside>
      </div>
    );
  }
  