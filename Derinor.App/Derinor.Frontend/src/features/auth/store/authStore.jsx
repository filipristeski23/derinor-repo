import { create } from "zustand";
import { jwtDecode } from "jwt-decode";

const useAuthStore = create((set, get) => ({
  token: null,
  user: null,
  isAuthenticated: false,
  isLoading: true,

  login: (token, user) => {
    localStorage.setItem("auth_token", token);
    localStorage.setItem("user", JSON.stringify(user));
    set({ token, user, isAuthenticated: true });
  },

  logout: () => {
    localStorage.removeItem("auth_token");
    localStorage.removeItem("user");
    set({ token: null, user: null, isAuthenticated: false, isLoading: false });
  },

  checkAuth: () => {
    try {
      const token = localStorage.getItem("auth_token");
      if (!token) {
        set({ isLoading: false });
        return;
      }

      const decodedToken = jwtDecode(token);

      if (decodedToken.exp * 1000 < Date.now()) {
        get().logout();
      } else {
        const user = JSON.parse(localStorage.getItem("user"));
        set({ token, user, isAuthenticated: true, isLoading: false });
      }
    } catch (error) {
      get().logout();
    }
  },
}));

export default useAuthStore;
