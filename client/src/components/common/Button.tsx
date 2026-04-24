import { type ButtonHTMLAttributes } from "react";

type ButtonProps = ButtonHTMLAttributes<HTMLButtonElement>;

const Button = ({ className = "", children, ...props }: ButtonProps) => (
  <button
    className={`bg-black text-white text-xl px-6 py-2 rounded-lg hover:bg-gray-900 transition-colors cursor-pointer disabled:opacity-50 ${className}`}
    {...props}
  >
    {children}
  </button>
);

export default Button;
