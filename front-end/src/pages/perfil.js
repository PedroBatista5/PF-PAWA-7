import React, { useState, useEffect } from "react";
import "../styles/ProfilePage.css";
import Button from "../components/BtForms";

const ProfilePage = () => {
  const [isEditPopupOpen, setIsEditPopupOpen] = useState(false);
  const [isNewProjectPopupOpen, setIsNewProjectPopupOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [formData, setFormData] = useState({
    nome: "",
    info: "",
    precos: "",
    servicos: "",
    nota: "",
  });
  const [newProjectData, setNewProjectData] = useState({
    titulo: "",
    preco: "",
    descricao: "",
    imagem: null,
  });

  useEffect(() => {
    const fetchProfileData = async () => {
      try {
        const response = await fetch("http://localhost:5289/api/utilizador/updateProfile", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            info: formData.info,
            servicos: formData.servicos,
          }),
        });

        if (response.ok) {
          const data = await response.json();
          setFormData({
            nome: data.nome,
            info: data.info,
            precos: data.precos,
            servicos: data.servicos,
            nota: data.nota,
          });
        } else {
          alert("Erro ao carregar os dados do perfil.");
        }
      } catch (error) {
        console.error("Erro ao carregar os dados:", error);
        alert("Erro de conexão com o servidor.");
      }
    };

    fetchProfileData();
  }, []);

  const handleEditButtonClick = () => {
    setIsEditPopupOpen(true);
  };

  const handleNewProjectButtonClick = () => {
    setIsNewProjectPopupOpen(true);
  };

  const handleCloseEditPopup = () => {
    setIsEditPopupOpen(false);
  };

  const handleCloseNewProjectPopup = () => {
    setIsNewProjectPopupOpen(false);
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
        body: JSON.stringify(formData),
      });

      if (response.ok) {
        alert("Dados salvos com sucesso!");
        setIsEditPopupOpen(false);
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

  const handleProjectChange = (e) => {
    const { name, value } = e.target;
    setNewProjectData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleImageChange = (e) => {
    setNewProjectData((prevData) => ({
      ...prevData,
      imagem: e.target.files[0],
    }));
  };

  const handleSaveProject = async () => {
    setIsLoading(true);
    const formDataProject = new FormData();
    formDataProject.append("Titulo_projetos", newProjectData.titulo);
    formDataProject.append("Preco", newProjectData.preco);
    formDataProject.append("Descricao_projeto", newProjectData.descricao);


    try {
      const response = await fetch("http://localhost:5289/api/projeto/criar", {
        method: "POST",
        body: formDataProject,
      });

      if (response.ok) {
        alert("Projeto criado com sucesso!");
        setIsNewProjectPopupOpen(false);
      } else {
        alert("Erro ao criar projeto.");
      }
    } catch (error) {
      console.error("Erro ao salvar projeto:", error);
      alert("Erro de conexão com o servidor.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="profile-page">
      <div className="profile-header">
        <div className="profile-image-container">
          <p className="profile-name">{formData.nome || "NOME"}</p>
        </div>
        <div className="profile-menu">
          <p>INFO</p>
          <p>PREÇOS</p>
          <p>SERVIÇOS</p>
          <p>NOTA</p>
          <Button text="Editar Informações" onClick={handleEditButtonClick} />
        </div>
      </div>

      <div className="projects-section">
        <h2>Projetos</h2>
        <Button text="Adicionar Novo Projeto" onClick={handleNewProjectButtonClick} />
      </div>

      {isEditPopupOpen && (
        <div className="popup-overlay">
          <div className="popup">
            <h2>Editar Informações</h2>
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
              <button onClick={handleCloseEditPopup} disabled={isLoading}>
                Cancelar
              </button>
              <button onClick={handleSave} disabled={isLoading}>
                {isLoading ? "Salvando..." : "Salvar"}
              </button>
            </div>
          </div>
        </div>
      )}

      {isNewProjectPopupOpen && (
        <div className="popup-overlay">
          <div className="popup">
            <h2>Criar Novo Projeto</h2>
            <p>Título</p>
            <input
              type="text"
              name="titulo"
              placeholder="Título do Projeto"
              value={newProjectData.titulo}
              onChange={handleProjectChange}
            />
            <p>Preço</p>
            <input
              type="number"
              name="preco"
              placeholder="Preço"
              value={newProjectData.preco}
              onChange={handleProjectChange}
            />
            <p>Descrição</p>
            <textarea
              name="descricao"
              placeholder="Descrição do Projeto"
              value={newProjectData.descricao}
              onChange={handleProjectChange}
            />
            <div className="popup-buttons">
              <button onClick={handleCloseNewProjectPopup} disabled={isLoading}>
                Cancelar
              </button>
              <button onClick={handleSaveProject} disabled={isLoading}>
                {isLoading ? "Salvando..." : "Salvar Projeto"}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProfilePage;
