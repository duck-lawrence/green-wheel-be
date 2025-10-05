"use client"
import { useFormik } from "formik"
import * as Yup from "yup"
import React, { useCallback } from "react"
import { ButtonStyled } from "@/components/styled"
import { InputOtp } from "@heroui/react"
import { ArrowLeftIcon, ArrowRightIcon } from "@phosphor-icons/react"
import { useTranslation } from "react-i18next"
import { useRegisterVerify } from "@/hooks"

interface RegisterOTPProps {
    email: string
    onBack: () => void
    onSuccess: () => void
}

export function RegisterOTP({ email, onBack, onSuccess }: RegisterOTPProps) {
    const { t } = useTranslation()
    const registerVerifyMutation = useRegisterVerify({ onSuccess })

    const handleRegisterVerify = useCallback(
        async (values: { otp: string; email: string }) => {
            await registerVerifyMutation.mutateAsync({ ...values })
        },
        [registerVerifyMutation]
    )

    const formik = useFormik({
        initialValues: {
            email: email,
            otp: ""
        },
        validationSchema: Yup.object({
            otp: Yup.string()
                .required("OTP is required")
                .matches(/^[0-9]{6}$/, "Invalid OTP format")
        }),
        onSubmit: handleRegisterVerify
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
            <div className="mx-12 mt-2 mb-2">
                <div className="text-center">{t("auth.verify_identity")}</div>
            </div>
            {/* Input OTP */}
            <div className=" mx-auto">
                <InputOtp
                    size="lg"
                    length={6}
                    value={formik.values.otp}
                    onValueChange={(value) => formik.setFieldValue("otp", value)}
                    isInvalid={!!(formik.touched.otp && formik.errors.otp)}
                    errorMessage={formik.errors.otp}
                    onBlur={() => {
                        formik.setFieldTouched("otp")
                    }}
                />
            </div>

            <div className="flex mx-auto gap-4 mt-4">
                <ButtonStyled onPress={onBack} className="w-2 h-10 mx-auto mt-0">
                    <ArrowLeftIcon />
                </ButtonStyled>

                <ButtonStyled
                    type="submit"
                    color="primary"
                    isLoading={formik.isSubmitting}
                    isDisabled={!formik.isValid}
                    className="w-4 h-10 mx-auto mt-0"
                >
                    {formik.isSubmitting ? "" : <ArrowRightIcon />}
                </ButtonStyled>
            </div>
        </form>
    )
}
