import React, { useEffect, useState } from "react";
import { FaLinkedin, FaInstagram, FaFacebook, FaWhatsapp } from "react-icons/fa";
import "./Contact.css";
import { httpPost } from "../../Services/httpService";
import Loader, { hideLoader, showLoader } from "../Loader/Loader";


export default function Contact() {
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    message: "",
  });
  const [errors, setErrors] = useState({
    name: "",
    email: "",
    message: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
    if(![null, "", undefined].includes(value) && errors[name]){
      let formErrors = errors;
      formErrors[name] = "";
      setErrors(formErrors);
    }
   
  };

  const validateForm = () => {
    let formErrors = { name: "", email: "", message: "" };
    let isValid = true;

    // Name validation
    if (!formData.name) {
      formErrors.name = "Name is required.";
      isValid = false;
    }

    // Email validation
    if (!formData.email) {
      formErrors.email = "Email is required.";
      isValid = false;
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      formErrors.email = "Email address is invalid.";
      isValid = false;
    }

    // Message validation
    if (!formData.message) {
      formErrors.message = "Message is required.";
      isValid = false;
    }

    setErrors(formErrors);
    return isValid;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (validateForm()) {
      console.log("Form submitted:", formData);
      let obj = {
        Name: formData.name,
        EmailAddress: formData.email,
        Message: formData.message
      }
      
      showLoader();
      httpPost("http://localhost:60550/Portfolio/SendEmail", obj).then(res =>{
        if(res && res.success){
          setFormData({ name: "", email: "", message: "" });
          setErrors({ name: "", email: "", message: "" });
          alert("Thank you for contacting me!");
        }else{
          setFormData({ name: "", email: "", message: "" });
          setErrors({ name: "", email: "", message: "" });
          alert("OOPS!! Something went wrong. Please try again");
        }
      }).finally(() =>{
        hideLoader()
      })
      
    }
  };


  return (
    <div className="contact-container">
      <h1 className="contact-heading">Contact Me</h1>
      <form onSubmit={handleSubmit} className="contact-form">
        <div className="form-group">
          <label htmlFor="name" style={formData.name ? {color : "rgb(45, 45, 45)"} : {}}>Your Name</label>
          <input
            type="text"
            id="name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            placeholder="Your Name"
          />
          {errors.name && <div className="error">{errors.name}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="email" style={formData.email ? {color : "rgb(45, 45, 45)"} : {}}>Your Email</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            placeholder="Your Email"
          />
          {errors.email && <div className="error">{errors.email}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="message" style={formData.message ? {color : "rgb(45, 45, 45)"} : {}}>Message</label>
          <textarea
            id="message"
            name="message"
            rows="5"
            value={formData.message}
            onChange={handleChange}
            placeholder="Your Message"
          ></textarea>
          {errors.message && <div className="error">{errors.message}</div>}
        </div>

        <button type="submit">Send</button>
      </form>

      <div className="connect-section">
        <h2>Connect with Me</h2>
        <div className="social-media-icons">
          <a href="https://www.linkedin.com/in/charan-vadla/" target="_blank" rel="noopener noreferrer">
            <FaLinkedin size={40} />
          </a>
          <a href="https://www.instagram.com/charanvadla" target="_blank" rel="noopener noreferrer">
            <FaInstagram size={40} />
          </a>
          <a href="https://www.facebook.com/princecharan3699" target="_blank" rel="noopener noreferrer">
            <FaFacebook size={40} />
          </a>
          <a href="https://wa.me/4915560735789" target="_blank" rel="noopener noreferrer">
            <FaWhatsapp size={40} />
          </a>
        </div>
      </div>
    </div>
  );
}
