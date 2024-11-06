import ReactDOM from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./pages/layout";
import Home from "./pages/home";
import Login from "./pages/login";
import Registro from "./pages/register";
import Perfil from "./pages/perfil";
import Procurar from "./pages/procurar";
import Projetos from "./pages/projetos";
import './App.css';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route path= "login" index element={<Login />} />
          <Route path="home" element={<Home />} />
          <Route path="register" element={<Registro/>}/>
          <Route path="perfil" element={<Perfil/>}/>
          <Route path="procurar" element={<Procurar/>}/>
          <Route path="projetos" element={<Projetos/>}/>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(<App />);