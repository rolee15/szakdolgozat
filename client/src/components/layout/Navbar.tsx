import { useState, useRef, useEffect } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import Logo from "./Logo";
import { useAuth } from "@/context/AuthContext";

const navLinkClass = ({ isActive }: { isActive: boolean }) =>
  isActive ? 'text-indigo-400 text-sm' : 'text-gray-300 hover:text-white text-sm';

type DropdownName = 'lessons' | 'reviews' | 'profile' | null;

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

  const profileDropdownPanelClass =
    'absolute top-full mt-1 right-0 z-50 bg-gray-900 border border-gray-700 rounded-md shadow-lg py-1 min-w-max';

  const dropdownLinkClass = ({ isActive }: { isActive: boolean }) =>
    `block px-4 py-2 text-sm ${isActive ? 'text-indigo-400' : 'text-gray-300 hover:text-white'}`;

  const avatarLetter = username ? username[0].toUpperCase() : '?';

  return (
    <header className="bg-black shadow-sm">
      <div className="max-w-screen-2xl mx-auto px-4 sm:px-6 lg:px-8" ref={navRef}>
        <div className="flex items-center h-16">
          <Logo />

          {/* Desktop nav — right-aligned */}
          <div className="hidden md:flex items-center gap-4 ml-auto">
            {isAuthenticated && (
              <>
                {/* Lessons dropdown */}
                <div className="relative">
                  <button
                    onClick={() => toggleDropdown('lessons')}
                    className="text-white text-sm flex items-center gap-1"
                    aria-expanded={openDropdown === 'lessons'}
                    aria-haspopup="true"
                  >
                    Lessons ▾
                  </button>
                  {openDropdown === 'lessons' && (
                    <div className={dropdownPanelClass} role="menu">
                      <NavLink to="/hiragana" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Hiragana</NavLink>
                      <NavLink to="/katakana" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Katakana</NavLink>
                      <NavLink to="/kanji" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Kanji</NavLink>
                      <NavLink to="/grammar" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Grammar</NavLink>
                      <NavLink to="/reading" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Reading</NavLink>
                    </div>
                  )}
                </div>

                {/* Reviews dropdown */}
                <div className="relative">
                  <button
                    onClick={() => toggleDropdown('reviews')}
                    className="text-white text-sm flex items-center gap-1"
                    aria-expanded={openDropdown === 'reviews'}
                    aria-haspopup="true"
                  >
                    Reviews ▾
                  </button>
                  {openDropdown === 'reviews' && (
                    <div className={dropdownPanelClass} role="menu">
                      <NavLink to="/flashcards" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Flash Cards</NavLink>
                      <NavLink to="/lessons/review" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Review</NavLink>
                      <NavLink to="/lessons/writing" className={dropdownLinkClass} onClick={() => setOpenDropdown(null)}>Writing</NavLink>
                    </div>
                  )}
                </div>

                {/* Learning Path */}
                <NavLink to="/path" className={navLinkClass}>Learning Path</NavLink>

                {/* Admin */}
                {isAdmin && (
                  <NavLink to="/admin" className={navLinkClass}>Admin</NavLink>
                )}

                {/* Profile avatar dropdown */}
                <div className="relative">
                  <button
                    onClick={() => toggleDropdown('profile')}
                    aria-label="Profile menu"
                    aria-expanded={openDropdown === 'profile'}
                    aria-haspopup="true"
                    className="w-8 h-8 rounded-full bg-gray-600 text-white text-sm font-medium flex items-center justify-center hover:bg-gray-500 focus:outline-none"
                  >
                    {avatarLetter}
                  </button>
                  {openDropdown === 'profile' && (
                    <div className={profileDropdownPanelClass} role="menu">
                      <NavLink
                        to="/settings"
                        className={dropdownLinkClass}
                        onClick={() => setOpenDropdown(null)}
                      >
                        Settings
                      </NavLink>
                      <button
                        onClick={handleLogout}
                        className="block w-full text-left px-4 py-2 text-sm text-gray-300 hover:text-white"
                      >
                        Logout
                      </button>
                    </div>
                  )}
                </div>
              </>
            )}

            {!isAuthenticated && (
              <>
                <NavLink to="/login" className="text-white text-sm hover:text-gray-300">Login</NavLink>
                <NavLink to="/register" className="text-white text-sm hover:text-gray-300">Register</NavLink>
              </>
            )}
          </div>

          {/* Mobile: auth + hamburger */}
          <div className="flex md:hidden items-center gap-3 ml-auto">
            {isAuthenticated ? (
              <button
                onClick={() => toggleDropdown('profile')}
                aria-label="Profile menu"
                aria-expanded={openDropdown === 'profile'}
                aria-haspopup="true"
                className="w-8 h-8 rounded-full bg-gray-600 text-white text-sm font-medium flex items-center justify-center hover:bg-gray-500 focus:outline-none"
              >
                {avatarLetter}
              </button>
            ) : (
              <>
                <NavLink to="/login" className="text-white text-sm hover:text-gray-300">Login</NavLink>
                <NavLink to="/register" className="text-white text-sm hover:text-gray-300">Register</NavLink>
              </>
            )}
            {isAuthenticated && (
              <button
                aria-label={mobileOpen ? 'Close menu' : 'Open menu'}
                onClick={() => setMobileOpen((prev) => !prev)}
                className="text-white text-2xl focus:outline-none"
              >
                {mobileOpen ? '✕' : '☰'}
              </button>
            )}
          </div>
        </div>

        {/* Mobile dropdown panel */}
        {mobileOpen && isAuthenticated && (
          <nav className="md:hidden pb-4 flex flex-col gap-4">
            {/* Lessons section */}
            <div>
              <p className="text-gray-500 text-xs uppercase tracking-wider mb-1">Lessons</p>
              <div className="flex flex-col gap-1 pl-2">
                <NavLink to="/hiragana" className={navLinkClass} onClick={() => setMobileOpen(false)}>Hiragana</NavLink>
                <NavLink to="/katakana" className={navLinkClass} onClick={() => setMobileOpen(false)}>Katakana</NavLink>
                <NavLink to="/kanji" className={navLinkClass} onClick={() => setMobileOpen(false)}>Kanji</NavLink>
                <NavLink to="/grammar" className={navLinkClass} onClick={() => setMobileOpen(false)}>Grammar</NavLink>
                <NavLink to="/reading" className={navLinkClass} onClick={() => setMobileOpen(false)}>Reading</NavLink>
              </div>
            </div>

            {/* Reviews section */}
            <div>
              <p className="text-gray-500 text-xs uppercase tracking-wider mb-1">Reviews</p>
              <div className="flex flex-col gap-1 pl-2">
                <NavLink to="/flashcards" className={navLinkClass} onClick={() => setMobileOpen(false)}>Flash Cards</NavLink>
                <NavLink to="/lessons/review" className={navLinkClass} onClick={() => setMobileOpen(false)}>Review</NavLink>
                <NavLink to="/lessons/writing" className={navLinkClass} onClick={() => setMobileOpen(false)}>Writing</NavLink>
              </div>
            </div>

            {/* Path */}
            <div>
              <p className="text-gray-500 text-xs uppercase tracking-wider mb-1">Path</p>
              <div className="flex flex-col gap-1 pl-2">
                <NavLink to="/path" className={navLinkClass} onClick={() => setMobileOpen(false)}>Learning Path</NavLink>
              </div>
            </div>

            {isAdmin && (
              <NavLink to="/admin" className={navLinkClass} onClick={() => setMobileOpen(false)}>Admin</NavLink>
            )}

            <NavLink to="/settings" className={navLinkClass} onClick={() => setMobileOpen(false)}>Settings</NavLink>

            <button
              onClick={handleLogout}
              className="text-left text-sm text-gray-300 hover:text-white"
            >
              Logout
            </button>
          </nav>
        )}
      </div>
    </header>
  );
};

export default Navbar;
