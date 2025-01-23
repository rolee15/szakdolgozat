import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import Layout from "./components/layout/Layout";
import HomePage from "./components/common/HomePage";
import ErrorPage from "./components/common/ErrorPage";
import HiraganaPage from "./components/hiragana/HiraganaPage";
import KatakanaPage from "./components/katakana/KatakanaPage";
import CharacterDetail from "./components/common/CharacterDetail";

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
