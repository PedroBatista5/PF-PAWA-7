import React, { useState } from 'react';
import '../styles/layout.css';
import '../styles/buttoninput.css';
import Input from '../components/input';
import BtForms from '../components/BtForms';

const Register = () => {
  const [Nome, setNome] = useState('');
  const [Sobrenome, setSobrenome] = useState('');
  const [Email, setEmail] = useState('');
  const [Password, setPassword] = useState('');
  const [TipoUtilizador, setTipoUtilizador] = useState('');
  const [Descricao_info, setDescricaoInfo] = useState('');
  const [Servicos, setServicos] = useState('');
  const [Imagem_perfil, setImagemPerfil] = useState(null);
  const [isLoading, setIsLoading] = useState(false);

  const handleRegistro = async () => {
    if (!Nome || !Sobrenome || !Email || !Password || !TipoUtilizador) {
      alert('Por favor, preencha todos os campos.');
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(Email)) {
      alert('Por favor, insira um Email válido.');
      return;
    }

    if (Password.length < 6) {
      alert('A senha deve ter no mínimo 6 caracteres.');
      return;
    }

    if (TipoUtilizador !== "Freelancer" && TipoUtilizador !== "Cliente") {
      alert('Por favor, selecione um tipo de utilizador válido.');
      return;
    }

    if (TipoUtilizador === "Freelancer" && (!Descricao_info || !Servicos)) {
      alert('Por favor, preencha todos os campos adicionais para Freelancer.');
      return;
    }

    setIsLoading(true);

    const formData = new FormData();
    formData.append('Nome', Nome);
    formData.append('Sobrenome', Sobrenome);
    formData.append('Email', Email);
    formData.append('Password', Password);
    formData.append('TipoUtilizador', TipoUtilizador);

    // Só envia os campos adicionais se o tipo de utilizador for Freelancer
    if (TipoUtilizador === "Freelancer") {
      formData.append('Descricao_info', Descricao_info);
      formData.append('Servicos', Servicos);
      if (Imagem_perfil) {
        formData.append('Imagem_perfil', Imagem_perfil);
      }
    }

    try {
      const response = await fetch('http://localhost:5289/api/utilizador/register', {
        method: 'POST',
        body: formData,
      });

      const result = await response.json();

      setIsLoading(false);

      if (response.ok) {
        alert('Conta criada com sucesso!');
        window.location.href = '/';
      } else {
        alert(`Erro: ${result.Message || 'Erro inesperado.'}`);
      }
    } catch (error) {
      setIsLoading(false);
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
          placeholder="Digite seu Nome"
          value={Nome}
          onChange={(e) => setNome(e.target.value)}
        />
        <label>Sobrenome</label>
        <Input
          type="text"
          placeholder="Digite seu Sobrenome"
          value={Sobrenome}
          onChange={(e) => setSobrenome(e.target.value)}
        />
        <label>Email</label>
        <Input
          type="Email"
          placeholder="Digite seu Email"
          value={Email}
          onChange={(e) => setEmail(e.target.value)}
        />
        <label>Senha</label>
        <Input
          type="Password"
          placeholder="Digite sua senha"
          value={Password}
          onChange={(e) => setPassword(e.target.value)}
        />

        <label>Tipo de Utilizador</label>
        <select
          value={TipoUtilizador}
          onChange={(e) => setTipoUtilizador(e.target.value)}
          required
        >
          <option value="">Selecione...</option>
          <option value="Freelancer">Freelancer</option>
          <option value="Cliente">Cliente</option>
        </select>

        {TipoUtilizador === "Freelancer" && (
          <div className="freelancer-info">
            <h2>Informações adicionais do Freelancer</h2>
            <label>Descrição</label>
            <Input
              type="text"
              placeholder="Digite uma descrição"
              value={Descricao_info}
              onChange={(e) => setDescricaoInfo(e.target.value)}
            />
            <label>Serviços</label>
            <Input
              type="text"
              placeholder="Digite seus serviços"
              value={Servicos}
              onChange={(e) => setServicos(e.target.value)}
            />
            <label>Imagem de Perfil</label>
            <input
              type="file"
              onChange={(e) => setImagemPerfil(e.target.files[0])}
            />
          </div>
        )}

        <BtForms
          text={isLoading ? 'Registrando...' : 'Registrar'}
          onClick={handleRegistro}
          disabled={isLoading}
        />
      </div>
    </div>
  );
};

export default Register;
