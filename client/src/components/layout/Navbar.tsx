import { NavLink } from "react-router-dom";
import Logo from "./Logo";
import MenuItem from "./MenuItem";

const Navbar = () => {
  const menuItems = ["Hiragana", "Katakana"];

  return (
    <header className="bg-black shadow-sm">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <Logo />

          <div className="flex items-center space-x-8">
            <nav className="space-x-4">
              {menuItems.map((item) => (
                <MenuItem key={item} text={item} />
              ))}
            </nav>

            <div className="flex items-center space-x-4">
              <NavLink to="/signin">
                <button>Sign In</button>
              </NavLink>
              <NavLink to="/register">
                <button>Register</button>
              </NavLink>
            </div>
          </div>
        </div>
      </div>
    </header>
  );
};

export default Navbar;
