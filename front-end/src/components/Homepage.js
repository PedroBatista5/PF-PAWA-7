import React from "react";
import Header from "../components/Header";
import "../styles/Homepage.css";
import { Link } from "react-router-dom";

const HomePage = () => {
  return (
    <div className="homepage">
      <Header text="Bem-vindo ao {Titulo}" />
      <div className="container">
        <h1></h1>
        <p>Aqui, você pode encontrar os melhores freelancers de it de uma maneira rápida e eficaz</p>
        <Link to="/procurar" className="btn-link">
          Procurar
        </Link>
      </div>
    </div>
  );
};

export default HomePage;
