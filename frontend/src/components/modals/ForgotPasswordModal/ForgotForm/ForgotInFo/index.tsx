"use client"
import { useFormik } from "formik"
import * as Yup from "yup"
import React, { useCallback, useState } from "react"
import { ButtonStyled, ButtonToggleVisibility, InputStyled } from "@/components/styled"
import { useTranslation } from "react-i18next"
import { useResetPassword } from "@/hooks"
import { PASSWORD_REGEX } from "@/constants/regex"

interface ForgotInfoProps {
    onSuccess?: () => void
}

export function ForgotInFo({ onSuccess }: ForgotInfoProps) {
    const { t } = useTranslation()
    const resetMutation = useResetPassword({ onSuccess })

    const [isVisible, setIsVisible] = useState(false)
    const toggleVisibility = () => setIsVisible(!isVisible)
    const [isConfirmVisible, setIsConfirmVisible] = useState(false)
    const toggleConFirmVisibility = () => setIsConfirmVisible(!isConfirmVisible)

    const handleResetPassword = useCallback(
        async ({ password, confirmPassword }: { password: string; confirmPassword: string }) => {
            await resetMutation.mutateAsync({
                newPassword: password,
                confirmNewPassword: confirmPassword
            })
        },
        [resetMutation]
    )

    const formik = useFormik({
        initialValues: {
            password: "",
            confirmPassword: ""
        },
        validationSchema: Yup.object({
            password: Yup.string()
                .required(t("user.password_can_not_empty"))
                .min(8, t("user.password_too_short"))
                .matches(PASSWORD_REGEX, t("user.password_strength")),
            confirmPassword: Yup.string()
                .oneOf([Yup.ref("password")], t("user.confirm_password_equal"))
                .required(t("user.password_can_not_empty"))
        }),
        onSubmit: handleResetPassword
    })

    return (
        <form
            onSubmit={(e) => {
                if (formik.isSubmitting) {
                    e.preventDefault()
                    return
                }
                formik.handleSubmit(e)
            }}
            className="flex flex-col"
        >
            {/* Title */}
            <div className="mx-auto mt-2 mb-2">
                <div className="text-center">{t("auth.please_reset_password")}</div>
            </div>

            {/* Input InFo */}
            <div className="w-110 mx-auto">
                {/* <div className="w-110 mx-auto">
                    <InputStyled
                        isDisabled
                        // className="my-3"
                        variant="bordered"
                        label="Email"
                        value={maskEmail("tduc01234@gmail.com")}
                    />
                </div> */}

                <InputStyled
                    className="my-3"
                    variant="bordered"
                    label={t("auth.new_password")}
                    type={isVisible ? "text" : "password"}
                    value={formik.values.password}
                    onValueChange={(value) => formik.setFieldValue("password", value)}
                    isInvalid={!!(formik.touched.password && formik.errors.password)}
                    errorMessage={formik.errors.password}
                    onBlur={() => {
                        formik.setFieldTouched("password")
                    }}
                    endContent={
                        <ButtonToggleVisibility
                            isVisible={isVisible}
                            toggleVisibility={toggleVisibility}
                        />
                    }
                />

                <InputStyled
                    className="my-3"
                    variant="bordered"
                    label={t("auth.confirm_new_password")}
                    type={isConfirmVisible ? "text" : "password"}
                    value={formik.values.confirmPassword}
                    onValueChange={(value) => formik.setFieldValue("confirmPassword", value)}
                    errorMessage={formik.errors.confirmPassword}
                    onBlur={() => {
                        formik.setFieldTouched("confirmPassword")
                    }}
                    isInvalid={!!(formik.touched.confirmPassword && formik.errors.confirmPassword)}
                    endContent={
                        <ButtonToggleVisibility
                            isVisible={isConfirmVisible}
                            toggleVisibility={toggleConFirmVisibility}
                        />
                    }
                />
            </div>

            <div className="mx-auto">
                {/* <ButtonStyled onPress={handleBack} className="w-5 h-16 mx-auto mt-0">
                    <ArrowLeftIcon />
                </ButtonStyled> */}

                <ButtonStyled
                    type="submit"
                    className="w-110 h-10 mx-auto mt-4"
                    isLoading={formik.isSubmitting}
                    color="primary"
                    isDisabled={!formik.isValid}
                >
                    {t("auth.reset_password")}
                </ButtonStyled>
            </div>
        </form>
    )
}
