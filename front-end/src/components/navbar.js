import { Link, useNavigate, Outlet } from 'react-router-dom'; 
import { useContext } from 'react';
import { AuthContext } from "../contexts/AuthContext"; 

const Layout = () => {
  const { currentUser, logout } = useContext(AuthContext); 
  const navigate = useNavigate(); // Hook para navegação

  const handleLogout = () => {
    logout(); // Chama a função de logout
    navigate('/login'); // Redireciona para a página de login
  };

  return (
    <>
      <nav>
        <ul className="nav-list">
          {!currentUser && ( 
            <div className="left-nav">
              <li>
                <Link to="/login">Login</Link>
              </li>
              <li>
                <Link to="/register">Registro</Link>
              </li>
            </div>
          )}
          {currentUser && ( 
            <div className="left-nav">
              <li>
                <Link to="/login" onClick={handleLogout}>Logout</Link> 
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
