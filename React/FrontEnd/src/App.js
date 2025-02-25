import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Content from "./Components/Content/Content";
import About from "./Components/About/About";
import Navbar from "./Components/Navbar/Navbar";
import "./App.css";
import Contact from "./Components/Contact/Contact";

function App() {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/" element={<Content />} />
        <Route path="/about" element={<About />} />
        <Route path="/contact" element={<Contact />} />
      </Routes>
    </Router>
  );
}

export default App;
