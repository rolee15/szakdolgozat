import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import Layout from "@components/layout/Layout";
import HomePage from "@pages/HomePage";
import ErrorPage from "@pages/ErrorPage";
import HiraganaPage from "@pages/HiraganaPage";
import KatakanaPage from "@pages/KatakanaPage";
import CharacterDetail from "@components/common/CharacterDetail";
import LoginPage from "@/pages/LoginPage";
import RegisterPage from "@pages/RegisterPage";
import LessonsPage from "./pages/LessonsPage";
import NewLessonsPage from "./pages/NewLessonsPage";
import ForgotPasswordPage from "./pages/ForgotPasswordPage";

const queryClient = new QueryClient();
const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    errorElement: <ErrorPage />,
    children: [
      {
        index: true,
        element: <HomePage />,
      },
      {
        path: "login",
        element: <LoginPage setToken={setToken} />,
      },
      {
        path: "register",
        element: <RegisterPage />,
      },
      {
        path: "forgot-password",
        element: <ForgotPasswordPage />,
      },
      {
        path: "hiragana",
        element: <HiraganaPage />,
      },
      {
        path: "katakana",
        element: <KatakanaPage />,
      },
      {
        path: "/:type/:character",
        element: <CharacterDetail />,
      },
      {
        path: "lessons",
        element: <LessonsPage />,
      },
      {
        path: "lessons/new",
        element: <NewLessonsPage />,
      },
    ],
  },
]);

function setToken(userToken: string) {
  sessionStorage.setItem('token', JSON.stringify(userToken));
}

// function getToken() {
//   const tokenString = sessionStorage.getItem('token');
//   const userToken = JSON.parse(tokenString as string);
//   return userToken?.token;
// }

function App() {
  // const token = getToken();

  // if(!token) {
  //   return <LoginPage setToken={setToken} />
  // }

  return (
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  );
}

export default App;
