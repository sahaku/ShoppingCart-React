import { useState } from "react";
import { useCart } from "../context/CartContext";
import { fetchApi } from "../api/aiApi";

export default function ProductCard({ product }) {
  const { addToCart } = useCart();
  const [aiDescription, setAiDescription] = useState("");
  const [loading, setLoading] = useState(false);
  const handleGenerateDescription = async () => {
    setLoading(true);
    try {
      const resp = await fetchApi("generate-description", product);
      setAiDescription(resp.data);
    } catch (err) {
      setAiDescription("Could not generate description.");
    } finally {
      setLoading(false);
    }
  };
  return (
    <>
      <div className="col-md-8 mb-1" key={product.id}>
        <div className="card shadow-sm border-0 h-100">
          <div className="card-body">
            <h5 className="card-title mb-1">{product.productName}</h5>
            <p className="text-muted mb-2">${product.price.toFixed(2)}</p>
            <p className="fw-semibold text-secondary mb-1">
              {product.category}
            </p>
            <p className="text-muted small mb-3">{product.description}</p>
            <button
              className="btn btn-primary mt-auto w-100 mb-2"
              onClick={() => addToCart(product)}
            >
              Add to Cart
            </button>
            <button
              className="btn btn-outline-secondary btn-sm mb-2"
              onClick={handleGenerateDescription}
              disabled={loading}
            >
              {loading ? "Generating..." : "AI Description"}
            </button>
            {aiDescription && (
              <p className="card-text small mt-2 border-top pt-2">
                {aiDescription}
              </p>
            )}
          </div>
        </div>
      </div>
    </>
  );
}
