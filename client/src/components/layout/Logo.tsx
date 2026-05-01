import { Link } from "react-router-dom";
import kanjikaLogo from "/assets/kanjika-logo.svg";

const Logo = () => {
  return (
    <Link to="/" className="flex items-center pr-8">
      <img src={kanjikaLogo} className="h-8 w-8" alt="Kanjika logo" />
      <span className="ml-2 text-xl font-semibold">
        Kanjika
      </span>
    </Link>
  );
};

export default Logo;