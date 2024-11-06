import { Outlet, Link } from "react-router-dom";
import './layout.css';

const Layout = () => {
  return (
    <>
      <nav>
        <ul className="nav-list">
          <div className="left-nav">
            <li>
              <Link to="/login">Login</Link>
            </li>
            <li>
              <Link to="/register">Registro</Link>
            </li>
          </div>
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
            <li>
              <Link to="/projetos">Projetos</Link>
            </li>
          </div>
        </ul>
      </nav>

      <Outlet />
    </>
  );
};

export default Layout;
