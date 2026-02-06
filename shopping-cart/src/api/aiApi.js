const BASE_URL = "https://localhost:7038/api/aiservice";

export async function fetchApi(endpoint, query) {
  const url = `${BASE_URL}/${endpoint}`;
  const resp = await fetch(url, {
    method: "POST",
    headers: { "content-type": "application/json" },
    body: JSON.stringify({ query }),
  });
  return resp.json();
}

// export async function chat(query) {
//   const url = `${BASE_URL}/chat`;
//   const resp = await fetch(url, {
//     method: "POST",
//     headers: { "content-type": "application/json" },
//     body: JSON.stringify({ query }),
//   });
//   return resp.json();
// }
