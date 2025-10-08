"use client"
import { ButtonStyled, ButtonToggleVisibility, InputStyled } from "@/components"
import React, { useCallback, useState } from "react"
import * as Yup from "yup"
import { useFormik } from "formik"
import { useTranslation } from "react-i18next"
import { useChangePassword } from "@/hooks"
import { UserChangePasswordReq } from "@/models/auth/schema/request"
import { PASSWORD_REGEX } from "@/constants/regex"

export default function ChangePasswordPage() {
    const { t } = useTranslation()

    const chagnePasswordMutation = useChangePassword({
        onSuccess: () => window.location.replace("/")
    })

    const handleChangePassword = useCallback(
        async (values: UserChangePasswordReq) => {
            await chagnePasswordMutation.mutateAsync(values)
        },
        [chagnePasswordMutation]
    )

    const [isVisible, setIsVisible] = useState(false)
    const toggleVisibility = () => setIsVisible(!isVisible)
    const [isNewVisible, setIsNewVisible] = useState(false)
    const toggleNewVisibility = () => setIsNewVisible(!isNewVisible)
    const [isConfirmVisible, setIsConfirmVisible] = useState(false)
    const toggleConFirmVisibility = () => setIsConfirmVisible(!isConfirmVisible)

    const formik = useFormik({
        initialValues: {
            oldPassword: "",
            password: "",
            confirmPassword: ""
        },
        validationSchema: Yup.object({
            oldPassword: Yup.string()
                .required(t("user.old_password_require"))
                .min(8, t("user.password_too_short"))
                .matches(PASSWORD_REGEX, t("user.password_strength")),
            password: Yup.string()
                .required(t("user.new_password_can_not_empty"))
                .min(8, t("user.password_too_short"))
                .matches(PASSWORD_REGEX, t("user.password_strength")),
            confirmPassword: Yup.string()
                .oneOf([Yup.ref("password")], t("user.confirm_password_equal"))
                .required(t("user.password_can_not_empty"))
        }),
        onSubmit: handleChangePassword
    })

    return (
        <form onSubmit={formik.handleSubmit}>
            {/* Title */}
            <div className="text-3xl mb-3 px-4 font-bold">
                <p>{t("auth.change_password")}</p>
            </div>

            {/* Form */}
            <div>
                {/* Current Password */}
                <InputStyled
                    className="my-3"
                    variant="bordered"
                    label={t("auth.old_password")}
                    type={isVisible ? "text" : "password"}
                    value={formik.values.oldPassword}
                    onValueChange={(value) => formik.setFieldValue("oldPassword", value)}
                    isInvalid={!!(formik.touched.oldPassword && formik.errors.oldPassword)}
                    errorMessage={formik.errors.oldPassword}
                    onBlur={() => {
                        formik.setFieldTouched("oldPassword")
                    }}
                    endContent={
                        <ButtonToggleVisibility
                            isVisible={isVisible}
                            toggleVisibility={toggleVisibility}
                        />
                    }
                />

                {/* New Password */}
                <InputStyled
                    className="my-3"
                    variant="bordered"
                    label={t("auth.new_password")}
                    type={isNewVisible ? "text" : "password"}
                    value={formik.values.password}
                    onValueChange={(value) => formik.setFieldValue("password", value)}
                    isInvalid={!!(formik.touched.password && formik.errors.password)}
                    errorMessage={formik.errors.password}
                    onBlur={() => {
                        formik.setFieldTouched("password")
                    }}
                    endContent={
                        <ButtonToggleVisibility
                            isVisible={isNewVisible}
                            toggleVisibility={toggleNewVisibility}
                        />
                    }
                />

                {/* Confirm New Password */}
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

            <div className="flex justify-end">
                <ButtonStyled
                    type="submit"
                    isLoading={formik.isSubmitting}
                    color="primary"
                    isDisabled={!formik.isValid}
                    className="flex min-w-30"
                >
                    {t("auth.change_password")}
                </ButtonStyled>
            </div>
        </form>
    )
}
