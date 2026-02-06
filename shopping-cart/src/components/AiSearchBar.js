import { useState } from "react";
import { fetchApi } from "../api/aiApi";

function AiSearchBar(onResults) {
  const [query, setQuery] = useState("");
  const handleSearch = async () => {
    const filters = await fetchApi("search", query);
    onResults(filters);
  };
  return (
    <>
      <input
        className="form-group mb-3"
        placeholder="Search with AI..."
        value={query}
        onChange={(e) => setQuery(e.target.value)}
      />
      <button className="btn btn-primary" onClick={handleSearch}>
        AI Search
      </button>
    </>
  );
}

export default AiSearchBar;
