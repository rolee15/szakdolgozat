import { Link } from "react-router-dom";

const ErrorPage = () => (
  <div className="container mx-auto p-6 text-center">
    <h1 className="text-3xl font-bold mb-4">Page Not Found</h1>
    <p className="mb-4">Sorry, the page you're looking for doesn't exist.</p>
    <Link to="/" className="text-blue-600 hover:text-blue-800">
      Return to Home
    </Link>
  </div>
);

export default ErrorPage;