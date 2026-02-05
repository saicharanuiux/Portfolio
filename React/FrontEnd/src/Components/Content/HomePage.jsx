// React Component with CSS
import React, { memo, useEffect, useState } from "react";
import "./Content.css";
import Typical from "react-typical";

export default function HomePage() {

  return (
    <>
      <div className="container">
        <div className="content">
          <p>Hi There, I'm</p>
          <h1>Charan Vadla</h1>
          <h2 style={{ fontSize: "24px", marginTop: "20px" }}>
            <Typical
              steps={[
                "I am a Full Stack Developer with front-end and back-end skills based in Berlin"
              ]}

              wrapper="span"
            />
          </h2>
        </div>
        <div className="image">
          {/* <a>Img</a> */}
          <img src="http://localhost:60550/Portfolio/get-file/Program.png" />
        </div>

      </div>
    </>
  );
}
