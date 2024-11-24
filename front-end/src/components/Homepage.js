import React from "react";
import Header from "../components/Header";
import Button from "../components/BtForms";
import "../styles/Homepage.css";

const HomePage = () => {
  const handleButtonClick = () => {
    console.log("Button clicked!");
  };

  return (
    <div className="homepage">
      <Header text="PLACEHOLDER" />
      <div className="container">
        <Button text="Procure Serviços" onClick={handleButtonClick} />
      </div>
    </div>
  );
};

export default HomePage;
