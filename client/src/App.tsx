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
import ChangePasswordPage from "./pages/ChangePasswordPage";
import SettingsPage from "./pages/SettingsPage";
import AdminDashboardPage from "./pages/admin/AdminDashboardPage";
import AdminUsersPage from "./pages/admin/AdminUsersPage";
import AdminUserDetailPage from "./pages/admin/AdminUserDetailPage";
import WritingPracticePage from "./pages/WritingPracticePage";
import GrammarListPage from "./pages/GrammarListPage";
import GrammarDetailPage from "./pages/GrammarDetailPage";
import ReadingListPage from "./pages/ReadingListPage";
import ReadingDetailPage from "./pages/ReadingDetailPage";
import LearningPathPage from "./pages/LearningPathPage";
import ShiritoriPage from "./pages/ShiritoriPage";
import UnitDetailPage from "./pages/UnitDetailPage";
import UnitTestPage from "./pages/UnitTestPage";
import { AuthProvider, useAuth } from "@/context/AuthContext";

const queryClient = new QueryClient();

function AuthenticatedRoute({ children }: { children: ReactNode }) {
  const { isAuthenticated } = useAuth();
  if (!isAuthenticated) return <Navigate to="/login" replace />;
  return <>{children}</>;
}

function ProtectedRoute({ children }: { children: ReactNode }) {
  const { isAuthenticated, mustChangePassword } = useAuth();
  if (!isAuthenticated) return <Navigate to="/login" replace />;
  if (mustChangePassword) return <Navigate to="/change-password" replace />;
  return <>{children}</>;
}

function AdminRoute({ children }: { children: ReactNode }) {
  const { isAuthenticated, mustChangePassword, isAdmin } = useAuth();
  if (!isAuthenticated) return <Navigate to="/login" replace />;
  if (mustChangePassword) return <Navigate to="/change-password" replace />;
  if (!isAdmin) return <Navigate to="/" replace />;
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
        path: "change-password",
        element: (
          <AuthenticatedRoute>
            <ChangePasswordPage />
          </AuthenticatedRoute>
        ),
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
        path: "lessons/writing",
        element: (
          <ProtectedRoute>
            <WritingPracticePage />
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
      {
        path: "grammar",
        element: (
          <ProtectedRoute>
            <GrammarListPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "grammar/:id",
        element: (
          <ProtectedRoute>
            <GrammarDetailPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "reading",
        element: (
          <ProtectedRoute>
            <ReadingListPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "reading/:id",
        element: (
          <ProtectedRoute>
            <ReadingDetailPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "path",
        element: (
          <ProtectedRoute>
            <LearningPathPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "path/:unitId",
        element: (
          <ProtectedRoute>
            <UnitDetailPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "path/:unitId/test",
        element: (
          <ProtectedRoute>
            <UnitTestPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "shiritori",
        element: (
          <ProtectedRoute>
            <ShiritoriPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "settings",
        element: (
          <ProtectedRoute>
            <SettingsPage />
          </ProtectedRoute>
        ),
      },
      {
        path: "admin",
        element: (
          <AdminRoute>
            <AdminDashboardPage />
          </AdminRoute>
        ),
      },
      {
        path: "admin/users",
        element: (
          <AdminRoute>
            <AdminUsersPage />
          </AdminRoute>
        ),
      },
      {
        path: "admin/users/:id",
        element: (
          <AdminRoute>
            <AdminUserDetailPage />
          </AdminRoute>
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
