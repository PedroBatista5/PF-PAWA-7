import { Link, useNavigate, Outlet } from 'react-router-dom'; // Importando useNavigate e Outlet
import { useContext } from 'react';
import { AuthContext } from "../contexts/AuthContext"; 

const Layout = () => {
  const { isAuthenticated, logout } = useContext(AuthContext);
  const navigate = useNavigate(); // Hook para navegação

  const handleLogout = () => {
    logout(); // Chama a função de logout
    navigate('/login'); // Redireciona para a página de login ou outra página desejada
  };

  return (
    <>
      <nav>
        <ul className="nav-list">
          {!isAuthenticated && (
            <div className="left-nav">
              <li>
                <Link to="/login">Login</Link>
              </li>
              <li>
                <Link to="/register">Registro</Link>
              </li>
            </div>
          )}
          {isAuthenticated && (
            <div className="left-nav">
              <li>
                <Link to="/login" onClick={handleLogout}>Logout</Link> {/* Link para Logout */}
              </li>
            </div>
          )}
          <div className="right-nav">
            <li>
              <Link to="/home">Home</Link>
            </li>
            <li>
              <Link to="/perfil">Perfil</Link>
            </li>
            <li>
              <Link to="/procurar">Procurar</Link>
            </li>
          </div>
        </ul>
      </nav>

      <Outlet />
    </>
  );
};

export default Layout;
