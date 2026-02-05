import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

const OAuthSuccess = () => {
    const navigate = useNavigate();

    useEffect(() => {
        const params = new URLSearchParams(window.location.search);
        const token = params.get("token");

        if (token) {
            localStorage.setItem("auth_token", token);
            navigate("/", { replace: true });
        } else {
            navigate("/login");
        }
    }, []);

    return <p>Signing you in...</p>;
};

export default OAuthSuccess;
