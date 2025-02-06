import { Link } from "react-router-dom";
import viteLogo from "/assets/vite.svg";

const Logo = () => {
  return (
    <Link to="/" className="flex items-center pr-8">
      <img src={viteLogo} className="h-8 w-8" alt="Vite logo" />
      <span className="ml-2 text-xl font-semibold">
        KanjiKa
      </span>
    </Link>
  );
};

export default Logo;