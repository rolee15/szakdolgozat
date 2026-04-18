import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "@/context/AuthContext";
import userService from "@/services/userService";

const ChangePasswordPage = () => {
  const { mustChangePassword, clearMustChangePassword } = useAuth();
  const navigate = useNavigate();
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError(null);

    if (newPassword.length < 8) {
      setError("New password must be at least 8 characters");
      return;
    }
    if (newPassword !== confirmPassword) {
      setError("Passwords do not match");
      return;
    }

    setIsSubmitting(true);
    try {
      await userService.changePassword(currentPassword, newPassword);
      clearMustChangePassword();
      navigate("/lessons", { replace: true });
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to change password");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="mx-auto my-24 max-w-md p-4">
      <h1 className="text-2xl font-bold mb-4">Change Password</h1>
      {mustChangePassword && (
        <p className="mb-4 text-amber-600">
          You must change your password before continuing.
        </p>
      )}
      <form onSubmit={handleSubmit}>
        <div className="flex flex-col gap-y-2 mb-4">
          <label htmlFor="currentPassword" className="text-xl">
            Current Password
          </label>
          <input
            type="password"
            id="currentPassword"
            className="w-80 p-2 text-xl"
            value={currentPassword}
            onChange={(e) => setCurrentPassword(e.target.value)}
          />
        </div>
        <div className="flex flex-col gap-y-2 mb-4">
          <label htmlFor="newPassword" className="text-xl">
            New Password
          </label>
          <input
            type="password"
            id="newPassword"
            className="w-80 p-2 text-xl"
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
          />
        </div>
        <div className="flex flex-col gap-y-2">
          <label htmlFor="confirmPassword" className="text-xl">
            Confirm New Password
          </label>
          <input
            type="password"
            id="confirmPassword"
            className="w-80 p-2 text-xl"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
          />
        </div>
        {error && (
          <p className="mt-3 text-red-600" role="alert">
            {error}
          </p>
        )}
        <button type="submit" className="text-xl mt-8" disabled={isSubmitting}>
          {isSubmitting ? "Changing..." : "Change Password"}
        </button>
      </form>
    </div>
  );
};

export default ChangePasswordPage;
