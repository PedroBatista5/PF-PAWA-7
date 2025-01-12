/* eslint-disable jsx-a11y/anchor-is-valid */
import React, { useState } from 'react';
import '../styles/layout.css';
import '../styles/buttoninput.css';
import Input from '../components/input';
import BtForms from '../components/BtForms';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errorMessage, setErrorMessage] = useState('');

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
        console.log('Login bem-sucedido:', data);
      } else {
        // Exibe a mensagem de erro retornada pela API
        setErrorMessage(data.Message || 'Erro ao realizar login');
      }
    } catch (error) {
      setErrorMessage('Erro de conexão ou outra falha.');
      console.error('Erro no login:', error);
    }
  };
 

  return (
    <div className="login-container">
      <div className="login-form">
        <label>Email</label>
        <Input
          type="email"
          placeholder="Digite seu email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <label>Password</label>
        <Input
          type="password"
          placeholder="Digite sua senha"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <BtForms text="Sign In" onClick={handleLogin} />
        {errorMessage && <p style={{ color: 'red' }}>{errorMessage}</p>}
        <p><a href="#">Forgot password?</a></p>
      </div>
    </div>
  );
};

export default Login;
