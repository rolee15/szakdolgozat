import { NavLink } from "react-router-dom";

type MenuItemProps = {
  to: string;
  children: React.ReactNode;
  onClick?: () => void;
  variant?: "nav" | "dropdown";
};

const navClass = ({ isActive }: { isActive: boolean }) =>
  isActive ? "text-primary text-sm" : "text-on-bar/80 hover:text-on-bar text-sm";

const dropdownClass = ({ isActive }: { isActive: boolean }) =>
  `block px-4 py-2 text-sm transition-colors ${
    isActive
      ? "text-primary hover:bg-primary/20"
      : "text-on-bar hover:bg-primary/20"
  }`;

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
