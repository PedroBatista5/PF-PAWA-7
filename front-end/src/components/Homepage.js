import React from "react";
import Header from "../components/Header";
import "../styles/Homepage.css";
import { Link } from "react-router-dom";


const HomePage = () => {

  return (
    <div className="homepage">
      <Header text="PLACEHOLDER" />
      <div className="container">
        <Link to="/procurar">Procurar</Link>
      </div>
    </div>
  );
};

export default HomePage;
