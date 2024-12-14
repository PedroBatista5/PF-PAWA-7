import React, { useState } from 'react';
import '../styles/layout.css';
import '../styles/buttoninput.css';
import Input from '../components/input';
import BtForms from '../components/BtForms';

const Registro = () => {
  const [nome, setNome] = useState('');
  const [sobrenome, setSobrenome] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleRegistro = () => {
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

    console.log('Nome:', nome);
    console.log('Sobrenome:', sobrenome);
    console.log('Email:', email);
    console.log('Senha:', password);

    alert('Registro concluído com sucesso!');
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

export default Registro;
