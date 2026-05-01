import { useState, useRef, useEffect } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import Logo from "./Logo";
import MenuItem from "./MenuItem";
import ThemeToggle from "@/components/common/ThemeToggle";
import { useAuth } from "@/context/useAuth";

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
    'absolute top-full mt-1 left-0 z-50 bg-surface border border-primary/40 rounded-md shadow-lg py-1 min-w-max';

  const profileDropdownPanelClass =
    'absolute top-full mt-1 right-0 z-50 bg-surface border border-primary/40 rounded-md shadow-lg py-1 min-w-max';

  const avatarLetter = username ? username[0].toUpperCase() : '?';

  return (
    <header className="w-screen bg-bar shadow-sm">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8" ref={navRef}>
        <div className="flex items-center h-16">
          <Logo />

          <div className="hidden md:flex items-center gap-4 ml-auto">
            <ThemeToggle />

            {isAuthenticated && (
              <>
                <div className="relative">
                  <button
                    onClick={() => toggleDropdown('lessons')}
                    className="text-on-bar text-sm flex items-center gap-1 cursor-pointer"
                    aria-expanded={openDropdown === 'lessons'}
                    aria-haspopup="true"
                  >
                    Lessons ▾
                  </button>
                  {openDropdown === 'lessons' && (
                    <div className={dropdownPanelClass} role="menu">
                      <MenuItem to="/hiragana" variant="dropdown" onClick={() => setOpenDropdown(null)}>Hiragana</MenuItem>
                      <MenuItem to="/katakana" variant="dropdown" onClick={() => setOpenDropdown(null)}>Katakana</MenuItem>
                      <MenuItem to="/kanji" variant="dropdown" onClick={() => setOpenDropdown(null)}>Kanji</MenuItem>
                      <MenuItem to="/grammar" variant="dropdown" onClick={() => setOpenDropdown(null)}>Grammar</MenuItem>
                      <MenuItem to="/reading" variant="dropdown" onClick={() => setOpenDropdown(null)}>Reading</MenuItem>
                    </div>
                  )}
                </div>

                <div className="relative">
                  <button
                    onClick={() => toggleDropdown('reviews')}
                    className="text-on-bar text-sm flex items-center gap-1 cursor-pointer"
                    aria-expanded={openDropdown === 'reviews'}
                    aria-haspopup="true"
                  >
                    Reviews ▾
                  </button>
                  {openDropdown === 'reviews' && (
                    <div className={dropdownPanelClass} role="menu">
                      <MenuItem to="/flashcards" variant="dropdown" onClick={() => setOpenDropdown(null)}>Flash Cards</MenuItem>
                      <MenuItem to="/lessons/review" variant="dropdown" onClick={() => setOpenDropdown(null)}>Review</MenuItem>
                      <MenuItem to="/lessons/writing" variant="dropdown" onClick={() => setOpenDropdown(null)}>Writing</MenuItem>
                    </div>
                  )}
                </div>

                <MenuItem to="/path">Learning Path</MenuItem>

                {isAdmin && (
                  <MenuItem to="/admin">Admin</MenuItem>
                )}

                <div className="relative">
                  <button
                    onClick={() => toggleDropdown('profile')}
                    aria-label="Profile menu"
                    aria-expanded={openDropdown === 'profile'}
                    aria-haspopup="true"
                    className="w-8 h-8 rounded-full bg-secondary text-on-primary text-sm font-medium flex items-center justify-center hover:bg-secondary-hover focus:outline-none cursor-pointer transition-colors"
                  >
                    {avatarLetter}
                  </button>
                  {openDropdown === 'profile' && (
                    <div className={profileDropdownPanelClass} role="menu">
                      <MenuItem to="/settings" variant="dropdown" onClick={() => setOpenDropdown(null)}>Settings</MenuItem>
                      <button
                        onClick={handleLogout}
                        className="block w-full text-left px-4 py-2 text-sm text-on-bar hover:bg-primary/20 bg-transparent border-0 rounded-none cursor-pointer transition-colors"
                      >
                        Log out
                      </button>
                    </div>
                  )}
                </div>
              </>
            )}

            {!isAuthenticated && (
              <>
                <NavLink to="/login" className="text-on-bar text-sm hover:text-primary transition-colors">Login</NavLink>
                <NavLink to="/register" className="text-on-bar text-sm hover:text-primary transition-colors">Register</NavLink>
              </>
            )}
          </div>

          <div className="flex md:hidden items-center gap-3 ml-auto">
            <ThemeToggle />
            {!isAuthenticated && (
              <>
                <NavLink to="/login" className="text-on-bar text-sm hover:text-primary transition-colors">Login</NavLink>
                <NavLink to="/register" className="text-on-bar text-sm hover:text-primary transition-colors">Register</NavLink>
              </>
            )}
            {isAuthenticated && (
              <button
                aria-label={mobileOpen ? 'Close menu' : 'Open menu'}
                onClick={() => setMobileOpen((prev) => !prev)}
                className="text-on-bar text-2xl focus:outline-none cursor-pointer"
              >
                {mobileOpen ? '✕' : '☰'}
              </button>
            )}
          </div>
        </div>

        {mobileOpen && isAuthenticated && (
          <nav className="md:hidden pb-4 flex flex-col gap-4">
            <div>
              <p className="text-muted text-xs uppercase tracking-wider mb-1">Lessons</p>
              <div className="flex flex-col gap-1 pl-2">
                <MenuItem to="/hiragana" onClick={() => setMobileOpen(false)}>Hiragana</MenuItem>
                <MenuItem to="/katakana" onClick={() => setMobileOpen(false)}>Katakana</MenuItem>
                <MenuItem to="/kanji" onClick={() => setMobileOpen(false)}>Kanji</MenuItem>
                <MenuItem to="/grammar" onClick={() => setMobileOpen(false)}>Grammar</MenuItem>
                <MenuItem to="/reading" onClick={() => setMobileOpen(false)}>Reading</MenuItem>
              </div>
            </div>

            <div>
              <p className="text-muted text-xs uppercase tracking-wider mb-1">Reviews</p>
              <div className="flex flex-col gap-1 pl-2">
                <MenuItem to="/flashcards" onClick={() => setMobileOpen(false)}>Flash Cards</MenuItem>
                <MenuItem to="/lessons/review" onClick={() => setMobileOpen(false)}>Review</MenuItem>
                <MenuItem to="/lessons/writing" onClick={() => setMobileOpen(false)}>Writing</MenuItem>
              </div>
            </div>

            <div>
              <p className="text-muted text-xs uppercase tracking-wider mb-1">Path</p>
              <div className="flex flex-col gap-1 pl-2">
                <MenuItem to="/path" onClick={() => setMobileOpen(false)}>Learning Path</MenuItem>
              </div>
            </div>

            {isAdmin && (
              <MenuItem to="/admin" onClick={() => setMobileOpen(false)}>Admin</MenuItem>
            )}

            <div>
              <p className="text-muted text-xs uppercase tracking-wider mb-1">{username}</p>
              <div className="flex flex-col gap-1 pl-2">
                <MenuItem to="/settings" onClick={() => setMobileOpen(false)}>Settings</MenuItem>
                <button
                  onClick={handleLogout}
                  className="text-left text-on-bar/80 hover:text-on-bar text-sm bg-transparent border-0 rounded-none p-0 cursor-pointer transition-colors"
                >
                  Log out
                </button>
              </div>
            </div>
          </nav>
        )}
      </div>
    </header>
  );
};

export default Navbar;
