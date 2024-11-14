// Login.js
import React, { useState } from 'react';
import './layout.css';
import Input from '../components/input';
import BtForms from '../components/BtForms';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = () => {
    console.log("Email:", email);
    console.log("Password:", password);
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
        <p><a href="#">Forgot password?</a></p>
      </div>
    </div>
  );
};

export default Login;
