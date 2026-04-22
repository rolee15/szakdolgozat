import { useState, useRef, useEffect } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import Logo from "./Logo";
import { useAuth } from "@/context/AuthContext";

const navLinkClass = ({ isActive }: { isActive: boolean }) =>
  isActive ? 'text-indigo-400' : 'text-gray-300 hover:text-white';

type DropdownName = 'study' | 'practice' | null;

const Navbar = () => {
  const { isAuthenticated, username, isAdmin, logout } = useAuth();
  const navigate = useNavigate();
  const [mobileOpen, setMobileOpen] = useState(false);
  const [openDropdown, setOpenDropdown] = useState<DropdownName>(null);
  const navRef = useRef<HTMLDivElement>(null);

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  const toggleDropdown = (name: DropdownName) => {
    setOpenDropdown((prev) => (prev === name ? null : name));
  };

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (navRef.current && !navRef.current.contains(event.target as Node)) {
        setOpenDropdown(null);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const dropdownPanelClass =
    'absolute top-full mt-1 left-0 z-50 bg-gray-900 border border-gray-700 rounded-md shadow-lg py-1 min-w-max';

  const dropdownLinkClass = ({ isActive }: { isActive: boolean }) =>
    `block px-4 py-2 text-sm ${isActive ? 'text-indigo-400' : 'text-gray-300 hover:text-white'}`;

  return (
    <header className="bg-black shadow-sm">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8" ref={navRef}>
        <div className="flex justify-between items-center h-16">
          <Logo />

          {/* Desktop nav */}
          <div className="hidden md:flex items-center space-x-6 flex-1 ml-8">
            <nav className="flex items-center gap-4">

              {/* Study dropdown */}
              <div className="relative">
                <button
                  onClick={() => toggleDropdown('study')}
                  className="text-white flex items-center gap-1"
                  aria-expanded={openDropdown === 'study'}
                  aria-haspopup="true"
                >
                  Study ▾
                </button>
                {openDropdown === 'study' && (
                  <div className={dropdownPanelClass} role="menu">
                    <NavLink to="/hiragana" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Hiragana</NavLink>
                    <NavLink to="/katakana" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Katakana</NavLink>
                    <NavLink to="/kanji" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Kanji</NavLink>
                    <NavLink to="/grammar" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Grammar</NavLink>
                    <NavLink to="/reading" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Reading</NavLink>
                  </div>
                )}
              </div>

              {/* Practice dropdown */}
              <div className="relative">
                <button
                  onClick={() => toggleDropdown('practice')}
                  className="text-white flex items-center gap-1"
                  aria-expanded={openDropdown === 'practice'}
                  aria-haspopup="true"
                >
                  Practice ▾
                </button>
                {openDropdown === 'practice' && (
                  <div className={dropdownPanelClass} role="menu">
                    <NavLink to="/lessons" end className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Lessons</NavLink>
                    <NavLink to="/flashcards" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Flash Cards</NavLink>
                  </div>
                )}
              </div>

              {/* Path — plain link */}
              <NavLink to="/path" className={navLinkClass}>Learning Path</NavLink>

              {/* Admin — conditionally shown */}
              {isAdmin && (
                <NavLink to="/admin" className={navLinkClass}>Admin</NavLink>
              )}
            </nav>

            {/* Auth section */}
            <div className="flex items-center space-x-4 ml-auto">
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

          {/* Mobile: auth + hamburger */}
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

        {/* Mobile dropdown panel */}
        {mobileOpen && (
          <nav className="md:hidden pb-4 flex flex-col gap-4">
            {/* Study section */}
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

            {/* Practice section */}
            <div>
              <p className="text-gray-500 text-xs uppercase tracking-wider mb-1">Practice</p>
              <div className="flex flex-col gap-1 pl-2">
                <NavLink to="/lessons" end className={navLinkClass} onClick={() => setMobileOpen(false)}>Lessons</NavLink>
                <NavLink to="/flashcards" className={navLinkClass} onClick={() => setMobileOpen(false)}>Flash Cards</NavLink>
              </div>
            </div>

            {/* Path */}
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
