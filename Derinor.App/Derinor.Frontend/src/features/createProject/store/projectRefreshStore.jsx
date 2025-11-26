import { create } from "zustand";

export const useProjectRefreshStore = create((set) => ({
  refreshTrigger: 0,
  triggerRefresh: () =>
    set((state) => ({ refreshTrigger: state.refreshTrigger + 1 })),
}));
