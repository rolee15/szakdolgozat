import { NavLink } from "react-router-dom";

const MenuItem = ({ text }: { text: string }) => (
    <NavLink to={text.toLocaleLowerCase()} className="text-white hover:text-gray-900">{text}</NavLink>
);

export default MenuItem;
