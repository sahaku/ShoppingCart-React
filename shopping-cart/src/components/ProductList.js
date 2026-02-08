import { useEffect, useState } from "react";
import { getProducts } from "../api/productApi";
import ProductCard from "./ProductCard";
import { fetchApi } from "../api/aiApi";

function ProductList() {
  // { products, addToCart }
  const [products, setProducts] = useState([]);

  const [searchText, setSearchText] = useState("");
  const [searchKeywords, setSearchKeywords] = useState("");
  const handleSmartSearch = async () => {
    try {
      const resp = await fetchApi("smart-search", { items: [searchText] });
      setProducts(resp);
      setSearchKeywords(resp.data.keywords);
    } catch (err) {
      console.error(err);
    } finally {
    }
  };
  useEffect(() => {
    getProducts().then(setProducts);
  }, []);
  return (
    <div className="container">
      <h1>Products</h1>
      <div className="d-flex mb-3">
        <input
          className="form-control me-2"
          placeholder="Search products (e.g. 'cheap running shoes')"
          value={searchText}
          onChange={(e) => setSearchText(e.target.value)}
        />
        <button className="btn btn-outline-primary" onClick={handleSmartSearch}>
          Smart Search
        </button>
      </div>
      {searchKeywords && (
        <p className="text-muted small mb-3">
          AI keywords: <strong>{searchKeywords}</strong>
        </p>
      )}
      <div className="row g-3">
        {products.map((p) => (
          <div className="col-md-4 mb-1" key={p.id}>
            <ProductCard product={p} />
          </div>
        ))}
      </div>
    </div>
  );
}

export default ProductList;
