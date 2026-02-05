import { Navigate, Outlet } from "react-router-dom";
import { authService } from "../../Services/AuthService.js";
import Navbar from "../Navbar/Navbar.jsx";

const AuthLayout = () => {
    if (!authService.isAuthenticated()) {
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
