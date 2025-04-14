import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Button, Container, Grid, Card, CardContent, CardMedia, TextField, Snackbar } from '@mui/material';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';

function App() {
  const [products, setProducts] = useState([]);
  const [cart, setCart] = useState({ items: [], total: 0 });
  const [searchTerm, setSearchTerm] = useState('');
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');

  useEffect(() => {
    // Fetch products from your API
    axios.get('http://localhost:5144/api/products')
      .then(response => setProducts(response.data))
      .catch(error => console.error('Error fetching products:', error));
  }, []);

  const addToCart = (product) => {
    setCart(prev => {
      const existingItem = prev.items.find(item => item.productId === product.id);
      if (existingItem) {
        return {
          ...prev,
          items: prev.items.map(item =>
            item.productId === product.id 
              ? { ...item, quantity: item.quantity + 1 } 
              : item
          ),
          total: prev.total + product.price
        };
      }
      return {
        ...prev,
        items: [...prev.items, {
          productId: product.id,
          product,
          quantity: 1,
          unitPrice: product.price
        }],
        total: prev.total + product.price
      };
    });
    setSnackbarMessage(`${product.name} added to cart`);
    setSnackbarOpen(true);
  };

  const removeFromCart = (productId) => {
    setCart(prev => {
      const itemToRemove = prev.items.find(item => item.productId === productId);
      if (!itemToRemove) return prev;

      if (itemToRemove.quantity > 1) {
        return {
          ...prev,
          items: prev.items.map(item =>
            item.productId === productId 
              ? { ...item, quantity: item.quantity - 1 } 
              : item
          ),
          total: prev.total - itemToRemove.unitPrice
        };
      }
      return {
        ...prev,
        items: prev.items.filter(item => item.productId !== productId),
        total: prev.total - itemToRemove.unitPrice
      };
    });
  };

  const placeOrder = async (orderData) => {
    try {
      await axios.post('http://localhost:5144/api/orders', orderData);
      setCart({ items: [], total: 0 });
      setSnackbarMessage('Order placed successfully!');
      setSnackbarOpen(true);
    } catch (error) {
      console.error('Error placing order:', error);
      setSnackbarMessage('Failed to place order');
      setSnackbarOpen(true);
    }
  };

  const filteredProducts = products.filter(product =>
    product.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    product.description.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <Router>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            My E-Commerce Store
          </Typography>
          <Button color="inherit" component={Link} to="/">
            Products
          </Button>
          <Button color="inherit" component={Link} to="/cart">
            <ShoppingCartIcon /> ({cart.items.reduce((acc, item) => acc + item.quantity, 0)})
          </Button>
        </Toolbar>
      </AppBar>

      <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
        <Routes>
          <Route path="/" element={
            <>
              <TextField
                label="Search Products"
                variant="outlined"
                fullWidth
                margin="normal"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
              <Grid container spacing={3}>
                {filteredProducts.map(product => (
                  <Grid item key={product.id} xs={12} sm={6} md={4}>
                    <Card>
                      <CardMedia
                        component="img"
                        height="140"
                        image={product.imageUrl || '/placeholder-product.jpg'}
                        alt={product.name}
                      />
                      <CardContent>
                        <Typography gutterBottom variant="h5" component="div">
                          {product.name}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                          {product.description}
                        </Typography>
                        <Typography variant="h6" sx={{ mt: 2 }}>
                          ${product.price.toFixed(2)}
                        </Typography>
                        <Button
                          variant="contained"
                          color="primary"
                          onClick={() => addToCart(product)}
                          fullWidth
                          sx={{ mt: 2 }}
                        >
                          Add to Cart
                        </Button>
                      </CardContent>
                    </Card>
                  </Grid>
                ))}
              </Grid>
            </>
          } />

          <Route path="/cart" element={
            <div>
              <Typography variant="h4" gutterBottom>
                Your Shopping Cart
              </Typography>
              {cart.items.length === 0 ? (
                <Typography>Your cart is empty</Typography>
              ) : (
                <>
                  {cart.items.map(item => (
                    <div key={item.productId} style={{ marginBottom: '1rem' }}>
                      <Typography variant="h6">{item.product.name}</Typography>
                      <Typography>Quantity: {item.quantity}</Typography>
                      <Typography>
                        Price: ${(item.quantity * item.unitPrice).toFixed(2)}
                      </Typography>
                      <Button
                        variant="outlined"
                        color="error"
                        onClick={() => removeFromCart(item.productId)}
                        sx={{ mt: 1 }}
                      >
                        Remove
                      </Button>
                    </div>
                  ))}
                  <Typography variant="h5" sx={{ mt: 2 }}>
                    Total: ${cart.total.toFixed(2)}
                  </Typography>
                  <Button
                    variant="contained"
                    color="success"
                    onClick={() => placeOrder({
                      items: cart.items.map(item => ({
                        productId: item.productId,
                        quantity: item.quantity,
                        unitPrice: item.unitPrice
                      })),
                      shippingAddress: "123 Main St" // Replace with form input
                    })}
                    sx={{ mt: 2 }}
                  >
                    Place Order
                  </Button>
                </>
              )}
            </div>
          } />
        </Routes>
      </Container>

      <Snackbar
        open={snackbarOpen}
        autoHideDuration={3000}
        onClose={() => setSnackbarOpen(false)}
        message={snackbarMessage}
      />
    </Router>
  );
}

export default App;