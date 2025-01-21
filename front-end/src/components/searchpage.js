import React, { useEffect, useState } from "react";
import Dropdown from "../components/dropdown.js";
import Input from "./input.js";
import "../styles/searchpage.css";

const Searchpage = () => {
  const [search, setSearch] = useState("");
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
    <div className="search-container">
      <Dropdown options={["Filtros", "Opção 1", "Opção 2", "Opção 3"]} />

      <Input
        type="text"
        placeholder="Pesquisa"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />

      <div className="projects-list">
        {projects.length > 0 ? (
          projects.map((project) => (
            <div key={project.id_projetos} className="project-card">
              <h3>{project.titulo_projetos}</h3>
              <p>{project.descricao_projeto}</p>
              <p>
                <strong>Preço:</strong> R$ {project.preco.toFixed(2)}
              </p>
            </div>
          ))
        ) : (
          <p>Nenhum projeto encontrado.</p>
        )}
      </div>

    </div>
  );
};

export default Searchpage;
