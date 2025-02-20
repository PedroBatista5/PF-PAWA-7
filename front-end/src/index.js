import ReactDOM from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./contexts/AuthContext";
import Layout from "./components/navbar";
import Home from "./pages/home";
import Login from "./pages/login";
import Registro from "./pages/register";
import Perfil from "./pages/perfil";
import Procurar from "./pages/procurar";
import PrivateRoute from "./components/PrivateRoute";
import ProjectPage from "./pages/ProjectPage";
import Projetos from "./pages/projeto";

import "./App.css";

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route path="login" element={<Login />} />
            <Route path="home" element={<Home />} />
            <Route path="register" element={<Registro />} />
            <Route path="procurar" element={<Procurar />} />
            <Route path="projeto" element={<Projetos />} />
            <Route path="/projeto/:id" element={<ProjectPage />} />
            <Route
              path="perfil"
              element={
                <PrivateRoute>
                  <Perfil />
                </PrivateRoute>
              }
            />
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(<App />);
