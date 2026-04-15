import { useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import Logo from "./Logo";
import { useAuth } from "@/context/AuthContext";

const navLinkClass = ({ isActive }: { isActive: boolean }) =>
  isActive ? 'text-indigo-400' : 'text-gray-300 hover:text-white';

const Navbar = () => {
  const { isAuthenticated, username, isAdmin, logout } = useAuth();
  const navigate = useNavigate();
  const [mobileOpen, setMobileOpen] = useState(false);

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <header className="bg-black shadow-sm">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <Logo />

          {/* Desktop nav */}
          <div className="hidden md:flex items-center space-x-8">
            <nav className="flex items-center gap-2">
              {/* Study group */}
              <span className="text-gray-500 text-xs uppercase tracking-wider">Study</span>
              <div className="flex gap-2">
                <NavLink to="/hiragana" className={navLinkClass}>Hiragana</NavLink>
                <NavLink to="/katakana" className={navLinkClass}>Katakana</NavLink>
                <NavLink to="/kanji" className={navLinkClass}>Kanji</NavLink>
                <NavLink to="/grammar" className={navLinkClass}>Grammar</NavLink>
                <NavLink to="/reading" className={navLinkClass}>Reading</NavLink>
              </div>

              {/* Divider */}
              <span className="border-l border-gray-600 pl-4 ml-2 flex items-center gap-2">
                <span className="text-gray-500 text-xs uppercase tracking-wider">Practice</span>
                <div className="flex gap-2">
                  <NavLink to="/lessons" className={navLinkClass}>Lessons</NavLink>
                  <NavLink to="/lessons/writing" className={navLinkClass}>Writing</NavLink>
                  <NavLink to="/flashcards" className={navLinkClass}>Flash Cards</NavLink>
                </div>
              </span>

              {/* Divider */}
              <span className="border-l border-gray-600 pl-4 ml-2 flex items-center gap-2">
                <span className="text-gray-500 text-xs uppercase tracking-wider">Path</span>
                <NavLink to="/path" className={navLinkClass}>Learning Path</NavLink>
              </span>

              {isAdmin && (
                <NavLink to="/admin" className={navLinkClass}>Admin</NavLink>
              )}
            </nav>

            <div className="flex items-center space-x-4">
              {isAuthenticated ? (
                <>
                  <span className="text-gray-300 text-sm">{username}</span>
                  <NavLink to="/settings" className={navLinkClass}>Settings</NavLink>
                  <button
                    onClick={handleLogout}
                    className="text-white hover:text-gray-300"
                  >
                    Logout
                  </button>
                </>
              ) : (
                <>
                  <NavLink to="/login" className="text-white hover:text-gray-300">Login</NavLink>
                  <NavLink to="/register" className="text-white hover:text-gray-300">Register</NavLink>
                </>
              )}
            </div>
          </div>

          {/* Mobile: hamburger button + auth */}
          <div className="flex md:hidden items-center gap-4">
            {isAuthenticated ? (
              <>
                <span className="text-gray-300 text-sm">{username}</span>
                <button onClick={handleLogout} className="text-white hover:text-gray-300">
                  Logout
                </button>
              </>
            ) : (
              <>
                <NavLink to="/login" className="text-white hover:text-gray-300">Login</NavLink>
                <NavLink to="/register" className="text-white hover:text-gray-300">Register</NavLink>
              </>
            )}
            <button
              aria-label={mobileOpen ? 'Close menu' : 'Open menu'}
              onClick={() => setMobileOpen((prev) => !prev)}
              className="text-white text-2xl focus:outline-none"
            >
              {mobileOpen ? '✕' : '☰'}
            </button>
          </div>
        </div>

        {/* Mobile dropdown */}
        {mobileOpen && (
          <nav className="md:hidden pb-4 flex flex-col gap-4">
            <div>
              <p className="text-gray-500 text-xs uppercase tracking-wider mb-1">Study</p>
              <div className="flex flex-col gap-1 pl-2">
                <NavLink to="/hiragana" className={navLinkClass} onClick={() => setMobileOpen(false)}>Hiragana</NavLink>
                <NavLink to="/katakana" className={navLinkClass} onClick={() => setMobileOpen(false)}>Katakana</NavLink>
                <NavLink to="/kanji" className={navLinkClass} onClick={() => setMobileOpen(false)}>Kanji</NavLink>
                <NavLink to="/grammar" className={navLinkClass} onClick={() => setMobileOpen(false)}>Grammar</NavLink>
                <NavLink to="/reading" className={navLinkClass} onClick={() => setMobileOpen(false)}>Reading</NavLink>
              </div>
            </div>
            <div>
              <p className="text-gray-500 text-xs uppercase tracking-wider mb-1">Practice</p>
              <div className="flex flex-col gap-1 pl-2">
                <NavLink to="/lessons" className={navLinkClass} onClick={() => setMobileOpen(false)}>Lessons</NavLink>
                <NavLink to="/lessons/writing" className={navLinkClass} onClick={() => setMobileOpen(false)}>Writing</NavLink>
                <NavLink to="/flashcards" className={navLinkClass} onClick={() => setMobileOpen(false)}>Flash Cards</NavLink>
              </div>
            </div>
            <div>
              <p className="text-gray-500 text-xs uppercase tracking-wider mb-1">Path</p>
              <div className="flex flex-col gap-1 pl-2">
                <NavLink to="/path" className={navLinkClass} onClick={() => setMobileOpen(false)}>Learning Path</NavLink>
              </div>
            </div>
            {isAuthenticated && (
              <NavLink to="/settings" className={navLinkClass} onClick={() => setMobileOpen(false)}>Settings</NavLink>
            )}
            {isAdmin && (
              <NavLink to="/admin" className={navLinkClass} onClick={() => setMobileOpen(false)}>Admin</NavLink>
            )}
          </nav>
        )}
      </div>
    </header>
  );
};

export default Navbar;
