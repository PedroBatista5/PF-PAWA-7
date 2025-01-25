import React, { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../contexts/AuthContext"; 
import "../styles/layout.css";
import "../styles/buttoninput.css";
import Input from "../components/input";

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();

  const { login } = useContext(AuthContext); 

  const handleLogin = async () => {
    try {
      const response = await fetch('http://localhost:5289/api/utilizador/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
      });
  
      const data = await response.json();
  
      if (response.ok) {
        const user = { token: data.token, email }; 
        login(user); // Passa as informações para o contexto
        navigate('/home'); // Redireciona para a página inicial
      } else {
        setErrorMessage(data.Message || 'Erro ao realizar login');
      }
    } catch (error) {
      setErrorMessage('Erro de conexão ou outra falha.');
    }
  };

  return (
    <div className="login-container">
      <div className="login-form">
        <h1>Login</h1>
        <label>Email</label>
        <Input
          type="email"
          placeholder="Digite seu email"
          value={email}
           
          onChange={(e) => setEmail(e.target.value)}
        />
        <label>Senha</label>
        <Input
          type="password"
          placeholder="Digite sua senha"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <button onClick={handleLogin} className="button-form">
          Login
        </button>
        {errorMessage && <p className="error-message">{errorMessage}</p>}
      </div>
    </div>
  );
};

export default Login;
