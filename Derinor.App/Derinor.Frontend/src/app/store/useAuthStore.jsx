import { create } from "zustand";

const useAuthStore = create((set) => ({
  isAuthenticated: false,
  token: null,
  user: null,
  isLoading: true,

  initialize: () => {
    try {
      const token = localStorage.getItem("auth_token");
      const user = localStorage.getItem("user");

      if (token && user) {
        const payload = JSON.parse(atob(token.split(".")[1]));
        const isExpired = payload.exp * 1000 < Date.now();

        if (!isExpired) {
          set({
            isAuthenticated: true,
            token,
            user: JSON.parse(user),
          });
        } else {
          localStorage.removeItem("auth_token");
          localStorage.removeItem("user");
        }
      }
    } catch (error) {
      localStorage.removeItem("auth_token");
      localStorage.removeItem("user");
    } finally {
      set({ isLoading: false });
    }
  },

  setAuth: (token, user) => {
    localStorage.setItem("auth_token", token);
    localStorage.setItem("user", JSON.stringify(user));
    set({ isAuthenticated: true, token, user });
  },

  logout: () => {
    localStorage.removeItem("auth_token");
    localStorage.removeItem("user");
    set({ isAuthenticated: false, token: null, user: null });
  },
}));

export default useAuthStore;
