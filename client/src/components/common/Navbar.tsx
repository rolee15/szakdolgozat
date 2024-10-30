import { Link, NavLink } from 'react-router-dom';
import viteLogo from '/assets/vite.svg';

const Navbar = () => (
    <nav className="flex items-center justify-between p-4 bg-white shadow-sm">
      <div className="flex items-center gap-6">
        <Link to="/" className="flex items-center">
          <img src={viteLogo} className="h-8 w-8" alt="Vite logo" />
          <span className="ml-2 text-xl font-semibold">
            Kanji <span className="text-blue-600">Ka</span>
          </span>
        </Link>
        <div className="flex gap-4">
          <NavLink
            to="/hiragana"
            className={({ isActive }) =>
              `transition-colors ${isActive ? 'text-blue-600' : 'text-gray-600 hover:text-gray-900'}`
            }
          >
            Hiragana
          </NavLink>
          <NavLink
            to="/katakana"
            className={({ isActive }) =>
              `transition-colors ${isActive ? 'text-blue-600' : 'text-gray-600 hover:text-gray-900'}`
            }
          >
            Katakana
          </NavLink>
        </div>
      </div>
      <div className="flex items-center gap-4">
        <button className="text-gray-600 hover:text-gray-900">Sign in</button>
        <button className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors">
          Register
        </button>
      </div>
    </nav>
  );

export default Navbar;
