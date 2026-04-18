import { useState } from "react";
import { Link } from "react-router-dom";
import { useForm } from "react-hook-form";
import userService from "@/services/userService";

type EmailFormValues = {
  email: string;
};

type ResetFormValues = {
  code: string;
  newPassword: string;
  confirmPassword: string;
};

const ForgotPasswordPage = () => {
  const [step, setStep] = useState<"email" | "reset" | "done">("email");
  const [submittedEmail, setSubmittedEmail] = useState("");
  const [error, setError] = useState<string | null>(null);

  const emailForm = useForm<EmailFormValues>();
  const resetForm = useForm<ResetFormValues>();

  const handleEmailSubmit = async (data: EmailFormValues) => {
    setError(null);
    try {
      await userService.forgotPassword(data.email);
      setSubmittedEmail(data.email);
      setStep("reset");
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to send reset code");
    }
  };

  const handleResetSubmit = async (data: ResetFormValues) => {
    setError(null);
    if (data.newPassword !== data.confirmPassword) {
      resetForm.setError("confirmPassword", { message: "Passwords do not match" });
      return;
    }
    try {
      await userService.confirmResetPassword(submittedEmail, data.code, data.newPassword);
      setStep("done");
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to reset password");
    }
  };

  if (step === "done") {
    return (
      <div className="mx-auto my-24 max-w-md p-4">
        <p className="text-xl mb-4">パスワードがリセットされました</p>
        <Link to="/login" className="text-xl underline">
          ログインに戻る
        </Link>
      </div>
    );
  }

  if (step === "reset") {
    return (
      <div className="mx-auto my-24 max-w-md p-4">
        <p className="text-xl mb-6">コードと新しいパスワードを入力してください</p>
        <form onSubmit={resetForm.handleSubmit(handleResetSubmit)}>
          <div className="flex flex-col gap-y-2 mb-4">
            <label htmlFor="code" className="text-xl">
              Reset Code
            </label>
            <input
              id="code"
              type="text"
              className="w-80 p-2 text-xl"
              {...resetForm.register("code", { required: "Code is required" })}
            />
            {resetForm.formState.errors.code && (
              <p className="text-red-600" role="alert">{resetForm.formState.errors.code.message}</p>
            )}
          </div>
          <div className="flex flex-col gap-y-2 mb-4">
            <label htmlFor="newPassword" className="text-xl">
              New Password
            </label>
            <input
              id="newPassword"
              type="password"
              className="w-80 p-2 text-xl"
              {...resetForm.register("newPassword", { required: "Password is required" })}
            />
            {resetForm.formState.errors.newPassword && (
              <p className="text-red-600" role="alert">{resetForm.formState.errors.newPassword.message}</p>
            )}
          </div>
          <div className="flex flex-col gap-y-2">
            <label htmlFor="confirmPassword" className="text-xl">
              Confirm Password
            </label>
            <input
              id="confirmPassword"
              type="password"
              className="w-80 p-2 text-xl"
              {...resetForm.register("confirmPassword", { required: "Please confirm your password" })}
            />
            {resetForm.formState.errors.confirmPassword && (
              <p className="text-red-600" role="alert">{resetForm.formState.errors.confirmPassword.message}</p>
            )}
          </div>
          {error && (
            <p className="mt-3 text-red-600" role="alert">
              {error}
            </p>
          )}
          <button
            type="submit"
            disabled={resetForm.formState.isSubmitting}
            className="text-xl mt-8"
          >
            {resetForm.formState.isSubmitting ? "Resetting..." : "Reset Password"}
          </button>
        </form>
      </div>
    );
  }

  return (
    <div className="mx-auto my-24 max-w-md p-4">
      <form onSubmit={emailForm.handleSubmit(handleEmailSubmit)}>
        <div className="flex flex-col gap-y-2">
          <label htmlFor="email" className="text-xl">
            Email
          </label>
          <input
            id="email"
            type="email"
            className="w-80 p-2 text-xl"
            {...emailForm.register("email", { required: "Email is required" })}
          />
          {emailForm.formState.errors.email && (
            <p className="text-red-600" role="alert">{emailForm.formState.errors.email.message}</p>
          )}
        </div>
        {error && (
          <p className="mt-3 text-red-600" role="alert">
            {error}
          </p>
        )}
        <button
          type="submit"
          disabled={emailForm.formState.isSubmitting}
          className="text-xl mt-8"
        >
          {emailForm.formState.isSubmitting ? "Sending..." : "Send reset code"}
        </button>
      </form>
    </div>
  );
};

export default ForgotPasswordPage;
