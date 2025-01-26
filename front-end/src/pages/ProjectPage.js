import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import "../styles/projectpage.css";

const ProjectPage = () => {
  const { id } = useParams(); // Captura o parâmetro da URL
  const [project, setProject] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProject = async () => {
      try {
        console.log(`Buscando projeto com ID: ${id}`);
        const response = await fetch(`http://localhost:5289/api/Projeto/${id}`);
        if (!response.ok) {
          console.error(
            "Erro ao buscar o projeto:",
            response.status,
            response.statusText
          );
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
  }, [id]);

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
        <strong>Criador:</strong> {project.nome_utilizador}
      </p>
      <div className="containerbt">
        <Link to="/procurar" className="back-link">
          Voltar à lista de projetos
        </Link>
        <Link to="/home" className="back-link">
          Contratar
        </Link>
      </div>
    </div>
  );
};

export default ProjectPage;
