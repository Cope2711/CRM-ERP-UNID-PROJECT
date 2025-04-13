import { ChevronRightIcon } from '@heroicons/react/24/outline';
import { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';

interface SidebarItemProps {
  active?: boolean;
  icon: React.ReactNode;
  text: string;
  expanded?: boolean;
  subMenu?: SubMenuItemProps[] | null;
  path?: string;
}

interface HoveredSubMenuItemProps {
    icon: React.ReactNode;
    text: string;
    active?: boolean;
  }
interface SubMenuItemProps extends Omit<SidebarItemProps, 'subMenu'> {}

function HoveredSubMenuItem({ icon, text, active }: HoveredSubMenuItemProps) {
    return (
      <div className={`my-2 rounded-md p-2 ${active ? 'bg-gray-300' : 'hover:bg-indigo-50'}`}>
        <div className="flex items-center justify-center">
          <span className="h-6 w-6">{icon}</span>
          <span className="ml-3 w-28 text-start">{text}</span>
        </div>
      </div>
    );
  }
export default function SidebarItem({
  icon,
  active = false,
  text,
  expanded = false,
  subMenu = null,
  path,
}: SidebarItemProps) {
  const [expandSubMenu, setExpandSubMenu] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  
  const isActive = path ? 
    (path === '/' ? location.pathname === '/' : location.pathname === path) : 
    active;
  
  const isSubMenuActive = subMenu && subMenu.some(item => 
    item.path && location.pathname === item.path
  );

  useEffect(() => {
    if (!expanded) setExpandSubMenu(false);
  }, [expanded]);

  const subMenuHeight = expandSubMenu ? `${(subMenu?.length || 0) * 40 + 15}px` : '0px';

  return (
    <>
      <li>
        <button
          className={`
            group relative my-1 flex w-full items-center rounded-md px-3 py-2 font-medium transition-colors
            ${(isActive || isSubMenuActive) && !subMenu ? 'bg-indigo-100 text-indigo-600' : 'text-gray-600 hover:bg-indigo-50'}
            ${!expanded && 'hidden sm:flex'}
          `}
          onClick={() => {
            if (subMenu) {
              setExpandSubMenu((curr) => expanded && !curr);
            } else if (path) {
              navigate(path); // ✅ Redirige usando la ruta
            }
          }}
        >
          <span className="h-6 w-6">{icon}</span>
          <span className={`overflow-hidden text-start transition-all ${expanded ? 'ml-3 w-44' : 'w-0'}`}>
            {text}
          </span>
          {subMenu && (
            <div className={`absolute right-2 transition-transform ${expandSubMenu ? 'rotate-90' : ''}`}>
              <ChevronRightIcon className="h-4 w-4" />
            </div>
          )}
          {!expanded && (
            <div
              className="
                invisible absolute left-full ml-6 -translate-x-3
                rounded-md bg-indigo-100 px-2 py-1 text-sm text-indigo-700 opacity-20 transition-all
                group-hover:visible group-hover:translate-x-0 group-hover:opacity-100
              "
            >
              {!subMenu
                ? text
                : subMenu.map((item, index) => (
                    <HoveredSubMenuItem key={index} text={item.text} icon={item.icon} />
                  ))}
            </div>
          )}
        </button>
      </li>
      <ul className="pl-6 transition-all duration-300 ease-in-out overflow-hidden" style={{ height: subMenuHeight }}>
        {expanded &&
          subMenu?.map((item, index) => (
            <SidebarItem key={index} {...item} expanded={expanded} />
          ))}
      </ul>
    </>
  );
}

/* import { ChevronRightIcon } from '@heroicons/react/24/outline';
import { useEffect, useState } from 'react';

interface SidebarItemProps {
  active?: boolean;
  icon: React.ReactNode;
  text: string;
  expanded: boolean;
  subMenu?: SubMenuItemProps[] | null;
  onClick?: () => void;
}

interface SubMenuItemProps {
  icon: React.ReactNode;
  text: string;
  active?: boolean;
}

function HoveredSubMenuItem({ icon, text, active }: SubMenuItemProps) {
  return (
    <div className={`my-2 rounded-md p-2 ${active ? 'bg-gray-300' : 'hover:bg-indigo-50'}`}>
      <div className="flex items-center justify-center">
        <span className="h-6 w-6">{icon}</span>
        <span className="ml-3 w-28 text-start">{text}</span>
      </div>
    </div>
  );
}

export default function SidebarItem({
  icon,
  active = false,
  text,
  expanded = false,
  subMenu = null,
}: SidebarItemProps) {
  const [expandSubMenu, setExpandSubMenu] = useState(false);

  useEffect(() => {
    if (!expanded) setExpandSubMenu(false);
  }, [expanded]);

  const subMenuHeight = expandSubMenu ? `${(subMenu?.length || 0) * 40 + 15}px` : '0px';

  return (
    <>
      <li>
        <button
          className={`
            group relative my-1 flex w-full items-center rounded-md px-3 py-2 font-medium transition-colors
            ${active && !subMenu ? 'bg-indigo-100 text-indigo-600' : 'text-gray-600 hover:bg-indigo-50'}
            ${!expanded && 'hidden sm:flex'}
          `}
          onClick={() => {
            if (subMenu) {
              setExpandSubMenu((curr) => expanded && !curr);
            } else {
                onclick?.(); // Ejecuta el onClick si existe
            }
          }}
        >
          <span className="h-6 w-6">{icon}</span>
          <span className={`overflow-hidden text-start transition-all ${expanded ? 'ml-3 w-44' : 'w-0'}`}>
            {text}
          </span>
          {subMenu && (
            <div
              className={`absolute right-2 transition-transform ${expandSubMenu ? 'rotate-90' : ''}`}
            >
              <ChevronRightIcon className="h-4 w-4" />
            </div>
          )}
          {!expanded && (
            <div
              className="
                invisible absolute left-full ml-6 -translate-x-3
                rounded-md bg-indigo-100 px-2 py-1 text-sm text-indigo-700 opacity-20 transition-all
                group-hover:visible group-hover:translate-x-0 group-hover:opacity-100
              "
            >
              {!subMenu
                ? text
                : subMenu.map((item, index) => (
                    <HoveredSubMenuItem
                      key={index}
                      text={item.text}
                      icon={item.icon}
                    />
                  ))}
            </div>
          )}
        </button>
      </li>
      <ul className="pl-6 transition-all duration-300 ease-in-out overflow-hidden" style={{ height: subMenuHeight }}>
        {expanded &&
          subMenu?.map((item, index) => (
            <SidebarItem key={index} {...item} expanded={expanded} />
          ))}
      </ul>
    </>
  );
}
 */