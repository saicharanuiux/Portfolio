import React, { useState, useEffect } from "react";
import { Document, Page, pdfjs } from "react-pdf";
import "./About.css";
import Popup from "../Popup/Popup";
import { httpGet } from "../../Services/httpService";
import { use } from "react";

pdfjs.GlobalWorkerOptions.workerSrc = new URL(
  'pdfjs-dist/build/pdf.worker.min.mjs',
  import.meta.url,
).toString();

export default function About() {

  
  const [num, setNum] = useState();
  const [page, setPage] = useState(1);
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  
  const pdfUrl = encodeURI("http://localhost:60550/Portfolio/file/pdf/1H9gshw4ynuFTe1wLMWWGT7uCMaSEYRO9");

  const skills = [{Name: "HTML5", Src: "https://cdn-icons-png.flaticon.com/512/732/732212.png"},
    {Name: "CSS3", Src: "https://cdn-icons-png.flaticon.com/512/732/732190.png"},
    {Name: "JavaScript", Src: "https://cdn-icons-png.flaticon.com/512/5968/5968292.png"},
    {Name: "React", Src: "https://img.icons8.com/?size=100&id=NfbyHexzVEDk&format=png&color=000000"},
    {Name: "Angular", Src: "https://img.icons8.com/?size=100&id=j9DnICNnlhGk&format=png&color=000000"},
    {Name: "ASP.NET CORE", Src: "https://img.icons8.com/?size=100&id=y7WGoWNuIWac&format=png&color=000000g"},
    {Name: "SQL", Src: "https://img.icons8.com/?size=100&id=ldAV1F3sx1VI&format=png&color=000000"},
    {Name: "TypeScript", Src: "https://img.icons8.com/?size=100&id=Xf1sHBmY73hA&format=png&color=000000"},
    {Name: "C#", Src: "https://img.icons8.com/?size=100&id=55251&format=png&color=000000"},
    {Name: "Azure", Src: "https://img.icons8.com/?size=100&id=VLKafOkk3sBX&format=png&color=000000"}
  ]

  const onDownload = async () => {
  try {
    const response = await httpGet(pdfUrl)

    if (!response.ok) {
      throw new Error("Download failed");
    }

    const blob = await response.blob();

    const url = window.URL.createObjectURL(
      new Blob([blob], { type: "application/pdf" })
    );

    const a = document.createElement("a");
    a.href = url;
    a.download = "Charan Vadla Full Stack.pdf";
    document.body.appendChild(a);
    a.click();

    a.remove();
    window.URL.revokeObjectURL(url);
  } catch (err) {
    console.error("Error downloading PDF:", err);
  }
};

const [fileUrl, setFileUrl] = useState("");
const [profilePicUrl, setProfilePicUrl] = useState("");

useEffect(() => {
  async function fetchProfilePic() {
    const res = await viewFile("http://localhost:60550/Portfolio/file/img/1kmUq2wVgYgF3ktiuTHuo5LUQRo1V_Zc_");
    setProfilePicUrl(res);
  }
  try{
    fetchProfilePic();
  }
  catch(err){
    console.error("Error fetching profile picture:", err);
  }
}, []);

const viewFile = async (data, type) => {
  
  const response = await httpGet(data);

    if (!response.ok) {
      throw new Error("Download failed");
    }

    const blob = await response.blob();

    const url = window.URL.createObjectURL(
      new Blob([blob], { type: type === "pdf" ? "application/pdf" : "application/octet-stream" })
    );
    setFileUrl(url);
  };


  
  
  return (
    <div className="about-section">
      <div className="about-container">
        {/* Picture Section */}
        <div className="about-picture">
          <img
            src={profilePicUrl}
            alt="Your Name"
            className="profile-picture"
          />
        </div>

        <div className="about-text">
          <h1>Who is Charan Vadla ?</h1>
          <p>
          Results-driven Full Stack Developer with hands-on experience in designing, developing, and deploying
web applications. Procient in both front-end and back-end technologies. Adept at Agile methodologies, version
control and CI/CD pipelines. Known for delivering ecient, scalable, and user-friendly applications, while
collaborating with cross-functional teams to meet business requirements.
          </p>
          <p>
            When I'm not coding, you can find me exploring nature, experimenting
            with new recipes, or reading about the latest trends in tech.
          </p>
        </div>
      </div>
      <div>
      
      </div>

      <div style={{ textAlign: "center", marginTop: "50px" }}>
      <button onClick={() => { viewFile(pdfUrl, "pdf"); setIsPopupOpen(true); }} className="rsm-btn">
        View Resume
      </button>
      
      {isPopupOpen ? 
      <Popup onClose={() => setIsPopupOpen(false)} title={"Charan Vadla Resume"} download={onDownload} >
      <Document file={fileUrl} className="border rounded-lg shadow-md">
            <Page pageNumber={page} renderTextLayer={false} renderAnnotationLayer={false}/>
          </Document>
      </Popup>
 : <></>}
    </div>

      {/* Skills Section */}
      <div className="skills-section">
        <h2>My Skills</h2>
        <div className="skills-container">
        {skills.map(skill =>(
          <div key={skill.Name} className="skill">
          <img
            src={skill.Src}
            alt={skill.Name}
          />
          <p>{skill.Name}</p>
        </div>
        ))}
        </div>
      </div>
    </div>
  );
}
