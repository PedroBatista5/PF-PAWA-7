import React from "react";
import "../styles/ProfilePage.css"; 

const ProfilePage = () => {
  return (
    <div className="profile-page">

      <div className="profile-header">

        <div className="profile-image-container">
          <img
            src="https://via.placeholder.com/150" 
            alt="Nome"
            className="profile-image"
          />
          <p className="profile-name">NOME</p>
        </div>

        <div className="profile-menu">
          <p>INFO</p>
          <p>PREÇOS</p>
          <p>SERVIÇOS</p>
          <p>NOTA</p>
        </div>
      </div>


      <div className="projects-section">
        <h2>Projetos</h2>
      </div>
    </div>
  );
};

export default ProfilePage;
