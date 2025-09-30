"use client"
import { useFormik } from "formik"
import * as Yup from "yup"
import React from "react"
import { ButtonStyled } from "@/components/styled"
import { InputOtp } from "@heroui/react"
import { ArrowLeftIcon, ArrowRightIcon } from "@phosphor-icons/react"
import { useTranslation } from "react-i18next"

interface RegisterOTPProps {
    handleBack: () => void
    handleNext: () => void
}

export function RegisterOTP({ handleBack, handleNext }: RegisterOTPProps) {
    const { t } = useTranslation()

    const formik = useFormik({
        initialValues: {
            opt: ""
        },
        validationSchema: Yup.object({
            opt: Yup.string()
                .required("OTP is required")
                .matches(/^[0-9]{6}$/, "Invalid OTP format")
        }),
        onSubmit: async (values) => {
            // await new Promise((resolve) => setTimeout(resolve, 4000))
            // let values = JSON.stringify(values)
            alert(JSON.stringify(values))
            handleNext()
        }
    })

    return (
        <form onSubmit={formik.handleSubmit} className="flex flex-col">
            {/* Title */}
            <div className="mx-12 mt-2 mb-2">
                <div className="text-center">{t("auth.verify_identity")}</div>
            </div>
            {/* Input OTP */}
            <div className=" mx-auto">
                <InputOtp
                    size="lg"
                    length={6}
                    value={formik.values.opt}
                    onValueChange={(value) => formik.setFieldValue("opt", value)}
                    isInvalid={!!(formik.touched.opt && formik.errors.opt)}
                    errorMessage={formik.errors.opt}
                    onBlur={() => {
                        formik.setFieldTouched("opt")
                    }}
                />
            </div>

            <div className="flex mx-auto gap-4 mt-4">
                <ButtonStyled
                    onPress={handleBack}
                    color="primary"
                    className="w-2 h-10 mx-auto mt-0"
                >
                    <ArrowLeftIcon />
                </ButtonStyled>

                <ButtonStyled
                    type="submit"
                    isLoading={formik.isSubmitting}
                    color="primary"
                    isDisabled={!formik.isValid}
                    className="w-4 h-10 mx-auto mt-0"
                >
                    {formik.isSubmitting ? "" : <ArrowRightIcon />}
                </ButtonStyled>
            </div>
        </form>
    )
}
