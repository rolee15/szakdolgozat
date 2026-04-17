import { FormEvent, useMemo, useState } from "react";
import { NavLink } from "react-router-dom";
import { useAuth } from "@/context/AuthContext";

const RegisterPage = () => {
  const { register } = useAuth();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [touched, setTouched] = useState<{ email?: boolean; password?: boolean; confirmPassword?: boolean }>({});
  const [serverError, setServerError] = useState<string | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const emailError = useMemo(() => {
    if (!touched.email) return "";
    if (!email) return "Email is required";
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) return "Please enter a valid email address";
    return "";
  }, [email, touched.email]);

  const passwordError = useMemo(() => {
    if (!touched.password) return "";
    if (!password) return "Password is required";
    if (password.length < 8) return "Password must be at least 8 characters";
    return "";
  }, [password, touched.password]);

  const confirmPasswordError = useMemo(() => {
    if (!touched.confirmPassword) return "";
    if (!confirmPassword) return "Please confirm your password";
    if (confirmPassword !== password) return "Passwords do not match";
    return "";
  }, [confirmPassword, password, touched.confirmPassword]);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setTouched({ email: true, password: true, confirmPassword: true });
    setServerError(null);

    const currentEmailError = (() => {
      if (!email) return "Email is required";
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(email)) return "Please enter a valid email address";
      return "";
    })();
    const currentPasswordError = (() => {
      if (!password) return "Password is required";
      if (password.length < 8) return "Password must be at least 8 characters";
      return "";
    })();
    const currentConfirmPasswordError = (() => {
      if (!confirmPassword) return "Please confirm your password";
      if (confirmPassword !== password) return "Passwords do not match";
      return "";
    })();

    if (currentEmailError || currentPasswordError || currentConfirmPasswordError) {
      return;
    }

    setIsSubmitting(true);
    try {
      await register(email, password);
      setSuccessMessage("Registration successful. Please check your email to activate your account.");
    } catch (err) {
      setServerError(err instanceof Error ? err.message : "Registration failed");
    } finally {
      setIsSubmitting(false);
    }
  };

  if (successMessage) {
    return (
      <div className="mx-auto my-24 max-w-md p-4">
        <h1 className="text-2xl font-semibold mb-6">Check your email</h1>
        <p className="mb-4">{successMessage}</p>
        <NavLink to="/login" className="text-blue-500">
          Go to login
        </NavLink>
      </div>
    );
  }

  return (
    <div className="mx-auto my-24 max-w-md p-4">
      <h1 className="text-2xl font-semibold mb-6">Create your account</h1>
      <form onSubmit={handleSubmit} noValidate>
        <div className="flex flex-col gap-y-2 mb-4">
          <label htmlFor="email" className="text-xl">
            Email
          </label>
          <input
            type="email"
            id="email"
            name="email"
            className="w-80 p-2 text-xl border"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            onBlur={() => setTouched((t) => ({ ...t, email: true }))}
            aria-invalid={!!emailError}
            aria-describedby="email-error"
          />
          {emailError && (
            <span id="email-error" className="text-red-600">
              {emailError}
            </span>
          )}
        </div>

        <div className="flex flex-col gap-y-2 mb-4">
          <label htmlFor="password" className="text-xl">
            Password
          </label>
          <input
            type="password"
            id="password"
            name="password"
            className="w-80 p-2 text-xl border"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            onBlur={() => setTouched((t) => ({ ...t, password: true }))}
            aria-invalid={!!passwordError}
            aria-describedby="password-error"
          />
          {passwordError && (
            <span id="password-error" className="text-red-600">
              {passwordError}
            </span>
          )}
          <p className="text-sm text-gray-600">Minimum 8 characters.</p>
        </div>

        <div className="flex flex-col gap-y-2 mb-6">
          <label htmlFor="confirmPassword" className="text-xl">
            Confirm password
          </label>
          <input
            type="password"
            id="confirmPassword"
            name="confirmPassword"
            className="w-80 p-2 text-xl border"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            onBlur={() => setTouched((t) => ({ ...t, confirmPassword: true }))}
            aria-invalid={!!confirmPasswordError}
            aria-describedby="confirm-password-error"
          />
          {confirmPasswordError && (
            <span id="confirm-password-error" className="text-red-600">
              {confirmPasswordError}
            </span>
          )}
        </div>

        {serverError && (
          <p className="mb-4 text-red-600" role="alert">
            {serverError}
          </p>
        )}

        <button type="submit" className="text-xl" disabled={isSubmitting}>
          {isSubmitting ? "Registering..." : "Register"}
        </button>

        <div className="mt-4">
          <NavLink to="/login" className="text-blue-500">
            Already have an account? Log in
          </NavLink>
        </div>
      </form>
    </div>
  );
};

export default RegisterPage;
