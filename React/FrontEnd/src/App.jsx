import { BrowserRouter, Routes, Route } from "react-router-dom";
import Login from "./Components/Login/login.jsx";
import PublicRoute from "./Components/Auth/PublicRoute.jsx";
import "./App.css"
import HomePage from "./Components/Content/HomePage.jsx";
import Contact from "./Components/Contact/Contact.jsx";
import About from "./Components/About/About.jsx"
import AuthLayout from "./Components/Auth/AuthLayout.jsx"
import OAuthSuccess from "./Components/Auth/OAuthSuccess.jsx";

function App() {
  return (
    <BrowserRouter>
      <Routes>

        {/* PUBLIC */}
        <Route
          path="/login"
          element={
            <PublicRoute>
              <Login />
            </PublicRoute>
          }
        />

        {/* PROTECTED LAYOUT */}
        <Route element={<AuthLayout />}>
          <Route path="/" element={<HomePage />} />
          <Route path="/contact" element={<Contact />} />
          <Route path="/about" element={<About />} />
        </Route>
        <Route path="/oauth-success" element={<OAuthSuccess />} />

      </Routes>
    </BrowserRouter>
  );
}

export default App;
