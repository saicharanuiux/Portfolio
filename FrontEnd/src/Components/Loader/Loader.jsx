import React, { useState } from "react";
import ReactDOM from "react-dom";
import "./Loader.css";

let showLoaderFunc, hideLoaderFunc; 

const Loader = () => {
    const [visible, setVisible] = useState(false);

    showLoaderFunc = () => setVisible(true);
    hideLoaderFunc = () => setVisible(false);

    if (!visible) return null; 

    return ReactDOM.createPortal(
        <div className="loader-overlay"> 
            <div className="loader"></div> 
        </div>,
        document.body 
    );
};

export const showLoader = () => showLoaderFunc && showLoaderFunc();
export const hideLoader = () => hideLoaderFunc && hideLoaderFunc();

export default Loader;
