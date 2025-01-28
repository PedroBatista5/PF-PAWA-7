import React, { createContext, useContext, useState, useEffect } from "react";

// Criação do contexto
export const AuthContext = createContext();

// Provedor de autenticação
export const AuthProvider = ({ children }) => {
  const [currentUser, setCurrentUser] = useState(null);

  // Ao iniciar, verifica se há um token e dados do usuário no localStorage
  useEffect(() => {
    const userData = localStorage.getItem("userData");
    if (userData) {
      setCurrentUser(JSON.parse(userData)); // Restaura o usuário completo
    }
  }, []);

  const login = (user) => {
    // Armazena tanto o token quanto os dados do usuário
    localStorage.setItem("authToken", user.token);
    localStorage.setItem("userData", JSON.stringify(user)); // Armazena o usuário completo
    setCurrentUser(user); // Atualiza o estado com os dados completos do usuário
  };

  const logout = () => {
    localStorage.removeItem("authToken");
    localStorage.removeItem("userData");
    setCurrentUser(null);
  };

  return (
    <AuthContext.Provider value={{ currentUser, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// Hook para consumir o contexto de autenticação
export const useAuth = () => useContext(AuthContext);
