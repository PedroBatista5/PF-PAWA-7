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
  const [tipoUtilizador, setTipoUtilizador] = useState('');
  const [descricaoInfo, setDescricaoInfo] = useState('');
  const [servicos, setServicos] = useState('');
  const [imagemPerfil, setImagemPerfil] = useState(null);
  const [isLoading, setIsLoading] = useState(false);

  const handleRegistro = async () => {
    if (!nome || !sobrenome || !email || !password || !tipoUtilizador) {
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

    if (tipoUtilizador !== "Freelancer" && tipoUtilizador !== "Cliente") {
      alert('Por favor, selecione um tipo de utilizador válido.');
      return;
    }

    // Se for Freelancer, precisa preencher as informações adicionais
    if (tipoUtilizador === "Freelancer" && (!descricaoInfo || !servicos)) {
      alert('Por favor, preencha todos os campos adicionais para Freelancer.');
      return;
    }

    setIsLoading(true);

    // Criação do objeto FormData para enviar os dados
    const formData = new FormData();
    formData.append('nome', nome);
    formData.append('sobrenome', sobrenome);
    formData.append('email', email);
    formData.append('password', password);
    formData.append('tipoUtilizador', tipoUtilizador);

    // Se for Freelancer, adicionar os campos adicionais
    if (tipoUtilizador === "Freelancer") {
      formData.append('descricaoInfo', descricaoInfo);
      formData.append('servicos', servicos);
      if (imagemPerfil) {
        formData.append('imagemPerfil', imagemPerfil);
      }
    }

    try {
      const response = await fetch('http://localhost:5289/api/Utilizador/register', {
        method: 'POST',
        body: formData, // Enviando FormData
      });

      const result = await response.json();

      setIsLoading(false);

      if (response.ok) {
        alert('Conta criada com sucesso!');
        window.location.href = '/'; // Redireciona para a página inicial após o sucesso
      } else {
        alert(`Erro: ${result.message}`);
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
        
        <label>Tipo de Utilizador</label>
        <select
          value={tipoUtilizador}
          onChange={(e) => setTipoUtilizador(e.target.value)}
          required
        >
          <option value="">Selecione...</option>
          <option value="Freelancer">Freelancer</option>
          <option value="Cliente">Cliente</option>
        </select>

        {/* Se o tipo de usuário for Freelancer, exibe os campos adicionais */}
        {tipoUtilizador === "Freelancer" && (
          <div className="freelancer-info">
            <h2>Informações adicionais do Freelancer</h2>
            <label>Descrição</label>
            <Input
              type="text"
              placeholder="Digite uma descrição"
              value={descricaoInfo}
              onChange={(e) => setDescricaoInfo(e.target.value)}
            />
            <label>Serviços</label>
            <Input
              type="text"
              placeholder="Digite seus serviços"
              value={servicos}
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
