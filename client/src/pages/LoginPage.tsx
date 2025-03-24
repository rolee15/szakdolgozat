import api from "@/services/userService";
import { useState } from "react";
import { NavLink } from "react-router-dom";

type PropType = {
  setToken: (token: string) => void;
};
const LoginPage = (props: PropType) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const dto = await api.login(email, password);
    props.setToken(dto.token);
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
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <div>
          <NavLink to="/forgot-password" className="text-blue-500">
            Forgot password?
          </NavLink>
        </div>
        <button type="submit" className="text-xl mt-8">
          Login
        </button>
      </form>
    </div>
  );
};

export default LoginPage;
