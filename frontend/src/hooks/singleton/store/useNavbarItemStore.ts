import { create } from "zustand"

type NavbarItemStore = {
    activeMenuKey: string
    setActiveMenuKey: (activeMenuKey: string) => void
}

export const useNavbarItemStore = create<NavbarItemStore>((set) => ({
    activeMenuKey: "",
    setActiveMenuKey: (activeMenuKey) => set({ activeMenuKey })
}))
