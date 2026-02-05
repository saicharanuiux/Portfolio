import React from "react";
import "./Popup.css"

export default function Popup({ onClose, children, title, download }) {

  return (
    <div
      style={{
        position: "fixed",
        top: 0,
        left: 0,
        width: "100%",
        height: "100%",
        backgroundColor: "rgba(0, 0, 0, 0.5)",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      <div
        style={{
          background: "#fff",
          padding: "20px",
          borderRadius: "10px",
          textAlign: "center",
          width: "50%",
        }}
      >
        <div className="header-container">
        <h2 className="header-title">{title}</h2>
        <button className="download-button" download onClick={download}>
          Download PDF ⬇️
        </button>
        </div>

<div style={{ maxHeight: "500px", overflowY: "auto", border: "1px solid #ddd" }}>
  {children}
</div>

        <button onClick={onClose} className="cls-btn">
          Close
        </button>
      </div>
    </div>
  );
}
