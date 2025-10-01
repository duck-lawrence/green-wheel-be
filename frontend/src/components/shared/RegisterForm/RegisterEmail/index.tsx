"use client"
import { useFormik } from "formik"
import * as Yup from "yup"
import React, { useCallback } from "react"
import { ButtonStyled, InputStyled } from "@/components"
import { useTranslation } from "react-i18next"
import { Link } from "@heroui/react"
import {
    useLoginDiscloresureSingleton,
    useRegister,
    useRegisterDiscloresureSingleton
} from "@/hooks"

interface RegisterEmail {
    setEmail: (email: string) => void
    onSuccess?: () => void
}

export function RegisterEmail({ setEmail, onSuccess }: RegisterEmail) {
    const { t } = useTranslation()
    const registerMutation = useRegister({ onSuccess })
    const { onClose: onCloseRegister } = useRegisterDiscloresureSingleton()
    const { onOpen: onOpenLogin } = useLoginDiscloresureSingleton()

    const handleOpenLogin = useCallback(() => {
        onCloseRegister()
        onOpenLogin()
    }, [onCloseRegister, onOpenLogin])

    const handleRegister = useCallback(
        async (values: { email: string }) => {
            await registerMutation.mutateAsync({ ...values })
            setEmail(values.email)
        },
        [registerMutation]
    )

    const formik = useFormik({
        initialValues: {
            email: ""
        },
        validationSchema: Yup.object({
            email: Yup.string()
                .required("Email is required")
                .matches(/^[\w.-]+@[\w.-]+\.[a-zA-Z]{2,}$/, "Invalid email format")
        }),
        onSubmit: handleRegister
    })

    return (
        <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4">
            {/* Title */}
            {/* <div className="mx-8 mt-2 mb-0">
                <h1 className="text-center font-bold text-xl">{t("auth.security_verification")}</h1>
            </div> */}

            {/* Input email */}
            <div className="w-110 mx-auto">
                <InputStyled
                    // className="my-3"
                    variant="bordered"
                    label="Email"
                    value={formik.values.email}
                    onValueChange={(value) => formik.setFieldValue("email", value)}
                    isInvalid={!!(formik.touched.email && formik.errors.email)}
                    errorMessage={formik.errors.email}
                    onBlur={() => {
                        formik.setFieldTouched("email")
                    }}
                    onClear={() => {}}
                />
            </div>
            {/* Button submit */}
            <ButtonStyled
                type="submit"
                className="w-110 h-10 mx-auto mt-4"
                isLoading={formik.isSubmitting}
                color="primary"
                isDisabled={!formik.isValid}
            >
                Send OTP
            </ButtonStyled>

            <p className="text-small text-center">
                {t("auth.already_have_account")}&nbsp;
                <Link isBlock onPress={handleOpenLogin} className="cursor-pointer">
                    {t("login.login")}
                </Link>
            </p>
        </form>
    )
}
