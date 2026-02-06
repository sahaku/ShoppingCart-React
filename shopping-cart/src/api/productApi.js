export async function getProducts() {
  const res = await fetch("https://localhost:7038/api/Cart/products");
  return res.json();
}
