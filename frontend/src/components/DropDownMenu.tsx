import { useState, useRef, useEffect, ReactNode } from 'react';

type DropdownOption = {
    button: ReactNode;
};

type DropdownMenuProps = {
    options: DropdownOption[];
    buttonIcon?: ReactNode;
};

export default function DropdownMenu({ options, buttonIcon }: DropdownMenuProps) {
    const [open, setOpen] = useState(false);
    const [openUpward, setOpenUpward] = useState(false);
    const dropdownRef = useRef<HTMLDivElement>(null);
    const buttonRef = useRef<HTMLButtonElement>(null);

    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            if (
                dropdownRef.current &&
                !dropdownRef.current.contains(event.target as Node)
            ) {
                setOpen(false);
            }
        };
        document.addEventListener('mousedown', handleClickOutside);
        return () => document.removeEventListener('mousedown', handleClickOutside);
    }, []);

    useEffect(() => {
        if (open && buttonRef.current) {
            const rect = buttonRef.current.getBoundingClientRect();
            const viewportHeight = window.innerHeight;
            const dropdownHeight = 150; // Aprox

            if (viewportHeight - rect.bottom < dropdownHeight) {
                setOpenUpward(true);
            } else {
                setOpenUpward(false);
            }
        }
    }, [open]);

    return (
        <div className="relative" ref={dropdownRef}>
            <button
                ref={buttonRef}
                onClick={() => setOpen(prev => !prev)}
                className="rounded-md p-1.5 hover:bg-gray-100 transition-colors"
            >
                {buttonIcon}
            </button>

            {open && (
                <div
                    className={`absolute right-0 w-44 rounded-md bg-white border border-gray-200 shadow-lg z-50 
                        ${openUpward ? 'bottom-full mb-2' : 'mt-2'}`}
                >
                    <ul className="py-1 text-sm text-gray-800">
                        {options.map((opt, idx) => (
                            <li key={idx}>
                                {opt.button}
                            </li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
}
