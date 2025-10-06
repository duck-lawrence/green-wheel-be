"use client"
import { ButtonStyled, InputStyled, TextareaStyled } from "@/components"
import { useNavbarItemStore } from "@/hooks/singleton/store/useNavbarItemStore"
import { useFormik } from "formik"
import * as Yup from "yup"
import React, { useEffect } from "react"
import { useTranslation } from "react-i18next"

export default function Contact() {
    const { t } = useTranslation()
    const setActiveMenuKey = useNavbarItemStore((s) => s.setActiveMenuKey)

    useEffect(() => {
        setActiveMenuKey("contact")
    }, [setActiveMenuKey])

    const formik = useFormik({
        initialValues: {
            name: "",
            email: "",
            message: ""
        },
        validationSchema: Yup.object().shape({
            name: Yup.string().required(t("user.full_name_require")),
            email: Yup.string()
                .required(t("user.email_require"))
                .matches(/^[\w.-]+@[\w.-]+\.[a-zA-Z]{2,}$/, t("user.invalid_email")),
            message: Yup.string()
        }),
        onSubmit: (value) => {
            console.log(value)
        }
    })

    return (
        <div>
            <div className="w-[672px] h-[540px] bg-white mt-40 space-y-4 rounded-2xl">
                <div className="flex flex-col justify-center items-center pt-6">
                    <p className="text-3xl font-bold text-primary">Contact Us</p>
                    <p className="text-[18px]">
                        We will love to hear from you! <br />
                    </p>
                    <p className="text-[18px]">
                        Fill out the form or reach us via the information below.
                    </p>
                </div>

                {/* Content */}
                <div className="flex justify-end items-center gap-12 p-10">
                    <div className="space-y-6">
                        <InputStyled
                            variant="bordered"
                            label={t("user.full_name")}
                            value={formik.values.name}
                            onValueChange={(value) => formik.setFieldValue("name", value)}
                            onBlur={() => {
                                formik.setFieldTouched("name")
                            }}
                            onClear={() => console.log("input cleared")}
                        />
                        <InputStyled
                            variant="bordered"
                            label={t("auth.email")}
                            value={formik.values.email}
                            onValueChange={(value) => formik.setFieldValue("email", value)}
                            isInvalid={!!(formik.touched.email && formik.errors.email)}
                            errorMessage={formik.errors.email}
                            onBlur={() => {
                                formik.setFieldTouched("email")
                            }}
                            onClear={() => console.log("input cleared")}
                        />

                        <TextareaStyled
                            isClearable
                            className="max-w-xs"
                            label="Message"
                            placeholder="Type your message herre..."
                            variant="bordered"
                            onClear={() => console.log("textarea cleared")}
                            onValueChange={(value) => formik.setFieldValue("message", value)}
                        />

                        <ButtonStyled
                            className="flex items-center bg-primary text-white gap-1 px-20 py-2
                                 cursor-pointer font-semibold 
                                 hover:opacity-75 duration-300 hover:text-black"
                            type="submit"
                            isDisabled={!formik.isValid}
                            onPress={() => formik.submitForm()}
                        >
                            Send Message
                            <svg
                                className="w-5 h-5"
                                stroke="currentColor"
                                strokeWidth="1.5"
                                viewBox="0 0 24 24"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M6 12 3.269 3.125A59.769 59.769 0 0 1 21.485 12 59.768 59.768 0 0 1 3.27 20.875L5.999 12Zm0 0h7.5"
                                    strokeLinejoin="round"
                                    strokeLinecap="round"
                                />
                            </svg>
                        </ButtonStyled>
                    </div>
                    <div className="flex flex-col gap-5 pb-22">
                        <p>
                            <span className="font-bold text-primary">Address: </span> TP HCM, Việt
                            Nam
                        </p>
                        <p>
                            <span className="font-bold text-primary">Phone: </span> 0900 123 432
                        </p>
                        <p>
                            <span className="font-bold text-primary">Email: </span>
                            greenwheel@gmail.com
                        </p>
                        <p className="text-gray-400 text-sm">
                            Our team will get back to you as soon as possible. For urgent matters,
                            please call our hotline
                        </p>
                    </div>
                </div>
            </div>
        </div>
    )
}
