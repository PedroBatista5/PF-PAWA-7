import React, { useEffect, useState } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import "../styles/projectpage.css";
import { useAuth } from "../contexts/AuthContext";

const ProjectPage = () => {
  const { id } = useParams(); // Captura o parâmetro da URL
  const [project, setProject] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate(); // Para redirecionar após contratar
  const { currentUser } = useAuth(); // Usa o hook para acessar o contexto de autenticação

  useEffect(() => {
    const fetchProject = async () => {
      try {
        console.log(`Buscando projeto com ID: ${id}`);
        const response = await fetch(`http://localhost:5289/api/Projeto/${id}`);
        if (!response.ok) {
          console.error("Erro ao buscar o projeto:", response.status, response.statusText);
          setProject(null);
          return;
        }
        const data = await response.json();
        console.log("Dados do projeto retornados:", data);
        setProject(data);
      } catch (error) {
        console.error("Erro ao buscar o projeto:", error);
      } finally {
        setLoading(false);
      }
    };
  
    fetchProject();
  
    console.log("currentUser:", currentUser);  
  }, [id, currentUser]);

  const handleContratar = async () => {
    console.log("currentUser:", currentUser); // Verifique o conteúdo de currentUser
    if (!currentUser || !currentUser.id_utilizador) {
      alert("É necessário estar autenticado para contratar um projeto.");
      return;
    }
  
    if (!project || !project.id_projetos) {
      alert("Projeto não encontrado.");
      return;
    }
    
    const requestBody = {
      Id_utilizador: currentUser.id_utilizador, 
      Id_projetos: project.id_projetos,
      Status_contratacao: "Pendente",
    };
    
    
    console.log("Enviando requisição com o seguinte corpo:", requestBody);
  
    try {
      const response = await fetch("http://localhost:5289/api/Contratacao", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(requestBody),
      });
  
      if (response.ok) {
        alert("Projeto contratado com sucesso!");
        navigate("/home");
      } else {
        console.error("Erro ao contratar o projeto:", response.statusText);
        alert("Não foi possível contratar o projeto.");
      }
    } catch (error) {
      console.error("Erro ao contratar o projeto:", error);
      alert("Erro de conexão com o servidor.");
    }
  };

  if (loading) {
    return <p>Carregando...</p>;
  }

  if (!project) {
    return <p>Projeto não encontrado.</p>;
  }

  return (
    <div className="project-details">
      <h1>{project.titulo_projetos}</h1>
      <p>
        <strong>Descrição:</strong> {project.descricao_projeto}
      </p>
      <p>
        <strong>Preço:</strong> {project.preco.toFixed(2)} €
      </p>
      <p>
        <strong>Criador:</strong> {project.NomeCriador}
      </p>
      <div className="containerbt">
        <Link to="/procurar" className="back-link">
          Voltar à lista de projetos
        </Link>
        <button onClick={handleContratar} className="contratar-button">
          Contratar
        </button>
      </div>
    </div>
  );
};

export default ProjectPage;
