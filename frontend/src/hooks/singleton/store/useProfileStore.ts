import { UserProfileViewRes } from "@/models/user/schema/response"
import { create } from "zustand"

type ProfileStore = {
    user: UserProfileViewRes | null
    setUser: (user: UserProfileViewRes | null) => void
    removeUser: () => void
}

export const useProfileStore = create<ProfileStore>((set) => ({
    user: null,

    setUser: (user) => set({ user }),

    removeUser: () => set({ user: null })
}))
