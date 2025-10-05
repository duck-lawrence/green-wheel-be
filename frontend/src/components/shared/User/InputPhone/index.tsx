"use client"
import { InputStyled } from "@/components/styled"
import { useFormik } from "formik"
import * as Yub from "yup"
import React from "react"
import { PHONE_REGEX } from "@/constants/regex"
export function InputPhone({ isReadOnly }: { isReadOnly: boolean }) {
    // const [isDisabled, setIsDisabled] = useState(style)

    const formik = useFormik({
        initialValues: {
            phone: ""
        },
        validationSchema: Yub.object({
            phone: Yub.string()
                .required("Phone is required")
                .matches(PHONE_REGEX, "Phone must be 10 digits and start with 0")
        }),
        onSubmit: (values) => {
            alert(JSON.stringify(values))
        }
    })

    return (
        <form>
            <InputStyled
                isReadOnly={isReadOnly}
                className="w-75"
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
        </form>
    )
}
