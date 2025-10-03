"use client"
import { ButtonStyled, InputStyled } from "@/components"
import React, { useCallback, useState } from "react"
import * as Yup from "yup"
import { useFormik } from "formik"
import { Icon } from "@iconify/react"
import { useTranslation } from "react-i18next"
import { useChangePassword } from "@/hooks"
import { UserChangePasswordReq } from "@/models/auth/schema/request"

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
                .required(t("user.old_password_is_required"))
                .min(8, t("user.password_too_short"))
                .matches(
                    /^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])[A-Za-z0-9!@#$%^&*]{6,}$/,
                    t("user.password_strength")
                ),
            password: Yup.string()
                .required(t("user.new_password_can_not_empty"))
                .min(8, t("user.password_too_short"))
                .matches(
                    /^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])[A-Za-z0-9!@#$%^&*]{6,}$/,
                    t("user.password_strength")
                ),
            confirmPassword: Yup.string()
                .oneOf([Yup.ref("password")], t("user.confirm_password_equal"))
                .required(t("user.password_can_not_empty"))
        }),
        onSubmit: handleChangePassword
    })

    return (
        <form onSubmit={formik.handleSubmit} className="p-4">
            {/* Title */}
            <div className="text-3xl mb-4 p-4 font-bold">
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
                        <button
                            aria-label="toggle password visibility"
                            className="focus:outline-solid outline-transparent"
                            type="button"
                            onClick={toggleVisibility}
                        >
                            {isVisible ? (
                                <Icon
                                    className="text-default-400 pointer-events-none text-2xl"
                                    icon="solar:eye-closed-linear"
                                />
                            ) : (
                                <Icon
                                    className="text-default-400 pointer-events-none text-2xl"
                                    icon="solar:eye-bold"
                                />
                            )}
                        </button>
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
                        <button
                            aria-label="toggle password visibility"
                            className="focus:outline-solid outline-transparent"
                            type="button"
                            onClick={toggleNewVisibility}
                        >
                            {isNewVisible ? (
                                <Icon
                                    className="text-default-400 pointer-events-none text-2xl"
                                    icon="solar:eye-closed-linear"
                                />
                            ) : (
                                <Icon
                                    className="text-default-400 pointer-events-none text-2xl"
                                    icon="solar:eye-bold"
                                />
                            )}
                        </button>
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
                        <button
                            aria-label="toggle password visibility"
                            className="focus:outline-solid outline-transparent"
                            type="button"
                            onClick={toggleConFirmVisibility}
                        >
                            {isConfirmVisible ? (
                                <Icon
                                    className="text-default-400 pointer-events-none text-2xl"
                                    icon="solar:eye-closed-linear"
                                />
                            ) : (
                                <Icon
                                    className="text-default-400 pointer-events-none text-2xl"
                                    icon="solar:eye-bold"
                                />
                            )}
                        </button>
                    }
                />
            </div>

            <div className="flex justify-end">
                <ButtonStyled
                    type="submit"
                    isLoading={formik.isSubmitting}
                    color="primary"
                    isDisabled={!formik.isValid}
                    className="flex min-w-30 mt-4 mb-4 mr-2"
                >
                    {t("auth.change_password")}
                </ButtonStyled>
            </div>
        </form>
    )
}
