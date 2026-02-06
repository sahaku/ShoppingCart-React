function parseNaturalQuery(input) {}
const text = input.toLowerCase();

const filters = {
  category: null,
  maxPrice: null,
  programmingFriendly: false,
  query: null,
};

if (text.includes("laptop") || text.includes("notebook")) {
  filters.category = "Laptop";
}
if (text.includes("cheap") || text.includes("low cost")) {
  filters.maxPrice = 800;
}

if (
  text.includes("programming") ||
  text.includes("develop") ||
  text.includes("coding")
) {
  filters.programmingFriendly = true;
}
filters.query = input;

return filters;
