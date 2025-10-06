"use client"
import { useFormik } from "formik"
import * as Yup from "yup"
import React, { useCallback, useState } from "react"
import {
    ButtonStyled,
    ButtonToggleVisibility,
    DatePickerStyled,
    InputStyled
} from "@/components/styled"
import { useTranslation } from "react-i18next"
import { useDay, useRegisterComplete } from "@/hooks"
import { UserRegisterCompleteReq } from "@/models/auth/schema/request"
import { Sex } from "@/constants/enum"
import { EnumPicker } from "@/components/modules/EnumPicker"
import { SexLabels } from "@/constants/labels"
import { NAME_REGEX, PASSWORD_REGEX, PHONE_REGEX } from "@/constants/regex"

interface RegisterInfoProps {
    // onBack: () => void
    onSuccess?: () => void
}

export function RegisterInFo({ onSuccess }: RegisterInfoProps) {
    const { t } = useTranslation()
    const { formatDateTime } = useDay({})
    const registerMutation = useRegisterComplete({ onSuccess })

    const [isVisible, setIsVisible] = useState(false)
    const toggleVisibility = () => setIsVisible(!isVisible)
    const [isConfirmVisible, setIsConfirmVisible] = useState(false)
    const toggleConFirmVisibility = () => setIsConfirmVisible(!isConfirmVisible)

    const handleRegisterComplete = useCallback(
        async (values: UserRegisterCompleteReq) => {
            await registerMutation.mutateAsync(values)
        },
        [registerMutation]
    )

    const formik = useFormik({
        initialValues: {
            lastName: "",
            firstName: "",
            password: "",
            confirmPassword: "",
            dateOfBirth: "",
            phone: "",
            sex: Sex.Male
        },
        validationSchema: Yup.object({
            lastName: Yup.string()
                .required(t("user.last_name_require"))
                .matches(NAME_REGEX, t("user.invalid_last_name")),
            firstName: Yup.string()
                .required(t("user.first_name_require"))
                .matches(NAME_REGEX, t("user.invalid_first_name")),
            password: Yup.string()
                .required(t("user.password_can_not_empty"))
                .min(8, t("user.password_too_short"))
                .matches(PASSWORD_REGEX, t("user.password_strength")),
            confirmPassword: Yup.string()
                .oneOf([Yup.ref("password")], t("user.confirm_password_equal"))
                .required(t("user.password_can_not_empty")),
            dateOfBirth: Yup.string().required(t("user.date_of_birth_require")),
            phone: Yup.string()
                .required(t("user.phone_require"))
                .matches(PHONE_REGEX, t("user.invalid_phone")),
            sex: Yup.number().required(t("user.sex_require"))
        }),
        onSubmit: handleRegisterComplete
    })

    return (
        <form onSubmit={formik.handleSubmit} className="flex flex-col">
            {/* Title */}
            <div className="mx-auto mt-2 mb-2">
                <div className="text-center">{t("auth.complete_register")}</div>
            </div>

            {/* Input InFo */}
            <div className="flex mx-auto w-110 gap-5">
                <InputStyled
                    label={t("user.last_name")}
                    variant="bordered"
                    value={formik.values.lastName}
                    onValueChange={(value) => formik.setFieldValue("lastName", value)}
                    isInvalid={!!(formik.touched.lastName && formik.errors.lastName)}
                    errorMessage={formik.errors.lastName}
                    onBlur={() => {
                        formik.setFieldTouched("lastName")
                    }}
                />

                <InputStyled
                    label={t("user.first_name")}
                    variant="bordered"
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
                    label={t("auth.password")}
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
                    label={t("auth.confirm_password")}
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
                <InputStyled
                    className="my-3"
                    variant="bordered"
                    label={t("user.phone")}
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

            <div className="flex mx-auto w-110 gap-5">
                <EnumPicker
                    label={t("user.sex")}
                    labels={SexLabels}
                    value={formik.values.sex}
                    onChange={(val) => formik.setFieldValue("sex", val)}
                />

                <DatePickerStyled
                    label={t("user.date_of_birth")}
                    isInvalid={!!(formik.touched.dateOfBirth && formik.errors.dateOfBirth)}
                    errorMessage={formik.errors.dateOfBirth}
                    onChange={(value) => {
                        if (!value) {
                            formik.setFieldValue("dateOfBirth", null)
                            return
                        }

                        const dob = formatDateTime({ value })

                        formik.setFieldValue("dateOfBirth", dob)
                    }}
                />
            </div>

            <div className="mx-auto">
                {/* <ButtonStyled onPress={onBack} className="w-5 h-10 mx-auto mt-0">
                    <ArrowLeftIcon />
                </ButtonStyled> */}

                <ButtonStyled
                    type="submit"
                    className="w-110 h-10 mx-auto mt-4"
                    isLoading={formik.isSubmitting}
                    color="primary"
                    isDisabled={!formik.isValid}
                >
                    {t("login.register")}
                </ButtonStyled>
            </div>
        </form>
    )
}
