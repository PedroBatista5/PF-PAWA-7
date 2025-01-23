import React, { createContext, useContext, useState, useEffect } from "react";

// Criação do contexto
export const AuthContext = createContext();

// Provedor de autenticação
export const AuthProvider = ({ children }) => {
  const [currentUser, setCurrentUser] = useState(null);

  // Ao iniciar, verifica se há um token no localStorage e restaura o usuário
  useEffect(() => {
    const token = localStorage.getItem("authToken");
    if (token) {
      setCurrentUser({ token }); // Ajuste se precisar restaurar mais dados do usuário
    }
  }, []);

  const login = (user) => {
    localStorage.setItem("authToken", user.token); // Armazena o token no localStorage
    setCurrentUser(user); // Atualiza o estado de currentUser
  };

  const logout = () => {
    localStorage.removeItem("authToken"); // Remove o token do localStorage
    setCurrentUser(null); // Atualiza o estado de currentUser
  };

  return (
    <AuthContext.Provider value={{ currentUser, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// Hook para consumir o contexto de autenticação
export const useAuth = () => useContext(AuthContext);
