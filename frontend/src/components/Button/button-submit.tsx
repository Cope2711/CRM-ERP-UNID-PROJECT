import React from 'react';

interface ButtonProps {
  children: React.ReactNode;
  onClick: () => void;
  className?: string;
}

const Button: React.FC<ButtonProps> = ({ children, onClick, className }) => {
  return (
    <button
      onClick={onClick}
      className={`bg-white text-black font-bold py-3 text-xl rounded-xl active:scale-[.98] active:duration-75 transition-all hover:scale-[1.01] ease-in-out ${className}`}
    >
      {children}
    </button>
  );
};

export default Button;
