"use client"
import { useFormik } from "formik"
import * as Yup from "yup"
import React from "react"
import { ButtonStyled } from "@/components/styled"
import { InputOtp } from "@heroui/react"
import { ArrowLeftIcon, ArrowRightIcon } from "@phosphor-icons/react"

interface RegisOTPProps {
    handleBack: () => void
    handleNext: () => void
}

export function RegisOTP({ handleBack, handleNext }: RegisOTPProps) {
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
                <h1 className="font-bold text-xl">Register Account (Step 2)</h1>
            </div>
            {/* Input OTP */}
            <div className=" mx-auto">
                <p className="text-default-600 mb-2 ml-22 text-xl font-bold">6 digits OTP</p>
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
                    className="w-2 h-16 mx-auto mt-0 bg-primary opacity-80"
                >
                    <ArrowLeftIcon />
                </ButtonStyled>

                <ButtonStyled
                    type="submit"
                    isLoading={formik.isSubmitting}
                    color="primary"
                    isDisabled={!formik.isValid}
                    className="w-4 h-16 mx-auto mt-0"
                >
                    {formik.isSubmitting ? "" : <ArrowRightIcon />}
                </ButtonStyled>
            </div>
        </form>
    )
}
