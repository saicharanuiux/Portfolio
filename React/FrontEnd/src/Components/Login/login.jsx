import React, { useState } from "react";
import "./login.css";
import { authService } from "../../Services/AuthService.js";
import { useNavigate } from "react-router-dom";

const Login = () => {
    const [error, setError] = useState("");
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        email: "",
        password: "",
    });
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await authService.login(formData.email, formData.password);
        } catch (err) {
            setError("Login failed");
        }
    };

    const handleGoogleLogin = () => {
        window.location.href = "http://localhost:60550/api/auth/google";
    };


    return (
        <div className="login-container">
            <form className="login-card" onSubmit={handleSubmit}>
                <h2>Sign in</h2>

                <input
                    type="email"
                    name="email"
                    placeholder="Email address"
                    value={formData.email}
                    onChange={handleChange}
                    required
                />

                <input
                    type="password"
                    name="password"
                    placeholder="Password"
                    value={formData.password}
                    onChange={handleChange}
                    required
                />

                <button className="btn-primary" type="submit">
                    Login
                </button>

                <div className="links">
                    <a href="/forgot-password">Forgot password?</a>
                    <a href="/register">Create account</a>
                </div>

                <div className="divider">OR</div>

                <button
                    type="button"
                    className="btn-oauth google"
                    onClick={handleGoogleLogin}
                >
                    Continue with Google
                </button>

            </form>
        </div>
    );
};

export default Login;
