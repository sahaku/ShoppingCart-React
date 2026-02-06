import { createContext, useContext, useState } from "react";

const CartContext=createContext(null)

export function CartProvider({children}){
    const [items, setItems]=useState([])
      const addToCart = (product) => {
    setItems((prev) => {
      const exists = prev.find((item) => item.id === product.id);
      if (exists) {
        return prev.map((item) =>
          item.id === product.id
            ? { ...item, quantity: item.quantity + 1 }
            : item,
        );
      }
      return [...prev, { ...product, quantity: 1 }];
    });
  };
   const removeFromCart = (id) => {
    setItems((prev) => prev.filter((x) => x.id !== id));
  };

  const clearCart = () => setItems([]);

  return(
    <CartContext.Provider value={{items,addToCart,removeFromCart,clearCart}}>
{children}
    </CartContext.Provider>
  )
}

export function useCart(){
    return useContext(CartContext);
}