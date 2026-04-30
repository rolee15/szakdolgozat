import Footer from "./Footer";
import Navbar from "./Navbar";
import { Outlet } from "react-router-dom";

const Layout = () => (
  <div className="min-h-screen flex flex-col">
    <Navbar />
    <main className="flex-grow flex flex-col">
        <div className="min-w-96 max-w-7xl w-full mx-auto px-4 sm:px-6 lg:px-8 py-8 flex-grow flex flex-col">
          <Outlet />
        </div>
      </main>
    <Footer />
  </div>
);

export default Layout;
