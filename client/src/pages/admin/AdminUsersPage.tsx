import { useState } from "react";
import { NavLink } from "react-router-dom";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import adminService from "@/services/adminService";

const AdminUsersPage = () => {
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const pageSize = 20;

  const { data, isLoading, error } = useQuery({
    queryKey: ["admin-users", page, search],
    queryFn: () => adminService.getUsers(page, pageSize, search || undefined),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: number) => adminService.deleteUser(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["admin-users"] }),
  });

  const handleSearch = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setPage(1);
  };

  return (
    <div className="mx-auto my-12 max-w-4xl p-4">
      <h1 className="text-3xl font-bold mb-6">User Management</h1>

      <form onSubmit={handleSearch} className="mb-6 flex gap-2">
        <input
          type="text"
          placeholder="Search by username..."
          className="flex-1 p-2 border rounded"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <button type="submit" className="px-4 py-2 bg-black text-white rounded">
          Search
        </button>
      </form>

      {isLoading && <p>Loading...</p>}
      {error && <p className="text-red-600">Failed to load users.</p>}

      {data && (
        <>
          <table className="w-full border-collapse">
            <thead>
              <tr className="border-b">
                <th className="text-left p-2">ID</th>
                <th className="text-left p-2">Username</th>
                <th className="text-left p-2">Role</th>
                <th className="text-left p-2">Proficiencies</th>
                <th className="text-left p-2">Lessons</th>
                <th className="text-left p-2">Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.items.map((user) => (
                <tr key={user.id} className="border-b hover:bg-gray-50">
                  <td className="p-2">{user.id}</td>
                  <td className="p-2">
                    <NavLink
                      to={`/admin/users/${user.id}`}
                      className="text-blue-600 hover:underline"
                    >
                      {user.username}
                    </NavLink>
                  </td>
                  <td className="p-2">{user.role}</td>
                  <td className="p-2">{user.proficiencyCount}</td>
                  <td className="p-2">{user.lessonCompletionCount}</td>
                  <td className="p-2">
                    {user.role !== "Admin" && (
                      <button
                        onClick={() => {
                          if (window.confirm(`Delete user "${user.username}"?`)) {
                            deleteMutation.mutate(user.id);
                          }
                        }}
                        className="text-red-600 hover:underline"
                      >
                        Delete
                      </button>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <div className="mt-4 flex justify-between items-center">
            <span className="text-gray-600">
              Showing {data.items.length} of {data.totalCount} users
            </span>
            <div className="flex gap-2">
              <button
                onClick={() => setPage((p) => Math.max(1, p - 1))}
                disabled={page <= 1}
                className="px-3 py-1 border rounded disabled:opacity-50"
              >
                Previous
              </button>
              <span className="px-3 py-1">Page {page}</span>
              <button
                onClick={() => setPage((p) => p + 1)}
                disabled={!data.hasNextPage}
                className="px-3 py-1 border rounded disabled:opacity-50"
              >
                Next
              </button>
            </div>
          </div>
        </>
      )}
    </div>
  );
};

export default AdminUsersPage;
