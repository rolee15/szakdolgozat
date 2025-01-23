import { Link } from "react-router-dom";
import viteLogo from "/assets/vite.svg";

const Logo = () => {
  return (
    <Link to="/" className="flex items-center">
      <img src={viteLogo} className="h-8 w-8" alt="Vite logo" />
      <span className="ml-2 text-xl font-semibold">
        Kanji <span className="text-blue-600">Ka</span>
      </span>
    </Link>
  );
};

export default Logo;