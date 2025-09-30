"use client"
import { ButtonStyled, InputStyled } from "@/components"
import React, { useState } from "react"
import * as Yup from "yup"
import { useFormik } from "formik"
import { Icon } from "@iconify/react"

export default function page() {
    const [isVisible, setIsVisible] = useState(false)
    const toggleVisibility = () => setIsVisible(!isVisible)
    const [isNewVisible, setIsNewVisible] = useState(false)
    const toggleNewVisibility = () => setIsNewVisible(!isNewVisible)
    const [isConfirmVisible, setIsConfirmVisible] = useState(false)
    const toggleConFirmVisibility = () => setIsConfirmVisible(!isConfirmVisible)

    const formik = useFormik({
        initialValues: {
            currentPassword: "",
            newPassword: "",
            confirmPassword: ""
        },
        validationSchema: Yup.object({
            currentPassword: Yup.string()
                .required("Current password is required")
                .min(6, "Current password must be at least 6 characters")
                .matches(
                    /^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])[A-Za-z0-9!@#$%^&*]{6,}$/,
                    "Current password must contain 1 uppercase, 1 number, 1 special char"
                ),
            newPassword: Yup.string()
                .required("New Password is required")
                .min(6, "New Password must be at least 6 characters")
                .matches(
                    /^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])[A-Za-z0-9!@#$%^&*]{6,}$/,
                    "Password must contain 1 uppercase, 1 number, 1 special char"
                ),
            confirmPassword: Yup.string()
                .oneOf([Yup.ref("newPassword")], "Passwords must match")
                .required("Please confirm your password")
        }),
        onSubmit: async (values) => {
            await new Promise((resolve) => setTimeout(resolve, 4000))
            alert(JSON.stringify(values))
        }
    })

    return (
        <form onSubmit={formik.handleSubmit} className="p-4">
            {/* Title */}
            <div className="text-3xl mb-4 p-4 font-bold">
                <p>Thay đổi mật khẩu</p>
            </div>

            {/* Form */}
            <div>
                {/* Current Password */}
                <InputStyled
                    className="my-3"
                    variant="bordered"
                    label="Current password"
                    type={isVisible ? "text" : "password"}
                    value={formik.values.currentPassword}
                    onValueChange={(value) => formik.setFieldValue("currentPassword", value)}
                    isInvalid={!!(formik.touched.currentPassword && formik.errors.currentPassword)}
                    errorMessage={formik.errors.currentPassword}
                    onBlur={() => {
                        formik.setFieldTouched("currentPassword")
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
                    label="New password"
                    type={isNewVisible ? "text" : "password"}
                    value={formik.values.newPassword}
                    onValueChange={(value) => formik.setFieldValue("newPassword", value)}
                    isInvalid={!!(formik.touched.newPassword && formik.errors.newPassword)}
                    errorMessage={formik.errors.newPassword}
                    onBlur={() => {
                        formik.setFieldTouched("newPassword")
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
                    label="Confirm new password"
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
                    isDisabled={!formik.isValid || !formik.dirty}
                    className="flex w-30 mt-4 mb-4 mr-2"
                >
                    Submit
                </ButtonStyled>
            </div>
        </form>
    )
}
