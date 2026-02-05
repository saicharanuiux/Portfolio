import { Navigate } from "react-router-dom";
import { authService } from "../../Services/AuthService.js";

const PublicRoute = ({ children }) => {
    if (authService.isAuthenticated()) {
        return <Navigate to="/" replace />;
    }
    return children;
};

export default PublicRoute;
