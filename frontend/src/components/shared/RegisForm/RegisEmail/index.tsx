"use client"
import { useFormik } from "formik"
import * as Yup from "yup"
import React from "react"
import { ButtonStyled, InputStyled } from "@/components/styled"

export function RegisEmail({ handleSubmit }: { handleSubmit: () => void }) {
    const formik = useFormik({
        initialValues: {
            email: ""
        },
        validationSchema: Yup.object({
            email: Yup.string()
                .required("Email is required")
                .matches(/^[\w.-]+@[\w.-]+\.[a-zA-Z]{2,}$/, "Invalid email format")
        }),
        onSubmit: async (values) => {
            // await new Promise((resolve) => setTimeout(resolve, 4000))
            handleSubmit()
            // console.log()
            alert(JSON.stringify(values))
        }
    })

    return (
        <form onSubmit={formik.handleSubmit} className="flex flex-col gap-4">
            {/* Title */}
            <div className="mx-8 mt-2 mb-0">
                <h1 className="font-bold text-xl">Register Account (Step 1)</h1>
            </div>

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
                    onClear={() => console.log("input cleared")}
                />
            </div>
            {/* Button submit */}
            <ButtonStyled
                type="submit"
                className="w-110 h-10 mx-auto mt-4"
                isLoading={formik.isSubmitting}
                color="primary"
                isDisabled={!formik.isValid}
                // onPress={() => formik.submitForm()}
            >
                Send OTP
            </ButtonStyled>
        </form>
    )
}
