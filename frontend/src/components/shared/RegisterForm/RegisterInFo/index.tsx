"use client"
import { useFormik } from "formik"
import * as Yup from "yup"
import { ArrowLeftIcon } from "@phosphor-icons/react"
import React from "react"
import { ButtonStyled, InputStyled } from "@/components/styled"
import { Icon } from "@iconify/react"
import { useTranslation } from "react-i18next"

interface RegisterInfoProps {
    handleBack: () => void
}
export function RegisterInFo({ handleBack }: RegisterInfoProps) {
    const { t } = useTranslation()

    // const [isShowPassword, setIsShowPassword] = useState(false)
    // const [isShowConfirmPassword, setIsShowConfirmPassword] = useState(false)
    const [isVisible, setIsVisible] = React.useState(false)
    const toggleVisibility = () => setIsVisible(!isVisible)
    const [isConfirmVisible, setIsConfirmVisible] = React.useState(false)
    const toggleConFirmVisibility = () => setIsConfirmVisible(!isConfirmVisible)
    const formik = useFormik({
        initialValues: {
            lastName: "",
            firstName: "",
            password: "",
            confirmPassword: "",
            phone: ""
        },
        validationSchema: Yup.object({
            lastName: Yup.string()
                .required("Last name is required")
                .matches(/^[A-Za-z\s]+$/, "Last name only contains letters"),
            firstName: Yup.string()
                .required("First name is required")
                .matches(/^[A-Za-z\s]+$/, "First name only contains letters"),
            password: Yup.string()
                .required("Password is required")
                .min(6, "Password must be at least 6 characters")
                .matches(
                    /^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])[A-Za-z0-9!@#$%^&*]{6,}$/,
                    "Password must contain 1 uppercase, 1 number, 1 special char"
                ),
            confirmPassword: Yup.string()
                .oneOf([Yup.ref("password")], "Passwords must match")
                .required("Please confirm your password"),
            phone: Yup.string()
                .required("Phone is required")
                .length(10, "Phone must be exactly 10 digits")
                .matches(/^(0[0-9]{9})$/, "Phone must be 10 digits and start with 0")
        }),
        onSubmit: async (values) => {
            await new Promise((resolve) => setTimeout(resolve, 4000))
            alert(JSON.stringify(values))
            // handleNext()
        }
    })

    return (
        <form onSubmit={formik.handleSubmit} className="flex flex-col">
            {/* Title */}
            <div className="mx-12 mt-2 mb-2">
                <div className="text-center">{t("auth.complete_register")}</div>
            </div>

            {/* Input InFo */}
            <div className="flex mx-auto w-110 gap-5">
                <InputStyled
                    variant="bordered"
                    label="Last name"
                    value={formik.values.lastName}
                    onValueChange={(value) => formik.setFieldValue("lastName", value)}
                    isInvalid={!!(formik.touched.lastName && formik.errors.lastName)}
                    errorMessage={formik.errors.lastName}
                    onBlur={() => {
                        formik.setFieldTouched("lastName")
                    }}
                />

                <InputStyled
                    variant="bordered"
                    label="First name"
                    value={formik.values.firstName}
                    onValueChange={(value) => formik.setFieldValue("firstName", value)}
                    isInvalid={!!(formik.touched.firstName && formik.errors.firstName)}
                    errorMessage={formik.errors.firstName}
                    onBlur={() => {
                        formik.setFieldTouched("firstName")
                    }}
                />
            </div>

            <div className="w-110 mx-auto">
                <InputStyled
                    className="my-3"
                    variant="bordered"
                    label="New password"
                    type={isVisible ? "text" : "password"}
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
                <InputStyled
                    className="my-3"
                    variant="bordered"
                    label="Phone number"
                    maxLength={10}
                    pattern="[0-9]*"
                    onInput={(e) => {
                        e.currentTarget.value = e.currentTarget.value.replace(/[^0-9]/g, "")
                    }}
                    value={formik.values.phone}
                    onValueChange={(value) => formik.setFieldValue("phone", value)}
                    isInvalid={!!(formik.touched.phone && formik.errors.phone)}
                    errorMessage={formik.errors.phone}
                    onBlur={() => {
                        formik.setFieldTouched("phone")
                    }}
                />
            </div>

            <div className="flex mx-auto gap-4 mt-4">
                <ButtonStyled onPress={handleBack} className="w-5 h-10 mx-auto mt-0">
                    <ArrowLeftIcon />
                </ButtonStyled>

                <ButtonStyled
                    type="submit"
                    isLoading={formik.isSubmitting}
                    color="primary"
                    isDisabled={!formik.isValid}
                    className="w-5 h-10 mx-auto mt-0"
                >
                    Submit
                </ButtonStyled>
            </div>
        </form>
    )
}
