import React from "react"
import { LoginModal } from "./LoginModal"
import { RegisterModal } from "./RegisterModal"
import { ChangePasswordModal } from "./ChangePasswordModal"
import { ForgotPasswordModal } from "./ForgotPasswordModal"

export const Modals = () => {
    return (
        <div>
            <ChangePasswordModal />
            <ForgotPasswordModal />
            <LoginModal />
            <RegisterModal />
        </div>
    )
}
