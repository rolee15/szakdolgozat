import { NavLink } from "react-router-dom";

type MenuItemProps = {
  to: string;
  children: React.ReactNode;
  onClick?: () => void;
  variant?: "nav" | "dropdown";
};

const navClass = ({ isActive }: { isActive: boolean }) =>
  isActive ? "text-brand-primary-light text-sm" : "text-brand-muted hover:text-brand-text text-sm";

const dropdownClass = ({ isActive }: { isActive: boolean }) =>
  `block px-4 py-2 text-sm transition-colors ${
    isActive
      ? "text-brand-primary-light hover:bg-brand-primary-deep"
      : "text-brand-text hover:bg-brand-primary-deep"
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
