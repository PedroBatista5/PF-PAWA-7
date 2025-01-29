import React, { useEffect, useState } from 'react';
import '../styles/projetos.css';

const Projetos = () => {
  const [projects, setProjects] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchProjects = async () => {
      const token = localStorage.getItem("authToken");
      
  
      if (!token) {
        alert("Usuário não autenticado.");
        return;
      }
  
      try {
        const response = await fetch("http://localhost:5289/api/projeto/meusprojetos", {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        });
  
        if (response.ok) {
          const data = await response.json();
          
          console.log("Dados recebidos:", data);  // Adicione este log para depuração
          setProjects(data);
        } else {
          alert("Erro ao carregar projetos contratados.");
        }
      } catch (error) {
        console.error("Erro de conexão:", error);
        alert("Erro ao carregar os dados.");
      } finally {
        setIsLoading(false);
      }
    };
  
    fetchProjects();
  }, []);
  

  return (
    <div className="projects-page">
      <h1>Meus Projetos</h1>
      {isLoading ? (
        <p>Carregando projetos...</p>
      ) : (
        <div className="projects-list">
            {projects.map((contratacao) => (
            <div key={contratacao.id_contratacao} className="project-card">
                <h3>{contratacao.titulo_projetos}</h3>
                <p>{contratacao.descricao_projeto}</p>
                <p>Preço: R${contratacao.preco}</p>
                <p>Status: {contratacao.status_contratacao}</p>

            </div>
            ))}
        </div>
      )}
    </div>
  );
};

export default Projetos;
