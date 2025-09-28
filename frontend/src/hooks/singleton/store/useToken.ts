import { create } from "zustand"

export type TokenStore = {
    accessToken: string | null
    isHydrated: boolean
    setAccessToken: (token: string, rememberMe?: boolean) => void
    removeAccessToken: () => void
    setRememberMe: (remember: boolean) => void
    removeRememberMe: () => void
}

const getRemeberMe = () => {
    if (typeof window === "undefined") return false
    return localStorage.getItem("remember_me") === "true"
}

const getStoredToken = () => {
    if (typeof window === "undefined") return null
    return getRemeberMe()
        ? localStorage.getItem("access_token")
        : sessionStorage.getItem("access_token")
}

const useToken = create<TokenStore>()((set) => ({
    accessToken: getStoredToken(),
    isHydrated: true,
    setAccessToken: (token: string, rememberMe = false) => {
        if (typeof window === "undefined") return
        if (rememberMe) {
            localStorage.setItem("access_token", token)
            localStorage.setItem("remember_me", "true")
            sessionStorage.removeItem("access_token")
        } else {
            sessionStorage.setItem("access_token", token)
            sessionStorage.setItem("remember_me", "true")
            localStorage.removeItem("access_token")
        }
        set({ accessToken: token })
    },
    removeAccessToken: () => {
        if (typeof window === "undefined") return
        localStorage.removeItem("accessToken")
        localStorage.removeItem("rememberMe")
        sessionStorage.removeItem("accessToken")
        set({ accessToken: null })
    },
    setRememberMe: (rememberMe: boolean) => {
        if (typeof window === "undefined") return
        if (rememberMe) localStorage.setItem("remember_me", "true")
        else localStorage.removeItem("remember_me")
    },
    removeRememberMe: () => {
        if (typeof window === "undefined") return
        localStorage.removeItem("remember_me")
    }
}))

export default useToken
