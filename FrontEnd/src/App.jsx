import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { useEffect, useState } from "react";
import Login from "./Components/Login/login.jsx";
import PublicRoute from "./Components/Auth/PublicRoute.jsx";
import "./App.css";
import HomePage from "./Components/Content/HomePage.jsx";
import Contact from "./Components/Contact/Contact.jsx";
import About from "./Components/About/About.jsx";
import AuthLayout from "./Components/Auth/AuthLayout.jsx";
import OAuthSuccess from "./Components/Auth/OAuthSuccess.jsx";
import { authService } from "./Services/AuthService.js";

function App() {
  const [isChecking, setIsChecking] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    let isMounted = true;

    const verifyOnLoad = async () => {
      const isValid = await authService.verifyToken();

      if (isMounted) {
        setIsAuthenticated(isValid);
        setIsChecking(false);
      }
    };

    verifyOnLoad();

    return () => {
      isMounted = false;
    };
  }, []);

  if (isChecking) {
    return null;
  }

  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/login"
          element={
            <PublicRoute>
              <Login />
            </PublicRoute>
          }
        />

        <Route element={<AuthLayout />}>
          <Route path="/" element={<HomePage />} />
          <Route path="/contact" element={<Contact />} />
          <Route path="/about" element={<About />} />
        </Route>

        <Route path="/oauth-success" element={<OAuthSuccess />} />
        <Route
          path="*"
          element={<Navigate to={isAuthenticated ? "/" : "/login"} replace />}
        />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
