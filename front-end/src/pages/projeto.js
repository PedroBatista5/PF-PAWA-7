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

  const atualizarStatus = async (id_contratacao, novoStatus) => {
    const token = localStorage.getItem("authToken");
  
    try {
      const response = await fetch(`http://localhost:5289/api/contratacao/${id_contratacao}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          Id_contratacao: id_contratacao,
          Status_contratacao: novoStatus
        }),
      });
  
      if (response.ok) {
        alert("Status atualizado com sucesso! "); 
        window.location.reload(); 
      } else {
        alert("Erro ao atualizar o status. ");
      }
    } catch (error) {
      console.error("Erro de conexão:", error);
      alert("Erro ao conectar com o servidor. ❌");
    }
  };
  
  
  

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
              {contratacao.status_contratacao !== "Concluído" && (
                <button onClick={() => atualizarStatus(contratacao.id_contratacao, "Concluída")}>
                  Marcar como Concluído
                </button>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default Projetos;
