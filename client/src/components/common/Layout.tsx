import Footer from "./Footer";
import Navbar from "./Navbar";
import { Outlet } from "react-router-dom";

const Layout = () => (
    <div className="min-h-screen flex flex-col">
      <Navbar />
      <main className="flex-grow container mx-auto px-4">
        <Outlet />
      </main>
      <Footer />
    </div>
  );

export default Layout;
