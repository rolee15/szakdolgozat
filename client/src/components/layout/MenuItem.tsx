import { NavLink } from "react-router-dom";

type MenuItemProps = {
  to: string;
  children: React.ReactNode;
  onClick?: () => void;
  variant?: "nav" | "dropdown";
};

const navClass = ({ isActive }: { isActive: boolean }) =>
  isActive ? "text-indigo-400 text-sm" : "text-gray-300 hover:text-white text-sm";

const dropdownClass = ({ isActive }: { isActive: boolean }) =>
  `block px-4 py-2 text-sm ${isActive ? "text-indigo-400" : "text-gray-300 hover:text-white"}`;

const MenuItem = ({ to, children, onClick, variant = "nav" }: MenuItemProps) => (
  <NavLink
    to={to}
    className={variant === "dropdown" ? dropdownClass : navClass}
    onClick={onClick}
  >
    {children}
  </NavLink>
);

export default MenuItem;
