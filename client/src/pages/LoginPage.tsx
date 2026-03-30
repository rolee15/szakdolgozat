import { useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "@/context/AuthContext";

const LoginPage = () => {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError(null);
    setIsSubmitting(true);
    try {
      await login(email, password);
      navigate("/lessons", { replace: true });
    } catch (err) {
      setError(err instanceof Error ? err.message : "Login failed");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="mx-auto my-24 max-w-md p-4">
      <form onSubmit={handleSubmit}>
        <div className="flex flex-col gap-y-2 mb-4">
          <label htmlFor="email" className="text-xl">
            Email
          </label>
          <input
            type="email"
            id="email"
            name="email"
            className="w-80 p-2 text-xl"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>
        <div className="flex flex-col gap-y-2">
          <label htmlFor="password" className="text-xl">
            Password
          </label>
          <input
            type="password"
            id="password"
            name="password"
            className="w-80 p-2 text-xl"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <div className="mt-1">
          <NavLink to="/forgot-password" className="text-blue-500">
            Forgot password?
          </NavLink>
        </div>
        {error && (
          <p className="mt-3 text-red-600" role="alert">
            {error}
          </p>
        )}
        <button type="submit" className="text-xl mt-8" disabled={isSubmitting}>
          {isSubmitting ? "Logging in..." : "Login"}
        </button>
      </form>
    </div>
  );
};

export default LoginPage;
