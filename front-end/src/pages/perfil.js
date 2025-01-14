import React, { useState } from "react";
import "../styles/ProfilePage.css";
import Button from "../components/BtForms";

const ProfilePage = () => {
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [profileImage, setProfileImage] = useState("https://via.placeholder.com/150");
  const [formData, setFormData] = useState({
    nome: "",
    info: "",
    precos: "",
    servicos: "",
    nota: "",
  });
  const [isLoading, setIsLoading] = useState(false);

  const handleButtonClick = () => {
    setIsPopupOpen(true);
  };

  const handleClosePopup = () => {
    setIsPopupOpen(false);
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (event) => {
        setProfileImage(event.target.result); 
      };
      reader.readAsDataURL(file);
    }
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSave = async () => {
    setIsLoading(true);
    try {
      const response = await fetch("http://localhost:5289/api/updateProfile", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          ...formData,
          profileImage, 
        }),
      });

      if (response.ok) {
        alert("Dados salvos com sucesso!");
        setIsPopupOpen(false); 
      } else {
        alert("Erro ao salvar os dados.");
      }
    } catch (error) {
      console.error("Erro ao salvar os dados:", error);
      alert("Erro de conexão com o servidor.");
    } finally {
      setIsLoading(false); 
    }
  };

  return (
    <div className="profile-page">
      <div className="profile-header">
        <div className="profile-image-container">
          <img src={profileImage} alt="Perfil" className="profile-image" />
          <p className="profile-name">{formData.nome || "NOME"}</p>
        </div>

        <div className="profile-menu">
          <p>INFO</p>
          <p>PREÇOS</p>
          <p>SERVIÇOS</p>
          <p>NOTA</p>
          <Button text="Editar Informações" onClick={handleButtonClick} />
        </div>
      </div>

      <div className="projects-section">
        <h2>Projetos</h2>
      </div>

      {isPopupOpen && (
        <div className="popup-overlay">
          <div className="popup">
            <h2>Editar Informações</h2>
            <p>Foto de Perfil</p>
            <input type="file" accept="image/*" onChange={handleImageChange} />
            <p>Descrição</p>
            <input
              type="text"
              name="info"
              placeholder="Atualizar Descrição"
              value={formData.info}
              onChange={handleChange}
            />
            <p>Serviços</p>
            <input
              type="text"
              name="servicos"
              placeholder="Atualizar Serviços"
              value={formData.servicos}
              onChange={handleChange}
            />
            <div className="popup-buttons">
              <button onClick={handleClosePopup} disabled={isLoading}>
                Cancelar
              </button>
              <button onClick={handleSave} disabled={isLoading}>
                {isLoading ? "Salvando..." : "Salvar"}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProfilePage;
