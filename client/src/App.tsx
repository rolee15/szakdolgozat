import { createBrowserRouter, RouterProvider, Navigate } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactNode } from "react";
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
import ReviewLessonsPage from "./pages/ReviewLessonsPage";
import FlashCardPage from "./pages/FlashCardPage";
import KanjiListPage from "./pages/KanjiListPage";
import KanjiDetailPage from "./pages/KanjiDetailPage";
import { AuthProvider, useAuth } from "@/context/AuthContext";

const queryClient = new QueryClient();

function ProtectedRoute({ children }: { children: ReactNode }) {
  const { isAuthenticated } = useAuth();
  if (!isAuthenticated) return <Navigate to="/login" replace />;
  return <>{children}</>;
}

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
        element: <LoginPage />,
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
        element: (
          <ProtectedRoute>
            <HiraganaPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "katakana",
        element: (
          <ProtectedRoute>
            <KatakanaPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "/:type/:character",
        element: (
          <ProtectedRoute>
            <CharacterDetail />
          </ProtectedRoute>
        ),
      },
      {
        path: "lessons",
        element: (
          <ProtectedRoute>
            <LessonsPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "lessons/new",
        element: (
          <ProtectedRoute>
            <NewLessonsPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "lessons/review",
        element: (
          <ProtectedRoute>
            <ReviewLessonsPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "flashcards",
        element: (
          <ProtectedRoute>
            <FlashCardPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "kanji",
        element: (
          <ProtectedRoute>
            <KanjiListPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "kanji/:character",
        element: (
          <ProtectedRoute>
            <KanjiDetailPage />
          </ProtectedRoute>
        ),
      },
    ],
  },
]);

function App() {
  return (
    <AuthProvider>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    </AuthProvider>
  );
}

export default App;
