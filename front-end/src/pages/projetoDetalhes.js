import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

const ProjectDetails = () => {
  const { id } = useParams(); // Pega o ID da URL
  const [project, setProject] = useState(null);

  useEffect(() => {
    const fetchProject = async () => {
      try {
        const response = await fetch(`http://localhost:5289/api/Projeto/${id}`);
        const data = await response.json();
        setProject(data);
      } catch (error) {
        console.error("Erro ao buscar os detalhes do projeto:", error);
      }
    };

    fetchProject();
  }, [id]);

  return (
    <div className="project-details">
      {project ? (
        <>
          <h1>{project.titulo_projetos}</h1>
          <p>{project.descricao_projeto}</p>
          <p>
            <strong>Preço:</strong> {project.preco.toFixed(2)} €
          </p>
          {/* Adicione mais informações sobre o projeto, se necessário */}
        </>
      ) : (
        <p>Carregando os detalhes do projeto...</p>
      )}
    </div>
  );
};

export default ProjectDetails;
