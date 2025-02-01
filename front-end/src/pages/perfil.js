import React, { useState, useEffect } from "react";
import "../styles/ProfilePage.css";
import Button from "../components/BtForms";
import { useAuth } from "../contexts/AuthContext";

const ProfilePage = () => {
  const [isEditPopupOpen, setIsEditPopupOpen] = useState(false);
  const [isCreateProjectPopupOpen, setIsCreateProjectPopupOpen] = useState(false);
  const { currentUser } = useAuth(); 
  const [isLoading, setIsLoading] = useState(false);
  const [formData, setFormData] = useState({
    nome: "",
    sobrenome: "",
    tipoUtilizador: "",
    descricao_info: "",
    servicos: "",
    projectName: "",
    projectDescription: "",
    projectPrice: "", 
  });

  useEffect(() => {
    const fetchProfileData = async () => {
      const token = localStorage.getItem("authToken");

      if (!token) {
        alert("Usuário não autenticado!");
        return;
      }

      try {
        const response = await fetch("http://localhost:5289/api/utilizador/getProfile", {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`, 
          },
        });

        if (response.ok) {
          const data = await response.json();
          setFormData({
            nome: data.nome,
            sobrenome: data.sobrenome,
            tipoUtilizador: data.tipoUtilizador,
            descricao_info: data.descricao_info,
            servicos: data.servicos,
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

  const handleCloseEditPopup = () => {
    setIsEditPopupOpen(false);
  };

  const handleCreateProjectButtonClick = () => {
    setIsCreateProjectPopupOpen(true);
  };

  const handleCloseCreateProjectPopup = () => {
    setIsCreateProjectPopupOpen(false);
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
      const token = localStorage.getItem("authToken");
      const user = JSON.parse(localStorage.getItem("userData")); 
      const response = await fetch(`http://localhost:5289/api/utilizador/${user.id_utilizador}`, {
        method: "PUT", 
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          descricao_info: formData.descricao_info,
          servicos: formData.servicos,
        }), 
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
  

  const handleCreateProject = async () => {
    const token = localStorage.getItem("authToken"); 
    if (!token) {
      console.error("Token não encontrado!");
      return;
    }

    const user = JSON.parse(localStorage.getItem("userData")); 
    if (!user || !user.id_utilizador) {
      console.error("Usuário não encontrado no localStorage");
      return;
    }

    const preco = parseFloat(formData.projectPrice);
    if (isNaN(preco)) {
      console.error("Preço inválido! Insira um número válido.");
      alert("Preço inválido! Insira um número válido.");
      return;
    }

    const projectData = {
      titulo_projetos: formData.projectName,
      preco: preco, 
      descricao_projeto: formData.projectDescription,
      id_utilizador: user.id_utilizador, 
    };

    try {
      const response = await fetch("http://localhost:5289/api/projeto/criar", { 
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(projectData),
      });

      if (!response.ok) {
        const errorText = await response.text();
        console.error("Erro ao criar projeto:", errorText);
        alert("Erro ao criar o projeto: " + errorText);
        return;
      }

      const data = await response.json().catch(() => null); 
      console.log("Projeto criado com sucesso:", data);
      alert("Projeto criado com sucesso!");
      setIsCreateProjectPopupOpen(false);
    } catch (error) {
      console.error("Erro na requisição:", error);
      alert("Erro de conexão com o servidor.");
    }
  };

  return (
    <div className="profile-page">
      <div className="profile-header">
        <div className="profile-image-container">
          <p className="profile-name">{formData.nome || "NOME"} {formData.sobrenome || "SOBRENOME"}</p>
        </div>
        <div className="profile-menu">
          <p>Nome: {formData.nome || "NOME"}</p>
          <p>Sobrenome: {formData.sobrenome || "SOBRENOME"}</p>
          <p>Tipo: {formData.tipoUtilizador || "TIPO"}</p>
          <p>Descrição: {formData.descricao_info || "Sem descrição"}</p>
          <p>Serviços: {formData.servicos || "SERVIÇOS"}</p>
          <Button text="Editar Informações" onClick={handleEditButtonClick} />
          <Button text="Criar Projeto" onClick={handleCreateProjectButtonClick} />
        </div>
      </div>

      {isEditPopupOpen && (
        <div className="popup-overlay">
          <div className="popup">
            <h2>Editar Informações</h2>
            <p>Descrição</p>
            <input
              type="text"
              name="descricao_info"
              placeholder="Atualizar Descrição"
              value={formData.descricao_info || ""}
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

      {isCreateProjectPopupOpen && (
        <div className="popup-overlay">
          <div className="popup">
            <h2>Criar Projeto</h2>
            <p>Nome do Projeto</p>
            <input
              type="text"
              name="projectName"
              placeholder="Nome do Projeto"
              value={formData.projectName}
              onChange={handleChange}
            />
            <p>Descrição do Projeto</p>
            <textarea
              name="projectDescription"
              placeholder="Descrição do Projeto"
              value={formData.projectDescription}
              onChange={handleChange}
            />
            <p>Preço</p>
            <input
              type="number"
              name="projectPrice"
              placeholder="Preço do Projeto"
              value={formData.projectPrice}
              onChange={handleChange}
            />
            <div className="popup-buttons">
              <button onClick={handleCloseCreateProjectPopup} disabled={isLoading}>
                Cancelar
              </button>
              <button onClick={handleCreateProject} disabled={isLoading}>
                {isLoading ? "Criando..." : "Criar Projeto"}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProfilePage;
