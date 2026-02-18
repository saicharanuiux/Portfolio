const TOKEN_KEY = "auth_token";

export const authService = {
  login: async (email, password) => {
    const res = await fetch("http://localhost:60550/api/Auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password })
    });

    if (!res.ok) throw new Error("Invalid credentials");
  },

  logout: async () => {
    await fetch("http://localhost:60550/api/Auth/logout", {
      method: "GET",
      headers: { "Authorization": `Bearer ${authService.getToken()}` }
    });
    localStorage.removeItem(TOKEN_KEY);
  },

  getToken: () => {
    return localStorage.getItem(TOKEN_KEY);
  },

  isAuthenticated: () => {
    return !!localStorage.getItem(TOKEN_KEY);
  },

  verifyToken: async () => {
  const token = localStorage.getItem(TOKEN_KEY);
  if (!token) return false;

  try {
    const res = await fetch("http://localhost:60550/api/Auth/getUserDetails", {
      method: "GET",
      headers: {
        withCredentials: true
      }
    });

    return res.ok;
  } catch {
    return false;
  }
}

};
