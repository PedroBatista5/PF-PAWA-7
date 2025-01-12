import React, { useState } from 'react';
import '../styles/layout.css';
import '../styles/buttoninput.css';
import Input from '../components/input';
import BtForms from '../components/BtForms';

const Register = () => {
  const [nome, setNome] = useState('');
  const [sobrenome, setSobrenome] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleRegistro = async () => {
    if (!nome || !sobrenome || !email || !password) {
      alert('Por favor, preencha todos os campos.');
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      alert('Por favor, insira um email válido.');
      return;
    }

    if (password.length < 6) {
      alert('A senha deve ter no mínimo 6 caracteres.');
      return;
    }
    
    try {
      const response = await fetch('http://localhost:5289/api/Utilizador/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ nome, sobrenome, email, password }),
      });

      const result = await response.json();

      if (response.ok) {
        alert('Registro bem-sucedido!');
        setNome('');
        setSobrenome('');
        setEmail('');
        setPassword('');
      } else {
        alert(`Erro: ${result.message}`);
      }
    } catch (error) {
      alert('Erro de rede. Por favor, tente novamente mais tarde.');
    }
  };

  return (
    <div className="registro-container">
      <div className="registro-form">
        <h1>Crie a sua conta</h1>
        <label>Nome</label>
        <Input
          type="text"
          placeholder="Digite seu nome"
          value={nome}
          onChange={(e) => setNome(e.target.value)}
        />
        <label>Sobrenome</label>
        <Input
          type="text"
          placeholder="Digite seu sobrenome"
          value={sobrenome}
          onChange={(e) => setSobrenome(e.target.value)}
        />
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
        <BtForms text="Registrar" onClick={handleRegistro} />
      </div>
    </div>
  );
};

export default Register;
