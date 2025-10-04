import { UserUpdateReq } from "@/models/user/schema/request"
import { UserProfileViewRes } from "@/models/user/schema/response"
import { create } from "zustand"

type ProfileStore = {
    user: UserProfileViewRes | null
    setUser: (data: UserProfileViewRes | null) => void
    updateUser: (data: UserUpdateReq) => void
    removeUser: () => void
}

export const useProfileStore = create<ProfileStore>((set) => ({
    user: null,

    setUser: (data) => set({ user: data }),

    updateUser: (data) =>
        set((state) => ({
            user: state.user
                ? ({ ...state.user, ...data } as UserProfileViewRes)
                : (data as UserProfileViewRes)
        })),

    removeUser: () => set({ user: null })
}))
