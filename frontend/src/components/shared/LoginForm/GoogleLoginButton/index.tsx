"use client"
import React, { useEffect } from "react"
import { useLoginGoogle } from "@/hooks"
import { GOOGLE_CLIENT_ID } from "@/constants/api"
import { GoogleCredentialResponse } from "@/types/google"

export function GoogleLoginButton({ onSuccess }: { onSuccess?: () => void }) {
    const loginGoogleMutation = useLoginGoogle({ onSuccess })

    useEffect(() => {
        if (window.google) {
            window.google.accounts.id.initialize({
                client_id: GOOGLE_CLIENT_ID!,
                callback: async (res: GoogleCredentialResponse) => {
                    await loginGoogleMutation.mutateAsync(res.credential)
                },
                auto_select: true,
                cancel_on_tap_outside: true,
                ux_mode: "popup"
            })
            window.google.accounts.id.renderButton(
                document.getElementById("google-signin-button")!,
                {
                    theme: "outline",
                    size: "large",
                    text: "signin_with",
                    logo_alignment: "center"
                }
            )
        }
    }, [])

    return (
        <div
            id="google-signin-button"
            className="overflow-hidden border-primary border-2 rounded-[12px] 
                    transform transition-transform duration-150 active:scale-95 ease-in-out"
        ></div>
    )
}
