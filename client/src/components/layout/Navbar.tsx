import { NavLink, useNavigate } from "react-router-dom";
import Logo from "./Logo";
import MenuItem from "./MenuItem";
import { useAuth } from "@/context/AuthContext";

const Navbar = () => {
  const { isAuthenticated, username, isAdmin, logout } = useAuth();
  const navigate = useNavigate();
  const menuItems = ["Hiragana", "Katakana", "Lessons"];

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

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
              <NavLink to="/kanji" className="text-white hover:text-gray-300">
                Kanji
              </NavLink>
              <NavLink to="/flashcards" className="text-white hover:text-gray-300">
                Flash Cards
              </NavLink>
              <NavLink to="/lessons/writing" className="text-white hover:text-gray-300">
                Writing
              </NavLink>
              <NavLink to="/grammar" className="text-white hover:text-gray-300">
                Grammar
              </NavLink>
              {isAdmin && (
                <NavLink to="/admin" className="text-white hover:text-gray-300">
                  Admin
                </NavLink>
              )}
            </nav>

            <div className="flex items-center space-x-4">
              {isAuthenticated ? (
                <>
                  <span className="text-gray-300 text-sm">{username}</span>
                  <button
                    onClick={handleLogout}
                    className="text-white hover:text-gray-300"
                  >
                    Logout
                  </button>
                </>
              ) : (
                <>
                  <NavLink to="/login">
                    <button>Login</button>
                  </NavLink>
                  <NavLink to="/register">
                    <button>Register</button>
                  </NavLink>
                </>
              )}
            </div>
          </div>
        </div>
      </div>
    </header>
  );
};

export default Navbar;
