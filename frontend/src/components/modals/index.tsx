import React from "react"
import { LoginModal } from "./LoginModal"
import { RegisterModal } from "./RegisterModal"
import { SetPasswordModal } from "./SetPasswordModal"
import { ForgotPasswordModal } from "./ForgotPasswordModal"

export * from "./ImageUploaderModal"

export const Modals = () => {
    return (
        <div>
            <SetPasswordModal />
            <ForgotPasswordModal />
            <LoginModal />
            <RegisterModal />
        </div>
    )
}
