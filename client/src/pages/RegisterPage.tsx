import { FormEvent, useMemo, useState } from "react";
import { NavLink } from "react-router-dom";

const RegisterPage = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [touched, setTouched] = useState<{ email?: boolean; password?: boolean; confirmPassword?: boolean }>({});
  const [submittedEmail, setSubmittedEmail] = useState<string | null>(null);

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

  const hasErrors = !!(emailError || passwordError || confirmPasswordError);

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    setTouched({ email: true, password: true, confirmPassword: true });

    // Re-evaluate errors after marking all as touched
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

    // Do not actually send email yet; just show confirmation message
    setSubmittedEmail(email);
  };

  if (submittedEmail) {
    return (
      <div className="mx-auto my-24 max-w-md p-4">
        <h1 className="text-2xl font-semibold mb-4">Check your inbox</h1>
        <p className="text-lg">
          We have sent a confirmation email to <span className="font-medium">{submittedEmail}</span>. Please click the
          link in the email to verify your account.
        </p>
        <div className="mt-6">
          <NavLink to="/login" className="text-blue-500">
            Back to login
          </NavLink>
        </div>
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

        <button type="submit" className="text-xl">
          Register
        </button>
      </form>
    </div>
  );
};

export default RegisterPage;