import { useEffect, useRef } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { useMutation } from "@tanstack/react-query";
import userService from "@/services/userService";

const ActivatePage = () => {
  const [searchParams] = useSearchParams();
  const token = searchParams.get("token");
  const hasTriggered = useRef(false);

  const { mutate, isPending, isSuccess, isError, data, error } = useMutation({
    mutationFn: (t: string) => userService.activateAccount(t),
  });

  useEffect(() => {
    if (token && !hasTriggered.current) {
      hasTriggered.current = true;
      mutate(token);
    }
  }, [token, mutate]);

  if (!token) {
    return (
      <div className="mx-auto my-24 max-w-md p-4">
        <h1 className="text-2xl font-semibold mb-4">Activation failed</h1>
        <p className="mb-4 text-red-600">Invalid activation link.</p>
        <Link to="/login" className="text-blue-500">
          Go to login
        </Link>
      </div>
    );
  }

  if (isPending) {
    return (
      <div className="mx-auto my-24 max-w-md p-4">
        <p>Activating your account...</p>
      </div>
    );
  }

  if (isSuccess) {
    return (
      <div className="mx-auto my-24 max-w-md p-4">
        <h1 className="text-2xl font-semibold mb-4">Account activated</h1>
        <p className="mb-4">{data?.message ?? "Account activated. You can now log in."}</p>
        <Link to="/login" className="text-blue-500">
          Go to login
        </Link>
      </div>
    );
  }

  if (isError) {
    return (
      <div className="mx-auto my-24 max-w-md p-4">
        <h1 className="text-2xl font-semibold mb-4">Activation failed</h1>
        <p className="mb-4 text-red-600">
          {error instanceof Error ? error.message : "Activation failed. Please try again."}
        </p>
        <Link to="/login" className="text-blue-500">
          Go to login
        </Link>
      </div>
    );
  }

  return null;
};

export default ActivatePage;
