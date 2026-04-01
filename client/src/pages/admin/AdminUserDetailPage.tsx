import { useParams, useNavigate } from "react-router-dom";
import { useQuery, useMutation } from "@tanstack/react-query";
import adminService from "@/services/adminService";

const AdminUserDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();

  const { data: user, isLoading, error } = useQuery({
    queryKey: ["admin-user", id],
    queryFn: () => adminService.getUserById(Number(id)),
    enabled: !!id,
  });

  const deleteMutation = useMutation({
    mutationFn: () => adminService.deleteUser(Number(id)),
    onSuccess: () => navigate("/admin/users"),
  });

  if (isLoading) return <p className="p-4">Loading...</p>;
  if (error || !user) return <p className="p-4 text-red-600">User not found.</p>;

  return (
    <div className="mx-auto my-12 max-w-3xl p-4">
      <h1 className="text-3xl font-bold mb-6">User Details</h1>

      <div className="mb-6 space-y-2">
        <p><strong>ID:</strong> {user.id}</p>
        <p><strong>Username:</strong> {user.username}</p>
        <p><strong>Role:</strong> {user.role}</p>
        <p><strong>Must Change Password:</strong> {user.mustChangePassword ? "Yes" : "No"}</p>
        <p><strong>Proficiencies:</strong> {user.proficiencyCount}</p>
        <p><strong>Lesson Completions:</strong> {user.lessonCompletionCount}</p>
      </div>

      {user.role !== "Admin" && (
        <button
          onClick={() => {
            if (window.confirm(`Delete user "${user.username}"?`)) {
              deleteMutation.mutate();
            }
          }}
          className="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700"
        >
          Delete User
        </button>
      )}

      {user.proficiencies.length > 0 && (
        <div className="mt-8">
          <h2 className="text-xl font-semibold mb-3">Proficiencies</h2>
          <table className="w-full border-collapse">
            <thead>
              <tr className="border-b">
                <th className="text-left p-2">Character</th>
                <th className="text-left p-2">Learned At</th>
                <th className="text-left p-2">Last Practiced</th>
              </tr>
            </thead>
            <tbody>
              {user.proficiencies.map((p) => (
                <tr key={p.characterId} className="border-b">
                  <td className="p-2 text-2xl">{p.characterSymbol}</td>
                  <td className="p-2">{new Date(p.learnedAt).toLocaleDateString()}</td>
                  <td className="p-2">{new Date(p.lastPracticed).toLocaleDateString()}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {user.lessonCompletions.length > 0 && (
        <div className="mt-8">
          <h2 className="text-xl font-semibold mb-3">Lesson Completions</h2>
          <table className="w-full border-collapse">
            <thead>
              <tr className="border-b">
                <th className="text-left p-2">Character</th>
                <th className="text-left p-2">Completed</th>
              </tr>
            </thead>
            <tbody>
              {user.lessonCompletions.map((lc) => (
                <tr key={lc.characterId} className="border-b">
                  <td className="p-2 text-2xl">{lc.characterSymbol}</td>
                  <td className="p-2">{new Date(lc.completionDate).toLocaleDateString()}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default AdminUserDetailPage;
