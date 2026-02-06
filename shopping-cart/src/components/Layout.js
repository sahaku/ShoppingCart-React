import { Link } from "react-router-dom";
import { useCart } from "../context/CartContext";
function Layout({ children }) {
  const { items } = useCart();
  const count = items.reduce((sum, i) => sum + i.quantity, 0);
  return (
    <>
      <nav className="navbar navbar-expand-lg navbar-dark bg-dark px-3">
        <Link className="navbar-brand" to="/">
          MyShop
        </Link>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav ms-auto">
            <li className="nav-item">
              <Link className="nav-link" to="/">
                Product
              </Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/cart">
                Cart ({count})
              </Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/aisearch">
                AI Search
              </Link>
            </li>
          </ul>
        </div>
      </nav>
      <div className="container mt-4">{children}</div>
    </>
  );
}

export default Layout;
