import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "../styles/searchpage.css";

const Searchpage = () => {
  const [projects, setProjects] = useState([]);

  useEffect(() => {
    const fetchProjects = async () => {
      try {
        const response = await fetch("http://localhost:5289/api/Projeto/todos");
        const data = await response.json();
        console.log("Dados retornados pela API:", data);
        setProjects(data);
      } catch (error) {
        console.error("Erro ao buscar projetos:", error);
      }
    };

    fetchProjects();
  }, []);

  return (
    <div className="projects-list">
      {projects.length > 0 ? (
        projects.map((project) => (
          <Link to={`/projeto/${project.id_projetos}`} key={project.id_projetos} className="project-card">
            <h3>{project.titulo_projetos}</h3>
            <p>{project.descricao_projeto}</p>
            <p>
              <strong>Preço:</strong> {project.preco.toFixed(2)} €
            </p>
          </Link>
        ))
      ) : (
        <p>Nenhum projeto encontrado.</p>
      )}
    </div>
  );
};

export default Searchpage;
