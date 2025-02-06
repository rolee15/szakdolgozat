import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import Layout from "@components/layout/Layout";
import HomePage from "@pages/HomePage";
import ErrorPage from "@pages/ErrorPage";
import HiraganaPage from "@pages/HiraganaPage";
import KatakanaPage from "@pages/KatakanaPage";
import CharacterDetail from "@components/common/CharacterDetail";
import SignInPage from "@pages/SignInPage";
import RegisterPage from "@pages/RegisterPage";

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
        path: "signin",
        element: <SignInPage />,
      },
      {
        path: "register",
        element: <RegisterPage />,
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
    ],
  },
]);

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  );
}

export default App;
