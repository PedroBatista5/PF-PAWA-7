import React, { useState, useEffect } from "react";
import "../styles/ProfilePage.css";
import Button from "../components/BtForms";
import { useAuth } from "../contexts/AuthContext";

const ProfilePage = () => {
  const [isEditPopupOpen, setIsEditPopupOpen] = useState(false);
  const [isCreateProjectPopupOpen, setIsCreateProjectPopupOpen] = useState(false);
  const { currentUser } = useAuth(); // Usa o hook para acessar o contexto de autenticação
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

    const handleCreateProject = async () => {
      const preco = parseFloat(formData.projectPrice);
      
      if (isNaN(preco) || preco <= 0) {
        alert("Por favor, insira um valor válido para o preço.");
        return;
      }
    
      // Verificar se os campos obrigatórios estão preenchidos
      if (!formData.projectName || !formData.projectDescription || isNaN(preco)) {
        alert("Por favor, preencha todos os campos obrigatórios.");
        return;
      }
    
      const token = localStorage.getItem("authToken");
      if (!token) {
        alert("Usuário não autenticado.");
        return;
      }
    
      const projetoData = {
        Titulo_projetos: formData.projectName,
        Preco: preco,
        Descricao_projeto: formData.projectDescription,
        Id_utilizador: currentUser?.id,  // Certifique-se de que está pegando o ID corretamente
      };
    
      console.log('Projeto Data:', projetoData); // Verifique se os dados estão corretos
    
      setIsLoading(true);
      try {
        const response = await fetch("http://localhost:5289/api/Projeto/criar", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify(projetoData),
        });
    
        if (response.ok) {
          alert(`Projeto criado com sucesso`);
          setIsCreateProjectPopupOpen(false);
        } else {
          const errorData = await response.json();
          console.error('Erro de API:', errorData);
          const erroTituloProjeto = errorData.errors && errorData.errors.Titulo_projetos ? errorData.errors.Titulo_projetos[0] : 'Erro desconhecido';
          alert(`Erro: ${erroTituloProjeto}`);
        }
      } catch (error) {
        console.error("Erro ao criar projeto:", error);
        alert("Erro de conexão com o servidor.");
      } finally {
        setIsLoading(false);
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
              value={formData.descricao_info}
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
