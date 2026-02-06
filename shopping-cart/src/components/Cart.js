import { useEffect, useState } from "react";
import { useCart } from "../context/CartContext";
import { fetchApi } from "../api/aiApi";

function Cart() {
  const { items, removeFromCart, clearCart } = useCart();
  const [suggestions, setSuggestions] = useState();
  const [loading, setLoading] = useState(false);
  const total = items.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0,
  );
  const handleSuggestions = async () => {
    setLoading(true);
    try {
      const cardProducts = items.map((i) => i.product);
      const resp = await fetchApi("cart-suggestions", cardProducts);
      setSuggestions(resp.data);
    } catch (err) {
      setSuggestions("Could not fetch suggestions.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (items.length === 0) {
      setSuggestions("");
    }
  }, [items]);
  return (
    <>
      <div>
        <h2 className="mb-3">Your Cart</h2>
        {items.length === 0 && <p>Your cart is empty.</p>}
        <ul className="list-group mb-3">
          {items.map((item) => (
            <li
              key={item.id}
              className="list-group-item d-flex justify-content-between align-items-center"
            >
              <div>
                <strong>{item.productName}</strong>{" "}
                <span className="text-muted">
                  x {item.quantity} (${item.price.toFixed(2)})
                </span>
              </div>
              <button
                className="btn btn-sm btn-outline-danger"
                onClick={() => removeFromCart(item.id)}
              >
                Remove
              </button>
            </li>
          ))}
        </ul>
        <div className="d-flex justify-content-between align-items-center mb-3">
          {items.length > 0 && (
            <h4>
              Card total:<b>${total}</b>
            </h4>
          )}
          <div>
            <button
              className="btn btn-outline-secondary me-2"
              onClick={clearCart}
            >
              Clear Cart
            </button>
            <button
              className="btn btn-outline-primary"
              onClick={handleSuggestions}
              disabled={loading}
            >
              {loading ? "Getting AI suggestions..." : "AI Suggestions"}
            </button>
          </div>
        </div>
      </div>
      {suggestions && (
        <div className="card">
          <div className="card-body">
            <h5 className="card-title">AI Suggestions</h5>
            <p className="card-text">{suggestions}</p>
          </div>
        </div>
      )}
    </>
  );
}

export default Cart;
