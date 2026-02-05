const TOKEN_KEY = "auth_token";

export const authService = {
  login: async (email, password) => {
    const res = await fetch("http://localhost:60550/api/Auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password })
    });

    if (!res.ok) throw new Error("Invalid credentials");

    const token = await res.text();
    localStorage.setItem(TOKEN_KEY, token);
    return token;
  },

  logout: () => {
    localStorage.removeItem(TOKEN_KEY);
  },

  getToken: () => {
    return localStorage.getItem(TOKEN_KEY);
  },

  isAuthenticated: () => {
    return !!localStorage.getItem(TOKEN_KEY);
  }
};
