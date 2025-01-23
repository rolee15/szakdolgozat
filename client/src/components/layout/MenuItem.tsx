import { NavLink } from "react-router-dom";

const MenuItem = ({ text }: { text: string }) => (
    <NavLink to={text} className="text-gray-600 hover:text-gray-900">{text}</NavLink>
);

export default MenuItem;
