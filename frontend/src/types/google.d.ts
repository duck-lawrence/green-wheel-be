// types/google.d.ts
export interface GoogleCredentialResponse {
    clientId: string
    credential: string
    select_by: string
}

export interface GoogleAccountsID {
    initialize: (options: {
        client_id: string
        callback: (res: GoogleCredentialResponse) => void
        auto_select?: boolean
        cancel_on_tap_outside?: boolean
        ux_mode?: "popup" | "redirect"
    }) => void

    renderButton: (
        element: HTMLElement,
        options?: {
            theme?: "outline" | "filled_blue" | "filled_black"
            size?: "large" | "medium" | "small"
            shape?: "rectangular" | "pill" | "circle" | "square"
            text?: "signin_with" | "signup_with" | "continue_with"
            logo_alignment?: "left" | "center"
        }
    ) => void
}

export interface Google {
    accounts: {
        id: GoogleAccountsID
    }
}

declare global {
    interface Window {
        google?: Google
    }
}

export {}
