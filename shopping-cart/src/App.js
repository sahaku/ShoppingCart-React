import { Route, Routes, BrowserRouter as Router } from "react-router-dom";
import { useState, useEffect, useContext } from "react";
import "./App.css";
import ProductList from "./components/ProductList";
import Layout from "./components/Layout";
import Cart from "./components/Cart";
import AiSearchBar from "./components/AiSearchBar";
import { getProducts } from "./api/productApi";
import { CartProvider } from "./context/CartContext";

function App() {
  // const [products, setProducts] = useState([]);
  // const [loading, setLoading] = useState(true);
  // useEffect(() => {
  //   getProducts().then(setProducts);
  //   async function Load() {
  //     try {
  //       // const resp = await fetch("https://localhost:7038/api/Cart/products");
  //       // const data = await resp.json();
  //       // setProducts(data);
  //       // console(data);
  //     } catch (err) {
  //     } finally {
  //       setLoading(false);
  //     }
  //   }
  //   Load();
  // }, []);

  // const [cart, setCart] = useState([]);
  // const addToCart = (product) => {
  //   setCart((prev) => {
  //     const exists = prev.find((item) => item.id === product.id);
  //     if (exists) {
  //       return prev.map((item) =>
  //         item.id === product.id
  //           ? { ...item, quantity: item.quantity + 1 }
  //           : item,
  //       );
  //     }
  //     return [...prev, { ...product, quantity: 1 }];
  //   });
  // };
  // const removeFromCart = (id) => {
  //   setCart((prev) => prev.filter((x) => x.id !== id));
  // };
  return (
    <div className="App">
      <CartProvider>
        <Router>
          <Layout>
            <Routes>
              <Route path="/" element={<ProductList />}></Route>
              <Route path="/cart" element={<Cart />}></Route>
              {/* <Route
                path="/aisearch"
                element={<AiSearchBar onResult={setProducts} />}
              /> */}
            </Routes>
          </Layout>
        </Router>
      </CartProvider>
    </div>
  );
}

export default App;
