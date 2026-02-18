import { Navigate, Outlet, useLocation } from "react-router-dom";
import { authService } from "../../Services/AuthService.js";
import Navbar from "../Navbar/Navbar.jsx";
import { useEffect, useState } from "react";

const AuthLayout = () => {
  const location = useLocation();
  const [isChecking, setIsChecking] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    let isMounted = true;

    const verify = async () => {
      setIsChecking(true);
      const isValid = await authService.verifyToken();

      if (isMounted) {
        setIsAuthenticated(isValid);
        setIsChecking(false);
      }
    };

    verify();

    return () => {
      isMounted = false;
    };
  }, [location.pathname]);

  if (isChecking) {
    return null;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return (
    <>
      <Navbar />
      <Outlet />
    </>
  );
};

export default AuthLayout;
