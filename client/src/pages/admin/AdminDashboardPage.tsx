import { NavLink } from "react-router-dom";

const AdminDashboardPage = () => {
  return (
    <div className="mx-auto my-12 max-w-2xl p-4">
      <h1 className="text-3xl font-bold mb-8">Admin Dashboard</h1>
      <div className="grid gap-4">
        <NavLink
          to="/admin/users"
          className="block p-6 border rounded-lg hover:bg-gray-50 transition"
        >
          <h2 className="text-xl font-semibold mb-2">User Management</h2>
          <p className="text-gray-600">
            View, search, and manage user accounts.
          </p>
        </NavLink>
      </div>
    </div>
  );
};

export default AdminDashboardPage;
