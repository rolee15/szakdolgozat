import api from "@/services/userService";
import { FormEvent, useState } from "react";

const ForgotPasswordPage = () => {
  const [email, setEmail] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      await api.forgotPassword(email);
      setLoading(false);
    } catch (error) {
      setError(error instanceof Error ? error.message : "An error occurred");
      setLoading(false);
    }
  };

  return (
    <div className="mx-auto my-24 max-w-md p-4">
      <form onSubmit={handleSubmit}>
        <div className="flex flex-col gap-y-2">
          <label htmlFor="email" className="text-xl">
            Email
          </label>
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} className="w-80 p-2 text-xl" />
        </div>
        <button type="submit" disabled={loading} className="text-xl mt-8">
          {loading ? "Loading..." : "Reset password"}
        </button>
        {error && <p>{error}</p>}
      </form>
    </div>
  );
};

export default ForgotPasswordPage;
